using System.Collections.Generic;
using SCERP.Model.AccountingModel;
using SCERP.Model.CommonModel;
using SCERP.Model.CRMModel;
using SCERP.Model.HRMModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Maintenance;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;
using SCERP.Model.CommercialModel;
using SCERP.Model.TaskManagementModel;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using SCERP.Model;

    public partial class SCERPDBContext : DbContext
    {
        public SCERPDBContext()
            : base("name=SCERPDBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = 3600;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        #region tables

        #region UserRightManagement
        public virtual DbSet<UserLogTime> UserLogTimes { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleFeature> ModuleFeatures { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserPermissionForDepartmentLevel> UserPermissionForDepartmentLevels { get; set; }
        public virtual DbSet<UserPermissionForEmployeeLevel> UserPermissionForEmployeeLevels { get; set; }

        #endregion

        #region HRM

        public virtual DbSet<SkillSetCategory> SkillSetCategories { get; set; }
        public virtual DbSet<SkillSetDifficulty> SkillSetDifficulties { get; set; }
        public virtual DbSet<EfficiencyRate> EfficiencyRates { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual DbSet<SkillOperation> SkillOperations { get; set; }
        public virtual DbSet<BankAccountType> BankAccountTypes { get; set; }
        public virtual DbSet<BranchUnitWorkShift> BranchUnitWorkShifts { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<DepartmentSection> DepartmentSections { get; set; }
        public virtual DbSet<DepartmentLine> DepartmentLines { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<BranchUnit> BranchUnits { get; set; }
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<MaritalState> MaritalStates { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyOrganogram> CompanyOrganograms { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeBankInfo> EmployeeBankInfoes { get; set; }
        public virtual DbSet<EmployeeCardInfo> EmployeeCardInfoes { get; set; }
        public virtual DbSet<EmployeeCategory> EmployeeCategories { get; set; }
        public virtual DbSet<EmployeeDailyAttendance> EmployeeDailyAttendances { get; set; }
        public virtual DbSet<EmployeeDesignation> EmployeeDesignations { get; set; }
        public virtual DbSet<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public virtual DbSet<EmployeeEntitlement> EmployeeEntitlements { get; set; }
        public virtual DbSet<EmployeeGrade> EmployeeGrades { get; set; }
        public virtual DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public virtual DbSet<EmployeeShortLeave> EmployeeShortLeaves { get; set; }
        public virtual DbSet<EmployeePermanentAddress> EmployeePermanentAddresses { get; set; }
        public virtual DbSet<EmployeePresentAddress> EmployeePresentAddresses { get; set; }
        public virtual DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public virtual DbSet<EmployeeSalary_Processed> EmployeeSalary_Processed { get; set; }
        public virtual DbSet<EmployeeSalary_Processed_Temp> EmployeeSalary_Processed_Temp { get; set; }
        public virtual DbSet<EmployeeSkillLevel> EmployeeSkillLevels { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Employment> Employments { get; set; }
        public virtual DbSet<Entitlement> Entitlements { get; set; }
        public virtual DbSet<HolidaysSetup> HolidaysSetups { get; set; }
        public virtual DbSet<LeaveSetting> LeaveSettings { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<SkillSet> SkillSets { get; set; }
        public virtual DbSet<Weekend> Weekends { get; set; }
        public virtual DbSet<WorkGroup> WorkGroups { get; set; }
        public virtual DbSet<WorkShift> WorkShifts { get; set; }
        public virtual DbSet<EmployeeInOut> EmployeeInOuts { get; set; }
        public virtual DbSet<EmployeeSkillSet> EmployeeSkillSets { get; set; }
        public virtual DbSet<AuthorizationType> AuthorizationTypes { get; set; }
        public virtual DbSet<AuthorizedPerson> AuthorizedPersons { get; set; }
        public virtual DbSet<EmployeeWorkGroup> EmployeeWorkGroups { get; set; }
        public virtual DbSet<EmployeeWorkShift> EmployeeWorkShifts { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<UnitDepartment> UnitDepartments { get; set; }
        public virtual DbSet<BranchUnitDepartment> BranchUnitDepartments { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<EmployeeFamilyInfo> EmployeeFamilyInfos { get; set; }
        public virtual DbSet<QuitType> QuitTypes { get; set; }
        public virtual DbSet<GeneralDaySetup> GeneralDaySetups { get; set; }
        public virtual DbSet<HeadOfDepartment> HeadOfDepartments { get; set; }
        public virtual DbSet<OutStationDuty> OutStationDutys { get; set; }
        public virtual DbSet<AttendanceBonusSetting> AttendanceBonusSetting { get; set; }
        public virtual DbSet<EmployeeBonus> EmployeeBonus { get; set; }
        public virtual DbSet<HrmPenaltyType> PenaltyTypes { get; set; }
        public virtual DbSet<HrmMaternityPayment> HrmMaternityPayments { get; set; }
        public virtual DbSet<EmployeeHoliday> EmployeeHolidays { get; set; }
        public virtual DbSet<EmployeeWeekend> EmployeeWeekends { get; set; }

        public virtual DbSet<SkillMatrix> SkillMatrices { get; set; }
        public virtual DbSet<SkillMatrixDetail> SkillMatrixDetails { get; set; }        
        public virtual DbSet<SkillMatrixMachineType> SkillMatrixMachineTypes { get; set; }
        public virtual DbSet<SkillMatrixProcessName> SkillMatrixProcessNames { get; set; }
        public virtual DbSet<SkillMatrixPointTable> SkillMatrixPointTables { get; set; }

        #endregion

        #region Payroll

        public virtual DbSet<OvertimeEligibleEmployee> OvertimeEligibleEmployees { get; set; }
        public virtual DbSet<SalaryHead> SalaryHeads { get; set; }
        public virtual DbSet<SalarySetup> SalarySetups { get; set; }
        public virtual DbSet<EmployeeGradeSalaryPercentage> EmployeeGradeSalaryPercentages { get; set; }
        public virtual DbSet<AttendanceBonus> AttendanceBonus { get; set; }
        public virtual DbSet<StampAmount> StampAmount { get; set; }
        public virtual DbSet<SalaryAdvance> SalaryAdvance { get; set; }
        public virtual DbSet<PayrollExcludedEmployeeFromSalaryProcess> PayrollExcludedEmployeeFromSalaryProcess { get; set; }

        #endregion

        #region Accounting

        public virtual DbSet<Acc_ActiveCompanySector> Acc_ActiveCompanySector { get; set; }
        public virtual DbSet<Acc_BankReconcilationMaster> Acc_BankReconcilationMaster { get; set; }
        public virtual DbSet<Acc_BankReconciliationDetail> Acc_BankReconciliationDetail { get; set; }
        public virtual DbSet<Acc_BankVoucherManual> Acc_BankVoucherManual { get; set; }
        public virtual DbSet<Acc_CompanySector> Acc_CompanySector { get; set; }
        public virtual DbSet<Acc_ControlAccounts> Acc_ControlAccounts { get; set; }
        public virtual DbSet<Acc_CostCentre> Acc_CostCentre { get; set; }
        public virtual DbSet<Acc_DepreciationChart> Acc_DepreciationChart { get; set; }
        public virtual DbSet<Acc_FinancialPeriod> Acc_FinancialPeriod { get; set; }
        public virtual DbSet<Acc_GLAccounts> Acc_GLAccounts { get; set; }
        public virtual DbSet<Acc_OpeningClosing> Acc_OpeningClosing { get; set; }
        public virtual DbSet<Acc_PermitedChartOfAccount> Acc_PermitedChartOfAccount { get; set; }
        public virtual DbSet<Acc_SalaryMapping> Acc_SalaryMapping { get; set; }
        public virtual DbSet<Acc_VoucherDetail> Acc_VoucherDetail { get; set; }
        public virtual DbSet<Acc_VoucherLimit> Acc_VoucherLimit { get; set; }
        public virtual DbSet<Acc_VoucherMaster> Acc_VoucherMaster { get; set; }
        public virtual DbSet<Acc_VoucherToCostcentre> Acc_VoucherToCostcentre { get; set; }
        public virtual DbSet<Acc_Currency> Acc_Currency { get; set; }
        public virtual DbSet<Acc_CostCentreMultiLayer> Acc_CostCentreMultiLayer { get; set; }
        public virtual DbSet<Acc_GLAccounts_Hidden> Acc_GLAccounts_Hiddens { get; set; }
        public virtual DbSet<Acc_GLAccounts_Hidden_Status> Acc_GLAccounts_Hidden_Statuses { get; set; }

        #endregion

        #region Merchandising

        public DbSet<OM_SampleStyle> OM_SampleStyle { get; set; }
        public virtual DbSet<OM_SampleOrder> OM_SampleOrder { get; set; }
        public virtual DbSet<OM_BuyerTnaTemplate> OM_BuyerTnaTemplate { get; set; }
        public virtual DbSet<PROD_CuttingProcessStyleActive> PROD_CuttingProcessStyleActive { get; set; }
        public virtual DbSet<OM_Component> OM_Component { get; set; }
        public virtual DbSet<MailInformation> MailInformations { get; set; }
        public virtual DbSet<MailSend> MailSends { get; set; }

        public virtual DbSet<Mrc_SupplierCompany> Mrc_SupplierCompany { get; set; }

        public virtual DbSet<OvertimeSettings> OvertimeSettingss { get; set; }
        public virtual DbSet<PoliceStation> PoliceStations { get; set; }
        public virtual DbSet<SMSInformation> SMSInformations { get; set; }
        public virtual DbSet<OM_YarnConsumption> OM_YarnConsumption { get; set; }
        public virtual DbSet<VBuyerOrder> VBuyerOrders { get; set; }
        public virtual DbSet<VBuyOrdShipDetail> VBuyOrdShipDetails { get; set; }
        public virtual DbSet<OM_Style> OM_Style { get; set; }
        public virtual DbSet<OM_Buyer> OM_Buyer { get; set; }
        public virtual DbSet<OM_Merchandiser> OM_Merchandiser { get; set; }
        public virtual DbSet<OM_Season> OM_Season { get; set; }
        public virtual DbSet<OM_BuyerOrder> OM_BuyerOrder { get; set; }
        public virtual DbSet<OM_BuyOrdStyle> OM_BuyOrdStyle { get; set; }

        #endregion

        #region Inventory
        public virtual DbSet<Inventory_GreyIssue> Inventory_GreyIssue { get; set; }
        public virtual DbSet<Inventory_GreyIssueDetail> Inventory_GreyIssueDetail { get; set; }
        public virtual DbSet<Inventory_Group> Inventory_Group { get; set; }
        public virtual DbSet<Inventory_Item> Inventory_Item { get; set; }
        public virtual DbSet<Inventory_ItemLedger> Inventory_ItemLedger { get; set; }
        public virtual DbSet<Inventory_ItemStore> Inventory_ItemStore { get; set; }
        public virtual DbSet<Inventory_ItemStoreDetail> Inventory_ItemStoreDetail { get; set; }
        public virtual DbSet<Inventory_QualityCertificate> Inventory_QualityCertificate { get; set; }
        public virtual DbSet<Inventory_QualityCertificateDetail> Inventory_QualityCertificateDetails { get; set; }
        public virtual DbSet<Inventory_SubGroup> Inventory_SubGroup { get; set; }
        public virtual DbSet<Inventory_GoodsReceivingNote> Inventory_GoodsReceivingNotes { get; set; }
        public virtual DbSet<Inventory_ApprovalStatus> Inventory_ApprovalStatus { get; set; }
        public virtual DbSet<Inventory_Brand> Inventory_Brand { get; set; }
        public virtual DbSet<Inventory_MaterialRequisition> Inventory_MaterialRequisition { get; set; }
        public virtual DbSet<Inventory_MaterialRequisitionDetail> Inventory_MaterialRequisitionDetail { get; set; }
        public virtual DbSet<Inventory_PurchaseRequisitionApprovalFlowConfiguration> Inventory_PurchaseRequisitionApprovalFlowConfiguration { get; set; }
        public virtual DbSet<Inventory_PurchaseType> Inventory_PurchaseType { get; set; }
        public virtual DbSet<Inventory_RequsitionType> Inventory_RequsitionType { get; set; }
        public virtual DbSet<Inventory_Size> Inventory_Size { get; set; }
        public virtual DbSet<Inventory_StoreLedger> Inventory_StoreLedger { get; set; }
        public virtual DbSet<Inventory_AuthorizedPerson> Inventory_AuthorizedPerson { get; set; }
        public virtual DbSet<Inventory_StorePurchaseRequisition> Inventory_StorePurchaseRequisition { get; set; }
        public virtual DbSet<Inventory_StorePurchaseRequisitionDetail> Inventory_StorePurchaseRequisitionDetail { get; set; }
        public virtual DbSet<Inventory_StorePurchaseRequisitionHistory> Inventory_StorePurchaseRequisitionHistory { get; set; }
        public virtual DbSet<Pro_Batch> Pro_Batch { get; set; }
        public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; }

        #endregion

        #region Planning
        public virtual DbSet<OM_TnaActivityLog> OM_TnaActivityLog { get; set; }
        public virtual DbSet<PLAN_Activity> PLAN_Activity { get; set; }
        public virtual DbSet<PLAN_Process> PLAN_Process { get; set; }
        public virtual DbSet<PLAN_ProcessTemplate> PLAN_ProcessTemplate { get; set; }
        public virtual DbSet<PLAN_Program> PLAN_Program { get; set; }
        public virtual DbSet<PLAN_ResponsiblePerson> PLAN_ResponsiblePerson { get; set; }
        public virtual DbSet<PLAN_TNA> PLAN_TNA { get; set; }
        public virtual DbSet<PLAN_TNA_Template> PLAN_TNA_Template { get; set; }
        public virtual DbSet<PLAN_ProductionLine> PLAN_ProductionLines { get; set; }
        public virtual DbSet<PLAN_StyleUF> PLAN_StyleUF { get; set; }

        #endregion

        #region Production

        public virtual DbSet<PROD_CuttingTag> PROD_CuttingTag { get; set; }
        public virtual DbSet<PROD_CuttingSequence> PROD_CuttingSequence { get; set; }
        public virtual DbSet<PROD_CuttingTagSupplier> PROD_CuttingTagSupplier { get; set; }
        public virtual DbSet<VwCuttFabReject> VwCuttFabRejects { get; set; }
        
        #endregion

        #region Commercial

        public virtual DbSet<COMMLcInfo> COMMLcInfos { get; set; }
        public virtual DbSet<COMMLcStyle> COMMLcStyles { get; set; }
        public virtual DbSet<CommBbLcInfo> CommBbLcInfos { get; set; }
        public virtual DbSet<CommCashBbLcInfo> CommCashBbLcInfos { get; set; }
        public virtual DbSet<CommCashBbLcDetail> CommCashBbLcDetails { get; set; }
        public virtual DbSet<CommBbLcPurchase> CommBbLcPurchases { get; set; }
        public virtual DbSet<CommPurchaseOrder> CommPurchaseOrders { get; set; }
        public virtual DbSet<CommPurchaseOrderDetail> CommPurchaseOrderDetails { get; set; }
        public virtual DbSet<CommBbLcPurchaseCommon> CommBbLcPurchaseCommons { get; set; }
        public virtual DbSet<CommBank> CommBanks { get; set; }
        public virtual DbSet<CommExport> CommExports { get; set; }
        public virtual DbSet<CommReceive> CommReceives { get; set; }
        public virtual DbSet<CommReceiveDetail> CommReceiveDetails { get; set; }
        public virtual DbSet<CommImportExportPerformance> CommImportExportPerformances { get; set; }
        public virtual DbSet<CommBankAdvice> CommBankAdvices { get; set; }
        public virtual DbSet<CommAccHead> CommAccHeads { get; set; }
        public virtual DbSet<VwCommLcInfo> VwCommLcInfos { get; set; }
     
        #endregion

        #region Tracking

        #endregion

        #region CRM

        public virtual DbSet<CRMDocumentationOperation> CRMDocumentationOperations { get; set; }

        public virtual DbSet<CRMDocumentationReport> CRMDocumentationReports { get; set; }

        public virtual DbSet<CRMFeedback> CRMFeedbacks { get; set; }

        public virtual DbSet<CRMCollaborator> CRMCollaborators { get; set; }

        #endregion

        #region Task Management

        #endregion

        #region Others

        public virtual DbSet<SqlReportParameter> SqlReportParameters { get; set; }

        #endregion

        #endregion

        #region stored procedures

        #region HRM

        public virtual ObjectResult<SPGetSpecificEmployeeActiveInfo_Result> SPGetSpecificEmployeeActiveInfo(Nullable<Guid> employeeId, Nullable<DateTime> fromDate)
        {
            var EmployeeID = employeeId.HasValue ?
                new ObjectParameter("EmployeeID", employeeId) :
                new ObjectParameter("EmployeeID", typeof (Guid));

            var FromDate = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof (DateTime));

            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetSpecificEmployeeActiveInfo_Result>("SPGetSpecificEmployeeActiveInfo", EmployeeID, FromDate);
        }

        public virtual List<SPGetAllEmployeeInfo_Result> SPGetAllEmployeeInfo(ObjectParameter companyIdParam, ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam, ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter departmentSectionIdParam, ObjectParameter departmentLineIdParam,
            ObjectParameter employeeTypeIdParam, ObjectParameter employeeGradeIdParam,
            ObjectParameter employeeDesignationIdParam,
            ObjectParameter genderIdParam, ObjectParameter employeeCardIdParam,
            ObjectParameter employeeNameParam, ObjectParameter employeeMobileNumberParam,
            ObjectParameter employeeStatusParam,
            ObjectParameter userNameParam, ObjectParameter fromDateParam,
            ObjectParameter startRowIndexParam, ObjectParameter maxRowsParam,
            ObjectParameter sortFieldParam, ObjectParameter sortDirectionParam,
            out int rowCountParam)
        {
            var TotalRows = new ObjectParameter("RowCount", typeof (int));


            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetAllEmployeeInfo_Result>("SPGetAllEmployeeInfo",
                companyIdParam, branchIdParam, branchUnitIdParam,
                branchUnitDepartmentIdParam,
                departmentSectionIdParam, departmentLineIdParam, employeeTypeIdParam,
                employeeGradeIdParam, employeeDesignationIdParam,
                genderIdParam, employeeCardIdParam,
                employeeNameParam, employeeMobileNumberParam,
                employeeStatusParam, userNameParam, fromDateParam,
                startRowIndexParam, maxRowsParam,
                sortFieldParam, sortDirectionParam,
                TotalRows).ToList();



            rowCountParam = Convert.ToInt32(TotalRows.Value);

            return result;
        }

        public virtual List<SPGetAllBranchInfo_Result> SPGetAllBranchInfo(ObjectParameter companyIdParam,
            ObjectParameter branchNameParam,
            ObjectParameter userNameParam,
            ObjectParameter startRowIndexParam,
            ObjectParameter maxRowsParam,
            ObjectParameter sortFieldParam,
            ObjectParameter sortDirectionParam,
            out int rowCountParam)
        {

            var TotalRows = new ObjectParameter("RowCount", typeof (int));


            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetAllBranchInfo_Result>("SPGetAllBranchInfo",
                companyIdParam, branchNameParam,
                userNameParam, startRowIndexParam,
                maxRowsParam, sortFieldParam, sortDirectionParam,
                TotalRows).ToList();



            rowCountParam = Convert.ToInt32(TotalRows.Value);

            return result;
        }

        public virtual List<SPGetSpecificEmployeeCompanyInfo_Result> SPGetSpecificEmployeeCompanyInfo(ObjectParameter fromDateParameter, ObjectParameter employeeIdParameter, ObjectParameter employeeCompanyInfoIdParameter)
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetSpecificEmployeeCompanyInfo_Result>("SPGetSpecificEmployeeCompanyInfo",
                fromDateParameter, employeeIdParameter, employeeCompanyInfoIdParameter).ToList();

            return result;
        }

        public virtual List<SPShortLeaveSummary_Result> SPGetShortLeaveSummary(ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter departmentSectionIdParam, ObjectParameter departmentLineIdParam,
            ObjectParameter employeeCardIdParam, ObjectParameter fromDateParam,
            ObjectParameter toDateParam, ObjectParameter userNameParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPShortLeaveSummary_Result>("SPShortLeaveSummary",
                branchUnitDepartmentIdParam,
                departmentSectionIdParam, departmentLineIdParam,
                employeeCardIdParam, fromDateParam, toDateParam,
                userNameParam).ToList();


            return result;
        }

        public virtual List<SPShortLeaveDetail_Result> SPGetShortLeaveDetail(ObjectParameter companyIdParam, ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam, ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter departmentSectionIdParam, ObjectParameter departmentLineIdParam,
            ObjectParameter employeeIdParam, ObjectParameter fromDateParam,
            ObjectParameter toDateParam, ObjectParameter reasonTypeParam,
            ObjectParameter userNameParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPShortLeaveDetail_Result>("SPShortLeaveDetail",
                companyIdParam, branchIdParam, branchUnitIdParam,
                branchUnitDepartmentIdParam,
                departmentSectionIdParam, departmentLineIdParam, employeeIdParam,
                fromDateParam, toDateParam, reasonTypeParam,
                userNameParam).ToList();


            return result;
        }

        public virtual List<SPGetEmployeesForWorkShift_Result> SPGetEmployeesForWorkShift(
            ObjectParameter employeeCardIdParam,
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter sectionIdParam,
            ObjectParameter lineIdParam,
            ObjectParameter workGroupIdParam,
            ObjectParameter branchUnitWorkShiftIdParam,
            ObjectParameter employeeTypeIdParam,
            ObjectParameter checkDateParam,
            ObjectParameter userNameParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetEmployeesForWorkShift_Result>("SPGetEmployeesForWorkShift",
                employeeCardIdParam,
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                sectionIdParam,
                lineIdParam,
                workGroupIdParam,
                branchUnitWorkShiftIdParam,
                employeeTypeIdParam,
                checkDateParam,
                userNameParam).ToList();


            return result;
        }

        #endregion

        #region Payroll

        public virtual List<SPGetAdvanceSalarySheet_Result> SPGetAdvanceSalarySheet(ObjectParameter employeeCardIdParam,
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter sectionIdParam,
            ObjectParameter lineIdParam,
            ObjectParameter fromDateParam,
            ObjectParameter toDateParam,
            ObjectParameter employeeTypeParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetAdvanceSalarySheet_Result>("SPGetAdvanceSalarySheet",
                employeeCardIdParam,
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                sectionIdParam,
                lineIdParam,
                fromDateParam,
                toDateParam,
                employeeTypeParam).ToList();


            return result;
        }

        public virtual List<SPGetEmployeeBonusSheet_Result> SPGetEmployeeBonusSheet(ObjectParameter employeeCardIdParam,
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter sectionIdParam,
            ObjectParameter lineIdParam,
            ObjectParameter employeeTypeParam,
            ObjectParameter effectiveDateParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetEmployeeBonusSheet_Result>("SPGetEmployeeBonusSheet",
                employeeCardIdParam,
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                sectionIdParam,
                lineIdParam,
                employeeTypeParam,
                effectiveDateParam
                ).ToList();


            return result;
        }

        public virtual List<SPGetExtraOTSheet_Result> SPGetExtraOTSheetInfo(
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter departmentSectionIdParam,
            ObjectParameter departmentLineIdParam,
            ObjectParameter employeeTypeIdParam,
            ObjectParameter employeeCardIdParam,
            ObjectParameter fromDateParam,
            ObjectParameter toDateParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetExtraOTSheet_Result>("SPGetExtraOTSheet",
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                departmentSectionIdParam,
                departmentLineIdParam,
                employeeTypeIdParam,
                employeeCardIdParam,
                fromDateParam,
                toDateParam
                ).ToList();


            return result;
        }


        public virtual List<SPGetEmployeesForAdvanceSalary_Result> SPGetEmployeesForAdvanceSalary(
            ObjectParameter employeeCardIdParam,
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter sectionIdParam,
            ObjectParameter lineIdParam,
            ObjectParameter percentageParam,
            ObjectParameter employeeTypeParam,
            ObjectParameter receivedDateParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetEmployeesForAdvanceSalary_Result>("SPGetEmployeesForAdvanceSalary",
                employeeCardIdParam,
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                sectionIdParam,
                lineIdParam,
                percentageParam,
                employeeTypeParam,
                receivedDateParam).ToList();


            return result;
        }


        public virtual List<SPGetEmployeesForBonus_Result> SPGetEmployeesForBonus(ObjectParameter employeeCardIdParam,
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter sectionIdParam,
            ObjectParameter lineIdParam,
            ObjectParameter employeeTypeParam,
            ObjectParameter effectiveDateParam
            )
        {

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGetEmployeesForBonus_Result>("SPGetEmployeesForBonus",
                employeeCardIdParam,
                companyIdParam,
                branchIdParam,
                branchUnitIdParam,
                branchUnitDepartmentIdParam,
                sectionIdParam,
                lineIdParam,
                employeeTypeParam,
                effectiveDateParam).ToList();


            return result;
        }

        public virtual List<spPayrollGetExcludedEmployeeFromSalaryProcessInfo_Result> spPayrollGetExcludedEmployeeFromSalaryProcessInfo(
            ObjectParameter companyIdParam,
            ObjectParameter branchIdParam,
            ObjectParameter branchUnitIdParam,
            ObjectParameter branchUnitDepartmentIdParam,
            ObjectParameter departmentSectionIdParam,
            ObjectParameter departmentLineIdParam,
            ObjectParameter employeeTypeIdParam,
            ObjectParameter employeeCardIdParam,
            ObjectParameter yearParam,
            ObjectParameter monthParam,
            ObjectParameter fromDateParam,
            ObjectParameter toDateParam,
            ObjectParameter userNameParam,
            ObjectParameter startRowIndexParam,
            ObjectParameter maxRowsParam,
            ObjectParameter sortFieldParam,
            ObjectParameter sortDirectionParam,
            out int rowCountParam)
        {
            var TotalRows = new ObjectParameter("RowCount", typeof (int));

            var result = ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<spPayrollGetExcludedEmployeeFromSalaryProcessInfo_Result>("spPayrollGetExcludedEmployeeFromSalaryProcessInfo",
                companyIdParam, branchIdParam, branchUnitIdParam, branchUnitDepartmentIdParam, departmentSectionIdParam, departmentLineIdParam,
                employeeTypeIdParam, employeeCardIdParam, yearParam, monthParam, fromDateParam, toDateParam, userNameParam,
                startRowIndexParam, maxRowsParam, sortFieldParam, sortDirectionParam, TotalRows).ToList();

            rowCountParam = Convert.ToInt32(TotalRows.Value);

            return result;

        }


        #endregion

        #region Accounting

        public virtual ObjectResult<SPVoucherReport_Result> SPVoucherReport(Nullable<long> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof (long));
            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPVoucherReport_Result>("SPVoucherReport", idParameter);
        }

        public virtual ObjectResult<SPGeneralLedgerDetailReport_Result> SPGeneralLedgerDetailReport(string accountCode, Nullable<int> sectorId, Nullable<int> costCentreId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var accountCodeParameter = accountCode != null ?
                new ObjectParameter("AccountCode", accountCode) :
                new ObjectParameter("AccountCode", typeof (string));

            var sectorIdParameter = sectorId.HasValue ?
                new ObjectParameter("SectorId", sectorId) :
                new ObjectParameter("SectorId", typeof (int));

            var costCentreIdParameter = costCentreId.HasValue ?
                new ObjectParameter("CostCentreId", costCentreId) :
                new ObjectParameter("CostCentreId", typeof (int));

            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof (System.DateTime));

            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof (System.DateTime));

            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SPGeneralLedgerDetailReport_Result>("SPGeneralLedgerDetailReport", accountCodeParameter, sectorIdParameter, costCentreIdParameter, fromDateParameter, toDateParameter);
        }

        #endregion

        #endregion

        #region views

        #region User Right Management

        public DbSet<VwModuleFeature> VwModuleFeatures { get; set; }
        public DbSet<UserMerchandiser> UserMerchandisers { get; set; }

        #endregion

        #region HRM 

        public DbSet<VwSkillMatrixEmployee> VwSkillMatrixEmployees { get; set; }
        public DbSet<VwSkillMatrix> VwSkillMatrixs { get; set; }
        public DbSet<VwPenaltyEmployee> VwPenaltyEmployee { get; set; }
        public virtual DbSet<VEmployeeDailyAttendanceDetail> VEmployeeDailyAttendanceDetails { get; set; }
        public virtual DbSet<VEmployeeCompanyInfoDetail> VEmployeeCompanyInfoDetails { get; set; }
        public virtual DbSet<VEmployeeWorkGroupDetail> VEmployeeWorkGroupDetails { get; set; }
        public virtual DbSet<VEmployeeWorkShiftDetail> VEmployeeWorkShiftDetails { get; set; }
        public virtual DbSet<VOutStationDutyDetail> VOutStationDutyDetails { get; set; }
        public virtual DbSet<VEmployeeIDCardInfoInEnglish> VEmployeeIDCardInfoInEnglish { get; set; }
        public virtual DbSet<VEmployeeIDCardInfoInBengali> VEmployeeIDCardInfoInBengali { get; set; }
        public virtual DbSet<VEmployeeShortLeave> VEmployeeShortLeave { get; set; }
        public virtual DbSet<VEmployeeSkillDetail> VEmployeeSkillDetails { get; set; }

        #endregion

        #region Payroll

        public virtual DbSet<VOvertimeEligibleEmployeeDetail> VOvertimeEligibleEmployeeDetails { get; set; }

        #endregion

        #region Accounting

        public virtual DbSet<VAccVoucherMaster> VAccVoucherMasters { get; set; }
        public virtual DbSet<VStylePayment> VStylePayments { get; set; }

        #endregion

        #region Inventory  

        public virtual DbSet<Inventory_StyleShipmentDetail> Inventory_StyleShipmentDetail { get; set; }
        public virtual DbSet<VwInventoryStyleShipment> VwInventoryStyleShipments { get; set; }
        public virtual DbSet<VInventoryItemStore> VInventoryItemStores { get; set; }
        public virtual DbSet<VInvItem> VInvItems { get; set; }
        public virtual DbSet<VMaterialRequisition> VMaterialRequisitions { get; set; }
        public virtual DbSet<VMaterialIssue> VMaterialIssues { get; set; }
        public virtual DbSet<VMaterialLoanReturn> VMaterialLoanReturns { get; set; }
        public virtual DbSet<VLoanGiven> VLoanGivens { get; set; }
        public virtual DbSet<VwBookingDetailReport> VwBookingDetailReports { get; set; }
        public virtual DbSet<VwBookingSummaryReport> VwBookingSummaryReports { get; set; }
        public virtual DbSet<VwMaterialReceiveAgainstPo> VwMaterialReceiveAgainstPos { get; set; }
        public virtual DbSet<VItemReceiveDetail> VItemReceiveDetails { get; set; }
        public virtual DbSet<VMaterialIssueDetail> VMaterialIssueDetails { get; set; }
        public virtual DbSet<VStorePurchaseRequisitionDetail> VStorePurchaseRequisitionDetails { get; set; }
        public virtual DbSet<VQualityCertificateDetail> VQualityCertificateDetails { get; set; }
        public virtual DbSet<VQualityCertificate> VQualityCertificates { get; set; }
        public virtual DbSet<VGoodsReceivingNote> VGoodsReceivingNotes { get; set; }
        public virtual DbSet<VStorePurchaseRequisition> VStorePurchaseRequisitions { get; set; }
        public virtual DbSet<VMaterialIssueRequisition> VMaterialIssueRequisitions { get; set; }
        public virtual DbSet<VwAdvanceMaterialIssue> VwAdvanceMaterialIssues { get; set; }
        public virtual DbSet<VwAdvanceMaterialIssueDetail> VwAdvanceMaterialIssueDetails { get; set; }

        #endregion

        #region Merchandising

        public virtual DbSet<VOMBuyOrdStyle> VOMBuyOrdStyles { get; set; }
        public virtual DbSet<VwConsuptionOrderStyle> VwConsuptionOrderStyles { get; set; }
        public virtual DbSet<VOM_BuyOrdStyle> VOM_BuyOrdStyles { get; set; }
        public virtual DbSet<VwStyleFollowupStatus> VwStyleFollowupStatus { get; set; }
        public virtual DbSet<VStyle> VStyles { get; set; }
        public virtual DbSet<VwCompConsumptionOrderStyle> VwCompConsumptionOrderStyles { get; set; }
        public virtual DbSet<VConsumption> VConsumptions { get; set; }
        public virtual DbSet<VConsumptionDetail> VConsumptionDetails { get; set; }
        public virtual DbSet<VCompConsumption> VCompConsumptions { get; set; }
        public virtual DbSet<VCompConsumptionDetail> VCompConsumptionDetails { get; set; }
        public virtual DbSet<VYarnConsumption> VYarnConsumptions { get; set; }
        public virtual DbSet<VCostOrderStyle> VCostOrderStyles { get; set; }
        public virtual DbSet<VOrderStyleDetail> VOrderStyleDetails { get; set; }
        public virtual DbSet<VwLot> VwLots { get; set; }
        public virtual DbSet<VwFabricOrderDetail> VwStyleFabricOrderDetails { get; set; }

        #endregion

        #region Planning

        public virtual DbSet<VwTargetProduction> VwTargetProductions { get; set; }
        public virtual DbSet<VProgramDetail> VProgramDetails { get; set; }
        public virtual DbSet<VwProgram> VwPrograms { get; set; }
        public virtual DbSet<VwAssignedProgram> VwAssignedPrograms { get; set; }

        #endregion

        #region Production

        public virtual DbSet<VwProdDyeingSpChallanDetail> VDyeingSpChallanDetail { get; set; }
        public virtual DbSet<VwProdBatchDetail> BatchDetail { get; set; }
        public virtual DbSet<VwSewingOutputProcess> VwSewingOutputProcesses { get; set; }
        public virtual DbSet<VwSewingOutputDetail> VwSewingOutputDetails { get; set; }
        public virtual DbSet<VwSewingInputProcess> VwSewingInputProcess { get; set; }
        public virtual DbSet<VwSewingInputProcessDetail> VwSewingInputProcessDetails { get; set; }
        public virtual DbSet<VwSewingInputDetail> VwSewingInputDetails { get; set; }
        public virtual DbSet<PROD_CutBank> CutBanks { get; set; }
        public virtual DbSet<VwCutBank> VwCutBanks { get; set; }
        public virtual DbSet<VwPartyWiseCuttiongProcess> VwPartyWiseCuttiongProcesses { get; set; }
        public virtual DbSet<VwCuttingProcessStyleActive> VwCuttingProcessStyleActives { get; set; }
        public virtual DbSet<VwCuttingBatch> vwCuttingBatches { get; set; }
        public virtual DbSet<VProBatch> VProBatches { get; set; }
        public virtual DbSet<VMachine> VMachines { get; set; }
        public virtual DbSet<VProcessor> VProcessors { get; set; }
        public virtual DbSet<VwStanderdMinValDetail> VwStanderdMinValDetails { get; set; }
        public virtual DbSet<VProductionDetail> VProductionDetails { get; set; }
        public virtual DbSet<VSubProcess> VSubProcesses { get; set; }
        public virtual DbSet<VwProduction> VwProductions { get; set; }
        public virtual DbSet<VwCuttingSequence> VwCuttingSequence { get; set; }
        public virtual DbSet<VwCuttingTag> VwCuttingTags { get; set; }
        public virtual DbSet<VwReceivedFabricProductionSummary> VwReceivedFabricProductionSummaries { get; set; }
        public virtual DbSet<VwCuttingApproval> VwCuttingApprovals { get; set; }
        public virtual DbSet<VwProcessDelivery> VwProcessDeliveries { get; set; }
        public virtual DbSet<VwFinishingProcessDetail> VwFinishingProcessDetails { get; set; }
        public virtual DbSet<VwFinishingProcess> VwFinishingProcesses { get; set; }
        public virtual DbSet<VwKnittingRoll> VwKnittingRolls { get; set; }
        public virtual DbSet<VwFabricOrder> VwFabricOrders { get; set; }
        public virtual DbSet<VwBatchRoll> VwBatchRolls { get; set; }



        #endregion

        #region Commercial

        public virtual DbSet<VwPurchaseOrderDetail> VwPurchaseOrderDetail { get; set; }

        #endregion

        #region Common

        public virtual DbSet<VwMailSend> VwMailSends { get; set; }
        public virtual DbSet<VwParty>VwParties { get; set; }

        #endregion

        #region Maintenance  

        public virtual DbSet<VwChallanReceiveMaster> VwChallanReceiveMasters { get; set; }
        public virtual DbSet<VwReceiveDetail> VwReceiveDetails { get; set; }
        public virtual DbSet<VwReturnableChallanReceive> VwReturnableChallanReceives { get; set; }
        public virtual DbSet<VwReturnableChallan> VwReturnableChallans { get; set; }

        #endregion

        #region Tracking

        #endregion

        #region CRM

        #endregion

        #region Task Management

        public DbSet<vwTmTaskInformation> VwTmTaskInformations { get; set; }

        #endregion

        #region Others

        public DbSet<VUserReport> VUserReports { get; set; }

        #endregion

        #endregion
    }
}
