using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System.Collections.Generic;
using SCERP.BLL.IManager.IAccountingManager;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class VoucherSagregationToCostCentreController : BaseAccountingController
    {
        private readonly IVoucherMasterManager voucherMasterManager;
        public VoucherSagregationToCostCentreController(IVoucherMasterManager voucherMasterManager)
        {
            this.voucherMasterManager = voucherMasterManager;
        }
        public ActionResult Edit(VoucherToCostCentreViewModel model)
        {
            ModelState.Clear();

            model.CostCentres = JournalVoucherEntryManager.GetAllCostCentres().ToList();
            model.GlAccounts = JournalVoucherEntryManager.GetGlAccountsByMasterId(model.Id).ToList();

            if (model.Id > 0)
            {
                var voucherMaster = voucherMasterManager.GetAllVoucherMaster(model.Id);
                model.Id = voucherMaster.Id;
                model.VoucherDate = DateTime.Now;
                model.VoucherNo = voucherMaster.VoucherNo;
                model.CostCentreId = voucherMaster.CostCentreId;
                model.VoucherRefNo = voucherMaster.VoucherRefNo;
            }

            return View(model);
        }

        public ActionResult Save(Acc_VoucherToCostcentre model)
        {
            int saveIndex = 1;
            string message = "";

            try
            {
                model.CreatedDate = DateTime.Now;
                model.EditedDate = DateTime.Now;
                model.IsActive = true;

                saveIndex = voucherMasterManager.SaveVoucherToCostCentre(model);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                Errorlog.WriteLog(exception);
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Internal Error ! " + message);
        }

        public ActionResult Delete(Acc_VoucherToCostcentre model)
        {
            int saveIndex = 1;
            string message = "";

            try
            {
                saveIndex = voucherMasterManager.DeleteVouchertoCostCentre(model);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                Errorlog.WriteLog(exception);
            }

            return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVoucherToCostCentreDetail(long Id)
        {
            var count = 1;

            var temp = voucherMasterManager.GetVoucherToCostCentre(Id);

            List<object> lt = new List<object>();

            foreach (var t in temp)
            {
                lt.Add(
                    new
                    {
                        Id = count,
                        Date = Convert.ToDateTime(t.Date).ToString("dd/MM/yyyy"),
                        RefNo = t.VoucherRefNo,
                        VoucherNo = t.VoucherNo,
                        AccountCode = t.AccountCode,
                        AccountName = voucherMasterManager.GetAccountNameByCode(t.AccountCode),
                        CostCentre = CostCentreManager.GetNewCostCentreById(t.CostCentreId).ItemName,
                        CID = t.CostCentreId,
                        Amount = Convert.ToDecimal(t.Amount).ToString(CultureInfo.InvariantCulture)
                    });

                count++;
            }

            return Json(new {data = lt, Success = true}, JsonRequestBehavior.AllowGet);
        }
    }
}
