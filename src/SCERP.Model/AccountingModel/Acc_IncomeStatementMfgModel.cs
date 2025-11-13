using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class Acc_IncomeStatementMfgModel
    {
        public int Id { get; set; }
        public string g1 { get; set; }
        public string g2 { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<int> CompanySectorId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanySector { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
