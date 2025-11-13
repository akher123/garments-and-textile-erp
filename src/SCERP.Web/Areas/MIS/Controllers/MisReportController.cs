using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.Manager.ProductionManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class MisReportController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IProductionReportManager _reportManager;
        public MisReportController(IOmBuyerManager buyerManager,IProductionReportManager reportManager)
        {
            _reportManager = reportManager;
            this._buyerManager = buyerManager;


        }
        public ActionResult DyeingProfitabilyAnalysis(MisDashboardViewModel model)
        {
            model.SummaryDataTable = _reportManager.DyeingProfitabilyAnalysis(model.YearId);
            return View(model);
        }



        public ActionResult ShowDyeingProfitabilyAnalysisReport(int yearId)
        {
            DataTable dataTable = _reportManager.DyeingProfitabilyAnalysis(yearId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DyeingProfitabilityAnalysisReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DyeingProfitibiltyDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop =.5, MarginLeft = .2, MarginRight =.2, MarginBottom =.2};
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }


        public ActionResult OrderClosingStatus(MisDashboardViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.Buyers = _buyerManager.GetAllBuyers();
                model.IsSearch = true;
                model.FilterDate = DateTime.Now;
                return View(model);
            }
            else
            {
                DataTable dataTable = _reportManager.GetOrderClosingStatus(model.BuyerRefId??"-1",model.OrderNo ?? "-1", model.OrderStyleRefId ?? "-1");
                string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "OrderClosingReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderClosing", dataTable) };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 36.5, PageHeight = 9, MarginTop = .1, MarginLeft = .1, MarginRight = .1, MarginBottom = .1 };
                var reportExport = ReportExtension.ToFile(((ReportType)Convert.ToInt32(model.ReportType)), path, reportDataSources, deviceInformation);
                return reportExport;
            }
         
        }

        public ActionResult ProductionCapacity(MisDashboardViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.FilterDate = DateTime.Now;
                return View(model);
            }
            else
            {
                DataTable dataTable = _reportManager.GetDailyProductionCapacity(model.FilterDate);
                string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "DailyProdcutivityEfficniencyReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DailyPductivityDSet", dataTable) };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.5, PageHeight = 11, MarginTop = 1, MarginLeft = 1, MarginRight = 1, MarginBottom = 1};
                var reportExport = ReportExtension.ToFile(((ReportType)Convert.ToInt32(model.ReportType)), path, reportDataSources, deviceInformation);
                return reportExport;
            }

        }
	}
}