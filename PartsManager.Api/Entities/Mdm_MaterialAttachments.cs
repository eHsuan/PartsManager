using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Mdm_MaterialAttachments")]
public class Mdm_MaterialAttachments
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int MaterialID { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    public DateTime UploadTime { get; set; } = DateTime.Now;

    [ForeignKey("MaterialID")]
    public virtual Mdm_Materials? Material { get; set; }
}
