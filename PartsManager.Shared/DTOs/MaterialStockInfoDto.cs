using System;
using System.Collections.Generic;

namespace PartsManager.Shared.DTOs
{
    public class MaterialStockInfoDto
    {
        public int MaterialID { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty; // 加入此屬性
        public int SafeStockQty { get; set; }
        public List<StockDetailDto> Stocks { get; set; } = new List<StockDetailDto>();
    }

    public class StockDetailDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
