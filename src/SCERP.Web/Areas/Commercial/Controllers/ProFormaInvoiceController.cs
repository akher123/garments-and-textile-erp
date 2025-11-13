using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
   
    public class ProFormaInvoiceController : BaseController
    {
        public readonly IProFormaInvoiceManager proFormaInvoiceManager;
        public readonly ISupplierCompanyManager supplierCompanyManager;
        public ProFormaInvoiceController(IProFormaInvoiceManager proFormaInvoiceManager, ISupplierCompanyManager supplierCompanyManager)
        {
            this.proFormaInvoiceManager = proFormaInvoiceManager;
            this.supplierCompanyManager = supplierCompanyManager;
        }
        [AjaxAuthorize(Roles = "proformainvoice-1,proformainvoice-2,proformainvoice-3")]
        public ActionResult Index(ProFormaInvoiceViewModel model)
        {

            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                model.ProFormaInvoices = proFormaInvoiceManager.GetProFormaInvoiceByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "proformainvoice-2,proformainvoice-3")]
        public ActionResult Edit(ProFormaInvoiceViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ProFormaInvoice.PiId > 0)
                {
                    ProFormaInvoice proFormaInvoice = proFormaInvoiceManager.GetProFormaInvoiceById(model.ProFormaInvoice.PiId);
                    model.ProFormaInvoice.PiRefId = proFormaInvoice.PiRefId;
                    model.ProFormaInvoice.PiNo = proFormaInvoice.PiNo;
                    model.ProFormaInvoice.SupplierId = proFormaInvoice.SupplierId;
                    model.ProFormaInvoice.ReceivedDate = proFormaInvoice.ReceivedDate;
                    model.ProFormaInvoice.EndDate = proFormaInvoice.EndDate;
                    model.ProFormaInvoice.BookingNo = proFormaInvoice.BookingNo;
                    model.ProFormaInvoice.Remarks = proFormaInvoice.Remarks;

                }
                else
                {
                    model.ProFormaInvoice.PiRefId = proFormaInvoiceManager.GetNewRefId(PortalContext.CurrentUser.CompId);
                }
                model.SupplierCompanies = supplierCompanyManager.GetAllSupplierCompany();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "proformainvoice-2,proformainvoice-3")]
        public ActionResult Save(ProFormaInvoiceViewModel model)
        {

            var index = 0;
            try
            {
                if (proFormaInvoiceManager.IsPiExist(model.ProFormaInvoice.SupplierId, model.ProFormaInvoice.PiNo, model.ProFormaInvoice.PiId))
                {
                    return ErrorResult("PI No :" + model.ProFormaInvoice.PiNo + " " + "Already Exist ! Please Entry another one");
                }
                else
                {
                    if (model.ProFormaInvoice.PiId > 0)
                    {
                        model.ProFormaInvoice.EditedDate = DateTime.Now;
                        model.ProFormaInvoice.EditedBy = PortalContext.CurrentUser.UserId;
                       index = proFormaInvoiceManager.EditProFormaInvoice(model.ProFormaInvoice);
                    }
                    else
                    {
                        model.ProFormaInvoice.CreatedDate = DateTime.Now;
                        model.ProFormaInvoice.CreatedBy = PortalContext.CurrentUser.UserId;
                        index = proFormaInvoiceManager.SaveProFormaInvoice(model.ProFormaInvoice);
                    }

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Hour :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Hour !");
        }
        [AjaxAuthorize(Roles = "proformainvoice-3")]
        public ActionResult Delete(int piId)
        {
            var index = 0;
            try
            {
                index = proFormaInvoiceManager.DeleteProFormaInvoice(piId);
                if (index == -1)
                {
                    return ErrorResult("Could not possible to delete PI No because of it's already used.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Hour :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete PI !");
        }

        public JsonResult GetPiBySupplier(int supplierId)
        {
            List<ProFormaInvoice> proFormaInvoices= proFormaInvoiceManager.GetPiBySupplier(supplierId, PortalContext.CurrentUser.CompId);
            return Json(proFormaInvoices, JsonRequestBehavior.AllowGet);
        }
    }
}