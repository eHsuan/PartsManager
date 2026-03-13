using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class CreateMaterialDto
    {
        [Required]
        public string PartNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Specification { get; set; } = "None";

        [Required]
        public string Supplier { get; set; } = "None";

        [Required]
        public string Manufacturer { get; set; } = "None";

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }

        public byte SourceType { get; set; } = 1; // ?�設 1: Line-Side Purchased
    }
}

