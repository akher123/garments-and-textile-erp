using System;
using System.Web;
using System.Linq;
using SCERP.Common;
using System.Web.Mvc;
using SCERP.Model.Custom;
using System.Globalization;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeFollowUpController : BaseHrmController
    {
        private readonly IPenaltyManager _penaltyManager;
        private readonly IEmployeeSalaryManager _employeeSalaryManager;
        private readonly IEmployeeCompanyInfoManager _employeeCompanyInfoManager;
        private readonly IEmployeeLeaveManager _employeeLeaveManager;

        public EmployeeFollowUpController(IPenaltyManager penaltyManager, IEmployeeSalaryManager employeeSalaryManager, IEmployeeCompanyInfoManager employeeCompanyInfoManager, IEmployeeLeaveManager employeeLeaveManager)
        {
            _penaltyManager = penaltyManager;
            _employeeSalaryManager = employeeSalaryManager;
            _employeeCompanyInfoManager = employeeCompanyInfoManager;
            _employeeLeaveManager = EmployeeLeaveManager;
        }

        public ActionResult Index(EmployeeFollowUpViewModel model)
        {
            EmployeeFollowUpViewModel followUp = new EmployeeFollowUpViewModel();

            if (model.EmployeeCardId != null)
            {
                Employee employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);

                EmployeeCompanyInfo companyInfo = new EmployeeCompanyInfo();
                companyInfo.EmployeeCompanyInfoId = 1;
                companyInfo.EmployeeId = employee.EmployeeId;

                followUp.EmployeeSalaries = _employeeSalaryManager.GetEmployeeSalaryById(employee.EmployeeId);
                followUp.EmployeeCompanyInfos = _employeeCompanyInfoManager.GetEmployeeCompanyInfosByEmployeeGuidId(companyInfo);
                followUp.leaveDatas = _employeeLeaveManager.GetEmployeeLeaveSummaryIndividual(employee.EmployeeId, DateTime.Now);
                followUp.salaries = _employeeLeaveManager.GetEmployeeSalarySummaryIndividual(employee.EmployeeId, DateTime.Now);
                followUp.attendances = _employeeLeaveManager.GetEmployeeAttendanceIndividual(employee.EmployeeId, DateTime.Now);
                followUp.penalties = _employeeLeaveManager.GetEmployeePenaltyIndividual(employee.EmployeeId, DateTime.Now);
                followUp.employeeBasicInfo = _employeeLeaveManager.GetEmployeeBasicInfo(model.EmployeeCardId, DateTime.Now).SingleOrDefault();
            }
            return View(followUp);
        }
    }
}