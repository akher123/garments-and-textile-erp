using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class PaySlipView
    {
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string JoiningDate { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string TotalDays { get; set; }
        public string WorkingDays { get; set; }
        public string WeekendDays { get; set; }
        public string HolidayDays { get; set; }
        public string PresentDays { get; set; }
        public string AbsentDays { get; set; }
        public string LateDays { get; set; }
        public string LeaveDays { get; set; }
        public string BasicSalary { get; set; }
        public string HouseRent { get; set; }
        public string MedicalAllowance { get; set; }
        public string Conveyance { get; set; }
        public string FoodAllowance { get; set; }
        public string EntertainmentAllowance { get; set; }
        public string GrossSalary { get; set; }
        public string AttendanceBonus { get; set; }
        public string ShiftingBonus { get; set; }
        public string TotalBonus { get; set; }
        public string OTRate { get; set; }
        public string OTHours { get; set; }
        public string TotalOTAmount { get; set; }
        public string TotalPaid { get; set; }
        public string Stamp { get; set; }
        public string AbsentFee { get; set; }
        public string Advance { get; set; }
        public string TotalDeduction { get; set; }
        public string NetAmount { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Rate { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public int SerialNo { get; set; }
        public string SerialId { get; set; }
    }
}
