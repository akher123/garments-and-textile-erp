using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class BasePayrollController : BaseController
    {
        #region Payroll

        public ISalarySetupManager SalarySetupManager
        {
            get { return Manager.SalarySetupManager; }
        }

        public IOvertimeSettingsManager OvertimeSettingsManager
        {
            get { return Manager.OvertimeSettingsManager; }
        }


        public IEmployeeSalaryProcessManager EmployeeSalaryProcessManager
        {
            get { return Manager.EmployeeSalaryProcessManager; }
        }

        public IEmployeeSalaryProcessConfirmManager EmployeeSalaryProcessConfirmManager
        {
            get { return Manager.EmployeeSalaryProcessConfirmManager; }
        }

        public IExcludedEmployeeFromSalaryProcessManager ExcludedEmployeeFromSalaryProcessManager
        {
            get { return Manager.ExcludedEmployeeFromSalaryProcessManager; }
        }
 

        #endregion
	}
}