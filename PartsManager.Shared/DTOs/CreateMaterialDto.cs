using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class CreateMaterialDto
    {
        [Required]
        public string PartNo { get; set; }

        [Required]
        public string Name { get; set; }

        public string Specification { get; set; }

        public string Station { get; set; }

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }

        public byte SourceType { get; set; } = 1; // ?�設 1: Line-Side Purchased
    }
}

