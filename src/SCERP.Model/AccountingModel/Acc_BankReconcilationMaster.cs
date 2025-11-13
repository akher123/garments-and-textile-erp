using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class Acc_BankReconcilationMaster
    {
        public Acc_BankReconcilationMaster()
        {
            this.Acc_BankReconciliationDetail = new HashSet<Acc_BankReconciliationDetail>();
            this.Acc_BankVoucherManual = new HashSet<Acc_BankVoucherManual>();
        }

        public int Id { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> FinancialPeriodId { get; set; }
        public Nullable<int> GLID { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }

        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual Acc_CompanySector Acc_CompanySector { get; set; }
        public virtual Acc_FinancialPeriod Acc_FinancialPeriod { get; set; }
        public virtual ICollection<Acc_BankReconciliationDetail> Acc_BankReconciliationDetail { get; set; }
        public virtual ICollection<Acc_BankVoucherManual> Acc_BankVoucherManual { get; set; }
    }
}
