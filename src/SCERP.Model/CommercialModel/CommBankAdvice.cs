using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.CommercialModel
{
    public partial class CommBankAdvice : ProSearchModel<CommBankAdvice>
    {
        public int BankAdviceId { get; set; }
        public Nullable<long> ExportId { get; set; }
        public Nullable<int> AccHeadId { get; set; }
        public string AccHeadName { get; set; }
        public string HeadType { get; set; }
        public Nullable<int> Currency { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> AmountInTaka { get; set; }
        public string Particulars { get; set; }
        public string BankRefNo { get; set; }
        public string CompId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual CommAccHead CommAccHead { get; set; }
    }
}
