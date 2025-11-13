using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public partial class Acc_VoucherDetail : BaseModel
    {
        public long Id { get; set; }
        public Nullable<long> RefId { get; set; }
        public Nullable<int> GLID { get; set; }
        public string Particulars { get; set; }

        [DefaultValue(0)]
        public Nullable<decimal> Debit { get; set; }

        [DefaultValue(0)]
        public Nullable<decimal> Credit { get; set; }

        public decimal FirstCurValue { get; set; }
        public decimal SecendCurValue { get; set; }
        public decimal ThirdCurValue { get; set; }
        public string CostCentreId { get; set; }
        public string IntRefId { get; set; }
        public int? IntType { get; set; }
        public virtual Acc_GLAccounts Acc_GLAccounts { get; set; }
        public virtual Acc_VoucherMaster Acc_VoucherMaster { get; set; }
    }
}
