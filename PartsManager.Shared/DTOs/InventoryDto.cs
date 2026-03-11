using System;

namespace PartsManager.Shared.DTOs
{
    public class InventoryDto : MaterialDto
    {
        public decimal CurrentQty { get; set; }
    }

    public class LowStockDto
    {
        public int MaterialID { get; set; }
        public string PartNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Specification { get; set; }
        public decimal TotalQuantity { get; set; }
        public int SafeStockQty { get; set; }
    }
}

