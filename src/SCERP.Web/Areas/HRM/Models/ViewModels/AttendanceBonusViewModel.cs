using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;
using System;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class AttendanceBonusViewModel : AttendanceBonus
    {
        public AttendanceBonusViewModel()
        {
            EmployeeTypes = new List<EmployeeType>();
            EmployeeGrades = new List<EmployeeGrade>();
            EmployeeDesignations = new List<EmployeeDesignation>();
            AttendanceBonuses = new List<AttendanceBonus>();
            SearchFieldModel = new SearchFieldModel();
            IsSearch = true;
        }

        public SearchFieldModel SearchFieldModel { get; set; }
        public List<AttendanceBonus> AttendanceBonuses { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int SerchCompanyId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int SerchBranchId { get; set; }

       
        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeSelectListItems
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        public List<EmployeeGrade> EmployeeGrades { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeGradeSelectListItem
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }
        }

        public List<EmployeeDesignation> EmployeeDesignations { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeDesignationSelectListItem
        {
            get { return new SelectList(EmployeeDesignations, "Id", "Title"); }
        }      
    }
}
