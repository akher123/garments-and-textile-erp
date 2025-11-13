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
    public class EmployeeInfoCustomModel :EmployeeCompanyInfoCustomModel 
    {
        public EmployeeInfoCustomModel()
        {
            Employee = new Employee();
            EmployeePresentAddress = new EmployeePresentAddress();
            EmployeePermanentAddress = new EmployeePermanentAddress();
            Employees=new List<EmployeeInfoCustomModel>();
            EmployeePresentAddress = new EmployeePresentAddress();
            EmployeePermanentAddress=new EmployeePermanentAddress();
            IsSearch = true;
        }

        public int SerialNo { get; set; }
        public override sealed Employee Employee { get; set; }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }

        public EmployeePermanentAddress EmployeePermanentAddress { get; set; }

        public List<EmployeeInfoCustomModel> Employees { get; set; }


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


        public new Guid EmployeeId { get; set; }
    }
}