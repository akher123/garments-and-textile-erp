using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeWorkGroupViewModel : EmployeeWorkGroup
    {
        public EmployeeWorkGroupViewModel()
        {
            EmployeeCompanyInfos = new List<VEmployeeCompanyInfoDetail>();
            EmployeeWorkGroups = new List<EmployeeWorkGroup>();
            VEmployeeWorkGroupDetails=new List<VEmployeeWorkGroupDetail>();
            SearchFieldModel = new SearchFieldModel();
            DepartmentLines=new List<DepartmentLine>();
            DepartmentSections=new List<DepartmentSection>();
            BranchUnits = new List<object>();
            WorkGroups = new List<WorkGroup>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            EmployeeGuidIdList=new List<Guid>();
            BranchUnitDepartments = new List<object>();
            EmployeeWorkGroupIdList=new List<int>();
           
        }

        public List<VEmployeeWorkGroupDetail> VEmployeeWorkGroupDetails { get; set; }
        public List<int> EmployeeWorkGroupIdList { get; set; }
        public List<Guid> EmployeeGuidIdList { get; set; }
        public List<WorkGroup> WorkGroups { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }
        public IList<VEmployeeCompanyInfoDetail> EmployeeCompanyInfos { get; set; }
        public List<EmployeeWorkGroup> EmployeeWorkGroups { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }

        //public List<Branch> Branches { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> BranchSelectListItem
        //{
        //    get { return new SelectList(Branches, "Id", "Name"); }
        //}

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }


        //public List<Company> Companies { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> CompanySelectListItem
        //{
        //    get { return new SelectList(Companies, "Id", "Name"); }
        //}

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable BranchUnits { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }

        }


        //public IEnumerable BranchUnitDepartments { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        //{
        //    get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }

        //}

        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName").ToList(); }

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

        public IEnumerable<System.Web.Mvc.SelectListItem> WorkGroupSelectListItem
        {
            get { return new SelectList(WorkGroups, "WorkGroupId", "Name"); }
        }
        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeeCompanyInfos.Count > 0 || VEmployeeWorkGroupDetails.Count>0 ? "" : "none");
            }
        }
        public List<EmployeeWorkGroup> GetSelectedEmployeeWorkGroups()
        {
             var   employeeWorkGroups=new List<EmployeeWorkGroup>();
             if (EmployeeGuidIdList.Count > 0)
                employeeWorkGroups = EmployeeGuidIdList.Select(employeeId => new EmployeeWorkGroup()
                {
                    EmployeeId = employeeId,
                    WorkGroupId = WorkGroupId,
                    EmployeeWorkGroupId = EmployeeWorkGroupId,
                    AssignedDate = Date.GetValueOrDefault(),
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    IsActive = true,
                    Status = true,
                    CreatedDate = DateTime.Now
                }).ToList();
            return employeeWorkGroups;
        }
        public List<EmployeeWorkGroup> GetEditedEmployeeWorkGroups()
        {
            if (EmployeeWorkGroupIdList.Count > 0)
                EmployeeWorkGroups = EmployeeWorkGroupIdList.Select(x => new EmployeeWorkGroup()
                {
                    WorkGroupId = WorkGroupId,
                    EmployeeWorkGroupId = x,
                    AssignedDate = Date.GetValueOrDefault(),
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    IsActive = true,
                    Status = true,
                    CreatedDate = DateTime.Now
                }).ToList();
            return EmployeeWorkGroups;
        }
    }
}