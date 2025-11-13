using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class VoucherEntryController : BaseAccountingController
    {
        private IControlAccountManager _controlAccountManager;
        private readonly IGLAccountManager _gLAccountManager;
        private readonly IVoucherMasterManager _VoucherMasterManager;
        public VoucherEntryController(IVoucherMasterManager VoucherMasterManager,IGLAccountManager gLAccountManager,IControlAccountManager controlAccountManager)
        {
            _controlAccountManager = controlAccountManager;
            _gLAccountManager = gLAccountManager;
            _VoucherMasterManager = VoucherMasterManager;
        }
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        [AjaxAuthorize(Roles = "voucherentry-1,voucherentry-2,voucherentry-3")]
        public ActionResult Index(VoucherEntryViewModel model)
        {
            ModelState.Clear();
            int totalRecords;
            model.VoucherList.sort = model.sort;
            model.VoucherList.sortdir = model.sortdir;
            model.VoucherList.page = model.page;

            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.VoucherList.FromDate = DateTime.Now.Date;
                model.VoucherList.ToDate = DateTime.Now.Date.AddDays(1);
                model.VoucherLists = _VoucherMasterManager.GetVoucherList(model.VoucherList, out totalRecords);
            }
            else
            {
                model.VoucherLists = _VoucherMasterManager.GetVoucherList(model.VoucherList, out totalRecords);
            }

            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "voucherentry-2,voucherentry-3")]
        public ActionResult Edit(VoucherEntryViewModel model)
        {
            ModelState.Clear();
            model.VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo();
            model.CompanySectors = JournalVoucherEntryManager.GetAllCompanySector().ToList();
            model.CostCentresMultilayers = JournalVoucherEntryManager.GetAllCostCentres().ToList();
            model.VoucherDate = DateTime.Now;
            model.CurrencyRate = 1;
            model.SectorId = _VoucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            if (model.Id > 0)
            {
                var voucherMaster = _VoucherMasterManager.GetAllVoucherMaster(model.Id);
                model.Id = voucherMaster.Id;
                model.VoucherType = voucherMaster.VoucherType;
                model.VoucherDate = voucherMaster.VoucherDate;
                model.VoucherNo = voucherMaster.VoucherNo;
                model.SectorId = voucherMaster.SectorId;
                model.CostCentreId = voucherMaster.CostCentreId;
                model.VoucherRefNo = voucherMaster.VoucherRefNo;
                model.TotalAmountInWord = voucherMaster.TotalAmountInWord;
                model.Particulars = voucherMaster.Particulars;
                model.VoucherDetails = voucherMaster.Acc_VoucherDetail.ToDictionary(x => Convert.ToString(x.Id), x => x);
                model.TotalDebitAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Debit) ?? 0.0M;
                model.TotalCreditAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Credit) ?? 0.0M;
                model.TotalAmount = model.TotalDebitAmount - model.TotalCreditAmount;
            }
            else
            {
                model.VoucherType = "CP";
                model.VoucherRefNo = _VoucherMasterManager.GetVoucherNoByType("CP", DateTime.Now);
                // model.CostCentreId = 1; // CostcenterId must be set 
            }
            return View(model);
        }

        public ActionResult EditMultiCurrency(VoucherEntryViewModel model)
        {
            ModelState.Clear();
            model.VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo();
            model.CompanySectors = JournalVoucherEntryManager.GetAllCompanySector().ToList();
            model.CostCentresMultilayers = JournalVoucherEntryManager.GetAllCostCentres().ToList();
            model.VoucherDate = DateTime.Now;
            model.CurrencyRate = 1;

            model.SectorId = _VoucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            var currencyList = from CurrencyType currency in Enum.GetValues(typeof(CurrencyType))
                               select new { Id = (int)currency, Name = currency.ToString() };

            if (model.Id > 0)
            {
                var voucherMaster = _VoucherMasterManager.GetAllVoucherMaster(model.Id);
                model.Id = voucherMaster.Id;
                model.VoucherType = voucherMaster.VoucherType;
                model.VoucherDate = voucherMaster.VoucherDate;
                model.VoucherNo = voucherMaster.VoucherNo;
                model.SectorId = voucherMaster.SectorId;
                model.CostCentreId = voucherMaster.CostCentreId;
                model.VoucherRefNo = voucherMaster.VoucherRefNo;
                model.TotalAmountInWord = voucherMaster.TotalAmountInWord;
                model.Particulars = voucherMaster.Particulars;
                model.VoucherDetails = voucherMaster.Acc_VoucherDetail.ToDictionary(x => Convert.ToString(x.Id), x => x);
                model.TotalDebitAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Debit) ?? 0.0M;
                model.TotalCreditAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Credit) ?? 0.0M;
                model.TotalAmount = model.TotalDebitAmount - model.TotalCreditAmount;

                var detail = voucherMaster.Acc_VoucherDetail.FirstOrDefault();

                if (detail != null)
                {
                    switch (voucherMaster.ActiveCurrencyId)
                    {
                        case 1:
                            model.CurrencyRate = detail.FirstCurValue;
                            break;
                        case 2:
                            model.CurrencyRate = detail.SecendCurValue;
                            break;
                        case 3:
                            model.CurrencyRate = detail.ThirdCurValue;
                            break;
                    }

                    foreach (var t in voucherMaster.Acc_VoucherDetail)
                    {
                        t.Debit = t.Debit / model.CurrencyRate;
                        t.Credit = t.Credit / model.CurrencyRate;
                    }

                    model.TotalDebitAmount = model.TotalDebitAmount / model.CurrencyRate;
                    model.TotalCreditAmount = model.TotalCreditAmount / model.CurrencyRate;
                    model.TotalAmount = model.TotalAmount / model.CurrencyRate;
                }

                ViewBag.ActiveCurrencyId = new SelectList(currencyList, "Id", "Name", voucherMaster.ActiveCurrencyId);
            }
            else
            {
                model.VoucherType = "CP";
                model.VoucherRefNo = _VoucherMasterManager.GetVoucherNoByType("CP", DateTime.Now);
                //model.CostCentreId = 1; // CostcenterId must be set 
                ViewBag.ActiveCurrencyId = new SelectList(currencyList, "Id", "Name", (int)CurrencyType.BDT);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "voucherentry-2,voucherentry-3")]
        public ActionResult Save(VoucherEntryViewModel model)
        {
            int saveIndex = 0;
            string message = "";

            model.SectorId = _VoucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            try
            {
                if (model.CurrencyRate < 1)
                {
                    return ErrorResult("Please Insert Currency Rate !");
                }
                if (model.TotalAmount != 0 && model.TotalCreditAmount != model.TotalDebitAmount)
                {
                    return ErrorResult("Debit and Credit amount not matched !");
                }
                else if (!model.VoucherDetails.Any())
                {
                    return ErrorResult("Please Insert Debit and Credit amount !");
                }

                else
                {
                    var voucherMaster = new Acc_VoucherMaster
                    {
                        VoucherType = model.VoucherType,
                        VoucherDate = model.VoucherDate.GetValueOrDefault(),
                        SectorId = model.SectorId,
                        CostCentreId = model.CostCentreId,
                        VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo(),
                        VoucherRefNo = model.VoucherRefNo,
                        Particulars = model.Particulars,
                        TotalAmountInWord = model.TotalAmountInWord,
                        ActiveCurrencyId = model.ActiveCurrencyId ?? 1,
                        CurrencyRate = model.CurrencyRate,
                        Acc_VoucherDetail = model.VoucherDetails.Select(x => x.Value).ToList(),
                        IsActive = true
                    };

                    var curr = _VoucherMasterManager.GetAllCurrency();

                    if (voucherMaster.ActiveCurrencyId == 2)
                        curr.SecendCurValue = voucherMaster.CurrencyRate;
                    if (voucherMaster.ActiveCurrencyId == 3)
                        curr.ThirdCurValue = voucherMaster.CurrencyRate;

                    foreach (var t in voucherMaster.Acc_VoucherDetail)
                    {
                        if (curr != null)
                        {
                            t.FirstCurValue = curr.FirstCurValue;
                            t.SecendCurValue = curr.SecendCurValue;
                            t.ThirdCurValue = curr.ThirdCurValue;
                        }

                        if (t.Debit > 0 && voucherMaster.CurrencyRate > 0)
                        {
                            t.Debit = t.Debit * voucherMaster.CurrencyRate;
                        }

                        if (t.Credit > 0 && voucherMaster.CurrencyRate > 0)
                        {
                            t.Credit = t.Credit * voucherMaster.CurrencyRate;
                        }
                    }

                    if (model.Id > 0)
                    {
                        voucherMaster.Id = model.Id;
                        saveIndex = _VoucherMasterManager.EditVoucher(voucherMaster);
                    }
                    else
                    {
                        saveIndex = _VoucherMasterManager.SaveVoucher(voucherMaster);
                    }
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
                Errorlog.WriteLog(exception);
            }

            if (model.IsPartial)
            {
                message = "update successfully";
                return Json(new { IsPartial = model.IsPartial, SuccessMessage = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return saveIndex > 0 ? Reload() : ErrorResult("Internal Error ! " + message);
            }

        }

        public ActionResult AddNewRow(VoucherEntryViewModel model)
        {
            if (model.VoucherType == "CP" || model.VoucherType == "CR")
            {
                var glId = 156; //156 for Garments-Cash in Hand
                var glAccount = _controlAccountManager.GetGLAccountsById(glId);
                var vdetail = new Acc_VoucherDetail
                {
                    Acc_GLAccounts = glAccount,
                    GLID = glAccount.Id,
                    Particulars = model.VoucherDetail.Particulars,
                    Credit = model.VoucherDetail.Credit,
                    Debit = model.VoucherDetail.Debit,
                    CostCentreId = model.VoucherDetail.CostCentreId
                };
                model.VoucherDetails.Add(glId.ToString(), vdetail);
            }
            else
            {
                model.Key = model.VoucherDetail.GLID.ToString();
                model.VoucherDetail.Acc_GLAccounts = new Acc_GLAccounts { AccountName = model.AccountName };
                model.VoucherDetails.Add(model.Key, model.VoucherDetail);
            }

            if (model.VoucherDetail != null && model.VoucherDetail.GLID == null)
            {
                return ErrorResult("Invalid GL Account Please Select Correct one");
            }
            else
            {
                return PartialView("~/Areas/Accounting/Views/VoucherEntry/_AddNewRow.cshtml", model);
            }
        }

        public JsonResult IsVoucherRefExist(VoucherList model)
        {
            bool isExist = !_VoucherMasterManager.IsVoucherRefExist(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutocompliteGLAccount(string accountName)
        {

            var glAccountList = _gLAccountManager.GetAutocompliteGLAccount(accountName);
            return Json(glAccountList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VoucherSummary(VoucherEntryViewModel model)
        {
            model.VoucherLists = _VoucherMasterManager.GetVoucherSummary(model.VoucherList);
            return View("~/Areas/Accounting/Views/VoucherEntry/VoucherSummary.cshtml", model);
        }

        [AjaxAuthorize(Roles = "voucherentry-3")]
        public ActionResult Delete(int? id)
        {
            var deleteIndex = 0;

            try
            {
                deleteIndex = _VoucherMasterManager.DeleteVoucher(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");
        }

        public JsonResult GetVoucherNoByType(string type, DateTime date)
        {
            string result = "";

            try
            {
                result = _VoucherMasterManager.GetVoucherNoByType(type, date);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return Json(new { Success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyById(int id)
        {
            string result = "";

            try
            {
                result = _VoucherMasterManager.GetCurrencyById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return Json(new { Success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditFromBalanceSheet(int? voucherNoShow)
        {
            ModelState.Clear();

            VoucherEntryViewModel model = new VoucherEntryViewModel
            {
                Id = ReportAccountManger.GetVoucherIdbyVoucherNo(voucherNoShow),
                VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo(),
                CompanySectors = JournalVoucherEntryManager.GetAllCompanySector().ToList(),
                CostCentres = JournalVoucherEntryManager.GetAllCostCentres(1).ToList(),
                VoucherDate = DateTime.Now,
                CurrencyRate = 1,
                IsPartial = true,
                SectorId = _VoucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId)
            };



            if (model.Id > 0)
            {
                var voucherMaster = _VoucherMasterManager.GetAllVoucherMaster(model.Id);
                model.Id = voucherMaster.Id;
                model.VoucherType = voucherMaster.VoucherType;
                model.VoucherDate = voucherMaster.VoucherDate;
                model.VoucherNo = voucherMaster.VoucherNo;
                model.SectorId = voucherMaster.SectorId;
                model.CostCentreId = voucherMaster.CostCentreId;
                model.VoucherRefNo = voucherMaster.VoucherRefNo;
                model.TotalAmountInWord = voucherMaster.TotalAmountInWord;
                model.Particulars = voucherMaster.Particulars;
                model.VoucherDetails = voucherMaster.Acc_VoucherDetail.ToDictionary(x => Convert.ToString(x.Id), x => x);
                model.TotalDebitAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Debit) ?? 0.0M;
                model.TotalCreditAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Credit) ?? 0.0M;
                model.TotalAmount = model.TotalDebitAmount - model.TotalCreditAmount;
            }
            else
            {
                model.VoucherType = "CP";
                model.VoucherRefNo = _VoucherMasterManager.GetVoucherNoByType("CP", DateTime.Now);
                model.CostCentreId = 1; // CostcenterId must be set 
            }
            return View(model);
        }

        public ActionResult EditFromBalanceSheetByMasterId(long Id)
        {
            ModelState.Clear();

            VoucherEntryViewModel model = new VoucherEntryViewModel
            {
                Id = Id,
                VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo(),
                CompanySectors = JournalVoucherEntryManager.GetAllCompanySector().ToList(),
                CostCentres = JournalVoucherEntryManager.GetAllCostCentres(1).ToList(),
                VoucherDate = DateTime.Now,
                CurrencyRate = 1,
                IsPartial = true,
                SectorId = _VoucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId)
            };



            if (model.Id > 0)
            {
                var voucherMaster = _VoucherMasterManager.GetAllVoucherMaster(model.Id);
                model.Id = voucherMaster.Id;
                model.VoucherType = voucherMaster.VoucherType;
                model.VoucherDate = voucherMaster.VoucherDate;
                model.VoucherNo = voucherMaster.VoucherNo;
                model.SectorId = voucherMaster.SectorId;
                model.CostCentreId = voucherMaster.CostCentreId;
                model.VoucherRefNo = voucherMaster.VoucherRefNo;
                model.TotalAmountInWord = voucherMaster.TotalAmountInWord;
                model.Particulars = voucherMaster.Particulars;
                model.VoucherDetails = voucherMaster.Acc_VoucherDetail.ToDictionary(x => Convert.ToString(x.Id), x => x);
                model.TotalDebitAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Debit) ?? 0.0M;
                model.TotalCreditAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Credit) ?? 0.0M;
                model.TotalAmount = model.TotalDebitAmount - model.TotalCreditAmount;
            }
            else
            {
                model.VoucherType = "CP";
                model.VoucherRefNo = _VoucherMasterManager.GetVoucherNoByType("CP", DateTime.Now);
                model.CostCentreId = 1; // CostcenterId must be set 
            }
            return View(model);
        }
    }
}