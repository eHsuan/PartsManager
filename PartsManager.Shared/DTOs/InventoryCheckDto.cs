using System;
using System.Collections.Generic;

namespace PartsManager.Shared.DTOs
{
    public class InventoryCheckItemDto
    {
        public string PartNo { get; set; }
        public decimal ScannedQty { get; set; }
        public DateTime ScanTime { get; set; }
    }

    public class InventoryCheckRequestDto
    {
        public int WarehouseId { get; set; }
        public List<InventoryCheckItemDto> Items { get; set; }
    }

    public class InventoryComparisonResultDto
    {
        public string PartNo { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public decimal SystemQty { get; set; }
        public decimal ScannedQty { get; set; }
        public decimal Difference => ScannedQty - SystemQty;
        public string Status { get; set; } // "Match", "Missing", "Excess", "Different"
    }
}
