using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommAccHead
    {
        public CommAccHead()
        {
            this.CommBankAdvice = new HashSet<CommBankAdvice>();
        }

        public int AccHeadId { get; set; }

        public Nullable<long> ExportId { get; set; }
        public string AccHeadName { get; set; }
        public string Particulars { get; set; }
        public string BankRefNo { get; set; }
        public string AccHeadType { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> AmountInTaka { get; set; }
        public string CompId { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<CommBankAdvice> CommBankAdvice { get; set; }
    }
}
