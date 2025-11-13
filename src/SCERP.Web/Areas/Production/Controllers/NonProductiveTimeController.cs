using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class NonProductiveTimeController : BaseController
    {
     
        private readonly IOmBuyerManager _buyerManager;
        private readonly IMachineManager _machineManager;
        private readonly IDownTimeCategoryManager _downTimeCategoryManager;
       
        private readonly INonProductiveTimeManager _nonProductiveTimeManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public NonProductiveTimeController(INonProductiveTimeManager nonProductiveTimeManager, IOmBuyerManager buyerManager, IMachineManager machineManager, IHourManager hourManager, IDownTimeCategoryManager downTimeCategoryManager, IOmBuyOrdStyleManager buyOrdStyle)
        {
            _nonProductiveTimeManager = nonProductiveTimeManager;
            _buyerManager = buyerManager;
            _machineManager = machineManager;
            _downTimeCategoryManager = downTimeCategoryManager;
            _buyOrdStyle = buyOrdStyle;
        }
        public ActionResult Index(NonProductiveTimeViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate??DateTime.Now;
            model.NonProductiveTimes = _nonProductiveTimeManager.GetNpts(model.FromDate,
                PortalContext.CurrentUser.CompId);
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(NonProductiveTimeViewModel model)
        {
            ModelState.Clear();
            if (model.NonProductiveTime.NonProductiveTimeId > 0)
            {
                model.NonProductiveTime =  _nonProductiveTimeManager.GetNptById(model.NonProductiveTime.NonProductiveTimeId);
                model.StartTime = model.NonProductiveTime.StartTime.ToString("hh:mm tt");
                model.EndTime =model.NonProductiveTime.EndTime!=null? model.NonProductiveTime.EndTime.GetValueOrDefault().ToString("hh:mm tt"):null;
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.NonProductiveTime.BuyerRefId);
                model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.NonProductiveTime.OrderNo);
            }
            else
            {
                model.NonProductiveTime.NptRefId= _nonProductiveTimeManager.GetNptRefId(PortalContext.CurrentUser.CompId);
                model.NonProductiveTime.EntryDate = DateTime.Now;
            }
            model.DownTimeCategories= _downTimeCategoryManager.GetDownTimeCategorys(PortalContext.CurrentUser.CompId);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.Lines = _machineManager.GetMachines(ProcessType.SEWING);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(NonProductiveTimeViewModel model)
        {
            int saveIndex = 0;
            model.NonProductiveTime.CompId = PortalContext.CurrentUser.CompId;
            model.NonProductiveTime.StartTime = model.NonProductiveTime.EntryDate.GetValueOrDefault().ToMargeDateAndTime(model.StartTime);
            if ( model.EndTime!=null)
            {
                model.NonProductiveTime.EndTime = model.NonProductiveTime.EntryDate.GetValueOrDefault().ToMargeDateAndTime(model.EndTime);
            } 
            saveIndex = model.NonProductiveTime.NonProductiveTimeId > 0 ? _nonProductiveTimeManager.EditNonProductiveTime(model.NonProductiveTime) : _nonProductiveTimeManager.SaveNonProductiveTime(model.NonProductiveTime);
            if (saveIndex > 0)
            {
                return Reload();
            }
            else
            {
                return ErrorResult("Save Failed");
            }

        }
       
        public ActionResult Delete(int nonProductiveTimeId)
        {
            int deleted = 0;
            deleted = _nonProductiveTimeManager.DeleteNpt(nonProductiveTimeId);
            if (deleted > 0)
            {
                return Reload();
            }
            else
            {
                return ErrorResult("Delete Failed");
            }
        }

        public ActionResult MonthlyNpt(NonProductiveTimeViewModel model)
        {
           ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now;
            model.ToDate = model.ToDate ?? DateTime.Now;
            model.NonProductiveTimes = _nonProductiveTimeManager.GetDateWiseNpts(model.FromDate, model.ToDate,
                PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult DailyNptReport(DateTime?currentDate, int reportType)
        {
            var rType = ReportType.PDF;

            switch (Convert.ToInt32(reportType))
            {
                case 1:
                    rType = ReportType.PDF;
                    break;
                case 3:
                    rType = ReportType.Excel;
                    break;
            }
            List<VwNonProductiveTime> nonProductiveTimes = _nonProductiveTimeManager.GetNpts(currentDate, PortalContext.CurrentUser.CompId);


            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyNpt.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DailyNptDSet", nonProductiveTimes) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(rType, path, reportDataSources, deviceInformation);
        }

        public ActionResult MonthlyNptReport(DateTime? fromDate,DateTime? toDate, int reportType)
        {
            var rType = ReportType.PDF;

            switch (Convert.ToInt32(reportType))
            {
                case 1:
                    rType = ReportType.PDF;
                    break;
                case 3:
                    rType = ReportType.Excel;
                    break;
            }
            List<VwNonProductiveTime> nonProductiveTimes = _nonProductiveTimeManager.GetDateWiseNpts(fromDate,toDate, PortalContext.CurrentUser.CompId);


            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyNptReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var fromDateParm = new ReportParameter("FromDate", fromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var toDateParm = new ReportParameter("ToDate", toDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDateParm, toDateParm };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MonthlyNptDSet", nonProductiveTimes) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(rType, path, reportDataSources, deviceInformation, reportParameters);



     
           
        }
    }
}