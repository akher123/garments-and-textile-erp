
namespace SCERP.Model.Production
{
    public class VwPartyWiseCuttiongProcess : SearchModel<VwPartyWiseCuttiongProcess>
    {
        public string BuyerName { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public string SequenceName { get; set; }
        public string TagName { get; set; }
        public string ColorName { get; set; }
        public long PartyId { get; set; }
        public string OrderStyleRefId { get; set; }
        public long CuttingTagId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        public string ApprovalStatus { get; set; }
        public string CuttingStatus { get; set; }
        public bool IsEmbroidery { get; set; }
        public bool IsPrint { get; set; }
        public bool IsSolid { get; set; }
        public string CompId { get; set; }
        public int FinalCutt { get; set; }
        public int TotalSent { get; set; }
        public string JobNo { get; set; }
    }
}
