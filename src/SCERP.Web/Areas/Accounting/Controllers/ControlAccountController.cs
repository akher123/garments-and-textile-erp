using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class ControlAccountController : BaseController
    {
        private readonly IControlAccountManager _controlAccountManager;

        public ControlAccountController(IControlAccountManager controlAccountManager)
        {
            _controlAccountManager = controlAccountManager;
        }

        [AjaxAuthorize(Roles = "chartofaccounts-1,chartofaccounts-2,chartofaccounts-3")]
        public ActionResult ChartOfAccount(string searchKey)
        {
            List<Acc_ControlAccounts> controlAccounts = _controlAccountManager.GetAllControlAccounts();
            List<Acc_GLAccounts> glAccountses = _controlAccountManager.GetAllGLAccounts(searchKey);
           ChartOfAccountTreeBuilder accountTreeBuilder = new ChartOfAccountTreeBuilder(controlAccounts, glAccountses);

          return View(accountTreeBuilder.GetAccountChart(searchKey));
        //  return View(accountTreeBuilder.GetCartOfAccountTree(controlAccounts, glAccountses));
        }

        [AjaxAuthorize(Roles = "chartofaccounts-1,chartofaccounts-2,chartofaccounts-3")]
        public ActionResult ChartOfAccountPermission()
        {
            List<Acc_ControlAccounts> controlAccounts = _controlAccountManager.GetAllControlAccounts();
            List<Acc_GLAccounts> glAccountses = _controlAccountManager.GetAllGLAccounts("");
            List<Acc_CompanySector> companySectors = _controlAccountManager.GetAllCompanySector();
            ViewBag.companySectorId = new SelectList(companySectors, "Id", "SectorName");
            ChartOfAccountTreeBuilder accountTreeBuilder = new ChartOfAccountTreeBuilder(controlAccounts, glAccountses);
            return View(accountTreeBuilder.GetAccountChart(""));
        }

        [AjaxAuthorize(Roles = "chartofaccounts-2,chartofaccounts-3")]
        public ActionResult Create(string contentAccount) //"Id_ParentCode_ControlCode_ControlLevel_SortOrder"
        {
            //return Content("You can not create cost centre at this level !");

            int parentCode = Convert.ToInt32(contentAccount.Split('_')[1]);
            int shortOrder = Convert.ToInt32(contentAccount.Split('_')[4]);
            int newParentCode = Convert.ToInt32(contentAccount.Split('_')[2]);
            int controlLavel = Convert.ToInt32(contentAccount.Split('_')[3]);
            Acc_ControlAccounts controlAccountObj = new Acc_ControlAccounts();

            if (controlLavel < 4)
            {
                int controlCode = _controlAccountManager.GetMaxControlCode(newParentCode);
                if (controlCode == 1)
                    controlAccountObj.ControlCode = Convert.ToDecimal(newParentCode + "01");
                else
                    controlAccountObj.ControlCode = controlCode;

                controlAccountObj.ParentCode = newParentCode;
                controlAccountObj.ControlLevel = controlLavel + 1;
                controlAccountObj.SortOrder = shortOrder + 1;
                controlAccountObj.IsActive = true;
            }
            else if (controlLavel >= 4)
            {
                Acc_GLAccounts glAccounts = new Acc_GLAccounts();
                decimal gLAccountCode = _controlAccountManager.GetMaxGlControlCode(newParentCode);

                if (gLAccountCode == 1)
                    glAccounts.AccountCode = Convert.ToDecimal(newParentCode + "001");
                else
                    glAccounts.AccountCode = gLAccountCode;

                glAccounts.ControlCode = newParentCode;
                //glAccounts.AccountCode = gLAccountCode+1;

                ViewBag.BalanceType =
                    new SelectList(new[] {new {Id = "N/A", Value = "N/A"}, new {Id = "CR", Value = "CR"}, new {Id = "DR", Value = "DR"}}, "Id", "Value");
                ViewBag.AccountType =
                    new SelectList(new[] {new {Id = "N/A", Value = "N/A"}, new {Id = "CASH", Value = "CASH"}, new {Id = "BANK", Value = "BANK"}}, "Id", "Value");
                return View("CreateGLAccunt", glAccounts);
            }
            return View(controlAccountObj);
        }

        [AjaxAuthorize(Roles = "chartofaccounts-2,chartofaccounts-3")]
        [HttpPost]
        public ActionResult SaveControlAccount(Acc_ControlAccounts accControlAccount)
        {
            var result = _controlAccountManager.CheckExistingName(accControlAccount);
            if (result)
                return ErrorMessageResult();

            int index = 0;
            accControlAccount.IsActive = true;
            index = accControlAccount.Id == 0 ? _controlAccountManager.SaveControlAccount(accControlAccount) : _controlAccountManager.EditControlAccount(accControlAccount);
            return (index > 0) ? Reload() : ErrorMessageResult();
        }

        [AjaxAuthorize(Roles = "chartofaccounts-2,chartofaccounts-3")]
        public ActionResult SaveGlAccount(Acc_GLAccounts glAccount)
        {
            var result = _controlAccountManager.CheckExistingName(glAccount);
            if (result)
            {
                const string message = "Duplicate Account Head exists !";
                return Json(new {Success = false, Message = message, Error = true}, JsonRequestBehavior.AllowGet);
            }

            int index = 0;
            glAccount.IsActive = true;
            index = glAccount.Id == 0 ? _controlAccountManager.SaveglAccount(glAccount) : _controlAccountManager.EditglAccount(glAccount);
            return (index > 0) ? Reload() : ErrorMessageResult();
        }

        [AjaxAuthorize(Roles = "chartofaccounts-2,chartofaccounts-3")]
        public ActionResult Edit(string contentAccount)
        {
            int id = Convert.ToInt32(contentAccount.Split('_')[0]);
            int controleLavel = Convert.ToInt32(contentAccount.Split('_')[3]);
            if (controleLavel == 5)
            {
                Acc_GLAccounts glAccounts = _controlAccountManager.GetGLAccountsById(id) ?? new Acc_GLAccounts();
                ViewBag.BalanceType = new SelectList(new[] {new {Id = "N/A", Value = "N/A"}, new {Id = "CR", Value = "CR"}, new {Id = "DR", Value = "DR"}}, "Id", "Value", glAccounts.BalanceType);
                ViewBag.AccountType = new SelectList(new[] {new {Id = "N/A", Value = "N/A"}, new {Id = "CASH", Value = "CASH"}, new {Id = "BANK", Value = "BANK"}}, "Id", "Value", glAccounts.AccountType);
                return View("CreateGLAccunt", glAccounts);
            }
            Acc_ControlAccounts controlAccount = _controlAccountManager.GetControlAccountsById(id) ?? new Acc_ControlAccounts();
            return View("Create", controlAccount);
        }

        [AjaxAuthorize(Roles = "chartofaccounts-3")]
        public ActionResult Delete(string contentAccount)
        {
            int index = 0;
            string message = "";
            int id = Convert.ToInt32(contentAccount.Split('_')[0]);
            int controleLavel = Convert.ToInt32(contentAccount.Split('_')[3]);

            if (_controlAccountManager.CheckExistVoucherByGLId(controleLavel, id, ref message))
                return ErrorMessageResult();

            index = controleLavel == 5 ? _controlAccountManager.DeleteGLAccount(id) : _controlAccountManager.DeleteControlAccount(id);
            return index > 0 ? Reload() : ErrorResult();
        }

        [AjaxAuthorize(Roles = "chartofaccounts-2,chartofaccounts-3")]
        public ActionResult SavePermitedChartOfAccount(List<string> permitedChartOfAccount, int companySectorId)
        {
            List<Acc_PermitedChartOfAccount> paOfAccounts = new List<Acc_PermitedChartOfAccount>();
            if (permitedChartOfAccount != null)
                paOfAccounts.AddRange(permitedChartOfAccount.Select(parmintedCoA => new Acc_PermitedChartOfAccount()
                {
                    SectorId = companySectorId,
                    ControlCode = Convert.ToDecimal(parmintedCoA.Split('_')[0]),
                    ControlLevel = Convert.ToInt32(parmintedCoA.Split('_')[1]),
                    IsActive = true
                }));
            int index = _controlAccountManager.SavePermitedChartOfAccounts(paOfAccounts);
            return index > 0 ? Reload() : ErrorMessageResult();
        }

        [AjaxAuthorize(Roles = "chartofaccounts-1,chartofaccounts-2,chartofaccounts-3")]
        public ActionResult GetPermitedChartOfAccount(int companySectorId)
        {
            var permitedChartOfAccounts = _controlAccountManager.GetPermitedChartOfAccount(companySectorId);
            return Json(permitedChartOfAccounts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParentName(string parentCode)
        {
            string controlName = _controlAccountManager.GetControlNameByCode(parentCode);
            return Json(new {Success = true, Message = controlName, Error = false}, JsonRequestBehavior.AllowGet);
        }
    }
}