using System;
using System.Collections.Generic;

namespace SCERP.Model.TaskManagementModel
{
    
    
    public partial class vwTmTaskInformation
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public Nullable<DateTime> AssignDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string Assignee { get; set; }
        public string TaskStatus { get; set; }
        public int AssigneeId { get; set; }
        public int TaskStatusId { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskType { get; set; }
    }
}
