using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public partial class Acc_GLAccounts_Hidden : SearchModel<Acc_GLAccounts_Hidden>
    {
        public Acc_GLAccounts_Hidden()
        {

        }
        public int Id { get; set; }
        public decimal ControlCode { get; set; }
        public decimal AccountCode { get; set; }
        public string AccountName { get; set; }
        public string BalanceType { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public string AccountType { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
