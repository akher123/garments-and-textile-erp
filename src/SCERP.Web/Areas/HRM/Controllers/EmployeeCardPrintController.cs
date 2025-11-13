using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Web.Helpers;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using System.Data.SqlClient;
using SCERP.Model.Custom;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeCardPrintController : BaseHrmController
    {
        private readonly int _pageSize = 9; //AppConfig.PageSize;

        private readonly int _maxRecord = 9;

        private readonly int _minRecord = 1;

        [AjaxAuthorize(Roles = "idcard-1,idcard-2,idcard-3")]
        public ActionResult Index(EmployeeCardPrintViewModel model)
        {
            Session["EmployeeCardInfos"] = null;

            ModelState.Clear();

            try
            {
                var languages = from LanguageType languageType in Enum.GetValues(typeof (LanguageType))
                    select new {Id = (int) languageType, Name = languageType.ToString()};

                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.LanguageTypes = languages;

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                var startPage = 0;

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.EmployeeCardInfos = EmployeeCardPrintManager.GetEmployeeIDCardInfoByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                Session["EmployeeCardInfos"] = model.EmployeeCardInfos;

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "idcard-2,idcard-3")]
        public ActionResult EmployeeCardPrint(EmployeeCardPrintViewModel model)
        {
            if (model.EmployeeIdList.Count < _minRecord)
            {
                return ErrorResult(string.Format("Please, select minimum {0} record!", _minRecord));
            }

            if (model.EmployeeIdList.Count > _maxRecord)
            {
                return ErrorResult(string.Format("Please, select maximum {0} record!", _maxRecord));
            }


            List<EmployeeCardPrintModel> employeeCardInfo = EmployeeCardPrintManager.GetEmployeeIDCardInfo(model.EmployeeIdList, model.SearchFieldModel);

            return View(employeeCardInfo);
        }

        [AjaxAuthorize(Roles = "idcardbackpage-1, idcardbackpage-2,idcardbackpage-3")]
        public ActionResult BackInfoIndex(EmployeeCardPrintViewModel model)
        {
            ModelState.Clear();

            try
            {
                List<SelectListItem> NoOfCard = new List<SelectListItem>();
                var languages = from LanguageType languageType in Enum.GetValues(typeof (LanguageType))
                    select new {Id = (int) languageType, Name = languageType.ToString()};

                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.LanguageTypes = languages;

                for (int i = 1; i <= _maxRecord; i++)
                {
                    NoOfCard.Add(new SelectListItem {Text = i.ToString(), Value = i.ToString()});
                }

                ViewBag.NoOfCard = NoOfCard;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "idcardbackpage-2,idcardbackpage-3")]
        public ActionResult EmployeeCardBackInfoPrint(EmployeeCardPrintViewModel model)
        {
            int noOfCard = 0;
            List<EmployeeCardInfo> employeeCardInfo;

            if (!string.IsNullOrEmpty(model.NoOfCard))
                noOfCard = Convert.ToInt32(model.NoOfCard);

            if (model.SearchFieldModel.SearchByCompanyId <= 0)
                return ErrorResult("Please, select a company!");

            if (model.SearchFieldModel.SearchLanguageId <= 0)
                return ErrorResult("Please, select a language!");

            employeeCardInfo = EmployeeCardPrintManager.GetCardBackInfo(model.SearchFieldModel.SearchByCompanyId, model.SearchFieldModel.SearchLanguageId, noOfCard);
            return View(employeeCardInfo);
        }





        [AjaxAuthorize(Roles = "idcard-1,idcard-2,idcard-3")]
        public ActionResult IndexNew(EmployeeCardPrintViewModel model)
        {
            Session["EmployeeCardInfos"] = null;

            ModelState.Clear();

            try
            {
                var languages = from LanguageType languageType in Enum.GetValues(typeof (LanguageType))
                    select new {Id = (int) languageType, Name = languageType.ToString()};

                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.LanguageTypes = languages;

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                var startPage = 0;

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.EmployeeCardInfos = EmployeeCardPrintManager.GetEmployeeIDCardInfoByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                Session["EmployeeCardInfos"] = model.EmployeeCardInfos;

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EmployeeCardPrintNew(EmployeeCardPrintViewModel model)
        {
            if (model.EmployeeIdList.Count < _minRecord)
            {
                return ErrorResult(string.Format("Please, select minimum {0} record!", _minRecord));
            }

            if (model.EmployeeIdList.Count > _maxRecord)
            {
                return ErrorResult(string.Format("Please, select maximum {0} record!", _maxRecord));
            }

            List<EmployeeCardPrintModel> employeeCardInfo = EmployeeCardPrintManager.GetEmployeeIDCardInfo(model.EmployeeIdList, model.SearchFieldModel);

            return View(employeeCardInfo);
        }
    }
}
