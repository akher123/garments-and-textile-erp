using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class AssigneeViewModel:TmAssignee
    {
        public AssigneeViewModel()
        {
            Assignees=new List<TmAssignee>();
        }
        public List<TmAssignee> Assignees { get; set; } 
    }
}