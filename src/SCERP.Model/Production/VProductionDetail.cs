using System;
namespace SCERP.Model.Production
{
    public class VProductionDetail
    {
        public long ProductionId { get; set; }
        public long ProductionDetailId { get; set; }
        public string CompId { get; set; }
        public string PType { get; set; }
        public string ProrgramRefId { get; set; }
        public string ProductionRefId { get; set; }
        public string ProcessorRefId { get; set; }
        public string ProcessRefId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ColorRefId { get; set; }
        public string ColorName { get; set; }
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public int? MeasurementUinitId { get; set; }
        public string UnitName { get; set; }
        public Decimal? Qty { get; set; }
        public Decimal EntryQty { get; set; }
    }
}
