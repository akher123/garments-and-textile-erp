using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TrackingModel
{


    public partial class TrackVisitorGateEntry : TrackingSearchModel<TrackVisitorGateEntry>
    {
        public long VisitorGateEntryId { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public string CompanyId { get; set; }
        public int ConfirmationMediaId { get; set; }
        [Required]
        public string VisitorName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Purpose { get; set; }
        [Required]
        public string ConfirmedBy { get; set; }
        [Required]
        public string VisitorCardId { get; set; }
        public int CheckOutStatus { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string CheckInTime { get; set; } 
        public string CheckOutTime { get; set; }
        public Nullable<System.DateTime> ExitDate { get; set; }
        public string Remarks { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual TrackConfirmationMedia TrackConfirmationMedia { get; set; }
    }
}
