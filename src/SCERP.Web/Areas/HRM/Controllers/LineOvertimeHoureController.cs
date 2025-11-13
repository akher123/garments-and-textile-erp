using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System.IO;
using Microsoft.Reporting.WebForms;
using SCERP.Web.Helpers;
using System.Collections.Generic;
using SCERP.Web.Models;
using System.Data;
using System.Globalization;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LineOvertimeHoureController : BaseController
    {

        private readonly ILineOvertimeHourManager _lineOvertimeHourManager;
        public LineOvertimeHoureController(ILineOvertimeHourManager lineOvertimeHourManager)
        {
            _lineOvertimeHourManager = lineOvertimeHourManager;
        }

        public ActionResult Index(ApprovedOvertimeHoureViewModel model)
        {
            ModelState.Clear();
            model.Companies = CompanyManager.GetAllPermittedCompanies();

            model.OtDate = model.OtDate ?? DateTime.Now.Date;
            model.LineOvertimeHours = _lineOvertimeHourManager.GetLineOvertimeHoureByOtDate(model.OtDate.GetValueOrDefault().Date);
            return View(model);
        }

        public ActionResult SendOvertimeHour(ApprovedOvertimeHoureViewModel model)
        {
            try
            {
                Guid? prepairedBy = PortalContext.CurrentUser.UserId;
                bool isSend = _lineOvertimeHourManager.SendOvertimeHour(prepairedBy, model.OtDate);
                model.DataTable = _lineOvertimeHourManager.GetOvertimeHoureByOtDate(model.OtDate.GetValueOrDefault().Date, true, true, true, true);
                var otList = RenderViewToString("~/Areas/HRM/Views/LineOvertimeHoure/_OTHList.cshtml", model);

                return Json(new { IsSend = isSend, OthLsit = otList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return Json(new { IsSend = false, OthLsit = "" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult OthList(ApprovedOvertimeHoureViewModel model)
        {
            ModelState.Clear();
            model.OtDate = model.OtDate ?? DateTime.Now.Date;
            model.DataTable = _lineOvertimeHourManager.GetOvertimeHoureByOtDate(model.OtDate.GetValueOrDefault().Date, model.All, model.Garments, model.Knitting, model.Dyeing);
            return PartialView("~/Areas/HRM/Views/LineOvertimeHoure/_OTHList.cshtml", model);
        }


        [HttpGet]
        public ActionResult DailyOtHoureApproval(ApprovedOvertimeHoureViewModel model)
        {
            ModelState.Clear();
            model.OtDate = model.OtDate ?? DateTime.Now.Date;
            model.LineOvertimeHours = _lineOvertimeHourManager.GetLineOvertimeHoureByOtDate(model.OtDate.GetValueOrDefault().Date);
            return View(model);
        }


        [HttpPost]
        public ActionResult FirstApprovedOtHour(long[] lineOvertimeHourIds)
        {
            Guid? userId = PortalContext.CurrentUser.UserId;
            if (lineOvertimeHourIds.Any())
            {
                int sta = _lineOvertimeHourManager.FirsApprovedOtHours(lineOvertimeHourIds, userId);
                return ErrorResult(sta > 0 ? "Approved Successfully!" : "Approved Failed!");
            }
            else
            {
                return ErrorResult("Select at least one for approval");
            }
        }

        public ActionResult SecondDailyOtApproval(ApprovedOvertimeHoureViewModel model)
        {
            ModelState.Clear();
            model.OtDate = model.OtDate ?? DateTime.Now.Date;
            model.LineOvertimeHours = _lineOvertimeHourManager.GetLineOvertimeHoureByOtDate(model.OtDate.GetValueOrDefault().Date);
            return View(model);
        }


        [HttpPost]
        public ActionResult ApprovedOtHourSecond(long[] lineOvertimeHourIds)
        {
            Guid? userId = PortalContext.CurrentUser.UserId;
            if (lineOvertimeHourIds.Any())
            {
                int sta = _lineOvertimeHourManager.SecondApprovedOtHours(lineOvertimeHourIds, userId);
                return ErrorResult(sta > 0 ? "Approved Successfully!" : "Approved Failed!");
            }
            else
            {
                return ErrorResult("Select atlist one for approval");
            }
        }


        public ActionResult EmployeeOtHour(ApprovedOvertimeHoureViewModel model)
        {
            ModelState.Clear();
            model.DataTable = _lineOvertimeHourManager.GetLineWiseEmployeeOTHours(model.TransactionDate, model.DepartmentLineId);
            return View(model);
        }

        public ActionResult OverTimeByLineReport(string date, bool all, bool garments, bool knitting, bool dyeing)
        {

            ModelState.Clear();

            DateTime dateSearch = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-gb"));

            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "OverTimeByLine.rdlc");

            DataTable dt = _lineOvertimeHourManager.GetOvertimeHoureByOtDate(dateSearch, all, garments, knitting, dyeing);

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", dt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }
    }
}