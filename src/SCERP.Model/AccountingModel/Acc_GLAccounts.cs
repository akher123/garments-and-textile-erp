using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class Acc_GLAccounts : BaseModel
    {
        public Acc_GLAccounts()
        {
            this.Acc_VoucherDetail = new HashSet<Acc_VoucherDetail>();
          
        }

        public int Id { get; set; }
        public decimal ControlCode { get; set; }
        public decimal AccountCode { get; set; }
        public string AccountName { get; set; }
        public string BalanceType { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public string AccountType { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<Acc_VoucherDetail> Acc_VoucherDetail { get; set; }  
    }
}
