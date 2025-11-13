using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;
using System.Collections;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ApprovedOvertimeHoureViewModel
    {
        public ApprovedOvertimeHoureViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            LineOvertimeHours = new List<LineOvertimeHour>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
        }
        public SearchFieldModel SearchFieldModel { get; set; }
        public List<LineOvertimeHour> LineOvertimeHours { get; set; }
        [Required]
        public DateTime? OtDate { get; set; }
        public string TransactionDate { get; set; }
        public DataTable DataTable { get; set; }
        public int DepartmentLineId { get; set; }

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

        public bool All { get; set; }
        public bool Garments { get; set; }
        public bool Knitting { get; set; }
        public bool Dyeing { get; set; }

    }
}