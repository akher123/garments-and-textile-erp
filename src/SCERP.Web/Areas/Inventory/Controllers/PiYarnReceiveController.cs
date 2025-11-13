using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class PiYarnReceiveController : BaseController
    {
        private readonly IMaterialReceiveAgainstPoManager _materialReceiveAgainstPoManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IProFormaInvoiceManager proFormaInvoiceManager;
        private IPurchaseOrderManager purchaseOrderManager;
        public PiYarnReceiveController(IPurchaseOrderManager purchaseOrderManager, IProFormaInvoiceManager proFormaInvoiceManager,IMaterialReceiveAgainstPoManager materialReceiveAgainstPoManager, ISupplierCompanyManager supplierCompanyManager)
        {
            this._materialReceiveAgainstPoManager = materialReceiveAgainstPoManager;
            this._supplierCompanyManager = supplierCompanyManager;
            this.proFormaInvoiceManager = proFormaInvoiceManager;
            this.purchaseOrderManager = purchaseOrderManager;
        }
        [AjaxAuthorize(Roles = "yarnreceive(pi)-1,yarnreceive(pi)-2,yarnreceive(pi)-3")]
        public ActionResult Index(ReceiveAgainstPoViewModel model)
        {

            ModelState.Clear();
            const int yarnStore = (int)StoreType.Yarn;
            string[] types = new[] { RType.PI };
            int totalRecords;
            model.ReceiveAgainstPos = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByPaging(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, out totalRecords, yarnStore, types);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarnreceive(pi)-2,yarnreceive(pi)-3")]
        public ActionResult Edit(ReceiveAgainstPoViewModel model)
        {

            ModelState.Clear();
            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId > 0)
            {
                var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
                model.ReceiveAgainstPo = receiveAgainstPo;
                model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
                model.PiBookings = purchaseOrderManager.GetApporovedBookingBySupplier(model.ReceiveAgainstPo.SupplierId, PortalContext.CurrentUser.CompId);
            }
            else
            {
                model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewRcvRefId();
                model.ReceiveAgainstPo.MRRDate = DateTime.Now;
                model.ReceiveAgainstPo.InvoiceDate = DateTime.Now;
                model.ReceiveAgainstPo.GateEntryDate = DateTime.Now;
                model.ReceiveAgainstPo.ReceiveRegDate = DateTime.Now;
                model.ReceiveAgainstPo.PoDate = DateTime.Now;
            }
            return View(model);
        
        }
        [AjaxAuthorize(Roles = "yarnreceive(pi)-2,yarnreceive(pi)-3")]
        public ActionResult Save(ReceiveAgainstPoViewModel model)
        {

            var savedIndex = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.CompId = compId;
                if (!model.Dictionary.Any())
                {
                    return ErrorResult("Please Add at least one item");
                }
                else
                {
                    model.ReceiveAgainstPo.StoreId = (int)StoreType.Yarn;
                    model.ReceiveAgainstPo.RType = RType.PI;
                    model.ReceiveAgainstPo.PoDate = DateTime.Now;
                    model.ReceiveAgainstPo.MRRDate = model.ReceiveAgainstPo.ReceiveRegDate;
                    model.ReceiveAgainstPo.MRRNo = model.ReceiveAgainstPo.ReceiveRegNo;
                    model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =model.Dictionary.Where(x => x.Value.ReceivedQty>0).Select(x=>x.Value)
                        .Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                        {
                            CompId = compId,
                            ItemId = x.ItemId,
                            ColorRefId = x.ColorRefId,
                            SizeRefId = x.SizeRefId,
                            FColorRefId = x.FColorRefId,
                            ReceivedQty = x.ReceivedQty,
                            ReceivedRate = x.ReceivedRate,
                            LotNo = x.LotNo,
                            OrderStyleRefId=x.OrderStyleRefId
                        }).ToList();
                }
                if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId == 0)
                {
                    model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewRcvRefId();
                }
                savedIndex = _materialReceiveAgainstPoManager.SaveReceiveAgainstPo(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Save Failed");
        }

        public ActionResult Delete()
        {
            return Reload();
        }
        [AjaxAuthorize(Roles = "yarnreceive(pi)-3")]
        public ActionResult Delete(long materialReceiveAgstPoId)
        {
            int delteIndex = 0;
            try
            {
                const int yarnReceve = (int)ActionType.YarnReceive;
                delteIndex = _materialReceiveAgainstPoManager.DeteteReceiveAgainstPo(materialReceiveAgstPoId, yarnReceve);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return delteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }
        public ActionResult EditQc(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.QcDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        public ActionResult EditGrn(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.GrnDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }

        public ActionResult UpdateGrn(ReceiveAgainstPoViewModel model)
        {
            int updated = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail = model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                {
                    CompId = compId,
                    ItemId = x.ItemId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    ReceivedQty = x.ReceivedQty,
                    ReceivedRate = x.ReceivedRate,
                    RejectedQty = x.RejectedQty ?? 0,
                    MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                    MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId
                }).ToList();
                model.ReceiveAgainstPo.GrnStatus = true;
                const int yarnReceve = (int)ActionType.YarnReceive;
                updated = _materialReceiveAgainstPoManager.UpdateGrn(model.ReceiveAgainstPo, yarnReceve);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }

   
        public ActionResult UpdateQc(ReceiveAgainstPoViewModel model)
        {
            int updated = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                    model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                    {
                        CompId = compId,
                        ItemId = x.ItemId,
                        ColorRefId = x.ColorRefId,
                        SizeRefId = x.SizeRefId,
                        ReceivedQty = x.ReceivedQty,
                        ReceivedRate = x.ReceivedRate,
                        RejectedQty = x.RejectedQty ?? 0,
                        MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                        MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId
                    }).ToList();
                model.ReceiveAgainstPo.QcStatus = true;
                updated = _materialReceiveAgainstPoManager.UpdateQc(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }
        public ActionResult GetBookedYarnsByPi(string piRefId)
        {
            ReceiveAgainstPoViewModel model = new ReceiveAgainstPoViewModel();
            model.Dictionary = purchaseOrderManager.GetYarnPurchaseOrderDetailsByPiRefId(piRefId);
            return View("~/Areas/Inventory/Views/PiYarnReceive/_PiBookingList.cshtml", model);
        }

        public JsonResult GetApporovedBookingBySupplier(int supplierId)
        {
           List<ProFormaInvoice> invoices= purchaseOrderManager.GetApporovedBookingBySupplier(supplierId, PortalContext.CurrentUser.CompId);
           return Json(invoices, JsonRequestBehavior.AllowGet);
        }
    }
}