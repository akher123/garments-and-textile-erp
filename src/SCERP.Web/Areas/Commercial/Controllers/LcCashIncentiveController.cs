using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.Manager.CommercialManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class LcCashIncentiveController : BaseController
    {
        private readonly ILcManager _lcManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly int _pageSize = AppConfig.PageSize;
        public LcCashIncentiveController(ILcManager lcManager, IOmBuyerManager buyerManager)
        {
            _lcManager = lcManager;
            _buyerManager = buyerManager;
        }

        public ActionResult Index(LcViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.Buyers = _buyerManager.GetAllBuyers();
                int totalRecords = 0;
                model.LcInfos = _lcManager.GetLcInfosByPaging(model.PageIndex, _pageSize, out totalRecords, model.BuyerId, model.FromDate, model.ToDate, model.SearchString, model.CompleteStatus);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(LcViewModel model)
        {
            ModelState.Clear();
            model.CommLcInfo = _lcManager.GetLcInfoById(model.CommLcInfo.LcId);
            model.CommLcInfo.AppliedDate=model.CommLcInfo.AppliedDate ?? DateTime.Now;
            //model.CommLcInfo.ReceiveDate = model.CommLcInfo.ReceiveDate ?? DateTime.Now;
            return View(model);
        }

        public ActionResult Save(LcViewModel model)
        {
            int edited= _lcManager.UpdateChashIncentive(model.CommLcInfo);
            return edited>0 ? Reload() : ErrorResult("Cash Incentive Update Failed");
        }

        public ActionResult CashIncentiveReport(LcViewModel model)
        {
            List<COMMLcInfo> commLcInfos = _lcManager.GetCashIncentiveReport(model.BuyerId, model.FromDate, model.ToDate, model.SearchString);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CashIncentiveStaatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            //ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
          //  ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
           // var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CashIncentiveDSet", commLcInfos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = .2, MarginRight = .2, MarginBottom = .2};
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult CashIncentiveByDateReport(LcViewModel model)
        {
            List<COMMLcInfo> commLcInfos = _lcManager.GetCashIncentiveByDateReport(model.FromDate, model.ToDate, model.SearchString);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CashIncentiveReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            //ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            //  ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            // var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("InvoiceDataSet", commLcInfos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14.69, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
    }
}