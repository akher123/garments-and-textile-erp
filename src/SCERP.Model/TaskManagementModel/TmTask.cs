using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TaskManagementModel
{
    public partial class TmTask : TmSearchModel<TmTask>
    {
        public int TaskId { get; set; }
        public string TaskNumber { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string RequirementFile { get; set; }
        public string OutPutFile { get; set; }
        public int ModuleId { get; set; }
        public int SubjectId { get; set; }
        [Required]
        public string TaskName { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStatusId { get; set; }
        public int AssigneeId { get; set; }
        public virtual TmModule TmModule { get; set; }
        public virtual TmSubject TmSubject { get; set; }
    }
}
