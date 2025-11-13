using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommBank : ProSearchModel<COMMLcInfo>
    {
        public CommBank()
        {

        }

        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankType { get; set; }
        public System.Nullable<System.DateTime> CreatedDate { get; set; }
        public System.Nullable<System.Guid> CreatedBy { get; set; }
        public System.Nullable<System.DateTime> EditedDate { get; set; }
        public System.Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
