using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class ReportController : BaseController
    {

        private readonly SupplierCompanyManager _supplierCompany;
        private readonly StringBuilder _urlBuilder;
        private readonly IStockRegisterManager _stockRegisterManager;
        private readonly IInventoryGroupManager _groupManager;
        private readonly IMaterialReceivedManager _materialReceivedManager;
        private readonly IStyleShipmentManager _shipmentManager;
        private IMaterialIssueManager _materialIssueManager;
        public ReportController(IStyleShipmentManager shipmentManager,SupplierCompanyManager supplierCompany,IInventoryGroupManager groupManager, IStockRegisterManager stockRegisterManager, IMaterialReceivedManager materialReceivedManager, IMaterialIssueManager materialIssueManager)
        {
            _shipmentManager = shipmentManager;
            _supplierCompany = supplierCompany;
            _groupManager = groupManager;
            _stockRegisterManager = stockRegisterManager;
            _urlBuilder = new StringBuilder();
            _urlBuilder.Append("http://");
            _urlBuilder.Append(AppConfig.ReportServerAddress);
            _urlBuilder.Append("/reportserver/Pages/ReportViewer.aspx?");
            _urlBuilder.Append("/SCERPREPORT.Inventory/");
            _materialReceivedManager = materialReceivedManager;
            _materialIssueManager = materialIssueManager;
        }

        public ActionResult ReportView(string reportName, string userName)
        {
            _urlBuilder.Append(reportName);
            if (userName == "currentUser")
            {
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&UserName=" + PortalContext.CurrentUser.Name);
            }
            ViewBag.ReportUrl = _urlBuilder;
            return PartialView("_SSRSReportContorl");
        }

        public ActionResult SsrsReportView(string reportName, string userName)
        {
            StringBuilder builder=new StringBuilder();
            builder.Append("/pages/Viewer.aspx?id=");
            builder.Append(reportName);

            ViewBag.ReportUrl = builder;
            return PartialView("_SSRSReportContorl");
        }
        public ActionResult GrnRegisterStatus(MaterialReceivedViewModel model)
        {
            ModelState.Clear();
            var reportType = ReportType.Excel;
            DataTable materialReceivedDataTable = _materialReceivedManager.GetMaterialReceivedDataTable(model.FromDate, model.ToDate, model.MaterialReceived.ChallanNo, model.MaterialReceived.RegisterType,PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "MaterialReceived.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", materialReceivedDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =11, PageHeight = 8.5, MarginTop = 0, MarginLeft = 0, MarginRight = 0.25, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult ItemWiseIssueStatement(InventoryReportViewModel model)
        {
            ModelState.Clear();
         
            if (!model.IsSearch)
            {
                model.ReprotUrl = null;
                model.IsSearch = true;
            }
            else
            {
                _urlBuilder.Append("ItemWiseIssueStatement");
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&FromDate=" + model.FromDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ToDate=" + model.ToDate.GetValueOrDefault().Date);
                if (model.ItemCode==null)
                {
                    model.ItemCode = "-1";
                }
                _urlBuilder.Append("&ItemCode=" + model.ItemCode);
                 model.ReprotUrl = _urlBuilder;
            
            }
            return View(model);
        }
        public ActionResult MaterialRateHistory(InventoryReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.ReprotUrl = null;
                model.IsSearch = true;
            }
            else
            {
                _urlBuilder.Append("MaterialRateHistory");
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&FromDate=" + model.FromDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ToDate=" + model.ToDate.GetValueOrDefault().Date);
                if (model.ItemCode == null)
                {
                    model.ItemCode = "-1";
                }
                _urlBuilder.Append("&ItemCode=" + model.ItemCode);
                _urlBuilder.Append("&CompId=" +PortalContext.CurrentUser.CompId);
                _urlBuilder.Append("&HostingServerAddress=" + AppConfig.HostingServerAddress);
                model.ReprotUrl = _urlBuilder;

            }
            return View(model);
        }
        public ActionResult ItemLedger(InventoryReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.ReprotUrl = null;
                model.IsSearch = true;
            }
            else
            {
                _urlBuilder.Append("ItemLedger");
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&FromDate=" + model.FromDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ToDate=" + model.ToDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ItemId=" + model.ItemId);
                _urlBuilder.Append("&CompId=" + PortalContext.CurrentUser.CompId);
                _urlBuilder.Append("&HostingServerAddress=" + AppConfig.HostingServerAddress);
                model.ReprotUrl = _urlBuilder;

            }
            return View(model);
        }
        public ActionResult LoanItemLedger(InventoryReportViewModel model)
        {
            ModelState.Clear();
            model.SupplierCompanies = _supplierCompany.GetAllSupplierCompany();
            if (!model.IsSearch)
            {
                model.ReprotUrl = null;
                model.IsSearch = true;
            }
            else
            {
                _urlBuilder.Append("LoanItemLedger");
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&FromDate=" + model.FromDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ToDate=" + model.ToDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ItemId=" + model.ItemId);
                _urlBuilder.Append("&SupplierId=" + model.SupplierId);
                _urlBuilder.Append("&CompId=" + PortalContext.CurrentUser.CompId);
                _urlBuilder.Append("&HostingServerAddress=" + AppConfig.HostingServerAddress);
                model.ReprotUrl = _urlBuilder;

            }
            return View(model);
        }
        public ActionResult ItemWiseStorePurchaseRequisition(InventoryReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.ReprotUrl = null;
                model.IsSearch = true;
            }
            else
            {
                _urlBuilder.Append("ItemWiseStorePurchaseRequisition");
                _urlBuilder.Append("&rs:Command=Render");
                _urlBuilder.Append("&FromDate=" + model.FromDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ToDate=" + model.ToDate.GetValueOrDefault().Date);
                _urlBuilder.Append("&ItemId=" + model.ItemId);
                _urlBuilder.Append("&CompId=" + PortalContext.CurrentUser.CompId);
                _urlBuilder.Append("&HostingServerAddress=" + AppConfig.HostingServerAddress);
                model.ReprotUrl = _urlBuilder;
                 if (model.ItemId==0)
                {
                    return ErrorResult("Invalid Item Name");
                }
            }
         
            return View(model);
        }

        public ActionResult ShowReport(InventoryReportViewModel model)
        {
            ModelState.Clear();
            model.Groups = _groupManager.GetGroups();
            return View(model);
        }

        public ActionResult StockReport(InventoryReportViewModel model)
        {
            List<VwStockPosition> stockPositions = _stockRegisterManager.GetStockPostion(model.FromDate, model.ToDate, model.GroupId, model.SubGroupId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AdvanceStockReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GnStockDataSet", stockPositions) };
            var deviceInformation = new DeviceInformation() { OutputFormat =5, PageWidth = 14, PageHeight = 8.5, MarginTop = 0, MarginLeft = 0.4, MarginRight =0, MarginBottom = 0 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
            
        }
        public ActionResult MonthlyShipmentSummary(InventoryReportViewModel model)
        {
            DataTable shipDataTable = _shipmentManager.GetMonthlyShipmentSummary(model.FromDate, model.ToDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "MonthlyShipmentSymmary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ShipSatatusDSet", shipDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft =.2, MarginRight = .2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);

        }

        public ActionResult StockReportDetail(InventoryReportViewModel model)
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
            List<VwStockPosition> stockPositions = _stockRegisterManager.GetStockPostionDetail(model.FromDate, model.ToDate, model.GroupId, model.SubGroupId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AdvanceStockPositionDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AdvanceStockPositionDetailDSet", stockPositions) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 5, PageWidth = 14, PageHeight = 8.5, MarginTop =.2, MarginLeft = 0.1, MarginRight =.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
        }



        public ActionResult BuyerWiseStockReportDetail(InventoryReportViewModel model)
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
            List<VwStockPosition> stockPositions = _stockRegisterManager.GetBuyerWiseStockPostionDetail(model.FromDate, model.ToDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "BuyerWiseYarnStockReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BuyerWiseYarnStockDSet", stockPositions) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 5, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
        }

        public ActionResult DyedYarnStockReportDetail(InventoryReportViewModel model)
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
            List<VwStockPosition> stockPositions = _stockRegisterManager.GetDyedYarnStockPostionDetail(model.FromDate, model.ToDate, model.GroupId, model.SubGroupId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "DyedYarnStockPositionDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AdvanceStockPositionDetailDSet", stockPositions) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);

        }

        public ActionResult ChemicalIssueReport(int materialIssueId)
        {
            DataTable chemicalIssueChallan = _materialIssueManager.GetChemicalIssueChallan(materialIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "ChemicalIssueReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ChemicalIssueDSet", chemicalIssueChallan) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 1, MarginRight = 1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

        public ActionResult GetGeneralItemIssue(long materialIssueId)
        {
            string reportName = "GeneralIssueReport";
            var reportParams = new List<ReportParameter> 
            { new ReportParameter("MaterialIssueId", materialIssueId.ToString()),
            new ReportParameter("CompId", PortalContext.CurrentUser.CompId),
            new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress) };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }

    }

    
}