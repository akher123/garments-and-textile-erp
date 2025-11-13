using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class WorkingDayViewModel : ProSearchModel<WorkingDayViewModel>
    {
       
        public int CureentYar{ get; set; }
        public bool HolidayStatus { get; set; }
        public List<int> Yars { get; set; }
        [DisplayName(@"Status")]
        public int WorkingDayStatus { get; set; }
        public List<PLAN_WorkingDay> WorkingDays { get; set; }
        public PLAN_WorkingDay WorkingDay { get; set; }
        public WorkingDayViewModel()
        {
           
            WorkingDay=new PLAN_WorkingDay();
            WorkingDays=new List<PLAN_WorkingDay>();
            Yars=new List<int>();
        }
        public IEnumerable<SelectListItem> YarSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Yars);
            }
        }
        public IEnumerable<SelectListItem>WorkingDayStatusSelectListItem
        {
            get
            {
                return Enum.GetValues(typeof(WorkingDayStatus)).Cast<WorkingDayStatus>().Select(x=>new SelectListItem(){Text = x.ToString(),Value =Convert.ToString((int) x)}) ;
            }
        }
    


    }
}