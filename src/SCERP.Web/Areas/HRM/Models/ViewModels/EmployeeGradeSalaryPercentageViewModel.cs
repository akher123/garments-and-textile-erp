using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeGradeSalaryPercentageViewModel : EmployeeGradeSalaryPercentage
    {

        public List<EmployeeGradeSalaryPercentage> EmployeeGradeSalaryPercentages { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
       [Required(ErrorMessage = @"Required!")]
        public  int EmployeeTypeId { get; set; }

        public EmployeeGradeSalaryPercentageViewModel()
        {
            SearchFieldModel=new SearchFieldModel();
            EmployeeTypes = new List<EmployeeType>();
            EmployeeGrades=new List<EmployeeGrade>();
            EmployeeGradeSalaryPercentages = new List<EmployeeGradeSalaryPercentage>();
            IsSearch = true;
        }
        [Remote("IsEmployeeTypeExist", "EmployeeGradeSalaryPercentage", ErrorMessage = @"Exist Employee Grade!!", AdditionalFields = "EmployeeGradeSalaryPercentageId")]
        public override int EmployeeGradeId { get; set; }
        public List<EmployeeType> EmployeeTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }

        }
        public List<EmployeeGrade> EmployeeGrades { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeGradeSelectListItem
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }

        }

    
    }
}