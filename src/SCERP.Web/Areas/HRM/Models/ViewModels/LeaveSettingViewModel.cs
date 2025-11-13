using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public sealed class LeaveSettingViewModel : LeaveSetting
    {

        public LeaveSettingViewModel()
        {
            Companies = new List<Company>();
            BranchUnits = new List<Object>();
            EmployeeTypes = new List<EmployeeType>();
            LeaveTypes = new List<LeaveType>();
            LeaveSettings = new List<LeaveSetting>();
            IsSearch = true;
            SearchFieldModel = new SearchFieldModel();
        }
          
        public List<LeaveSetting> LeaveSettings { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }
       

        [Required(ErrorMessage = @"Required!")]
        public int CompanyId { get; set; }

       
        public List<Company> Companies { get; set; }
        public List<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name").ToList(); }
        }

        public IEnumerable BranchUnits { get; set; }
        public List<SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName").ToList(); }
        }

        public List<EmployeeType> EmployeeTypes { get; set; }
        public List<SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title").ToList(); }
        }

        public List<LeaveType> LeaveTypes { get; set; }
        public List<SelectListItem> LeaveTypeSelectListItem
        {
            get { return new SelectList(LeaveTypes, "Id", "Title").ToList(); }
        }

    }
}