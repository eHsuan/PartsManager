using System;

namespace PartsManager.Shared.DTOs
{
    public class TransactionDto
    {
        public long TransID { get; set; }
        public string TransType { get; set; }
        public string PartNo { get; set; }
        public string MaterialName { get; set; }
        public decimal ChangeQty { get; set; }
        public decimal AfterQty { get; set; }
        public string ReasonCode { get; set; }
        public string OperatorID { get; set; }
        public DateTime TransTime { get; set; }
        public string WarehouseName { get; set; }
    }

    public class TransactionQueryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
