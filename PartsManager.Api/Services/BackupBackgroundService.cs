using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PartsManager.Api.Services
{
    public class BackupBackgroundService : BackgroundService
    {
        private readonly ILogger<BackupBackgroundService> _logger;
        private readonly GoogleDriveService _driveService;
        private readonly int _intervalHours;
        private readonly int _maxRetention;
        private readonly string _dbPath;
        private readonly string _attachmentsPath;

        public BackupBackgroundService(ILogger<BackupBackgroundService> logger, GoogleDriveService driveService, IConfiguration config)
        {
            _logger = logger;
            _driveService = driveService;
            
            _intervalHours = int.TryParse(config["Backup:BackupIntervalHours"], out int h) ? h : 24;
            _maxRetention = int.TryParse(config["Backup:MaxRetentionCount"], out int r) ? r : 7;
            
            string connStr = config.GetConnectionString("DefaultConnection") ?? "";
            _dbPath = connStr.Replace("Data Source=", "").Trim();
            _attachmentsPath = config["System:AttachmentPath"] ?? "Attachments";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_driveService.IsConfigured)
            {
                _logger.LogWarning("Google Drive is not configured. BackupBackgroundService will not run.");
                return;
            }

            try
            {
                var backups = await _driveService.GetBackupsAsync();
                var lastAutoBackup = System.Linq.Enumerable.FirstOrDefault(
                    System.Linq.Enumerable.OrderByDescending(
                        System.Linq.Enumerable.Where(backups, b => b.FileName.StartsWith("AutoBackup_")),
                        b => b.CreatedTime));

                if (lastAutoBackup != null)
                {
                    TimeSpan elapsed = DateTime.Now - lastAutoBackup.CreatedTime.ToLocalTime();
                    if (elapsed.TotalHours < _intervalHours)
                    {
                        TimeSpan waitTime = TimeSpan.FromHours(_intervalHours) - elapsed;
                        _logger.LogInformation($"Last auto backup was at {lastAutoBackup.CreatedTime.ToLocalTime()}. Waiting for {waitTime.TotalHours:F2} hours before next backup.");
                        await Task.Delay(waitTime, stoppingToken);
                    }
                }
            }
            catch (TaskCanceledException) { return; }
            catch (Exception ex)
            {
                _logger.LogWarning($"Initial backup check failed: {ex.Message}. Proceeding to immediate backup.");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting scheduled backup...");
                    await _driveService.UploadBackupAsync(_dbPath, _attachmentsPath);
                    await _driveService.DeleteOldBackupsAsync(_maxRetention);
                    _logger.LogInformation("Scheduled backup completed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Scheduled backup failed: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(_intervalHours), stoppingToken);
            }
        }
    }
}
