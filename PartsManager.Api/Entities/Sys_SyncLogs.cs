using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Sys_SyncLogs")]
public class Sys_SyncLogs
{
    [Key]
    public int LogID { get; set; }

    [Required]
    [MaxLength(100)]
    public string TaskName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty; // "Success" / "Fail"

    public int RecordsProcessed { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? ErrorMessage { get; set; }
}

