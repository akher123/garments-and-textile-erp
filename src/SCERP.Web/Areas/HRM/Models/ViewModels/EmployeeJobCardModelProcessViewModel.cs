using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeJobCardModelProcessViewModel :EmployeeJobCardModel
    {

        public EmployeeJobCardModelProcessViewModel()
        {
            SearchFieldModel = new SearchFieldModel();        
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentSections = new List<DepartmentSection>();
            DepartmentLines = new List<DepartmentLine>();
            EmployeeTypes = new List<EmployeeType>();
            EmployeeJobCardModelRecords = new List<EmployeeJobCardModel>();
            EmployeesForJobCardModelProcess = new List<EmployeesForJobCardModelProcessModel>();
            EmployeeIdList = new List<Guid>();
            IsSearch = true;
        }

        public int Id { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? TransactionDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? EndDate { get; set; }


        public SearchFieldModel SearchFieldModel { get; set; }

        public List<EmployeeJobCardModel> EmployeeJobCardModelRecords { get; set; }
      
        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }

        public IEnumerable BranchUnits { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }

        public List<DepartmentLine> DepartmentLines { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title").ToList(); }

        }

        public IList<EmployeesForJobCardModelProcessModel> EmployeesForJobCardModelProcess { get; set; }
        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeesForJobCardModelProcess.Count > 0 ? "" : "none");
            }
        }

        public List<Guid> EmployeeIdList { get; set; }


        public IEnumerable<SelectListItem> Months
        {
            get
            {
                for (int index = 0; index < (DateTimeFormatInfo.InvariantInfo.MonthNames.Length - 1); index++)
                    yield return new SelectListItem
                    {
                        Value = (index + 1).ToString(),
                        Text = DateTimeFormatInfo.InvariantInfo.MonthNames[index]
                    };
            }
        }

        public IEnumerable<SelectListItem> Years
        {
            get
            {
                for (int index = 2014; index <= 2030; index++)
                    yield return new SelectListItem
                    {
                        Value = (index).ToString(),
                        Text = (index).ToString()
                    };
            }
        }
       
    }
}