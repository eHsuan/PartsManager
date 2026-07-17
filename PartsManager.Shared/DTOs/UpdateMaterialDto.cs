using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class UpdateMaterialDto
    {
        [Required]
        public string PartNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Specification { get; set; }

        public string StorageLocation { get; set; } = string.Empty;

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }

        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerPartNo { get; set; }
        [Required]
        public int MachineID { get; set; }

        public decimal CurrentStock { get; set; }
        public int? WarehouseId { get; set; }
        public string OperatorID { get; set; } = "SYSTEM";
    }
}
