using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class WorkShiftViewModel : WorkShift
    {
        public WorkShiftViewModel()
        {
            WorkShifts = new List<WorkShift>();
            IsSearch = true;
        }
        [Required(ErrorMessage = @"Required")]
        public string InTime { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string OutTime { get; set; }

        public string SearchKey { get; set; }

        public List<WorkShift> WorkShifts { get; set; }

    }
}