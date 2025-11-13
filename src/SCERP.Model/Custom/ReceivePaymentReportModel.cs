using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class ReceivePaymentReportModel
    {
        public int OrderId { get; set; }
        public string BalanceType { get; set; }
        public string ControlHead { get; set; }
        public string AccountHead { get; set; }
        public decimal? Balance { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
