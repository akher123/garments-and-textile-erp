using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VOM_BuyOrdStyle
    {
        public string OrderStyleRefId { get; set; }
        public string StyleName { get; set; }
        public string RefNo { get; set; }
        public string BuyerName { get; set; }
        public long OrderStyleId { get; set; }
        public string CompId { get; set; }
        public string OrderNo { get; set; }
  
        public string BuyerRefId { get; set; }
     
        public string MerchandiserId { get; set; }
        public string Merchandiser { get; set; }
        public string BuyerRef { get; set; }
    
        public string StyleRefId { get; set; }
     
        public bool ActiveStatus { get; set; }
        public string Closed { get; set; }
        public string BuyerArt { get; set; }
        public string TnaMode { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public Nullable<DateTime> EFD { get; set; }

    }
}
