using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TrackingModel
{


    public partial class TrackVehicle : TrackingSearchModel<TrackVehicle>
    {
        public TrackVehicle()
        {
            this.TrackVehicleGateEntry = new HashSet<TrackVehicleGateEntry>();
        }
    
        public int VehicleId { get; set; }
        public string CompanyId { get; set; }
        [Required]
        public string VehicheType { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<TrackVehicleGateEntry> TrackVehicleGateEntry { get; set; }
    }
}
