using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeWorkShiftViewModel : EmployeeWorkShift
    {
        public EmployeeWorkShiftViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            WorkShifts = new List<WorkShift>();
            BranchUnits = new List<object>();
            WorkGroups = new List<WorkGroup>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            DepartmentLines = new List<DepartmentLine>();
            DepartmentSections = new List<DepartmentSection>();
            BranchUnitDepartments = new List<object>();
            EmployeeWorkGroupDetails = new List<VEmployeeWorkGroupDetail>();
            EmployeeWorkShiftDetails = new List<VEmployeeWorkShiftDetail>();
            IsSearch = true;
            EmployeeGuidIdList = new List<Guid>();
            WorkShiftIdList = new List<int>();
            VEmployeeWorkShiftDetail = new VEmployeeWorkShiftDetail();
            EmployeesForWorkShift = new List<EmployeesForWorkShiftCustomModel>();
        }

        public int SearchByBranchUnitWorkShiftId { get; set; }
        public VEmployeeWorkShiftDetail VEmployeeWorkShiftDetail { get; set; }
        public List<Guid> EmployeeGuidIdList { get; set; }
        public List<int> WorkShiftIdList { get; set; } 
        public List<WorkShift> WorkShifts { get; set; }
        public List<WorkGroup> WorkGroups { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }
        public IList<VEmployeeWorkShiftDetail> EmployeeWorkShiftDetails { get; set; }
        public List<VEmployeeWorkGroupDetail> EmployeeWorkGroupDetails { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }
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
        public IEnumerable<System.Web.Mvc.SelectListItem> WorkGroupSelectListItem
        {
            get { return new SelectList(WorkGroups, "WorkGroupId", "Name"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> WorkShiftSelectListItem
        {
            get { return new SelectList(WorkShifts, "ValueMember", "DisplayMember"); }
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
        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeesForWorkShift.Count > 0 ? "" : "none");
            }
        }
        public List<EmployeeWorkShift> GetSelectedWorkShift()
        {
            var employeeWorkShiftList = (from employeeId in EmployeeGuidIdList.ToList()
                                         from date in DateTimeExtension.ToDays(StartDate, EndDate)
                                         select new EmployeeWorkShift()
                                         {
                                             EmployeeId = employeeId,
                                             Remarks = Remarks,
                                             BranchUnitWorkShiftId = BranchUnitWorkShiftId,
                                             ShiftDate = date,
                                             StartDate = StartDate,
                                             EndDate = EndDate,
                                             CreatedBy = PortalContext.CurrentUser.UserId,
                                             CreatedDate = DateTime.Now,
                                             IsActive = true,
                                             Status = true,
                                         }).ToList();
            return employeeWorkShiftList;
        }

        

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItems
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }
        public List<EmployeesForWorkShiftCustomModel> EmployeesForWorkShift { get; set; }

        public List<EmployeeWorkShift> WorkShiftQuickEmployee()
        {
            var workShiftQuickEmployeeList = (from employeeId in EmployeeGuidIdList

                                                select new EmployeeWorkShift()
                                                {
                                                    EmployeeId = employeeId,
                                                }).ToList();
            return workShiftQuickEmployeeList;
        }


    }

}