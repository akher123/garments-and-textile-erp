using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SalarySetupViewModel : SalarySetup
    {
        public SalarySetupViewModel()
        {
            SalarySetup=new List<SalarySetup>();
            EmployeeTypes=new List<EmployeeType>();
            EmployeeGrades=new List<EmployeeGrade>();
            EmployeeGrade=new EmployeeGrade();
            IsSearch = true;
        }
        public List<SalarySetup> SalarySetup { get; set; }

        public List<EmployeeType> EmployeeTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }

        }
        public virtual int SearchEmployeeTypeId { get; set; }
        public virtual int SearchEmployeeGradeId { get; set; }

        public List<EmployeeGrade> EmployeeGrades { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeGradeSelectListItem
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }
        }
        public decimal TotalSalary()
        {
            return BasicSalary + GetValue(HouseRent) + GetValue(MedicalAllowance) +GetValue(Conveyance) + GetValue(FoodAllowance) + GetValue(EntertainmentAllowance);
        }
        public decimal GetValue(decimal? amount)
        {
            
            return amount.HasValue ? amount.GetValueOrDefault() : 0;
        }

        public bool Is_GrossSalary_Equal_To_TotalSalary()
        {
            return GrossSalary==TotalSalary();
        }
    }
}