using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
   
    
    public partial class VwCuttingProcessStyleActive
    {
        public long CuttingProcessStyleActiveId { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderRefNo { get; set; }
        public string StyleName { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
