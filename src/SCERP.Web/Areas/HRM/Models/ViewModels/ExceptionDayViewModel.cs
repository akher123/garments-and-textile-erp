using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ExceptionDayViewModel : ExceptionDay
    {
        public List<ExceptionDay> ExceptionDays { get; set; }
        public ExceptionDay ExceptionDay { get; set; }
        public Company Company { get; set; }
        public List<Company> Companies { get; set; }
        public List<Branch> Branches { get; set; }
        public IEnumerable BranchUnits { get; set; }
       
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
      
        public ExceptionDayViewModel()
        {
           
            ExceptionDays=new List<ExceptionDay>();
            ExceptionDay=new ExceptionDay();
            Company=new Company();
            Branches=new List<Branch>();
            BranchUnits=new List<BranchUnit>();
            
        }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name"); }
        }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "Id", "Name"); }

        }
        public IEnumerable<SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName").ToList(); }
        }
     
    }
}