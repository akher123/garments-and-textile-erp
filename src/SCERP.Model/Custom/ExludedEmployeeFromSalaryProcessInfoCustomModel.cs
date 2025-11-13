using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;

namespace SCERP.Model.Custom
{
    public class ExludedEmployeeFromSalaryProcessInfoCustomModel : PayrollExcludedEmployeeFromSalaryProcess
    {
        public ExludedEmployeeFromSalaryProcessInfoCustomModel()
        {

            SearchFieldModel = new SearchFieldModel();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentSections = new List<DepartmentSection>();
            DepartmentLines = new List<DepartmentLine>();
            EmployeeTypes = new List<EmployeeType>();
            ExcludedEmployeeFromSalaryProcessRecords = new List<PayrollExcludedEmployeeFromSalaryProcess>();
            EmployeeIdList = new List<Guid>();
            IsSearch = true;
            EmployeeCompanyInformation = new VEmployeeCompanyInfoDetail();
            EmployeesForExcludingFromSalryProcess = new List<ExludedEmployeeFromSalaryProcessInfoCustomModel>();
        }

        public VEmployeeCompanyInfoDetail EmployeeCompanyInformation { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public List<PayrollExcludedEmployeeFromSalaryProcess> ExcludedEmployeeFromSalaryProcessRecords { get; set; }

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

        public IEnumerable<SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable BranchUnitDepartments { get; set; }

        public IEnumerable<SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }

        public List<DepartmentLine> DepartmentLines { get; set; }

        public IEnumerable<SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public List<DepartmentSection> DepartmentSections { get; set; }

        public IEnumerable<SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeTypes { get; set; }

        public IEnumerable<SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title").ToList(); }

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

        public IList<ExludedEmployeeFromSalaryProcessInfoCustomModel> EmployeesForExcludingFromSalryProcess { get; set; }
        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeesForExcludingFromSalryProcess.Count > 0 ? "" : "none");
            }
        }


        public string Name { get; set; }

        public string Company { get; set; }

        public string Branch { get; set; }

        public string Unit { get; set; }

        public string Department { get; set; }

        public string Section { get; set; }

        public string Line { get; set; }

        public string EmployeeType { get; set; }

        public string Designation { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? QuitDate { get; set; }

    }
}
