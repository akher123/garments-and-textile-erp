using System;

namespace SCERP.Model.Production
{
    public partial class SpPrintEmbroiderySummary
    {
        public string CuttingBatchRefId { get; set; }
        public Nullable<System.DateTime> CuttingDate { get; set; }
        public string JobNo { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string BuyerRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderNo { get; set; }
        public string OrderName { get; set; }
        public string OrderStyleRefId { get; set; }
        public string StyleName { get; set; }
        public string ColorRefId { get; set; }
        public string ColorName { get; set; }
        public string ComponentRefId { get; set; }
        public string Sequence { get; set; }
        public Nullable<long> CuttingSequenceId { get; set; }
        public Nullable<long> CuttingTagId { get; set; }
        public string TagRefId { get; set; }
        public string TagName { get; set; }
        public Nullable<bool> IsPrint { get; set; }
        public Nullable<bool> IsEmbroidery { get; set; }
        public Nullable<int> EmblishmentStatus { get; set; }
        public Nullable<int> FinalCut { get; set; }
        public int TotalSend { get; set; }
        public string PartyName { get; set; }
        public int RemainingQty { get { return FinalCut - TotalSend??0; } } 
    }
}
