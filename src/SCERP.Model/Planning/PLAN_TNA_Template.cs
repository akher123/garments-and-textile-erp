using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.Planning
{
    public class PLAN_TNA_Template 
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public Nullable<int> FromLeadTime { get; set; }
        public Nullable<int> ToLeadTime { get; set; }      
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
