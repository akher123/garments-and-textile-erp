using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class ReceiveAgainstPoController : BaseController
    {
        private readonly IMaterialReceiveAgainstPoManager _materialReceiveAgainstPoManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IOmBuyerManager _omBuyerManager;

        public ReceiveAgainstPoController(IOmBuyerManager omBuyerManager,
            IMaterialReceiveAgainstPoManager materialReceiveAgainstPoManager,
            ISupplierCompanyManager supplierCompanyManager)
        {
            _materialReceiveAgainstPoManager = materialReceiveAgainstPoManager;
            _supplierCompanyManager = supplierCompanyManager;
            _omBuyerManager = omBuyerManager;
        }
        [AjaxAuthorize(Roles = "received(pi/booking)-1,received(pi/booking)-2,received(pi/booking)-3")]
        public ActionResult Index(ReceiveAgainstPoViewModel model)
        {

            ModelState.Clear();
               const int acessories = (int) StoreType.Acessories;
            string[] types = new[] { RType.PI };
            int totalRecords = 0;
            model.ReceiveAgainstPos = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByPaging(model.PageIndex,
                model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, out totalRecords, acessories, types);
            model.TotalRecords = totalRecords;
            return View(model);
        }
           [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
        public ActionResult Edit(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId > 0)
            {
                var receiveAgainstPo =
                    _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(
                        model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
                model.ReceiveAgainstPo = receiveAgainstPo;
                model.Dictionary =
                    _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
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
            model.StoreList =
                Enum.GetValues(typeof(StoreType))
                    .Cast<StoreType>()
                    .Select(x => new { StoreId = Convert.ToInt16(x), Name = x });
            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            return View(model);
        }
             [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
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
                    model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                        model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                        {
                            CompId = compId,
                            ItemId = x.ItemId,
                            ColorRefId = x.ColorRefId,
                            SizeRefId = x.SizeRefId,
                            ReceivedQty = x.ReceivedQty,
                            ReceivedRate = x.ReceivedRate
                        }).ToList();
                }
                savedIndex = _materialReceiveAgainstPoManager.SaveReceiveAgainstPo(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Save Failed");
        }

        public ActionResult AddNewRow(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.Dictionary.Add(model.Key, model.PoDetail);
            return View("~/Areas/Inventory/Views/ReceiveAgainstPo/AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "received(pi/booking)-3")]
        public ActionResult Delete(long materialReceiveAgstPoId)
        {
            int delteIndex = 0;
            try
            {
                const int accessoriesReceve = (int)ActionType.AccessoriesReceive;
                delteIndex = _materialReceiveAgainstPoManager.DeteteReceiveAgainstPo(materialReceiveAgstPoId, accessoriesReceve);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return delteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }
        [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
        public ActionResult EditQc(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.QcDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
        public ActionResult EditGrn(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.GrnDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
        public ActionResult UpdateGrn(ReceiveAgainstPoViewModel model)
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
                        RejectedQty = x.RejectedQty,
                        MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                        MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId
                    }).ToList();
                model.ReceiveAgainstPo.GrnStatus = true;
                const int accessoriesReceve = (int)ActionType.AccessoriesReceive;
                updated = _materialReceiveAgainstPoManager.UpdateGrn(model.ReceiveAgainstPo, accessoriesReceve);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }

        [AjaxAuthorize(Roles = "received(pi/booking)-2,received(pi/booking)-3")]
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
                        RejectedQty = x.RejectedQty,
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


        public ActionResult MaterialReceivedAgainstPoOrBookingReport(long materialReceiveAgstPoId, string key)
        {

            List<VwMaterialReceiveAgainstPoDetail> receiveAgainstPoDetails = _materialReceiveAgainstPoManager.GetVwMaterialReceiveAgainstPoDetail(materialReceiveAgstPoId);
            string reportName = "";
            switch (key)
            {
                case "GRN":
                    reportName = "GrnReport";
                    break;
                case "QC":
                    reportName = "QcReport";
                    break;
                default:
                    reportName = "ReceivedPoOrBookingReport";
                    break;
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), reportName + ".rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RcvAgPoBKDSet", receiveAgainstPoDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult MrrSummaryReport(ReceiveAgainstPoViewModel model)
        {

            List<VwMaterialReceiveAgainstPo> receiveAgainstPos = _materialReceiveAgainstPoManager.GetMrrSummaryReport(model.FromDate, model.ToDate, model.SearchString);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
            if (model.FromDate == null && model.ToDate == null)
            {
                fromDateParameter = new ReportParameter("FromDate", "ALL");
                toDateParameter = new ReportParameter("ToDate", "ALL");
            }
            else
            {
                fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "MRRSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MRRDSet", receiveAgainstPos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.3, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }
    }
}