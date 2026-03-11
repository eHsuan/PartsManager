using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class UpdateMaterialDto
    {
        [Required]
        public string PartNo { get; set; }

        [Required]
        public string Name { get; set; }

        public string Specification { get; set; }

        public string Station { get; set; } // ?�援修改站別

        public int SafeStockQty { get; set; }

        public int LeadTimeDays { get; set; }
    }
}

