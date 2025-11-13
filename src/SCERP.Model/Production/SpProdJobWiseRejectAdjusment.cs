
namespace SCERP.Model.Production
{
    public class SpProdJobWiseRejectAdjusment
    {
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public int Quantity { get; set; }
        public int RejectQty { get; set; }
        public string SSL { get; set; }
        public int TotalDelivery { get; set; }
    }
}
