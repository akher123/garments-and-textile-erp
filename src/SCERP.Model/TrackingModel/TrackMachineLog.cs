using System;
using System.ComponentModel.DataAnnotations;


namespace SCERP.Model.TrackingModel
{


    public partial class TrackMachineLog : TrackingSearchModel<TrackMachineLog>
    { 
        public long MachineLogId { get; set; }
        public string MachineLogRefId { get; set; }
        public int MachineId { get; set; }
        public string CompanyId { get; set; }
        public int MachineActionId { get; set; }
        public string ActionTime { get; set; }
        [Required]
        public Nullable<System.Guid> EmployeeId { get; set; }
        [Required]
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string Remarks { get; set; } 
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Production_Machine Production_Machine { get; set; }
        public virtual TrackMachineAction TrackMachineAction { get; set; }
    }
}
