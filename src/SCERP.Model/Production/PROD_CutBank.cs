using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_CutBank
    {
        public long CutBankId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string ComponentRefId { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<int> CutFQty { get; set; }
        public Nullable<int> BankQty { get; set; }
        public Nullable<int> BalanceQty { get; set; }
        public string CompId { get; set; }
        public string ComponentType { get; set; }
        public Nullable<int> SolidQty { get; set; }
        public Nullable<int> PrintRcvQty { get; set; }
        public Nullable<int> EmbRcvQty { get; set; }
        public Nullable<int> PrintRejQty { get; set; }
        public Nullable<int> EmbRejQty { get; set; }
        public Nullable<int> FabricRejQty { get; set; }
    }
}
