using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.ICRMManager;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.BLL.Manager.CommonManager;
using SCERP.BLL.Manager.CRMManager;
using SCERP.BLL.Manager.HRMManager;
using SCERP.BLL.Manager.InventoryManager;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.BLL.Manager.PayrollManager;
using SCERP.BLL.Manager.ProductionManager;
using SCERP.BLL.Manager.UserManagementManager;
using SCERP.DAL;
using System.Web;


namespace SCERP.BLL.Manager
{
    public class Manager
    {

        #region Manager
        internal SCERPDBContext Context;

        public static Manager Instance
        {
            get
            {
                var manager = (Manager)HttpContext.Current.Items["DatabaseManager"];
                if (manager == null)
                {
                    manager = new Manager();
                    HttpContext.Current.Items["DatabaseManager"] = manager;
                }
                return manager;
            }
        }

        public Manager()
        {
            Context = new SCERPDBContext();
        }
        #endregion
        #region Production
        //private IMachineManager _machineManager;
        //public IMachineManager MachineManager
        //{
        //    get
        //    {
        //        return _machineManager ?? (_machineManager = new MachineManager(Context));
        //    }
        //}

        //private IProcessorManager _processorManager;
        //public IProcessorManager ProcessorManager
        //{
        //    get
        //    {
        //        return _processorManager ?? (_processorManager = new ProcessorManager(Context));
        //    }
        //}
        private ISubProcessManager _subprocessManager;
        //public ISubProcessManager SubProcessManager
        //{
        //    get
        //    {
        //        return _subprocessManager ?? (_subprocessManager = new SubProcessManager(Context));
        //    }
        //}

        #endregion
        #region HRM Manager

        
        private IEfficiencyRateManager _efficiencyRateManager;
        public IEfficiencyRateManager EfficiencyRateManager
        {
            get
            {
                return _efficiencyRateManager ?? (_efficiencyRateManager = new EfficiencyRateManager(Context));
            }
        }



        private IEmployeeSkillManager _employeeSkillManager;
        public IEmployeeSkillManager EmployeeSkillManager
        {
            get
            {
                return _employeeSkillManager ?? (_employeeSkillManager = new EmployeeSkillManager(Context));
            }
        }



        private ISkillOperationManager _skillOperationManager;
        public ISkillOperationManager SkillOperationManager
        {
            get
            {
                return _skillOperationManager ?? (_skillOperationManager = new SkillOperationManager(Context));
            }
        }

        private ISkillSetDifficultyManager _skillSetDifficultyManager;
        public ISkillSetDifficultyManager SkillSetDifficultyManager
        {
            get
            {
                return _skillSetDifficultyManager ?? (_skillSetDifficultyManager = new SkillSetDifficultyManager(Context));
            }
        }

        private ISkillSetCategoryManager _skillSetCategoryManager;
        public ISkillSetCategoryManager SkillSetCategoryManager
        {
            get
            {
                return _skillSetCategoryManager ?? (_skillSetCategoryManager = new SkillSetCategoryManager(Context));
            }
        }

        private IOutStationDutyManager _outStationDutyManager;
        public IOutStationDutyManager OutStationDutyManager
        {
            get { return _outStationDutyManager ?? (_outStationDutyManager = new OutStationDutyManager(Context)); }
        }

        private IAuthorizationTypeManager _authorizationTypeManager;
        public IAuthorizationTypeManager AuthorizationTypeManager
        {
            get { return _authorizationTypeManager ?? (_authorizationTypeManager = new AuthorizationTypeManager(Context)); }
        }
        private IBankAccountTypeManager _bankAccountTypeManager;
        public IBankAccountTypeManager BankAccountTypeManager
        {
            get { return _bankAccountTypeManager ?? (_bankAccountTypeManager = new BankAccountTypeManager(Context)); }
        }

        private IEmployeeGradeSalaryPercentageManager _employeeGradeSalaryPercentageManager;
        public IEmployeeGradeSalaryPercentageManager EmployeeGradeSalaryPercentageManager
        {
            get { return _employeeGradeSalaryPercentageManager ?? (_employeeGradeSalaryPercentageManager = new EmployeeGradeSalaryPercentageManager(Context)); }
        }
        private IBranchUnitWorkShiftManager _branchUnitWorkShiftManager;
        public IBranchUnitWorkShiftManager BranchUnitWorkShiftManager
        {
            get { return _branchUnitWorkShiftManager ?? (_branchUnitWorkShiftManager = new BranchUnitWorkShiftManager(Context)); }
        }
        private IOvertimeEligibleEmployeeManager _overtimeEligibleEmployeeManager;
        public IOvertimeEligibleEmployeeManager OvertimeEligibleEmployeeManager
        {
            get { return _overtimeEligibleEmployeeManager ?? (_overtimeEligibleEmployeeManager = new OvertimeEligibleEmployeeManager(Context)); }
        }

        private IHRMReportManager _hrmReportManager;
        public IHRMReportManager HrmReportManager
        {
            get { return _hrmReportManager ?? (_hrmReportManager = new HRMReportManager(Context)); }
        }
        private IEmployeeShortLeaveManager _employeeShortLeaveManager;
        public IEmployeeShortLeaveManager EmployeeShortLeaveManager
        {
            get
            {
                return _employeeShortLeaveManager ?? (_employeeShortLeaveManager = new EmployeeShortLeaveManager(Context));
            }
        }

        private IDepartmentSectionManager _departmentSectionManager;
        public IDepartmentSectionManager DepartmentSectionManager
        {
            get { return _departmentSectionManager ?? (_departmentSectionManager = new DepartmentSectionManager(Context)); }
        }

        private IDepartmentLineManager _departmentLineManager;
        public IDepartmentLineManager DepartmentLineManager
        {
            get { return _departmentLineManager ?? (_departmentLineManager = new DepartmentLineManager(Context)); }
        }
        private IBranchUnitManager _branchUnitManager;
        private IBranchUnitDepartmentManager _branchUnitDepartmentManager;

        public IBranchUnitDepartmentManager BranchUnitDepartmentManager
        {
            get { return _branchUnitDepartmentManager ?? (_branchUnitDepartmentManager = new BranchUnitDepartmentManager(Context)); }
        }
        public SCERP.BLL.IManager.IHRMManager.IBranchUnitManager BranchUnitManager
        {
            get { return _branchUnitManager ?? (_branchUnitManager = new BranchUnitManager(Context)); }
        }
        private SCERP.BLL.IManager.IHRMManager.IUnitManager _unitManager;
        public SCERP.BLL.IManager.IHRMManager.IUnitManager UnitManager
        {
            get { return _unitManager ?? (_unitManager = new UnitManager(Context)); }
        }

        private IEmployeeWorkShiftManager _employeeWorkShift;

        public IEmployeeWorkShiftManager EmployeeWorkShiftManager
        {
            get { return _employeeWorkShift ?? (_employeeWorkShift = new EmployeeWorkShiftManager(Context)); }
        }

        private IEmployeeWorkGroupManager _employeeWorkGroupManager;
        public IEmployeeWorkGroupManager EmployeeWorkGroupManager
        {
            get { return _employeeWorkGroupManager ?? (_employeeWorkGroupManager = new EmployeeWorkGroupManager(Context)); }
        }
        private ISectionManager _sectionManager;
        public ISectionManager SectionManager
        {
            get { return _sectionManager ?? (_sectionManager = new SectionManager(Context)); }
        }


        private IBranchManager _branchManager;
        public IBranchManager BranceManager
        {
            get { return _branchManager ?? (_branchManager = new BranchManager(Context)); }
        }


        private IAttendanceBonusManager _attendanceBonusManager;

        public IAttendanceBonusManager AttendanceBonusManager
        {
            get { return _attendanceBonusManager ?? (_attendanceBonusManager = new AttendanceBonusManager(Context)); }
        }
        private IEmployeeManualAttendanceManager _employeeManualAttendanceManager;
        public IEmployeeManualAttendanceManager EmployeeManualAttendanceManager
        {
            get
            {
                return _employeeManualAttendanceManager ??
                       (_employeeManualAttendanceManager = new EmployeeManualAttendanceManager(Context));
            }
        }

        private IWeekendManager _weekendManager;
        public IWeekendManager WeekendManager
        {
            get
            {
                return _weekendManager ?? (_weekendManager = new WeekendManager(Context));
            }
        }

        private IAuthorizedPersonManager _authorizedPersonManager;
        public IAuthorizedPersonManager AuthorizedPersonManager
        {
            get { return _authorizedPersonManager ?? (_authorizedPersonManager = new AuthorizedPersonManager(Context)); }
        }

        private IDistrictManager _districtManager;
        public IDistrictManager DistrictManager
        {
            get { return _districtManager ?? (_districtManager = new DistrictManager(Context)); }
        }

        private IAttendanceBonusSettingManager _attendanceBonusSettingManager;
        public IAttendanceBonusSettingManager AttendanceBonusSettingManager
        {
            get { return _attendanceBonusSettingManager ?? (_attendanceBonusSettingManager = new AttendanceBonusSettingManager(Context)); }
        }

        private IEmployeeAppointmentManager _employeeAppointmentManager;

        public IEmployeeAppointmentManager EmployeeAppointmentManager
        {
            get { return _employeeAppointmentManager ?? (_employeeAppointmentManager = new EmployeeAppointmentManager(Context)); }
        }

        private ISalaryIncrementManager _salaryIncrementManager;

        public ISalaryIncrementManager SalaryIncrementManager
        {
            get { return _salaryIncrementManager ?? (_salaryIncrementManager = new SalaryIncrementManager(Context)); }
        }

        private IPoliceStationManager _policeStationManager;
        public IPoliceStationManager PoliceStationManager
        {
            get { return _policeStationManager ?? (_policeStationManager = new PoliceStationManager(Context)); }
        }

        private IEmployeeManager _employeeManager;
        public IEmployeeManager EmployeeManager
        {
            get { return _employeeManager ?? (_employeeManager = new EmployeeManager(Context)); }
        }

        private IEmployeeBankInfoManager _employeeBankInfoManager;
        public IEmployeeBankInfoManager EmployeeBankInfoManager
        {
            get { return _employeeBankInfoManager ?? (_employeeBankInfoManager = new EmployeeBankInfoManager(Context)); }

        }

        private IEmployeeEducationManager _employeeEducationManager;
        public IEmployeeEducationManager EmployeeEducationManager
        {
            get
            {
                return _employeeEducationManager ?? (_employeeEducationManager = new EmployeeEducationManager(Context));
            }

        }

        private IEmployeeDesignationManager _employeeDesignationManager;
        public IEmployeeDesignationManager EmployeeDesignationManager
        {
            get
            {
                return _employeeDesignationManager ??
                       (_employeeDesignationManager = new EmployeeDesignationManager(Context));
            }

        }

        private IWorkShiftManager _workShiftManager;
        public IWorkShiftManager WorkShiftManager
        {
            get
            {
                return _workShiftManager ?? (_workShiftManager = new WorkShiftManager(Context));
            }
        }

        private IEmployeeTypeManager _employeeTypeManager;
        public IEmployeeTypeManager EmployeeTypeManager
        {
            get
            {
                return _employeeTypeManager ?? (_employeeTypeManager = new EmployeeTypeManager(Context));
            }
        }

        private IDepartmentManager _departmentManager;
        public IDepartmentManager DepartmentManager
        {
            get
            {
                return _departmentManager ?? (_departmentManager = new DepartmentManager(Context));
            }
        }



        private IEmployeeGradeManager _employeeGradeManager;
        public IEmployeeGradeManager EmployeeGradeManager
        {
            get
            {
                return _employeeGradeManager ?? (_employeeGradeManager = new EmployeeGradeManager(Context));
            }
        }

        private IWorkGroupManager _workGroupManager;
        public IWorkGroupManager WorkGroupManager
        {
            get
            {
                return _workGroupManager ?? (_workGroupManager = new WorkGroupManager(Context));
            }
        }

        private IEmploymentManager _employmentManager;
        public IEmploymentManager EmploymentManager
        {
            get
            {
                return _employmentManager ?? (_employmentManager = new EmploymentManager(Context));
            }
        }

        private IEmployeeDocumentManager _employeeDocumentManager;
        public IEmployeeDocumentManager EmployeeDocumentManager
        {
            get
            {
                return _employeeDocumentManager ?? (_employeeDocumentManager = new EmployeeDocumentManager(Context));
            }
        }

        private IEmployeeJobCardManager _employeeJobCardManager;
        public IEmployeeJobCardManager EmployeeJobCardManager
        {
            get
            {
                return _employeeJobCardManager ?? (_employeeJobCardManager = new EmployeeJobCardManager(Context));
            }
        }

        private IEmployeeSalaryManager _employeeSalaryManager;
        public IEmployeeSalaryManager EmployeeSalaryManager
        {
            get
            {
                return _employeeSalaryManager ?? (_employeeSalaryManager = new EmployeeSalaryManager(Context));
            }
        }
        private IEmployeeCardPrintManager _employeeCardPrintManager;

        public IEmployeeCardPrintManager EmployeeCardPrintManager
        {
            get
            {
                return _employeeCardPrintManager ?? (_employeeCardPrintManager = new EmployeeCardPrintManager(Context));
            }
        }

        private IEducationLevelManager _educationLevelManager;
        public IEducationLevelManager EducationLevelManager
        {
            get
            {
                return _educationLevelManager ?? (_educationLevelManager = new EducationLevelManager(Context));
            }
        }

        private ISkillSetManager _skillSetManager;
        public ISkillSetManager SkillSetManager
        {
            get
            {
                return _skillSetManager ?? (_skillSetManager = new SkillSetManager(Context));
            }
        }

        private ILeaveTypeManager _leaveTypeManager;
        public ILeaveTypeManager LeaveTypeManager
        {
            get
            {
                return _leaveTypeManager ?? (_leaveTypeManager = new LeaveTypeManager(Context));
            }
        }

        private ILeaveSettingManager _leaveSettingManager;
        public ILeaveSettingManager LeaveSettingManager
        {
            get
            {
                return _leaveSettingManager ?? (_leaveSettingManager = new LeaveSettingManager(Context));
            }
        }

        private IEntitlementManager _entitlementManager;
        public IEntitlementManager EntitlementManager
        {
            get
            {
                return _entitlementManager ?? (_entitlementManager = new EntitlementManager(Context));
            }
        }

        private IEmployeeEntitlementManager _employeeEntitlementManager;
        public IEmployeeEntitlementManager EmployeeEntitlementManager
        {
            get
            {
                return _employeeEntitlementManager ??
                       (_employeeEntitlementManager = new EmployeeEntitlementManager(Context));
            }
        }

        private ICompanyManager _companyManager;
        public ICompanyManager CompanyManager
        {
            get
            {
                return _companyManager ?? (_companyManager = new CompanyManager(Context));
            }
        }

        private IStampAmountManager _stampAmountManager;
        public IStampAmountManager StampAmountManager
        {
            get
            {
                return _stampAmountManager ?? (_stampAmountManager = new StampAmountManager(Context));
            }
        }

        private ISalaryAdvanceManager _salaryAdvanceManager;
        public ISalaryAdvanceManager SalaryAdvanceManager
        {
            get
            {
                return _salaryAdvanceManager ?? (_salaryAdvanceManager = new SalaryAdvanceManager(Context));
            }
        }


        private IEmployeeBonusManager _employeeBonusManager;
        public IEmployeeBonusManager EmployeeBonusManager
        {
            get
            {
                return _employeeBonusManager ?? (_employeeBonusManager = new EmployeeBonusManager(Context));
            }
        }


        private ISalarySetupManager _salarySetupManager;
        public ISalarySetupManager SalarySetupManager
        {
            get
            {
                return _salarySetupManager ?? (_salarySetupManager = new SalarySetupManager(Context));
            }
        }

        private IEmployeeLeaveManager _employeeLeaveManager;
        public IEmployeeLeaveManager EmployeeLeaveManager
        {
            get
            {
                return _employeeLeaveManager ?? (_employeeLeaveManager = new EmployeeLeaveManager(Context));
            }
        }

        private IHolidaySetupManager _holidaySetupManager;
        public IHolidaySetupManager HolidaySetupManager
        {
            get
            {
                return _holidaySetupManager ?? (_holidaySetupManager = new HolidaySetupManager(Context));
            }
        }

        private IBloodGroupManager _bloodGroupManager;
        public IBloodGroupManager BloodGroupManager
        {
            get
            {
                return _bloodGroupManager ?? (_bloodGroupManager = new BloodGroupManager(Context));
            }
        }

        private IEmployeeDailyAttendanceManager _employeeDailyAttendanceManager;
        public IEmployeeDailyAttendanceManager EmployeeDailyAttendanceManager
        {
            get
            {
                return _employeeDailyAttendanceManager ??
                       (_employeeDailyAttendanceManager = new EmployeeDailyAttendanceManager(Context));
            }
        }

        private ICompanyOrganogramManager _companyOrganogramManager;
        public ICompanyOrganogramManager CompanyOrganogramManager
        {
            get
            {
                return _companyOrganogramManager ??
                       (_companyOrganogramManager = new CompanyOrganogramManager(Context));
            }
        }

        private ICountryManager _countryManager;
        public ICountryManager CountryManager
        {
            get
            {
                return _countryManager ??
                       (_countryManager = new CountryManager(Context));
            }
        }

        private ILineManager _lineManager;
        public ILineManager LineManager
        {
            get
            {
                return _lineManager ??
                       (_lineManager = new LineManager(Context));
            }
        }

        private IReligionManager _religionManager;
        public IReligionManager ReligionManager
        {
            get
            {
                return _religionManager ??
                       (_religionManager = new ReligionManager(Context));
            }
        }

        private IMaritalStatusManager _maritalStatusManager;
        public IMaritalStatusManager MaritalStatusManager
        {
            get
            {
                return _maritalStatusManager ??
                       (_maritalStatusManager = new MaritalStatusManager(Context));
            }
        }

        private IUnitDepartmentManager _unitDepartmentManager;
        public IUnitDepartmentManager UnitDepartmentManager
        {
            get
            {
                return _unitDepartmentManager ??
                       (_unitDepartmentManager = new UnitDepartmentManager(Context));
            }
        }

        private IGenderManager _genderManager;
        public IGenderManager GenderManager
        {
            get
            {
                return _genderManager ??
                       (_genderManager = new GenderManager(Context));
            }
        }

        private IEmployeeCompanyInfoManager _employeeCompanyInfoManager;
        public IEmployeeCompanyInfoManager EmployeeCompanyInfoManager
        {
            get
            {
                return _employeeCompanyInfoManager ??
                       (_employeeCompanyInfoManager = new EmployeeCompanyInfoManager(Context));
            }
        }

        private IEmployeeFamilyInfoManager _employeeFamilyInfoManager;
        public IEmployeeFamilyInfoManager EmployeeFamilyInfoManager
        {
            get
            {
                return _employeeFamilyInfoManager ??
                       (_employeeFamilyInfoManager = new EmployeeFamilyInfoManager(Context));
            }
        }

        private IEmployeeAddressInfoManager _employeeAddressInfoManager;
        public IEmployeeAddressInfoManager EmployeeAddressInfoManager
        {
            get
            {
                return _employeeAddressInfoManager ??
                       (_employeeAddressInfoManager = new EmployeeAddressInfoManager(Context));
            }
        }

        private IQuitTypeManager _quitTypeManager;
        public IQuitTypeManager QuitTypeManager
        {
            get
            {
                return _quitTypeManager ??
                       (_quitTypeManager = new QuitTypeManager(Context));
            }
        }


        private IEmployeeInOutProcessManager _employeeInOutProcessManager;
        public IEmployeeInOutProcessManager EmployeeInOutProcessManager
        {
            get
            {
                return _employeeInOutProcessManager ??
                       (_employeeInOutProcessManager = new EmployeeInOutProcessManager(Context));
            }
        }

        private IEmployeeJobCardProcessManager _employeeJobCardProcessManager;
        public IEmployeeJobCardProcessManager EmployeeJobCardProcessManager
        {
            get
            {
                return _employeeJobCardProcessManager ??
                       (_employeeJobCardProcessManager = new EmployeeJobCardProcessManager(Context));
            }
        }

        private IEmployeeInOutModelProcessManager _employeeInOutModelProcessManager;
        public IEmployeeInOutModelProcessManager EmployeeInOutModelProcessManager
        {
            get
            {
                return _employeeInOutModelProcessManager ??
                       (_employeeInOutModelProcessManager = new EmployeeInOutModelProcessManager(Context));
            }
        }

        private IEmployeeJobCardModelProcessManager _employeeJobCardModelProcessManager;
        public IEmployeeJobCardModelProcessManager EmployeeJobCardModelProcessManager
        {
            get
            {
                return _employeeJobCardModelProcessManager ??
                       (_employeeJobCardModelProcessManager = new EmployeeJobCardModelProcessManager(Context));
            }
        }

        private IGeneralDaySetupManager _generalDaySetupManager;
        public IGeneralDaySetupManager GeneralDaySetupManager
        {
            get
            {
                return _generalDaySetupManager ??
                       (_generalDaySetupManager = new GeneralDaySetupManager(Context));
            }
        }


        #endregion

        #region Payroll Manager

        private IPayrollReportManager _payrollReportManager;

        public IPayrollReportManager PayrollReportManager
        {
            get
            {
                return _payrollReportManager ?? (_payrollReportManager = new PayrollReportManager(Context));
            }
        }

        private IOvertimeSettingsManager _overtimeSettingsManager;
        public IOvertimeSettingsManager OvertimeSettingsManager
        {
            get { return _overtimeSettingsManager ?? (_overtimeSettingsManager = new OvertimeSettingsManager(Context)); }
        }


        private IEmployeeSalaryProcessManager _employeeSalaryProcessManager;
        public IEmployeeSalaryProcessManager EmployeeSalaryProcessManager
        {
            get
            {
                return _employeeSalaryProcessManager ??
                       (_employeeSalaryProcessManager = new EmployeeSalaryProcessManager(Context));
            }
        }

        private IEmployeeSalaryProcessConfirmManager _employeeSalaryProcessConfirmManager;
        public IEmployeeSalaryProcessConfirmManager EmployeeSalaryProcessConfirmManager
        {
            get
            {
                return _employeeSalaryProcessConfirmManager ??
                       (_employeeSalaryProcessConfirmManager = new EmployeeSalaryProcessConfirmManager(Context));
            }
        }

        private ISalaryMappingManager _salaryMappingManager;
        public ISalaryMappingManager SalaryMappingManager
        {
            get
            {
                return _salaryMappingManager ?? (_salaryMappingManager = new SalaryMappingManager(Context));
            }
        }

        private IExcludedEmployeeFromSalaryProcessManager _excludedEmployeeFromSalaryProcessManager;
        public IExcludedEmployeeFromSalaryProcessManager ExcludedEmployeeFromSalaryProcessManager
        {
            get
            {
                return _excludedEmployeeFromSalaryProcessManager ?? (_excludedEmployeeFromSalaryProcessManager = new ExcludedEmployeeFromSalaryProcessManager(Context));
            }
        }

        #endregion

        #region User Right Management Manager

        private IUserManager _userManager;
        public IUserManager UserManager
        {
            get { return _userManager ?? (_userManager = new UserManager(Context)); }
        }

        private IUserRoleManager _userRoleManager;
        public IUserRoleManager UserRoleManager
        {
            get { return _userRoleManager ?? (_userRoleManager = new UserRoleManager(Context)); }
        }

        private IModuleManager _moduleManager;
        public IModuleManager ModuleManager
        {
            get { return _moduleManager ?? (_moduleManager = new ModuleManager(Context)); }
        }

        private IModuleFeatureManager _moduleFeatureManager;
        public IModuleFeatureManager ModuleFeatureManager
        {
            get { return _moduleFeatureManager ?? (_moduleFeatureManager = new ModuleFeatureManager(Context)); }
        }
        private IUserPermissionForDepartmentLevelManager _userPermissionForDepartmentLevelManager;
        public IUserPermissionForDepartmentLevelManager UserPermissionForDepartmentLevelManager
        {
            get
            {
                return _userPermissionForDepartmentLevelManager ??
                       (_userPermissionForDepartmentLevelManager = new UserPermissionForDepartmentLevelManager(Context));
            }
        }

        private IUserPermissionForEmployeeLevelManager _userPermissionForEmployeeLevelManager;
        public IUserPermissionForEmployeeLevelManager UserPermissionForEmployeeLevelManager
        {
            get
            {
                return _userPermissionForEmployeeLevelManager ??
                       (_userPermissionForEmployeeLevelManager = new UserPermissionForEmployeeLevelManager(Context));
            }
        }
        #endregion

        #region Accounting Manager

        private ICompanySectorManager _companySectorManager;
        public ICompanySectorManager CompanySectorManager
        {
            get { return _companySectorManager ?? (_companySectorManager = new CompanySectorManager(Context)); }
        }

        private ICostCentreManager _costCentreManager;
        public ICostCentreManager CostCentreManager
        {
            get { return _costCentreManager ?? (_costCentreManager = new CostCentreManager(Context)); }
        }

        private IFinancialPeriodManager _financialPeriodManager;
        public IFinancialPeriodManager FinancialPeriodManager
        {
            get { return _financialPeriodManager ?? (_financialPeriodManager = new FinancialPeriodManager(Context)); }
        }

        private IJournalVoucherEntryManager _journalVoucherEntryManager;
        public IJournalVoucherEntryManager JournalVoucherEntryManager
        {
            get
            {
                return _journalVoucherEntryManager ??
                       (_journalVoucherEntryManager = new JournalVoucherEntryManager(Context));
            }
        }

        //private IControlAccountManager _controlAccountManager;
        //public IControlAccountManager ControlAccountManager
        //{
        //    get { return _controlAccountManager ?? (_controlAccountManager = new ControlAccountManager(Context)); }
        //}

        private ICashVoucherEntryManager _cashVoucherEntryManager;
        public ICashVoucherEntryManager CashVoucherEntryManager
        {
            get { return _cashVoucherEntryManager ?? (_cashVoucherEntryManager = new CashVoucherEntryManager(Context)); }
        }

        private IBankVoucherEntryManager _bankVoucherEntryManager;
        public IBankVoucherEntryManager BankVoucherEntryManager
        {
            get { return _bankVoucherEntryManager ?? (_bankVoucherEntryManager = new BankVoucherEntryManager(Context)); }
        }

        private IContraVoucherEntryManager _contraVoucherEntryManager;
        public IContraVoucherEntryManager ContraVoucherEntryManager
        {
            get
            {
                return _contraVoucherEntryManager ?? (_contraVoucherEntryManager = new ContraVoucherEntryManager(Context));
            }
        }

        private IOpeningBalaceManager _openingBalaceManager;
        public IOpeningBalaceManager OpeningBalaceManager
        {
            get
            {
                return _openingBalaceManager ?? (_openingBalaceManager = new OpeningBalaceManager(Context));
            }
        }

        private IReportAccountManger _reportAccountManger;
        public IReportAccountManger ReportAccountManger
        {
            get
            {
                return _reportAccountManger ?? (_reportAccountManger = new ReportAccountManger(Context));
            }
        }

        private IVoucherListManager _voucherListManager;
        public IVoucherListManager VoucherListManager
        {
            get
            {
                return _voucherListManager ?? (_voucherListManager = new VoucherListManager(Context));
            }
        }

        private IBankReconcilationManager _bankReconcilationManager;
        public IBankReconcilationManager BankReconcilationManager
        {
            get
            {
                return _bankReconcilationManager ?? (_bankReconcilationManager = new BankReconcilationManager(Context));
            }
        }

        private IBankReconcilationListManager _bankReconcilationListManager;
        public IBankReconcilationListManager BankReconcilationListManager
        {
            get
            {
                return _bankReconcilationListManager ?? (_bankReconcilationListManager = new BankReconcilationListManager(Context));
            }
        }

        private IDepreciationChartManager _depreciationChartManager;
        public IDepreciationChartManager DepreciationChartManager
        {
            get { return _depreciationChartManager ?? (_depreciationChartManager = new DepreciationChartManager(Context)); }
        }

        private IGLAccountManager _glAccountManager;
        //public IGLAccountManager GlAccountManager
        //{
        //    get { return _glAccountManager ?? (_glAccountManager = new GLAccountManager(Context)); }
        //}

        //private IVoucherMasterManager _voucherMasterManager;
        //public IVoucherMasterManager VoucherMasterManager
        //{
        //    get { return _voucherMasterManager ?? (_voucherMasterManager = new VoucherMasterManager(Context)); }
        //}

        private IAccCurrencManager _accCurrencManager;
        public IAccCurrencManager AccCurrencManager
        {
            get { return _accCurrencManager ?? (_accCurrencManager = new AccCurrencManager(Context)); }
        }

        private IGLAccountHiddenManager _gLAccountHiddenManager;    
        public IGLAccountHiddenManager GLAccountHiddenManager
        {
            get { return _gLAccountHiddenManager ?? (_gLAccountHiddenManager = new GLAccountHiddenManager(Context)); }
        }

        #endregion

        #region Merchandising Manager



        private IOmBuyerManager _omBuyerManager;
        private ISupplierCompanyManager _supplierCompanyManager;
        public ISupplierCompanyManager SupplierCompanyManager
        {
            get { return _supplierCompanyManager ?? (_supplierCompanyManager = new SupplierCompanyManager(Context)); }
        }

        #endregion

        #region Common Manager

        private ICityManager _cityManager;
        public ICityManager CityManager
        {
            get
            {
                return _cityManager ?? (_cityManager = new CityManager(Context));
            }
        }

        private IStateManager _stateManager;
        public IStateManager StateManager
        {
            get
            {
                return _stateManager ?? (_stateManager = new StateManager(Context));
            }
        }

        //private IColorManager _colorManager;
        //public IColorManager ColorManager
        //{
        //    get
        //    {
        //        return _colorManager ?? (_colorManager = new ColorManager(Context));
        //    }
        //}


        //private IBatchManager _batchManager;
        //public IBatchManager BatchManager
        //{
        //    get
        //    {
        //        return _batchManager ?? (_batchManager = new BatchManager(Context));
        //    }
        //}
        //private IPartyManager _partyManager;
        //public IPartyManager PartyManager
        //{
        //    get
        //    {
        //        return _partyManager ?? (_partyManager = new PartyManager(Context));
        //    }
        //}




        private IManager.ICommonManager.ICurrencyManagerCommon _currencyManagerCommon;
        public IManager.ICommonManager.ICurrencyManagerCommon CurrencyManagerCommon
        {
            get
            {
                return _currencyManagerCommon ?? (_currencyManagerCommon = new CurrencyManagerCommon(Context));
            }
        }

        private IMeasurementUnitManager _measurementUnitManager;
        public IManager.ICommonManager.IMeasurementUnitManager MeasurementUnitManager
        {
            get
            {
                return _measurementUnitManager ?? (_measurementUnitManager = new MeasurementUnitManager(Context));
            }
        }

        private ICustomSqlQuaryManager _customSqlQuaryManager;
        public IManager.ICommonManager.ICustomSqlQuaryManager CustomSqlQuaryManager
        {
            get
            {
                return _customSqlQuaryManager ?? (_customSqlQuaryManager = new CustomSqlQuaryManager(Context));
            }
        }
        private IDocumentManager _documentManager;
        public IDocumentManager DocumentManager
        {
            get
            {
                return _documentManager ?? (_documentManager = new DocumentManager(Context));
            }
        }

        
        #endregion

        #region Inventory Manager

        private IMaterialRequisitionManager _materialRequisitionManager;
        public IMaterialRequisitionManager MaterialRequisitionManager
        {
            get
            {
                return _materialRequisitionManager ?? (_materialRequisitionManager = new MaterialRequisitionManager(Context));
            }
        }
        private IInventoryApprovalStatusManager _inventoryApprovalStatusManager;
        public IInventoryApprovalStatusManager InventoryApprovalStatusManager
        {
            get
            {
                return _inventoryApprovalStatusManager ?? (_inventoryApprovalStatusManager = new InventoryApprovalStatusManager(Context));
            }
        }
        private IStorePurchaseRequisitionManager _storePurchaseRequisitionManager;
        public IStorePurchaseRequisitionManager StorePurchaseRequisitionManager
        {
            get
            {
                return _storePurchaseRequisitionManager ?? (_storePurchaseRequisitionManager = new StorePurchaseRequisitionManager(Context));
            }
        }

        private IPurchaseTypeManager _purchaseTypeManager;
        public IPurchaseTypeManager PurchaseTypeManager
        {
            get
            {
                return _purchaseTypeManager ?? (_purchaseTypeManager = new PurchaseTypeManager(Context));
            }
        }


        private IRequisitiontypeManager _requisitiontypeManager;
        public IRequisitiontypeManager RequisitiontypeManager
        {
            get
            {
                return _requisitiontypeManager ?? (_requisitiontypeManager = new RequisitiontypeManager(Context));
            }
        }
        private IBrandManager _brandManager;
        public IBrandManager BrandManager
        {
            get
            {
                return _brandManager ?? (_brandManager = new BrandManager(Context));
            }
        }
        private ISizeManager _sizeManager;

        public ISizeManager SizeManager
        {
            get
            {
                return _sizeManager ?? (_sizeManager = new SizeManager(Context));
            }
        }
        private IInventoryGroupManager _inventoryGroupManager;
        public IInventoryGroupManager InventoryGroupManager
        {
            get
            {
                return _inventoryGroupManager ?? (_inventoryGroupManager = new InventoryGroupManager(Context));
            }
        }

        private IInventorySubGroupManager _inventorySubGroupManager;
        public IInventorySubGroupManager InventorySubGroupManager
        {
            get
            {
                return _inventorySubGroupManager ?? (_inventorySubGroupManager = new InventorySubGroupManager(Context));
            }
        }

        private IInventoryItemManager _inventoryItemManager;

        public IInventoryItemManager InventoryItemManager
        {
            get
            {
                return _inventoryItemManager ?? (_inventoryItemManager = new InventoryItemManager(Context));
            }
        }
        private IItemStoreManager _itemStoreManager;
        public IItemStoreManager ItemStoreManager
        {
            get
            {
                return _itemStoreManager ?? (_itemStoreManager = new ItemStoreManager(Context));
            }
        }

        private IQualityCertificateManager _qualityCertificateManager;
        public IQualityCertificateManager QualityCertificateManager
        {
            get
            {
                return _qualityCertificateManager ?? (_qualityCertificateManager = new QualityCertificateManager(Context));
            }
        }


        private IGoodsReceivingNoteManager _goodsReceivingNoteManager;
        public IGoodsReceivingNoteManager GoodsReceivingNoteManager
        {
            get
            {
                return _goodsReceivingNoteManager ?? (_goodsReceivingNoteManager = new GoodsReceivingNoteManager(Context));
            }
        }

        private IInventoryAuthorizedPersonManager _inventoryAuthorizedPersonManager;
        public IInventoryAuthorizedPersonManager InventoryAuthorizedPersonManager
        {
            get
            {
                return _inventoryAuthorizedPersonManager ?? (_inventoryAuthorizedPersonManager = new InventoryAuthorizedPersonManager(Context));
            }
        }
        private IMaterialIssueRequisitionManager _materialIssueRequisitionManager;
        public IMaterialIssueRequisitionManager MaterialIssueRequisitionManager
        {
            get
            {
                return _materialIssueRequisitionManager ?? (_materialIssueRequisitionManager = new MaterialIssueRequisitionManager(Context));
            }
        }
        private IMaterialIssueManager _materialIssueManager;
        public IMaterialIssueManager MaterialIssueManager
        {
            get
            {
                return _materialIssueManager ?? (_materialIssueManager = new MaterialIssueManager(Context));
            }
        }

        private IStoreLedgerManager _storeLedgerManager;
        public IStoreLedgerManager StoreLedgerManager
        {
            get
            {
                return _storeLedgerManager ?? (_storeLedgerManager = new StoreLedgerManager(Context));
            }
        }
        #endregion

        #region CRM

        private SCERP.BLL.IManager.ICRMManager.IProjectDocumentInfoManager _documentInfoManager;
        public SCERP.BLL.IManager.ICRMManager.IProjectDocumentInfoManager ProjectDocumentInfoManager
        {
            get
            {
                return _documentInfoManager ?? (_documentInfoManager = new SCERP.BLL.Manager.CRMManager.ProjectDocumentInfoManager(Context));
            }
        }

        private IFeedbackManager _feedbackManager;
        public IFeedbackManager FeedbackManager
        {
            get
            {
                return _feedbackManager ?? (_feedbackManager = new FeedbackManager(Context));
            }
        }

        private ICRMReportManager _crmReportManager;

        public ICRMReportManager CrmReportManager
        {
            get
            {
                return _crmReportManager ?? (_crmReportManager = new CRMReportManager(Context));
            }
        }
        #endregion



     
       

    }
}
