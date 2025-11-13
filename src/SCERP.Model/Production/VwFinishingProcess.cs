using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class VwFinishingProcess
    {
        public long FinishingProcessId { get; set; }
        public string FinishingProcessRefId { get; set; }
        public string CompId { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public int HourId { get; set; }
        public Nullable<System.Guid> PreparedBy { get; set; }
        public int FType { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public string Remarks { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string HourName { get; set; }

        public string OrderShipRefId { get; set; }
        public Nullable<int> InputQuantity { get; set; }
    }
}
