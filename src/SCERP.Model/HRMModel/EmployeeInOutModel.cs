namespace SCERP.Model
{
    using System;
   
    public partial class EmployeeInOutModel : SearchModel<EmployeeInOutModel>
    {      
        public Nullable<long> RowNumber { get; set; }
        public int TotalRows { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TransactionDate { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDesignation { get; set; }
        public string MobileNo { get; set; }
        public string JoiningDate { get; set; }
        public string WorkShiftName { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string LateInMinutes { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public string Remarks { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
