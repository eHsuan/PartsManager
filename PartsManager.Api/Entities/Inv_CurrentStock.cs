using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Inv_CurrentStock")]
public class Inv_CurrentStock
{
    [Key]
    public long StockID { get; set; }

    public int MaterialID { get; set; }
    [ForeignKey("MaterialID")]
    public Mdm_Materials? Material { get; set; }

    public int WarehouseID { get; set; }
    [ForeignKey("WarehouseID")]
    public Mdm_Warehouses? Warehouse { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    public DateTime LastUpdated { get; set; }
}
