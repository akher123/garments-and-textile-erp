using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
   public class EmployeeSalary_Processed_Temp:SearchModel<EmployeeShortLeave>
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseRent { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal Conveyance { get; set; }      
        public decimal? FoodAllowance { get; set; }
        public decimal? EntertainmentAllowance { get; set; }
        public decimal? LWP { get; set; }
        public decimal? Absent { get; set; }
        public decimal? Advance { get; set; }
        public decimal? Stamp { get; set; }
        public decimal? TotalDeduction { get; set; }
        public decimal? AttendanceBonus { get; set; }
        public decimal? ShiftingAllowance { get; set; }
        public decimal? PayableAmount { get; set; }
        public double? OTHours { get; set; }
        public decimal? OTRate { get; set; }
        public decimal? OTAmount { get; set; }
        public decimal? NetSalaryPaid { get; set; }
        public decimal? Tax { get; set; }
        public decimal? ProvidentFund { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
