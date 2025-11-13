using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeJobTypeInfoViewModel : EmployeeCompanyInfoModel
    {
        public EmployeeJobTypeInfoViewModel ()
        {
            EmployeeCompanyInfos=new List<EmployeeCompanyInfo>();
            EmployeeCompanyInfo = new EmployeeCompanyInfo();

            UnitDepartment = new UnitDepartment();
            Department = new Department();
            BranchUnit = new BranchUnit();
            Unit = new Unit();
            Branch = new Branch();
            Company = new Company();

            SearchFieldModel =new SearchFieldModel();
            EmployeeCompanyInfoModel = new EmployeeCompanyInfoModel();

            EmployeeCompanyInfoModels = new List<EmployeeCompanyInfoModel>();

            IsSearch = true;
        }

        public SearchFieldModel SearchFieldModel { get; set; }

        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public List<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }

        public EmployeeCompanyInfoModel EmployeeCompanyInfoModel { get; set; }
  
        public UnitDepartment UnitDepartment { get; set; }
        public Department Department { get; set; }
        public BranchUnit BranchUnit { get; set; }
        public Unit Unit { get; set; }
        public Branch Branch { get; set; }
        public Company Company { get; set; }

       

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
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName").ToList(); }

        }

        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName").ToList(); }

        }

        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public List<DepartmentLine> DepartmentLines { get; set; }
        
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title").ToList(); }

        }

        public List<EmployeeGrade> EmployeeGrades { get; set; }
        public List<SelectListItem> EmployeeGradeSelectListItem
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name").ToList(); }

        }

        public List<EmployeeDesignation> EmployeeDesignations { get; set; }
        public List<SelectListItem> EmployeeDesignationSelectListItem
        {
            get { return new SelectList(EmployeeDesignations, "Id", "Title").ToList(); }

        }

        public List<SkillSet> JobTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> JobTypeSelectListItem
        {
            get { return new SelectList(JobTypes, "Id", "Title"); }
        }


        [Required(ErrorMessage = @"Required!")]
        public int EmployeeCompanyId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeBranchId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeBranchUnitId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeBranchUnitDepartmentId { get; set; }

        
        public int? EmployeeDepartmentSectionId { get; set; }


        public int? EmployeeDepartmentLineId { get; set; }



        [Required(ErrorMessage = @"Required!")]
        public int EmployeeGradeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeDesignationId { get; set; }


        public List<Guid> EmployeeIdList { get; set; }

        public IList<EmployeeCompanyInfoModel> EmployeeCompanyInfoModels { get; set; }
        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeeCompanyInfoModels.Count > 0 ? "" : "none");
            }
        }



    }
}