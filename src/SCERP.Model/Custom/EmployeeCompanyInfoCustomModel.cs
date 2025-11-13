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
    public class EmployeeCompanyInfoCustomModel : EmployeeCompanyInfo
    {
        public EmployeeCompanyInfoCustomModel()
        {
            EmployeeCompanyInfos=new List<EmployeeCompanyInfo>();
            EmployeeCompanyInfo = new EmployeeCompanyInfo();
            DepartmentSection = new DepartmentSection();
            DepartmentLine = new DepartmentLine();
            BranchUnitDepartment = new BranchUnitDepartment();
            UnitDepartment = new UnitDepartment();
            Department = new Department();
            BranchUnit = new BranchUnit();
            Unit = new Unit();
            Branch = new Branch();
            Company = new Company();
            EmployeeDesignation = new EmployeeDesignation();
            EmployeeGrade = new EmployeeGrade();
            EmployeeType = new EmployeeType();
            SkillSet = new SkillSet();
            SearchFieldModel =new SearchFieldModel();
        }

        public SearchFieldModel SearchFieldModel { get; set; }
        public List<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }

        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public BranchUnitDepartment BranchUnitDepartment { get; set; }
        public UnitDepartment UnitDepartment { get; set; }
        public Department Department { get; set; }
        public BranchUnit BranchUnit { get; set; }
        public Unit Unit { get; set; }
        public Branch Branch { get; set; }
        public Company Company { get; set; }

        public DepartmentSection DepartmentSection { get; set; }

        public DepartmentLine DepartmentLine { get; set; }

        public override EmployeeDesignation EmployeeDesignation { get; set; }
        public EmployeeGrade EmployeeGrade { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public SkillSet SkillSet { get; set; }

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

        public List<BloodGroup> EmployeeBloodGroups { get; set; }
        public List<SelectListItem> EmployeeBloodGroupSelectListItem
        {
            get { return new SelectList(EmployeeBloodGroups, "Id", "GroupName").ToList(); }

        }

        public List<SkillSet> SkillSets { get; set; }
        public List<SelectListItem> SkillSetSelectListItem
        {
            get { return new SelectList(SkillSets, "Id", "Title").ToList(); }

        }


        [Required(ErrorMessage = @"Required!")]
        public new int EmployeeCompanyInfoId { get; set; }

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
        public int EmployeeTypeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeGradeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int EmployeeDesignationId { get; set; }

        public int? JobTypeId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        [DataType(DataType.Date)]
        public new System.DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public new System.DateTime? ToDate { get; set; }
    }
}