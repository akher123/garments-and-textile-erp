using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class SPOrderStyleDetailForBOM
    {
        public DateTime? ShipDate { get; set; }
        public string BuyerName { get; set; }
        public string RefNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string EmpName { get; set; }
        public string Fab { get; set; }
        public int? OrderQty { get; set; }
        public string OTypeName { get; set; }
        public string Article { get; set; }
        public string ItemName { get; set; }
        public string StyleName { get; set; }
        public decimal? StyleQty { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int? Quantity { get; set; }

    }
}
