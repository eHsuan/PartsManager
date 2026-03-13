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

        [Required]
        public string Supplier { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }
    }
}
