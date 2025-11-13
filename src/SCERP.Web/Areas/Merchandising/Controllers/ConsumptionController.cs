using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ConsumptionController : BaseController
    {
      
        private readonly IConsumptionManager _consumptionManager;
        private readonly IConsumptionTypeManager _consumptionTypeManager;
        private readonly IConsumptionGroupManager _consumptionGroupManager;
        private readonly IOmStyleManager _omStyleManager;
        private readonly IConsumptionDetailManager _consumptionDetailManager;
        private IOmBuyOrdStyleManager _buyOrdStyleManager;
        public ConsumptionController(IConsumptionManager consumptionManager,
            IConsumptionTypeManager consumptionTypeManager, IConsumptionDetailManager consumptionDetailManager,
            IConsumptionGroupManager consumptionGroupManager, IOmStyleManager omStyleManager, IOmBuyOrdStyleManager buyOrdStyleManager)
        {
            
            this._consumptionManager = consumptionManager;
            this._consumptionTypeManager = consumptionTypeManager;
            this._consumptionGroupManager = consumptionGroupManager;
            this._omStyleManager = omStyleManager;
            this._consumptionDetailManager = consumptionDetailManager;
            this._buyOrdStyleManager = buyOrdStyleManager;

        }
        [AjaxAuthorize(Roles = "consumptionanalysis-1,consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult ConsumptionProcess(ConsumptionViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }
        [AjaxAuthorize(Roles = "consumptionanalysis-1,consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult OrderStyleList(ConsumptionViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            var omBuyOrdStyle = new OM_BuyOrdStyle { page = model.page, sort = model.sort, sortdir = model.sortdir, FromDate = model.FromDate, ToDate = model.ToDate, SearchString = model.SearchString };
            model.OmBuyOrdStyles = _consumptionManager.GetVwConsuptionOrderStyle(omBuyOrdStyle, out totalRecords);
            model.TotalRecords = totalRecords;
            return View("~/Areas/Merchandising/Views/Consumption/OrderStyleList.cshtml", model);
        }
        [AjaxAuthorize(Roles = "consumptionanalysis-1,consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult Index(ConsumptionViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.Consumptions = _consumptionManager.GetConsumptionList(model.OrderStyleRefId);
                model.ConsRefId = _consumptionManager.GetNewConsRefId();
                model.ConsumptionTypes = _consumptionTypeManager.GetConsumptionTypes();
                model.ConsumptionGroups = _consumptionGroupManager.GetConsumptionGroups();
                model.ConsDate = DateTime.Now;
                model.ButtonName = "Save";
                model.BuyOrdStyle = _buyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View("~/Areas/Merchandising/Views/Consumption/Index.cshtml", model);
        }
        [AjaxAuthorize(Roles = "consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult Save(OM_Consumption model)
        {
            try
            {
                var saveIndex = 0;
                ModelState.Clear();
                if (model.ItemCode.Any())
                {
                    saveIndex = model.ConsumptionId > 0 ? _consumptionManager.EditConsumption(model) : _consumptionManager.SaveConsumption(model);
                }
                else
                {
                    return ErrorResult("Invalid Item Name , Select Correct one !!!");
                }
                if (saveIndex > 0)
                {
                    return RedirectToAction("Index", new
                    {
                        model.OrderNo,
                        model.OrderStyleRefId
                    });
                }
                else
                {
                    return ErrorResult("Save Fail!");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
        [AjaxAuthorize(Roles = "consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult Edit(ConsumptionViewModel model)
        {
            ModelState.Clear();
            VConsumption consumption = _consumptionManager.GetVConsumptionById(model.ConsumptionId) ?? new VConsumption();
            model.ConsRefId = consumption.ConsRefId;
            model.OrderStyleRefId = consumption.OrderStyleRefId;
            model.OrderNo = consumption.OrderNo;
            model.ConsDate = consumption.ConsDate;
            model.ConsGroup = consumption.ConsGroup;
            model.ConsTypeRefId = consumption.ConsTypeRefId;
            model.ItemCode = consumption.ItemCode;
            model.SearchItemKey = consumption.ItemName;
            model.Quantity = consumption.Quantity;
            model.ItemDescription = consumption.ItemDescription;
            model.ButtonName = "Update";
            model.ConsumptionTypes = _consumptionTypeManager.GetConsumptionTypes();
            model.ConsumptionGroups = _consumptionGroupManager.GetConsumptionGroups();
            return PartialView("~/Areas/Merchandising/Views/Consumption/_Edit.cshtml", model);
        }
        public ActionResult Refresh(ConsumptionViewModel model)
        {
            ModelState.Clear();
            var refrsh = new ConsumptionViewModel
            {
                ConsDate = DateTime.Now,
                OrderNo = model.OrderNo,
                OrderStyleRefId = model.OrderStyleRefId,
                ConsRefId = _consumptionManager.GetNewConsRefId(),
                ButtonName = "Save",
                ConsumptionTypes = _consumptionTypeManager.GetConsumptionTypes(),
                ConsumptionGroups = _consumptionGroupManager.GetConsumptionGroups()
            };
            return PartialView("~/Areas/Merchandising/Views/Consumption/_Edit.cshtml", refrsh);
        }

        public JsonResult AutocompliteItem(string searchItemKey)
        {
            var items = _omStyleManager.GetItemForStyle(searchItemKey);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string consRefId, long consumptionId, string orderStyleRefId, string orderNo)
        {
            try
            {
                int deletedIndex = _consumptionManager.DeleteConsumption(consRefId, consumptionId);
                if (deletedIndex > 0)
                {
                    return RedirectToAction("Index", new
                    {
                        OrderStyleRefId = orderStyleRefId
                    });
                }
                else
                {
                    return ErrorResult("Delete Failed");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }

        public ActionResult StyleWiseConsumtionDetail([Bind(Include = "OrderStyleRefId,IsShowReport,ReportType")]ConsumptionViewModel model)
        {
            model.ConsumptionDetailDataTable = _consumptionDetailManager.GetAccessoriesConsumptionDetail(model.OrderStyleRefId);
            if (model.IsShowReport)
            {
                var reportParameters = new List<ReportParameter>() { new ReportParameter("ReportTitle","ACCESSORIES" )};
                var reportType = new ReportType();

                switch (Convert.ToInt32(model.ReportType))
                {
                    case 1:
                        reportType = ReportType.PDF;
                        break;
                    case 3:
                        reportType = ReportType.Excel;
                        break;
                }
                List<SPOrderStyleDetailForBOM> bomdetail = _consumptionDetailManager.GetOrderStyleDetailForBOM(model.OrderStyleRefId);
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "BillOfMaterialReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                   return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderStyleDetailForBOM", bomdetail), new ReportDataSource("BOMDS", model.ConsumptionDetailDataTable)
            };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
                return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
            }
            return View(model);
        }


        public ActionResult OrderWiseAccBOM(string orderNo,int reportType)
        {
                DataTable onsumptionDetailDataTable = _consumptionDetailManager.GetAccessoriesConsumptionDetailByOrder(orderNo);
                var reportParameters = new List<ReportParameter>() { new ReportParameter("ReportTitle", "ACCESSORIES") };

                List<SPOrderStyleDetailForBOM> bomdetail =new List<SPOrderStyleDetailForBOM>();
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "BillOfMaterialReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderStyleDetailForBOM", bomdetail), new ReportDataSource("BOMDS", onsumptionDetailDataTable)
                };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
                return ReportExtension.ToFile((ReportType)reportType, path, reportDataSources, deviceInformation, reportParameters);
            }
        
        
   
   
        public ActionResult StyleWiseFabrConsumtionDetail([Bind(Include = "OrderStyleRefId,IsShowReport,ReportType")]ConsumptionViewModel model)
        {
            model.ConsumptionDetailDataTable = _consumptionDetailManager.GetVConsumptionDetailsByStyleRefId(model.OrderStyleRefId);
            if (model.IsShowReport)
            {
                
                var reportType = new ReportType();

                switch (Convert.ToInt32(model.ReportType))
                {
                    case 1:
                        reportType = ReportType.PDF;
                        break;
                    case 3:
                        reportType = ReportType.Excel;
                        break;
                }
                List<SPOrderStyleDetailForBOM> bomdetail = _consumptionDetailManager.GetOrderStyleDetailForBOM(model.OrderStyleRefId);
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "FabricConsumtionDetailBoMReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =14, PageHeight = 8.50, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .1 };
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderInfoDSet", bomdetail), new ReportDataSource("FabricConsDSet", model.ConsumptionDetailDataTable)
            };
                return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
             
            }
            return PartialView("_CustomError");
        }
    }
}