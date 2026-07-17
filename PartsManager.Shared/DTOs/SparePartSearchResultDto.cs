namespace PartsManager.Shared.DTOs
{
    public class SparePartSearchResultDto
    {
        public int MaterialId { get; set; }
        public string PartNo { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string StorageLocation { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string WarehouseName { get; set; }
        public decimal Quantity { get; set; }
        public int SafeStockQty { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerPartNo { get; set; }
        public decimal TotalAmount { get; set; }
        public int LeadTimeDays { get; set; }
        public System.Collections.Generic.List<string> AttachmentFileNames { get; set; } = new System.Collections.Generic.List<string>();
    }
}

