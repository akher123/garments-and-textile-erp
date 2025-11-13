using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
    public partial class VwTargetProduction
    {

        public string CompId { get; set; }
        public long TargetProductionId { get; set; }
        public string TargetProductionRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public int TotalTargetQty { get; set; }
        public Nullable<int> AcheivedQty { get; set; }
        public int LineId { get; set; }
        public string Line { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Remarks { get; set; }

    }
}