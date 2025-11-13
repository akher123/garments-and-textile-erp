using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.Manager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class BaseHrmController : BaseController
    {  
     
        #region HRM
        public IOutStationDutyManager OutStationDutyManager
        {
            get { return Manager.OutStationDutyManager; }
        }
        public IBankAccountTypeManager BankAccountTypeManager
        {
            get { return Manager.BankAccountTypeManager; }
        }
   
        public IBranchUnitWorkShiftManager BranchUnitWorkShiftManager
        {
            get { return Manager.BranchUnitWorkShiftManager; }
        }
        public IOvertimeEligibleEmployeeManager OvertimeEligibleEmployeeManager
        {
            get { return Manager.OvertimeEligibleEmployeeManager; }
        }

        public IEmployeeShortLeaveManager EmployeeShortLeaveManager
        {
            get { return Manager.EmployeeShortLeaveManager; }
        }

        public IHRMReportManager HrmReportManager
        {
            get { return Manager.HrmReportManager; }
        }

    
        public IEmployeeWorkShiftManager EmployeeWorkShiftManager
        {
            get { return Manager.EmployeeWorkShiftManager; }
        }
        public IEmployeeWorkGroupManager EmployeeWorkGroupManager
        {
            get { return Manager.EmployeeWorkGroupManager; }
        }
     

        public IAttendanceBonusManager AttendanceBonusManager
        {
            get { return Manager.AttendanceBonusManager; }
        }
    
    
 

        public IEmployeeBankInfoManager EmployeeBankInfoManager
        {
            get { return Manager.EmployeeBankInfoManager; }
        }

        public IEmployeeEducationManager EmployeeEducationManager
        {
            get { return Manager.EmployeeEducationManager; }
        }

        public IEmployeeDesignationManager EmployeeDesignationManager
        {
            get { return Manager.EmployeeDesignationManager; }
        }

    
  
   
        public IEmployeeJobCardManager EmployeeJobCardManager
        {
            get { return Manager.EmployeeJobCardManager; }
        }
        public IEmployeeCardPrintManager EmployeeCardPrintManager
        {
            get { return Manager.EmployeeCardPrintManager; }
        }
        public IEmployeeGradeManager EmployeeGradeManager
        {
            get { return Manager.EmployeeGradeManager; }
        }

        public IWorkGroupManager WorkGroupManager
        {
            get { return Manager.WorkGroupManager; }
        }

        public IEmploymentManager EmploymentManager
        {
            get { return Manager.EmploymentManager; }
        }

        public IEmployeeDocumentManager EmployeeDocumentManager
        {
            get { return Manager.EmployeeDocumentManager; }
        }


        public IEducationLevelManager EducationLevelManager
        {
            get { return Manager.EducationLevelManager; }
        }

        public ISkillSetManager SkillSetManager
        {
            get { return Manager.SkillSetManager; }
        }

        public ILeaveTypeManager LeaveTypeManager
        {
            get { return Manager.LeaveTypeManager; }
        }

        public IEntitlementManager EntitlementManager
        {
            get { return Manager.EntitlementManager; }
        }

 
        public IEmployeeEntitlementManager EmployeeEntitlementManager
        {
            get { return Manager.EmployeeEntitlementManager; }
        }

        public IEmployeeLeaveManager EmployeeLeaveManager
        {
            get { return Manager.EmployeeLeaveManager; }
        }

        public ILeaveSettingManager LeaveSettingManager
        {
            get { return Manager.LeaveSettingManager; }
        }

   
        public IEmployeeManualAttendanceManager EmployeeManualAttendanceManager
        {
            get { return Manager.EmployeeManualAttendanceManager; }
        }

        public IHolidaySetupManager HolidaySetupManager
        {
            get { return Manager.HolidaySetupManager; }
        }

        public IEmployeeAppointmentManager EmployeeAppointmentManager
        {
            get { return Manager.EmployeeAppointmentManager; }
        }

        public ISalaryIncrementManager SalaryIncrementManager
        {
            get { return Manager.SalaryIncrementManager; }
        }

        public IEmployeeDailyAttendanceManager EmployeeDailyAttendanceManager
        {
            get { return Manager.EmployeeDailyAttendanceManager; }
        }

        public ICompanyOrganogramManager CompanyOrganogramManager
        {
            get { return Manager.CompanyOrganogramManager; }
        }




        public IReligionManager ReligionManager
        {
            get { return Manager.ReligionManager; }
        }

        public IMaritalStatusManager MaritalStatusManager
        {
            get { return Manager.MaritalStatusManager; }
        }

        public IEmployeeFamilyInfoManager EmployeeFamilyInfoManager
        {
            get { return Manager.EmployeeFamilyInfoManager; }
        }

        public IEmployeeAddressInfoManager EmployeeAddressInfoManager
        {
            get { return Manager.EmployeeAddressInfoManager; }
        }


        public IHRMReportManager HRMReportManager
        {
            get { return Manager.HrmReportManager; }
        }

        public IEmployeeInOutProcessManager EmployeeInOutProcessManager
        {
            get { return Manager.EmployeeInOutProcessManager; }
        }

        public IEmployeeJobCardProcessManager EmployeeJobCardProcessManager
        {
            get { return Manager.EmployeeJobCardProcessManager; }
        }

        public IEmployeeInOutModelProcessManager EmployeeInOutModelProcessManager
        {
            get { return Manager.EmployeeInOutModelProcessManager; }
        }

        public IEmployeeJobCardModelProcessManager EmployeeJobCardModelProcessManager
        {
            get { return Manager.EmployeeJobCardModelProcessManager; }
        }

        public IGeneralDaySetupManager GeneralDaySetupManager
        {
            get { return Manager.GeneralDaySetupManager; }
        }


        #endregion
      

	}
}