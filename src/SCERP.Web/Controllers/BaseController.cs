using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.ICRMManager;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager;
using SCERP.Common;
using SCERP.Web.App_Start;


namespace SCERP.Web.Controllers
{
    [SessionTimeout]
    public abstract class BaseController : Controller
    {

        #region Manager
        public Manager Manager
        {
            get
            {
                return Manager.Instance;
            }
        }
        #endregion

        #region Common
        public JsonResult CreateJsonResult(object data)
        {
            return new JsonResult { Data = data };
        }
        public JsonResult Reload()
        {
            return Json(new { Success = true, Reload = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ErrorResult(string message)
        {
            return Json(new
            {
                Success = false,
                Message = message,

            }, JsonRequestBehavior.AllowGet);
        }

        internal List<string> CurrentErrors
        {
            get
            {
                return ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
        }

        public JsonResult ErrorResult()
        {
            return Json(new { Success = false, Errors = CurrentErrors });
        }

        public JsonResult ErrorMessageResult()
        {
            const string message = "Failed to save data.";
            return Json(new { Success = false, Message = message, Error = true }, JsonRequestBehavior.AllowGet);
        }
        

        public IMeasurementUnitManager MeasurementUnitManager
        {
            get
            {
                return Manager.MeasurementUnitManager; 
            }
        }
        public ICustomSqlQuaryManager CustomSqlQuaryManager
        {
            get
            {
                return Manager.CustomSqlQuaryManager; 
            }
        }

        public IDocumentManager DocumentManager
        {
            get
            {
                return Manager.DocumentManager; 
            }
        }

        #endregion


        public   ResponsModel ResponsModel  { get; set; }
        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public ICityManager CityManager
        {
            get
            {
                return Manager.CityManager; 
            }
        }
        public IStateManager StateManager
        {
            get
            {
                return Manager.StateManager; 
            }
        }
        public IUserManager UserManager
        {
            get { return Manager.UserManager; }
        }
        public IEmployeeSkillManager EmployeeSkillManager
        {
            get { return Manager.EmployeeSkillManager; }
        }
        public IEfficiencyRateManager EfficiencyRateManager
        {
            get { return Manager.EfficiencyRateManager; }
        }

        public ISkillOperationManager SkillOperationManager
        {
            get { return Manager.SkillOperationManager; }
        }

        public ISkillSetDifficultyManager SkillSetDifficultyManager
        {
            get { return Manager.SkillSetDifficultyManager; }
        }

        public ISkillSetCategoryManager SkillSetCategoryManager
        {
            get { return Manager.SkillSetCategoryManager; }
        }

        public IOutStationDutyManager OutStationDutyManager
        {
            get { return Manager.OutStationDutyManager; }
        }

        public ICurrencyManagerCommon CurrencyManagerCommon
        {
            get { return Manager.CurrencyManagerCommon; }
        }

        public ISupplierCompanyManager SupplierCompanyManager
        {
            get { return Manager.SupplierCompanyManager; }
        }

        public IEmployeeGradeSalaryPercentageManager EmployeeGradeSalaryPercentageManager
        {
            get { return Manager.EmployeeGradeSalaryPercentageManager; }
        }
        public IPayrollReportManager PayrollReportManager
        {
            get { return Manager.PayrollReportManager; }
        }
        public IEmployeeSalaryManager EmployeeSalaryManager
        {
            get { return Manager.EmployeeSalaryManager; }
        }

        public IStampAmountManager StampAmountManager
        {
            get { return Manager.StampAmountManager; }
        }

        public ISalaryAdvanceManager SalaryAdvanceManager
        {
            get { return Manager.SalaryAdvanceManager; }
        }

        public IEmployeeBonusManager EmployeeBonusManager
        {
            get { return Manager.EmployeeBonusManager; }
        }

        public IAuthorizationTypeManager AuthorizationTypeManager
        {
            get { return Manager.AuthorizationTypeManager; }
        }
        public IEmployeeCompanyInfoManager EmployeeCompanyInfoManager
        {
            get { return Manager.EmployeeCompanyInfoManager; }
        }
        public ISalarySetupManager SalarySetupManager
        {
            get { return Manager.SalarySetupManager; }
        }

        public IEmployeeManager EmployeeManager
        {
            get { return Manager.EmployeeManager; }
        }
        public IEmployeeTypeManager EmployeeTypeManager
        {
            get { return Manager.EmployeeTypeManager; }
        }
        public IWorkShiftManager WorkShiftManager
        {
            get { return Manager.WorkShiftManager; }
        }

        public IDepartmentSectionManager DepartmentSectionManager
        {
            get { return Manager.DepartmentSectionManager; }
        }

        public IDepartmentLineManager DepartmentLineManager
        {
            get { return Manager.DepartmentLineManager; }
        }
        public SCERP.BLL.IManager.IHRMManager.IBranchUnitDepartmentManager BranchUnitDepartmentManager
        {
            get { return Manager.BranchUnitDepartmentManager; }
        }
        public IBranchUnitManager BranchUnitManager
        {
            get { return Manager.BranchUnitManager; }
        }
        public SCERP.BLL.IManager.IHRMManager.IUnitManager UnitManager
        {
            get { return Manager.UnitManager; }
        }

        public IFeedbackManager FeedbackManager
        {
            get { return Manager.FeedbackManager; }
        }

        public ISectionManager SectionManager
        {
            get { return Manager.SectionManager; }
        }
   
        public IAuthorizedPersonManager AuthorizedPersonManager
        {
            get { return Manager.AuthorizedPersonManager; }
        }

        public IBranchManager BranchManager
        {
            get { return Manager.BranceManager; }
        }


        public IDepartmentManager DepartmentManager
        {
            get { return Manager.DepartmentManager; }
        }

        public SCERP.BLL.IManager.ICRMManager.IProjectDocumentInfoManager ProjectDocumentInfoManager
        {
            get { return Manager.ProjectDocumentInfoManager; }
        }

        public IDistrictManager DistrictManager
        {
            get { return Manager.DistrictManager; }
        }

        public IAttendanceBonusSettingManager AttendanceBonusSettingManager
        {
            get { return Manager.AttendanceBonusSettingManager; }
        }

        public IPoliceStationManager PoliceStationManager
        {
            get { return Manager.PoliceStationManager; }
        }

        public ICompanyManager CompanyManager
        {
            get { return Manager.CompanyManager; }
        }

        public IBloodGroupManager BloodGroupManager
        {
            get { return Manager.BloodGroupManager; }
        }

        public ICountryManager CountryManager
        {
            get { return Manager.CountryManager; }
        }

        public ILineManager LineManager
        {
            get { return Manager.LineManager; }
        }

        public IUnitDepartmentManager UnitDepartmentManager
        {
            get { return Manager.UnitDepartmentManager; }
        }

        public IGenderManager GenderManager
        {
            get { return Manager.GenderManager; }
        }

        public IQuitTypeManager QuitTypeManager
        {
            get { return Manager.QuitTypeManager; }
        }


        public ISalaryMappingManager SalaryMappingManager
        {
            get { return Manager.SalaryMappingManager; }
        }


        //public IMachineManager MachineManager
        //{
        //    get { return Manager.MachineManager; }
        //}

        //public IPartyManager PartyManager
        //{
        //    get { return Manager.PartyManager; }
        //}
   
    }
}
