using System;
using SCERP.Common;

namespace SCERP.Model.Production
{


    public partial class VwCuttingBatch
    {
        public string BuyerRefId { get; set; }
        public string CompId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public Nullable<System.DateTime> CuttingDate { get; set; }
        public string CuttingStatus { get; set; }
        public string FIT { get; set; }
        public string JobNo { get; set; }
        public string OrderNo { get; set; }
        public string Rmks { get; set; }
        public string StyleRefId { get; set; }
        public string BuyerName { get; set; }
        public string StyleName { get; set; }
        public int MarkerPcs { get; set; }
        public int PLY { get; set; }
        public int BundleQuantity { get; set; }
        public int Total { get; set; }
        public int RejectQty { get; set; }
        public string OrderStyleRefId { get; set; }
        public string OrderName { get; set; }
        public string ColorRefId { get; set; }
        public string ColorName { get; set; }
        public string ComponentRefId { get; set; }
        public string ComponentName { get; set; }
        public string TableName { get; set; }
        public int? MachineId { get; set; }
        public int FinalQty { get { return Total - RejectQty; } }

    }
}
