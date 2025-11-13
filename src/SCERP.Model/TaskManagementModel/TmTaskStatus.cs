using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SCERP.Model.TaskManagementModel
{

    public partial class TmTaskStatus : TmSearchModel<TmTaskStatus>
    {
        public int TaskStatusId { get; set; }
        [Required]
        public string TaskStatus { get; set; }
        public string CompId { get; set; }
    } 
}
