using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Model.TrackingModel;

namespace SCERP.Model.TaskManagementModel
{
    
    public partial class TmAssignee:TmSearchModel<TmAssignee>
    {
        public int AssigneeId { get; set; }
        [Required]
        public string Assignee { get; set; }
        public string CompId { get; set; }
    }
}
