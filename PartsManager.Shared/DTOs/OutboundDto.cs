using System;
using System.ComponentModel.DataAnnotations;

namespace PartsManager.Shared.DTOs
{
    public class OutboundDto
    {
        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        public int MachineId { get; set; }

        public string ReasonCode { get; set; }

        public string OperatorId { get; set; }
    }

    public class OutboundResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
        public string LowStockMessage { get; set; } = string.Empty;
    }
}
