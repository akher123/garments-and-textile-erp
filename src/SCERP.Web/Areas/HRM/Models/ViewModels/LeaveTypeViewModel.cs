using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class LeaveTypeViewModel:LeaveType
    {
        public LeaveTypeViewModel()
        {
            LeaveTypes=new List<LeaveType>();
            IsSearch = true;

        }
        public string SearchKey { get; set; }
        public List<LeaveType> LeaveTypes { get; set; }
    }
}