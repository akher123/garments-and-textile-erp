using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TaskManagementModel
{
   
    public partial class TmTaskType:TmSearchModel<TmTaskType>
    {
        public int TaskTypeId { get; set; }
        [Required]
        public string TaskType { get; set; }
        public string CompId { get; set; }
    }
}
