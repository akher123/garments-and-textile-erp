using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeDailyAttendanceModel : EmployeeDailyAttendanceViewModel
    {
        public EmployeeDailyAttendanceModel()
        {
            EmployeeDailyAttendanceViewModel=new List<EmployeeDailyAttendanceViewModel>();
        }
        public List<EmployeeDailyAttendanceViewModel> EmployeeDailyAttendanceViewModel { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        public string EmployeeCardId { get; set; }

    }
}