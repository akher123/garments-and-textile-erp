using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TrackingModel
{


    public partial class TrackVehicleGateEntry : TrackingSearchModel<TrackVehicleGateEntry>
    {
        public long VehicleGateEntryId { get; set; }
        public string CompanyId { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public int ConfirmationMediaId { get; set; }
        [Required]
        public int GateEntryNumber { get; set; }
        public int VehicleId { get; set; }
        [Required]
        public string VehicleNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string FromWhere { get; set; }
        [Required]
        public string ConfirmedBy { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<System.DateTime> ExitDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public int CheckOutStatus { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    
        public virtual TrackConfirmationMedia TrackConfirmationMedia { get; set; }
        public virtual TrackVehicle TrackVehicle { get; set; }
    }
}
