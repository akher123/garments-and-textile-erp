using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwFinishingProcessDetail
    {
        public int TtlOrderQty { get; set; }
        public int TinQuantity { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public int InputQuantity { get; set; }
        public string CompId { get; set; }
        public string SizeName { get; set; }
        public int TtlSwInputQty { get; set; }
        public int TtlSwOutQty { get; set; }
        public int SizeRow { get; set; }
        public int FType { get; set; }
        public int TotalCuttQty { get; set; }
                                           //  public string OrderShipRefId { get; set; }

    }
}
