using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Mdm_Warehouses")]
public class Mdm_Warehouses
{
    [Key]
    public int WarehouseID { get; set; }

    [Required]
    [MaxLength(50)]
    public string WarehouseCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string WarehouseName { get; set; } = string.Empty;

    public bool IsExternalMES { get; set; }
}
