using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IHRMManager;
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
    public class StyleShipmentController : BaseController
    {
        private readonly IStyleShipmentManager _styleShipmentManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        private readonly IEmployeeManager _employeeManager;
        public StyleShipmentController(IOmBuyOrdStyleManager buyOrdStyle, IStyleShipmentManager styleShipmentManager, IOmBuyerManager buyerManager, IEmployeeManager employeeManager)
        {
            _styleShipmentManager = styleShipmentManager;
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
            _employeeManager = employeeManager;
        }
        [AjaxAuthorize(Roles = "shipmentchallan-1,shipmentchallan-2,shipmentchallan-3")]
        public ActionResult Index(StyleShipmentViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.Buyers = _buyerManager.GetAllBuyers();
                model.StyleShipments = _styleShipmentManager.GetStyleShipmentByPaging(model.PageIndex, model.sort, model.sortdir, model.StyleShipment.BuyerRefId, model.searchKey, PortalContext.CurrentUser.CompId, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "shipmentchallan-2,shipmentchallan-3")]
        public ActionResult Edit(StyleShipmentViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            if (model.StyleShipment.StyleShipmentId > 0)
            {
                bool isApproved = _styleShipmentManager.IsShipmentApproved(model.StyleShipment.StyleShipmentId);
                if (isApproved)
                {
                    return ErrorResult("Already Approded.");
                }
                model.StyleShipment = _styleShipmentManager.GetStyleShipmentById(model.StyleShipment.StyleShipmentId);

                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.StyleShipment.BuyerRefId);
                model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.StyleShipment.OrderNo);
                List<VwInventoryStyleShipment> styleShipmentDetails =
                    _styleShipmentManager.GetShipmentStyleRefIds(model.StyleShipment.StyleShipmentId);
                foreach (var sh in styleShipmentDetails)
                {
                    List<SpInventoryStyleShipment> orderShipDetails =
               _styleShipmentManager.GetStyleShipment(sh.OrderStyleRefId,

                   PortalContext.CurrentUser.CompId, model.StyleShipment.StyleShipmentId);
                    var colorList = orderShipDetails.Select(x => new { x.ColorRefId, x.ColorName }).Distinct();

                    var sizeList = orderShipDetails.Select(x => new { x.SizeRefId, x.SizeName }).Distinct();
                    var keyValuePairs = new Dictionary<string, Dictionary<string, SpInventoryStyleShipment>>();
                    var keyValuePair = sizeList.ToDictionary(size => size.SizeRefId,
                        size =>
                            new SpInventoryStyleShipment()
                            {
                                ColorRefId = "Color",
                                SizeRefId = size.SizeName,
                                ShipmentQty = -1
                            });
                    keyValuePairs.Add("Color", keyValuePair);
                    foreach (var color in colorList)
                    {
                        keyValuePair =
                            orderShipDetails.Where(x => x.ColorRefId == color.ColorRefId)
                                .ToDictionary(bsd => bsd.SizeRefId,
                                    bsd =>
                                        new SpInventoryStyleShipment()
                                        {
                                            CompId = bsd.CompId,
                                            OrderStyleRefId = bsd.OrderStyleRefId,
                                            ColorRefId = color.ColorRefId,
                                            SizeRefId = bsd.SizeRefId,
                                            ShipmentQty = bsd.ShipmentQty
                                        });
                        keyValuePairs.Add(color.ColorName, keyValuePair);
                    }
                    string key = sh.StyleName + "X" + sh.OrderStyleRefId;
                    model.DictionaryList.Add(key, keyValuePairs);
                }

            }
            else
            {
                model.StyleShipment.StyleShipmentRefId = _styleShipmentManager.GetNewStyleShipmentRefId();
                model.StyleShipment.InvoiceDate = DateTime.Now;
                model.StyleShipment.ShipDate = DateTime.Now;
            }

            return View(model);
        }

        public ActionResult GetStyleShipment(StyleShipmentViewModel model)
        {
            ModelState.Clear();
            List<SpInventoryStyleShipment> orderShipDetails = _styleShipmentManager.GetStyleShipment(model.StyleShipment.OrderStyleRefId, PortalContext.CurrentUser.CompId, model.StyleShipment.StyleShipmentId);
            var colorList = orderShipDetails.Select(x => new { x.ColorRefId, x.ColorName }).Distinct().ToList();

            var sizeList = orderShipDetails.Select(x => new { x.SizeRefId, x.SizeName }).Distinct();
            var keyValuePairs = new Dictionary<string, Dictionary<string, SpInventoryStyleShipment>>();
            var keyValuePair = sizeList.ToDictionary(size => size.SizeRefId, size => new SpInventoryStyleShipment() { ColorRefId = "Color", SizeRefId = size.SizeName, ShipmentQty = -1 });
            keyValuePairs.Add("Color", keyValuePair);
            foreach (var color in colorList)
            {
                keyValuePair = orderShipDetails.Where(x => x.ColorRefId == color.ColorRefId).ToDictionary(bsd => bsd.SizeRefId, bsd => new SpInventoryStyleShipment() { CompId = bsd.CompId, OrderStyleRefId = bsd.OrderStyleRefId, ColorRefId = color.ColorRefId, SizeRefId = bsd.SizeRefId, ShipmentQty = bsd.ShipmentQty });
                keyValuePairs.Add(color.ColorName, keyValuePair);
            }
            model.Dictionary = keyValuePairs;
            var orderStyle = _buyOrdStyle.GetVBuyOrdStyleByRefId(model.StyleShipment.OrderStyleRefId);
            string key = orderStyle.StyleName + "X" + orderStyle.OrderStyleRefId;

            model.DictionaryList.Add(key, model.Dictionary);
            return PartialView("~/Areas/Inventory/Views/StyleShipment/_ShipmentMatrix.cshtml", model);
        }

        [HttpPost]
        [AjaxAuthorize(Roles = "shipmentchallan-2,shipmentchallan-3")]
        public ActionResult Save(StyleShipmentViewModel model)
        {
            int index = 0;
            try
            {
                model.StyleShipment.CompId = PortalContext.CurrentUser.CompId;
                foreach (var dictionary in model.DictionaryList)
                {
                    foreach (var dic in dictionary.Value)
                    {
                        foreach (var d in dic.Value.Where(x => x.Value.ShipmentQty > 0))
                        {
                            model.StyleShipment.Inventory_StyleShipmentDetail.Add(new Inventory_StyleShipmentDetail()
                            {
                                ColorRefId = d.Value.ColorRefId,
                                SizeRefId = d.Value.SizeRefId,
                                OrderStyleRefId = d.Value.OrderStyleRefId,
                                CompId = d.Value.CompId,
                                ShipmentQty = d.Value.ShipmentQty,
                                StyleShipmentId = model.StyleShipment.StyleShipmentId

                            });
                        }
                    }
                }
                index = model.StyleShipment.StyleShipmentId > 0 ? _styleShipmentManager.EditStyleShipment(model.StyleShipment) : _styleShipmentManager.SaveStyleShipment(model.StyleShipment);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Style Shipment :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Style Shipment !");
        }

        public ActionResult Delete(long styleShipmentId)
        {
            int index = 0;
            index = _styleShipmentManager.DeleteStyleShipmentById(styleShipmentId);
            return index > 0 ? Reload() : ErrorResult("Failed to Delete Style Shipment !");
        }
        public ActionResult AddNewStyleShipment(StyleShipmentViewModel model)
        {
            ModelState.Clear();
            return PartialView("~/Areas/Inventory/Views/StyleShipment/_ShipmentMatrix.cshtml", model);
        }
        public ActionResult ApprovedStyleShipmentList(StyleShipmentViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.StyleShipments = _styleShipmentManager.GetApprovedStyleShipmentByPaging(model.PageIndex, model.sort, model.sortdir, model.StyleShipment.IsApproved, PortalContext.CurrentUser.CompId, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult ApprovedStyleShipment(long styleShipmentId)
        {
            int index = 0;
            index = _styleShipmentManager.ApprovedStyleShipment(styleShipmentId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderByBuyer(string buyerRefId)
        {
            IEnumerable orderList = _buyOrdStyle.GetOrderByBuyer(buyerRefId);
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStyleByOrderNo(string orderNo)
        {
            IEnumerable styleList = _buyOrdStyle.GetStyleByOrderNo(orderNo);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeesBySearchCharacter(string searchCharacter)
        {
            var employees = _employeeManager.GetEmployeesByCardIdAndName(searchCharacter);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShipmentDeliveryChalan(long styleShipmentId)
        {
            DataTable shipmentChallan = _styleShipmentManager.GetShipmentChallan(styleShipmentId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "StyleShipmentReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ShipmentDset", shipmentChallan) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .1, MarginRight = .1, MarginBottom = .5 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult ShipmentStatus(StyleShipmentViewModel model)
        {
            DataTable shipmeDataTable = _styleShipmentManager.GetStockPostionDetail(model.StyleShipment.BuyerRefId,PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "ShipmentStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ShipDSet", shipmeDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 5, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .5, MarginLeft = 0.5, MarginRight = .5, MarginBottom = .5 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

    }
}