namespace PartsManager.Shared.DTOs
{
    public class BackupFileDto
    {
        public string FolderId { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public System.DateTime CreatedTime { get; set; }
    }

    public class BackupRestoreProgressDto
    {
        public int Progress { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
