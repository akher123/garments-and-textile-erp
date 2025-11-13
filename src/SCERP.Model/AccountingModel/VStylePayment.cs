using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public partial class VStylePayment
    {
        public int StylePaymnetId { get; set; }
        public string StylePaymentRefId { get; set; }
        public string PayDate { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string CostGroup { get; set; }
        public string Remarks { get; set; }
        public string GroupName { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public double PayAount { get; set; }
    }
}
