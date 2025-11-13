using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class Acc_BankVoucherManual
    {
        public int Id { get; set; }
        public Nullable<int> RefId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Particulars { get; set; }
        public string CheckNo { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public virtual Acc_BankReconcilationMaster Acc_BankReconcilationMaster { get; set; }
    }
}
