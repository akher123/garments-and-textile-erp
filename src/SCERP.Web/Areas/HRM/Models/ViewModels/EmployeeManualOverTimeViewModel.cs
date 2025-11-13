using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeManualOverTimeViewModel : EmployeeManualOverTime
    {
        public EmployeeManualOverTimeViewModel()
        {
            EmployeeManualOTs = new List<EmployeeManualOverTime>();
            EmployeeManualOT = new EmployeeManualOverTime();

        }

        public List<EmployeeManualOverTime> EmployeeManualOTs { get; set; }
        public EmployeeManualOverTime EmployeeManualOT { get; set; }
    }
}