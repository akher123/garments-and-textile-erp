using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class BankAccountTypeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "bankaccounttype-1,bankaccounttype-2,bankaccounttype-3")]
        public ActionResult Index(BankAccountTypeViewModel model)
        {
            try
            {
                ModelState.Clear();
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.AccountType = model.SearchKeyAccountType;
                model.BankAccountTypes = BankAccountTypeManager.GetBankAccountTypes(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "bankaccounttype-2,bankaccounttype-3")]
        public ActionResult Edit(BankAccountTypeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.BankAccountTypeId > 0)
                {
                    var bankAccountType = BankAccountTypeManager.GetBankAccountTypeById(model.BankAccountTypeId);
                    model.AccountType = bankAccountType.AccountType;
                    model.Description = bankAccountType.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "bankaccounttype-2,bankaccounttype-3")]
        public ActionResult Save(BankAccountType model)
        {
            var saveIndex = 0;
            bool isExist = BankAccountTypeManager.IsExistBankAccountType(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.BankAccountTypeId > 0 ? BankAccountTypeManager.EditBankAccountType(model) : BankAccountTypeManager.SaveBankAccountType(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("{0} AccountType already exist!", model.AccountType));
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "bankaccounttype-3")]
        public ActionResult Delete(BankAccountType accountType)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = BankAccountTypeManager.DeleteBankAccountType(accountType.BankAccountTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

	}
}