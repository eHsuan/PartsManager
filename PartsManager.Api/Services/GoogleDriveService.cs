using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Download;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PartsManager.Api.Services
{
    public class BackupItem
    {
        public string FileId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }
    }

    public class GoogleDriveService
    {
        private readonly ILogger<GoogleDriveService> _logger;
        private readonly string _credentialPath;
        private readonly string _parentFolderId;
        private readonly DriveService? _driveService;

        public GoogleDriveService(IConfiguration config, ILogger<GoogleDriveService> logger)
        {
            _logger = logger;
            _credentialPath = config["Backup:DriveCredentialPath"] ?? "service_account.json";
            _parentFolderId = config["Backup:DriveParentFolderId"] ?? string.Empty;

            string oauthClientId = config["Backup:OAuthClientId"] ?? "";
            string oauthClientSecret = config["Backup:OAuthClientSecret"] ?? "";
            string oauthRefreshToken = config["Backup:OAuthRefreshToken"] ?? "";

            if (!string.IsNullOrEmpty(oauthRefreshToken) && !string.IsNullOrEmpty(oauthClientId))
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = oauthClientId,
                        ClientSecret = oauthClientSecret
                    },
                    Scopes = new[] { DriveService.Scope.DriveFile }
                });

                var credential = new UserCredential(flow, "user", new TokenResponse
                {
                    RefreshToken = oauthRefreshToken
                });

                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "PartsManagerBackup",
                });
                
                _logger.LogInformation("Google Drive API initialized using OAuth 2.0 Refresh Token.");
            }
            else if (File.Exists(_credentialPath))
            {
                GoogleCredential credential;
                using (var stream = new FileStream(_credentialPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                                     .CreateScoped(DriveService.Scope.DriveFile);
                }

                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "PartsManagerBackup",
                });
                
                _logger.LogInformation("Google Drive API initialized using Service Account JSON.");
            }
            else
            {
                _logger.LogWarning($"Google Drive OAuth/Service Account credentials not configured. Backup features disabled.");
            }
        }

        public bool IsConfigured => _driveService != null;

        public async Task UploadBackupAsync(string dbPath, string attachmentsPath, bool isManual = false)
        {
            if (_driveService == null) throw new Exception("Google Drive API 尚未初始化，請確認 config.ini 中的 OAuth 憑證或 service_account.json 設定是否正確。");

            string tempDir = Path.Combine(Path.GetTempPath(), "PartsManager_Backup_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                // Copy DB files
                if (File.Exists(dbPath)) File.Copy(dbPath, Path.Combine(tempDir, Path.GetFileName(dbPath)), true);
                string walPath = dbPath + "-wal";
                if (File.Exists(walPath)) File.Copy(walPath, Path.Combine(tempDir, Path.GetFileName(walPath)), true);
                string shmPath = dbPath + "-shm";
                if (File.Exists(shmPath)) File.Copy(shmPath, Path.Combine(tempDir, Path.GetFileName(shmPath)), true);

                // Copy Attachments
                string targetAttPath = Path.Combine(tempDir, "Attachments");
                if (Directory.Exists(attachmentsPath))
                {
                    Directory.CreateDirectory(targetAttPath);
                    foreach (string file in Directory.GetFiles(attachmentsPath))
                    {
                        File.Copy(file, Path.Combine(targetAttPath, Path.GetFileName(file)), true);
                    }
                }

                // Zip
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string prefix = isManual ? "ManualBackup" : "AutoBackup";
                string zipPath = Path.Combine(Path.GetTempPath(), $"{prefix}_{timestamp}.zip");
                if (File.Exists(zipPath)) File.Delete(zipPath);
                ZipFile.CreateFromDirectory(tempDir, zipPath, CompressionLevel.Optimal, false);

                // Upload
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(zipPath),
                    MimeType = "application/zip"
                };
                if (!string.IsNullOrEmpty(_parentFolderId))
                {
                    fileMetadata.Parents = new List<string> { _parentFolderId };
                }

                using (var stream = new FileStream(zipPath, FileMode.Open))
                {
                    var request = _driveService.Files.Create(fileMetadata, stream, "application/zip");
                    request.Fields = "id";
                    var results = await request.UploadAsync();
                    if (results.Status == UploadStatus.Failed)
                    {
                        _logger.LogError($"Error uploading backup: {results.Exception?.Message}");
                        throw new Exception($"上傳至 Google Drive 失敗: {results.Exception?.Message}");
                    }
                    else
                    {
                        _logger.LogInformation($"Backup uploaded successfully. ID: {request.ResponseBody?.Id}");
                    }
                }

                File.Delete(zipPath);
            }
            finally
            {
                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            }
        }

        public async Task<List<BackupItem>> GetBackupsAsync()
        {
            var result = new List<BackupItem>();
            if (_driveService == null) return result;

            string query = "name contains 'Backup_' and mimeType = 'application/zip' and trashed = false";
            if (!string.IsNullOrEmpty(_parentFolderId))
            {
                query += $" and '{_parentFolderId}' in parents";
            }

            var request = _driveService.Files.List();
            request.Q = query;
            request.Fields = "files(id, name, createdTime)";

            var response = await request.ExecuteAsync();
            if (response.Files != null)
            {
                foreach (var file in response.Files)
                {
                    result.Add(new BackupItem
                    {
                        FileId = file.Id,
                        FileName = file.Name,
                        CreatedTime = file.CreatedTime ?? DateTime.MinValue
                    });
                }
            }

            return result.OrderByDescending(x => x.CreatedTime).ToList();
        }

        public async Task DeleteOldBackupsAsync(int maxRetention, bool isManual = false)
        {
            var backups = await GetBackupsAsync();
            string prefix = isManual ? "ManualBackup" : "AutoBackup";
            var targetBackups = backups.Where(x => x.FileName.StartsWith(prefix)).ToList();

            if (targetBackups.Count > maxRetention)
            {
                var toDelete = targetBackups.Skip(maxRetention).ToList();
                foreach (var item in toDelete)
                {
                    try
                    {
                        await _driveService.Files.Delete(item.FileId).ExecuteAsync();
                        _logger.LogInformation($"Deleted old backup: {item.FileName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to delete old backup {item.FileName}: {ex.Message}");
                    }
                }
            }
        }

        public async Task DownloadAndExtractBackupAsync(string fileId, string targetDbDir, string targetAttachmentsDir, Action<int> progressCallback)
        {
            if (_driveService == null) throw new Exception("Google Drive is not configured.");

            var metaRequest = _driveService.Files.Get(fileId);
            metaRequest.Fields = "size";
            var file = await metaRequest.ExecuteAsync();
            long totalBytes = file.Size ?? 1;

            var request = _driveService.Files.Get(fileId);
            string zipPath = Path.Combine(Path.GetTempPath(), $"Restore_{Guid.NewGuid()}.zip");
            
            using (var stream = new FileStream(zipPath, FileMode.Create))
            {
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            if (totalBytes > 0)
                            {
                                int percentage = (int)((progress.BytesDownloaded * 100) / totalBytes);
                                // leave 10% for extraction
                                progressCallback((int)(percentage * 0.9));
                            }
                            break;
                    }
                };
                await request.DownloadAsync(stream);
            }

            progressCallback(90);

            // Extract
            string tempDir = Path.Combine(Path.GetTempPath(), "RestoreExt_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            try
            {
                ZipFile.ExtractToDirectory(zipPath, tempDir, true);

                // Copy DB files
                foreach (string dbFile in Directory.GetFiles(tempDir, "Parts.db*"))
                {
                    File.Copy(dbFile, Path.Combine(targetDbDir, Path.GetFileName(dbFile)), true);
                }

                // Copy Attachments
                string attDir = Path.Combine(tempDir, "Attachments");
                if (Directory.Exists(attDir))
                {
                    if (!Directory.Exists(targetAttachmentsDir)) Directory.CreateDirectory(targetAttachmentsDir);
                    foreach (string attFile in Directory.GetFiles(attDir))
                    {
                        File.Copy(attFile, Path.Combine(targetAttachmentsDir, Path.GetFileName(attFile)), true);
                    }
                }
            }
            finally
            {
                if (File.Exists(zipPath)) File.Delete(zipPath);
                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
                progressCallback(100);
            }
        }
    }
}
