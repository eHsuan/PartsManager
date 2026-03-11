using System;
using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class AuditDto
    {
        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal CountQty { get; set; }

        public string ReasonCode { get; set; }

        public string OperatorId { get; set; }
    }
}
