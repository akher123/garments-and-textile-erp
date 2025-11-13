using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VBuyOrdShipDetail
    {

        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string OrderShipRefId { get; set; }
        public long OrderShipDetailId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> PAllow { get; set; }
        public Nullable<int> QuantityP { get; set; }
        public Nullable<int> ShQty { get; set; }
        public Nullable<int> ColorRow { get; set; }
        public Nullable<int> SizeRow { get; set; }
    }
}
