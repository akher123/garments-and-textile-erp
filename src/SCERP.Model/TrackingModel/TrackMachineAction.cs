using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TrackingModel
{

    public partial class TrackMachineAction : TrackingSearchModel<TrackMachineAction>
    {
        public TrackMachineAction()
        {
            this.TrackMachineLog = new HashSet<TrackMachineLog>();
        }
    
        public int MachineActionId { get; set; }
        [Required]
        public string MachineActionName { get; set; }
        public string CompanyId { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<TrackMachineLog> TrackMachineLog { get; set; } 
    }
}
