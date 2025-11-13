using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeFollowUpViewModel
    { 
        public EmployeeFollowUpViewModel()
        {
            EmployeeSalaries = new List<EmployeeSalary>();
            EmployeeCompanyInfos = new List<EmployeeCompanyInfo>();
            leaveDatas = new List<EmployeeLeaveHistoryIndividual>();
            salaries = new List<EmployeeSalaryIndividual>();
            attendances = new List<EmployeeAttendanceIndividual>();
            penalties = new List<EmployeePenaltyIndividual>();            
        }
        public List<EmployeeSalary> EmployeeSalaries { get; set; }
        public List<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }
        public List<EmployeeLeaveHistoryIndividual> leaveDatas { get; set; }          
        public List<EmployeeSalaryIndividual> salaries { get; set; }
        public List<EmployeeAttendanceIndividual> attendances { get; set; }
        public List<EmployeePenaltyIndividual> penalties { get; set; }
        public EmployeeBasicInfo employeeBasicInfo { get; set; }
        public string EmployeeCardId { get; set; }
        public DateTime? Date { get; set; }
    }
}