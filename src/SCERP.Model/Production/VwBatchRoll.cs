namespace SCERP.Model.Production
{
    public class VwBatchRoll
    {
        public long BatchRollId { get; set; }
        public string OrderStyleRefId { get; set; }
        public long BatchId { get; set; }
        public string Remarks { get; set; }
        public string BatchNo { get; set; }
        public long KnittingRollId { get; set; }
        public string RollRefNo { get; set; }
        public string CharllRollNo { get; set; }
        public string GSM { get; set; }
        public string SizeName { get; set; }
        public string FinishSizeName { get; set; }
        public string RollIssueRefId { get; set; }
        public int KnittingRollIssueId { get; set; }
        public double Quantity { get; set; }
        public string ItemName { get; set; }
    }
}
