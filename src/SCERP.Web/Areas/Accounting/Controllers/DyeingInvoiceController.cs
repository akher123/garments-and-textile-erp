using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Web.Controllers;
using Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public sealed class DyeingInvoiceController : BaseController
    {
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;
        private readonly IAccountIntegrationManager _accountIntegrationManager;
        private readonly IJournalVoucherEntryManager journalVoucherEntryManager;

        private readonly IVoucherMasterManager voucherMasterManager;
        public DyeingInvoiceController(IJournalVoucherEntryManager journalVoucherEntryManager,IVoucherMasterManager voucherMasterManager,IAccountIntegrationManager accountIntegrationManager)
        {
            _accountIntegrationManager = accountIntegrationManager;
            this.journalVoucherEntryManager = journalVoucherEntryManager;
            this.voucherMasterManager = voucherMasterManager;
        }
        public ActionResult Index(DyeingInvoiceViewModel model)
        {
            model.InvoiceDate= model.InvoiceDate ?? DateTime.Now;
            model.DyeingAccountInvoices = _accountIntegrationManager.GetDyingBillInvoices(model.InvoiceDate);
            ModelState.Clear();
            return View(model);
        }

        public ActionResult DyeingReceivableVoucher(VoucherEntryViewModel model)
        {
            ModelState.Clear();
            model.VoucherNo = journalVoucherEntryManager.GetLatestVoucherNo();
            model.CompanySectors = journalVoucherEntryManager.GetAllCompanySector().ToList();
            model.CostCentresMultilayers = journalVoucherEntryManager.GetAllCostCentres().ToList();
            model.CurrencyRate = 1;
            model.SectorId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            Acc_VoucherMaster voucherMaster = _accountIntegrationManager.GetDyingBillVoucher(model.Key, (int)BillTable.Inventory_FinishFabricIssue);
            model.CheckDate = voucherMaster.CheckDate;
            model.CheckNo = voucherMaster.CheckNo;
            model.VoucherDate = voucherMaster.VoucherDate;
            model.VoucherRefNo = voucherMaster.VoucherRefNo;
            model.Particulars = voucherMaster.Particulars;
            model.IntRefId = voucherMaster.IntRefId;
            model.IntType = voucherMaster.IntType;
            model.VoucherDetails = voucherMaster.Acc_VoucherDetail.ToDictionary(x => Convert.ToString(x.GLID), x => x);
            model.TotalDebitAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Debit) ?? 0.0M;
            model.TotalCreditAmount = voucherMaster.Acc_VoucherDetail.Sum(x => (decimal?)x.Credit) ?? 0.0M;
            model.TotalAmount = model.TotalDebitAmount - model.TotalCreditAmount;
            model.TotalAmountInWord = SpellAmount.InWrods(model.TotalDebitAmount);
            model.VoucherType = "IV";
            return View(model);
        }
        public ActionResult Save(VoucherEntryViewModel model)
        {
            int saveIndex = 0;
            string message = "";

            model.SectorId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

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
                        VoucherType = "IV",
                        VoucherDate = model.VoucherDate.GetValueOrDefault(),
                        SectorId = model.SectorId,
                        CostCentreId = model.CostCentreId,
                        VoucherNo = journalVoucherEntryManager.GetLatestVoucherNo(),
                        VoucherRefNo = model.VoucherRefNo,
                        Particulars = model.Particulars,
                        CheckNo = model.CheckNo,
                        CheckDate = model.CheckDate,
                        TotalAmountInWord = model.TotalAmountInWord,
                        ActiveCurrencyId = model.ActiveCurrencyId ?? 1,
                        CurrencyRate = model.CurrencyRate,
                        IntRefId = model.IntRefId,
                        IntType = (int)BillTable.Inventory_FinishFabricIssue,
                        Acc_VoucherDetail = model.VoucherDetails.Select(x => x.Value).ToList(),
                        IsActive = true
                    };

                    var curr = voucherMasterManager.GetAllCurrency();

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

                    if (!String.IsNullOrEmpty(model.IntRefId))
                    {
                        saveIndex = voucherMasterManager.SaveIniteGrationVoucher(voucherMaster);

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
    }
}