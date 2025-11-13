using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class TaskTypeViewModel:TmTaskType
    {
        public TaskTypeViewModel()
        {
            TaskTypes=new List<TmTaskType>();
        }
        public List<TmTaskType> TaskTypes { get; set; }
    }
}