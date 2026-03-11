using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Inv_Transactions")]
public class Inv_Transactions
{
    [Key]
    public long TransID { get; set; }

    [Required]
    [MaxLength(20)]
    public string TransType { get; set; } = string.Empty; // "IN", "OUT", "ADJ", "SYNC"

    public int MaterialID { get; set; }
    [ForeignKey("MaterialID")]
    public Mdm_Materials? Material { get; set; }

    public int WarehouseID { get; set; }
    [ForeignKey("WarehouseID")]
    public Mdm_Warehouses? Warehouse { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ChangeQty { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AfterQty { get; set; }

    [MaxLength(200)]
    public string? ReasonCode { get; set; }

    [MaxLength(50)]
    public string? OperatorID { get; set; }

    public DateTime TransTime { get; set; }
}
