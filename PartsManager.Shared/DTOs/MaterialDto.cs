using System;

namespace PartsManager.Shared.DTOs
{
    public class MaterialDto
    {
        public int MaterialID { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string StorageLocation { get; set; } = string.Empty;
        public string PartNo { get; set; }
        public int SafeStockQty { get; set; }
        public int LeadTimeDays { get; set; }
        public decimal Price { get; set; }

        public int MachineID { get; set; }
        public string MachineName { get; set; } = string.Empty;

        public decimal CurrentStock { get; set; }
        public int? WarehouseId { get; set; }
    }
}

