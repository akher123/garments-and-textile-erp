using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TrackingModel
{


    public partial class TrackConfirmationMedia : TrackingSearchModel<TrackConfirmationMedia>
    {
        public TrackConfirmationMedia()
        {
            this.TrackVehicleGateEntry = new HashSet<TrackVehicleGateEntry>();
            this.TrackVisitorGateEntry = new HashSet<TrackVisitorGateEntry>();
        }
    
        public int ConfirmationMediaId { get; set; }
        [Required]
        public string ConfirmationMedia { get; set; }
        public string CompanyId { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<TrackVehicleGateEntry> TrackVehicleGateEntry  { get; set; }
        public virtual ICollection<TrackVisitorGateEntry> TrackVisitorGateEntry { get; set; }
    }
}
