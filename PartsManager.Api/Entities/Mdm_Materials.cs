using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Mdm_Materials")]
public class Mdm_Materials
{
    [Key]
    public int MaterialID { get; set; }

    [MaxLength(100)]
    public string? BarCode { get; set; }

    [Required]
    [MaxLength(50)]
    public string PartNo { get; set; } = string.Empty;

    public byte SourceType { get; set; } // 0 = MES Spare Part, 1 = Line-Side Purchased

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Specification { get; set; } = "None";

    [Required]
    [MaxLength(200)]
    public string Supplier { get; set; } = "None";

    [Required]
    [MaxLength(200)]
    public string Manufacturer { get; set; } = "None";

    public bool NeedsPrintLabel { get; set; } = true;

    public int LeadTimeDays { get; set; }

    public int SafeStockQty { get; set; }

    public DateTime? LastSyncTime { get; set; }
}
