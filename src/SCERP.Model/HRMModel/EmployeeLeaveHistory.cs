namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeLeaveHistory
    {
        public int EmployeeLeaveHistoryId { get; set; }
        public int BranchUnitId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public int Year { get; set; }
        public int LeaveTypeId { get; set; }
        public int NoOfAllowableLeaveDays { get; set; }
        public Nullable<int> NoOfConsumedLeaveDays { get; set; }
        public Nullable<int> NoOfRemainingLeaveDays { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
