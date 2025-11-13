using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Custom;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class DailyFabricReceiveController : BaseController
    {
        private readonly IDailyFabricReceiveManager _dailyFabricReceiveManager;
        private readonly IOmBuyerManager _buyerManager;
        public DailyFabricReceiveController(IDailyFabricReceiveManager dailyFabricReceiveManager, IOmBuyerManager buyerManager)
        {
            _dailyFabricReceiveManager = dailyFabricReceiveManager;
            _buyerManager = buyerManager;
        }
        public ActionResult Index(DailyFabricReceiveViewModel model)
        {
            ModelState.Clear();
            model.OmBuyers = _buyerManager.GetAllBuyers();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.ReceivedDate = DateTime.Now;
            }
            else
            {
                model.ReceivedDate = model.ReceivedDate;
                model.ReceivedFabricProductionSummaries = _dailyFabricReceiveManager.GetVwReceivedFabricProductionSummary(model.SearchString, model.BuyerRefId, model.ReceivedDate);
            }

            return View(model);
        }

        public ActionResult Edit(DailyFabricReceiveViewModel model)
        {
            ModelState.Clear();
            model.DailyFabricReceive = _dailyFabricReceiveManager.GetDailyFabricReceiveByTodayDate(model.ReceivedDate, model.OrderStyleRefId, model.ComponentRefId, model.ConsRefId, model.ColorRefId) ?? new PROD_DailyFabricReceive();
            model.DailyFabricReceive.OrderStyleRefId = model.OrderStyleRefId;
            model.DailyFabricReceive.ConsRefId = model.ConsRefId;
            model.DailyFabricReceive.ColorRefId = model.ColorRefId;
            model.DailyFabricReceive.ComponentRefId = model.ComponentRefId;
            model.DailyFabricReceive.ReceivedDate = model.ReceivedDate;
            model.FabricProductionSummary = _dailyFabricReceiveManager.GetDailyFabricReceive(model.StyleName, model.OrderNo, model.OrderStyleRefId, model.ConsRefId, model.ComponentRefId, model.ColorRefId);
            return View(model);
        }

        public ActionResult Save(DailyFabricReceiveViewModel model)
        {
            var saveIndex = 0;
            saveIndex = model.DailyFabricReceive.FabricReceiveId > 0 ? _dailyFabricReceiveManager.EditDailyFabricReceive(model.DailyFabricReceive) : _dailyFabricReceiveManager.SaveDailyFabricReceive(model.DailyFabricReceive);
            return saveIndex > 0 ? Reload() : ErrorResult("Save Failed");

        }

        public ActionResult FabricReceivdSummaryReport(DailyFabricReceiveViewModel model)
        {

            var dailyFabricReceiveReport = _dailyFabricReceiveManager.GetVwReceivedFabricProductionSummary(model.SearchString, model.BuyerRefId, model.ReceivedDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "DailyFabricReceiveReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { new ReportParameter("ReceivedDate", model.ReceivedDate.ToString())};
            var reportDataSources = new List<ReportDataSource>() {new ReportDataSource("FabricConsumptionDS", dailyFabricReceiveReport) };
            var deviceInformation = new DeviceInformation(){ OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0,  MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
           
        }



    }
}