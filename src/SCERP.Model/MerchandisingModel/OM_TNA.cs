using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
   public class OM_TNA
    {
        public int TnaRowId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ActivityName { get; set; }
        public Nullable<int> SerialId { get; set; }
        public Nullable<int> LeadTime { get; set; }
        public string PSDate { get; set; }
        public string PEDate { get; set; }
        public string Rmks { get; set; }
        public string XWho { get; set; }
        public string XWhen { get; set; }
 
        public string FlagValue { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public string ASDate { get; set; }
        public string AEDate { get; set; }
        public string Responsible { get; set; }
        public string UpdateRemarks { get; set; }
        public string ShortName { get; set; }

        public double? RequiredQty { get; set; }
        public double? ActualQty { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    }
}
