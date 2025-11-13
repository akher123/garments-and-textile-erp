using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;

namespace SCERP.Model.Custom
{
    public class EmployeeMandatoryInfoCustomModel :EmployeeCompanyInfoCustomModel 
    {
        public EmployeeMandatoryInfoCustomModel()
        {
            Employee = new Employee();
            EmployeePresentAddress = new EmployeePresentAddress();
        }


        public override sealed Employee Employee { get; set; }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }

        public List<Employee> Employees { get; set; }

      
        public List<Gender> Genders { get; set; }
        public List<SelectListItem> GenderSelectListItem
        {
            get { return new SelectList(Genders, "GenderId", "Title").ToList(); }

        }

        public List<Religion> Religions { get; set; }
        public List<SelectListItem> ReligionSelectListItem
        {
            get { return new SelectList(Religions, "ReligionId", "Name").ToList(); }

        }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeCompanyId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeBranchId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeBranchUnitId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeBranchUnitDepartmentId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeTypeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeGradeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeDesignationId { get; set; }
    }
}