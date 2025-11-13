using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.Planning
{
    public partial class PLAN_ProcessTemplate : ProSearchModel<PLAN_ProcessTemplate>
    {
        public int Id { get; set; }
        public string StylerefId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public Nullable<int> LeadTime { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PlannedStartDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PlannedEndDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ActualStartDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ActrualEndDate { get; set; }

        public Nullable<System.Guid> ResponsiblePerson { get; set; }
        public Nullable<int> NotifyBeforeDays { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual PLAN_Process PLAN_Process { get; set; }
        public virtual PLAN_ResponsiblePerson PLAN_ResponsiblePerson { get; set; }
    }
}
