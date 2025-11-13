using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class TaskStatusViewModel:TmTaskStatus
    {
        public TaskStatusViewModel()
        {
            TaskStatusList=new List<TmTaskStatus>();
        }
        public List<TmTaskStatus> TaskStatusList { get; set; } 
    }
}