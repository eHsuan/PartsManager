using System;
using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class InboundDto
    {
        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        public string OperatorId { get; set; }
    }
}
