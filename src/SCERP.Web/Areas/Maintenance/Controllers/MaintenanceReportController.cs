using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.Model.Maintenance;
using SCERP.Model.Production;
using SCERP.Web.Areas.Maintenance.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Maintenance.Controllers
{
    public class MaintenanceReportController : BaseController
    {
        private readonly IReturnableChallanManager _returnableChallanManager;
        private readonly IReturnableChallanReceiveMasterManager _challanReceiveMasterManager;
        public MaintenanceReportController(IReturnableChallanManager returnableChallanManager, IReturnableChallanReceiveMasterManager challanReceiveMasterManager)
        {
            _returnableChallanManager = returnableChallanManager;
            _challanReceiveMasterManager = challanReceiveMasterManager;
        }
        public ActionResult ReturnableChallan(ReturnableChallanViewModel model)
        {
            ModelState.Clear();
            var reportType = ReportType.PDF;
            List<VwReturnableChallan> returnableChallans = _returnableChallanManager.GetReturnableChallanForReport(model.ReturnableChallan.ReturnableChallanId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Maintenance/Report"), "ReturnableChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", returnableChallans) }; 
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .75, MarginLeft = 0.70, MarginRight = 0.75, MarginBottom = .5 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult ReturnableChallanInfo(ReturnableChallanViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            var reportType = new ReportType();
          string challanType=  ChallanType.Maintenance;
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable returnableChallanInfoList = _returnableChallanManager.GetReturnableChallanInfo(model.DateFrom, model.DateTo,challanType, model.ChallanStatus, PortalContext.CurrentUser.CompId) ?? new DataTable();
            string path = Path.Combine(Server.MapPath("~/Areas/Maintenance/Report"), "ReturnableChallanInfo.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", returnableChallanInfoList) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult FabSpReturnableChallanStatus(ReturnableChallanViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            var reportType = new ReportType();
            string challanType = ChallanType.Fabric;
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable returnableChallanInfoList = _returnableChallanManager.GetReturnableChallanInfo(model.DateFrom, model.DateTo, challanType, model.ChallanStatus, PortalContext.CurrentUser.CompId) ?? new DataTable();
            string path = Path.Combine(Server.MapPath("~/Areas/Maintenance/Report"), "FabricSubProcessReturnableChallanStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FabSpRtStatusDSet", returnableChallanInfoList) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult ReturnableChallanReceive(ChallanReceiveMasterViewModel model) 
        {
            ModelState.Clear();
            var reportType = ReportType.PDF;
            DataTable returnableChallanReceives = _challanReceiveMasterManager.GetReturnableChallanReceive(model.ChallanReceiveMaster.ReturnableChallanReceiveMasterId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Maintenance/Report"), "ReturnableChallanReceive.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", returnableChallanReceives) }; ;
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .5 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
	}
}