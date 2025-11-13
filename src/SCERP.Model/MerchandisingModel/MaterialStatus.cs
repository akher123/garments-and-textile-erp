using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class MaterialStatus
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ItemCode { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public decimal? ReqQty { get; set; }
        public decimal? PiQty { get; set; }
        public int RecvdQty { get; set; }
        public int StockQty { get; set; }
        public int RemainingQty { get; set; }
        public int PiBalanceQty { get; set; }
        public int PracticalQty { get; set; }
        public string SupplierName { get; set; }
    }
}
