
namespace SCERP.Model.Production
{
    public class PROD_ProcessDeliveryDetail
    {
        public long ProcessDeliveryDetailId { get; set; }
        public long ProcessDeliveryId { get; set; }
        public string CompId { get; set; }
        public long CuttingTagId { get; set; }
        public long CuttingBatchId { get; set; }
        public string ComponentRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public virtual PROD_ProcessDelivery PROD_ProcessDelivery { get; set; }
    

    }
}
