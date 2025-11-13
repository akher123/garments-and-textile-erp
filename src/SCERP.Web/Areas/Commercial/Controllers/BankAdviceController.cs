using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using System.Collections;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class BankAdviceController : BaseController
    {
        private readonly IBankAdviceManager _bankAdviceManager;
        private readonly IExportManager _exportManager;

        public BankAdviceController(IBankAdviceManager bankAdviceManager, IExportManager exportManager)
        {
            this._bankAdviceManager = bankAdviceManager;
            this._exportManager = exportManager;
        }

        public ActionResult Index(ExportViewModel model)
        {
            try
            {
                ModelState.Clear();
                int totalRecords;
                model.CommExports = _exportManager.GetExportByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(Int64 id)
        {
            ModelState.Clear();
            BankAdviceViewModel model = new BankAdviceViewModel();

            try
            {
                if (id > 0)
                {
                    model.BankAdvices = _bankAdviceManager.GetAccHead("Static", id);
                    model.BankAdviceFixeds = _bankAdviceManager.GetAccHead("Fixed", id);
                    ViewBag.Title = "Edit LC";
                }
                else
                {
                    ViewBag.Title = "Add LC";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(BankAdviceViewModel model)
        {
            int saveIndex = 0;
            decimal? amountInTaka = 0;
            decimal? amountInTakaFixed = 0;

            try
            {
                amountInTakaFixed = model.BankAdviceFixeds[0].AmountInTaka - model.BankAdviceFixeds[1].AmountInTaka;
                amountInTaka = model.BankAdvices.Where(t => t.AmountInTaka > 0).Aggregate(amountInTaka, (current, t) => current + t.AmountInTaka);

                if (amountInTakaFixed != amountInTaka)
                    return ErrorResult("Balance amount are not matched !");

                foreach (var t in model.BankAdviceFixeds)
                {
                    if (t.AmountInTaka > 0)
                    {
                        int count = 0;
                        CommBankAdvice bankAdvice = _bankAdviceManager.GetBankAdviceByExportAndHeadId(t.ExportId, t.AccHeadId, out count) ?? new CommBankAdvice();

                        bankAdvice.ExportId = t.ExportId;
                        bankAdvice.AccHeadId = t.AccHeadId;
                        bankAdvice.Amount = t.Amount;
                        bankAdvice.Rate = t.Rate;
                        bankAdvice.AmountInTaka = t.AmountInTaka;
                        bankAdvice.Particulars = t.Particulars;
                        bankAdvice.BankRefNo = t.BankRefNo;
                        bankAdvice.IsActive = true;
                        bankAdvice.CompId = PortalContext.CurrentUser.CompId;
                        saveIndex = (count > 0) ? _bankAdviceManager.EditBankAdvice(bankAdvice) : _bankAdviceManager.SaveBankAdvice(bankAdvice);
                    }
                }

                foreach (var t in model.BankAdvices)
                {
                    if (t.AmountInTaka > 0)
                    {
                        int count = 0;
                        CommBankAdvice bankAdvice = _bankAdviceManager.GetBankAdviceByExportAndHeadId(t.ExportId, t.AccHeadId, out count) ?? new CommBankAdvice();

                        bankAdvice.ExportId = t.ExportId;
                        bankAdvice.AccHeadId = t.AccHeadId;
                        bankAdvice.AmountInTaka = t.AmountInTaka;
                        bankAdvice.Particulars = t.Particulars;
                        bankAdvice.BankRefNo = t.BankRefNo;
                        bankAdvice.IsActive = true;
                        bankAdvice.CompId = PortalContext.CurrentUser.CompId;
                        saveIndex = (count > 0) ? _bankAdviceManager.EditBankAdvice(bankAdvice) : _bankAdviceManager.SaveBankAdvice(bankAdvice);
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult AddNewRow(BankAdviceViewModel model)
        {
            //var bankAdvice = new CommBankAdvice
            //{
            //    AccHeadId = model.BankAdvice.AccHeadId,
            //    ExportId = model.BankAdvice.ExportId,
            //    AccHeadName = model.BankAdvice.AccHeadName,
            //    Particulars = model.BankAdvice.Particulars,
            //    BankRefNo = model.BankAdvice.BankRefNo,
            //    Amount = model.BankAdvice.Amount,
            //};

            //model.BankAdvices.Add("0", bankAdvice);

            //if (model.BankAdvice != null && model.BankAdvice.AccHeadId < 0)
            //    return ErrorResult("Please Insert valid Bank Advice information !");
            //else
            return PartialView("~/Areas/Commercial/Views/BankAdvice/_AddNewRow.cshtml", model);
        }

        public ActionResult TagHeadSearch(string term)
        {
            List<string> tags = _bankAdviceManager.GetAccHead("Static").Select(p => p.AccHeadName).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }
    }
}