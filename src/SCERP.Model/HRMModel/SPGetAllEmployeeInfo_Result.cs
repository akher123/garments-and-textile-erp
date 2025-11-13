namespace SCERP.Model
{
    using System;

    public partial class SPGetAllEmployeeInfo_Result
    {
        public Nullable<int> RowID { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public string EmployeeCardNo { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public Nullable<int> EmployeeStatus { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string Designation { get; set; }
        //public string BloodGroup { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }

    }
}
