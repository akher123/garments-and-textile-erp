using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.MisModel;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class UserActivityViewModel
    {
        public List<MIS_UserActivity> Activities { get; set; }
        public Dictionary<string, List<MIS_UserActivity>> Dictionary { get;  private set; }

        public UserActivityViewModel(List<MIS_UserActivity> activities)
        {
            Activities = activities;
            Dictionary=new Dictionary<string, List<MIS_UserActivity>>();
            GetUserActivityLis();
        }


        private  void GetUserActivityLis()
        {
            List<string> moduleList = Activities.Select(x => x.ModuleName).Distinct().ToList();
            foreach (string modeule in moduleList)
            {
                List<MIS_UserActivity> activities = Activities.Where(x => x.ModuleName == modeule).ToList();
                Dictionary.Add(modeule, activities);
            }

          
        }
    }
}