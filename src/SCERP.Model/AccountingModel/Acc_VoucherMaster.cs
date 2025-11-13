using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SCERP.Common;

namespace SCERP.Model
{
    public partial class Acc_VoucherMaster : SearchModel<Acc_VoucherMaster>
    {
        public Acc_VoucherMaster()
        {
            this.Acc_VoucherDetail = new HashSet<Acc_VoucherDetail>();
            this.Acc_BankReconciliationDetail = new HashSet<Acc_BankReconciliationDetail>();
        }
        public long Id { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string VoucherType { get; set; }

        public long VoucherNo { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        [Remote("IsVoucherRefExist", "VoucherEntry", AdditionalFields = "Id", ErrorMessage = @"VoucherNo Exist")]
        public string VoucherRefNo { get; set; }

        public virtual System.DateTime VoucherDate { get; set; }
        public string CheckNo { get; set; }
        public string CheckDate { get; set; }
        public string Particulars { get; set; }
        public string TotalAmountInWord { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<int> SectorId { get; set; }

        //[Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<int> CostCentreId { get; set; }
        public Nullable<int> FinancialPeriodId { get; set; }
        public Nullable<int> ActiveCurrencyId { get; set; }
        public decimal CurrencyRate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string IntRefId { get; set; }
        public int? IntType { get; set; }

        public virtual Acc_CompanySector Acc_CompanySector { get; set; }
        public virtual Acc_FinancialPeriod Acc_FinancialPeriod { get; set; }
        public virtual ICollection<Acc_VoucherDetail> Acc_VoucherDetail { get; set; }
        public virtual ICollection<Acc_BankReconciliationDetail> Acc_BankReconciliationDetail { get; set; }
    }
}
