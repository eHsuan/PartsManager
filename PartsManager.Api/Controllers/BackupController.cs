using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using PartsManager.Api.Services;
using PartsManager.Shared.DTOs;

namespace PartsManager.Api.Controllers
{
    public static class RestoreStateManager
    {
        public static int Progress { get; set; } = 0;
        public static string Status { get; set; } = "Ready";
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly GoogleDriveService _driveService;
        private readonly IConfiguration _config;
        private readonly string _dbPath;
        private readonly string _attachmentsPath;

        public BackupController(GoogleDriveService driveService, IConfiguration config)
        {
            _driveService = driveService;
            _config = config;
            string connStr = config.GetConnectionString("DefaultConnection") ?? "";
            _dbPath = connStr.Replace("Data Source=", "").Trim();
            _attachmentsPath = config["System:AttachmentPath"] ?? "Attachments";
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunBackup()
        {
            try
            {
                await _driveService.UploadBackupAsync(_dbPath, _attachmentsPath, true);
                int retention = int.TryParse(_config["Backup:MaxRetentionCount"], out int r) ? r : 7;
                await _driveService.DeleteOldBackupsAsync(retention, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<BackupFileDto>>> GetList()
        {
            try
            {
                var backups = await _driveService.GetBackupsAsync();
                var result = new List<BackupFileDto>();
                foreach (var b in backups)
                {
                    result.Add(new BackupFileDto
                    {
                        FolderId = b.FileId,
                        FolderName = b.FileName,
                        CreatedTime = b.CreatedTime
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("restore-progress")]
        public ActionResult<BackupRestoreProgressDto> GetProgress()
        {
            return Ok(new BackupRestoreProgressDto
            {
                Progress = RestoreStateManager.Progress,
                Status = RestoreStateManager.Status
            });
        }

        [HttpPost("restore/{fileId}")]
        public IActionResult RestoreBackup(string fileId)
        {
            if (RestoreStateManager.Status == "Downloading" || RestoreStateManager.Status == "Extracting")
            {
                return BadRequest("A restore operation is already in progress.");
            }

            RestoreStateManager.Progress = 0;
            RestoreStateManager.Status = "Downloading";

            Task.Run(async () =>
            {
                try
                {
                    string targetDbDir = Path.GetDirectoryName(_dbPath) ?? "";
                    
                    // Clear DB connections
                    SqliteConnection.ClearAllPools();
                    // Let the OS release the lock
                    await Task.Delay(500);

                    await _driveService.DownloadAndExtractBackupAsync(fileId, targetDbDir, _attachmentsPath, (int progress) =>
                    {
                        RestoreStateManager.Progress = progress;
                        if (progress >= 90) RestoreStateManager.Status = "Extracting";
                    });

                    RestoreStateManager.Progress = 100;
                    RestoreStateManager.Status = "Completed";
                }
                catch (Exception ex)
                {
                    RestoreStateManager.Status = "Failed: " + ex.Message;
                }
            });

            return Accepted();
        }
    }
}
