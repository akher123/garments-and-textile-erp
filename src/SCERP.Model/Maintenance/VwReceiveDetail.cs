using System;
using System.Collections.Generic;

namespace SCERP.Model.Maintenance
{
   
    
    public partial class VwReceiveDetail
    {

        public long ReturnableChallanReceiveId { get; set; }
        public long ReturnableChallanDetailId { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public int ReceiveQty { get; set; }
        public string CompId { get; set; }
        public Nullable<double> RejectQty { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<double> Amount { get; set; }
        public long ReturnableChallanId { get; set; }
        public string ItemName { get; set; }
    }
}
