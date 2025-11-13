using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public partial class Acc_VoucherToCostcentre
    {
        public int Id { get; set; }
        public long VoucherNo { get; set; }
        public string VoucherRefNo { get; set; }
        public int CostCentreId { get; set; }
        public decimal AccountCode { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
