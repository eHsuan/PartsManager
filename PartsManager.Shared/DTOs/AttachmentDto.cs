using System;

namespace PartsManager.Shared.DTOs
{
    public class AttachmentDto
    {
        public int ID { get; set; }
        public int MaterialID { get; set; }
        public string FileName { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
