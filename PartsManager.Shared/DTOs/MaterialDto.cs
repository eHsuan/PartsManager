using System;

namespace PartsManager.Shared.DTOs
{
    public class MaterialDto
    {
        public int MaterialID { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Station { get; set; }
        public string PartNo { get; set; }
        public int SafeStockQty { get; set; }
        public int LeadTimeDays { get; set; }
    }
}

