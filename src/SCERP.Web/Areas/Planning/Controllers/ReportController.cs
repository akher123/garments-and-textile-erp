using System.Globalization;
using System.IO;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ReportController : BasePlanningController
    {
        private readonly IPlanningReportManager _planningReportManager;
        private readonly IProcessManager _processManager;
        private readonly IProgramManager _programManager;
        private readonly IPartyManager _partyManager;
        public ReportController(IPartyManager partyManager, IPlanningReportManager planningReportManager, IProcessManager processManager, IProgramManager programManager)
        {
            this._planningReportManager = planningReportManager;
            this._processManager = processManager;
            _programManager = programManager;
            _partyManager = partyManager;
        }

        public ActionResult TNAReportPrint(TnaViewModel model)
        {
          
            List<PLAN_TNAReport> reportdata = _planningReportManager.GetTNAReportData(model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TNAReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("TNADS", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .3, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

        public ActionResult TnaResponsePersonReportPrint(string person, int? activityId, string fromDate, string toDate, Guid? responsible)
        {
            List<PLAN_TNAReport> reportdata = _planningReportManager.GetTnaResponsePersonReportData(person);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TnaResponsePerson.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .3, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

        public ActionResult TnaGroupUpdatePrint(string seasonId, string buyerId, string merchandiserId, string styleUfId, string styleUfName, string fromDate, string toDate)
        {
            try
            {
                PLAN_TNAHorizontal tna = new PLAN_TNAHorizontal();
                tna.SeasonRefId = seasonId;
                tna.BuyerName = buyerId;
                tna.MerchandiserName = merchandiserId;
                tna.OrderStyleRefId = styleUfId.Trim();
                if (fromDate != null) tna.FromDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
                if (toDate != null) tna.ToDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

                List<PLAN_TNAHorizontal> reportdata = _planningReportManager.GetTnaGroupUpdateReport(tna);

                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TnaUpdateReport.rdlc");
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;
                else
                    return View("Index");

                ReportParameter[] parameters = new ReportParameter[3];

                if (styleUfName != null)
                    parameters[0] = new ReportParameter("param_Particulars", styleUfName.Trim());
                else
                    parameters[0] = new ReportParameter("param_Particulars", "Not found");

                if (fromDate != null)
                    parameters[1] = new ReportParameter("param_FromDate", fromDate);
                else
                    parameters[1] = new ReportParameter("param_FromDate", "Not found");

                if (toDate != null)
                    parameters[2] = new ReportParameter("param_ToDate", toDate);
                else
                    parameters[2] = new ReportParameter("param_ToDate", "Not found");

                ReportDataSource rd = new ReportDataSource("TnaUpdateDS", reportdata);
                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);
                string reportType = "pdf";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>" + 2 + "</OutputFormat>" +
                    "  <PageWidth>11.69in</PageWidth>" +
                    "  <PageHeight>8.27in</PageHeight>" +
                    "  <MarginTop>0..2in</MarginTop>" +
                    "  <MarginLeft>.4in</MarginLeft>" +
                    "  <MarginRight>.2in</MarginRight>" +
                    "  <MarginBottom>0.2in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return null;
        }



        public ActionResult StyleWiseSmvDetail(PlanningReportViewModel model)
        {

            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = _planningReportManager.GetStyleWiseSmvDetal(model.FromDate, model.ToDate);
                ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.ToString());
                ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.ToString());
                ReportParameter[] reportParameters = new[] { fromDateParameter, toDateParameter };
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Planning/Reports/StyleWiseSmvDetailReport.rdlc",
                    DataSetName = "StyleWiseSmvDetailDataSet",
                    ReportParameters = reportParameters
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }

        public ActionResult StyleWiseSmv(PlanningReportViewModel model)
        {

            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = _planningReportManager.GetStyleWiseSmv();

                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Planning/Reports/StyleWiseSMVReport.rdlc",
                    DataSetName = "StyleWiseSmvDataSet",

                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }

        public ActionResult ShowMachineCapacity(PlanningReportViewModel model)
        {

            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.Processes = _processManager.GetProcess();
                return View(model);
            }
            else
            {
                model.DataTable = _planningReportManager.GetMachineCapacity(model.ProcessRefId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Planning/Reports/MachineCapacityReport.rdlc",
                    DataSetName = "MachineCapacityDataSet",
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }

        public ActionResult KnittingSatus(ProgramViewModel model)
        {
            ModelState.Clear();
            model.Parties = _partyManager.GetParties("P");
            return View(model);
        }

        public ActionResult KnittingProgramStatusReport(ProgramViewModel model)
        {
            var reportType = ReportType.PDF;
            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            List<VwProgram> vPrograms = _programManager.GetKnittingProgramStatus(model.FromDate, model.ToDate, model.ProcessRefId, model.Program.PartyId, model.Program.ProcessorRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "KnittingProgramStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KntProgramStatusDSet", vPrograms) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult PartyWiseKnittingBalance(ProgramViewModel model)
        {
            var reportType = ReportType.PDF;
            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable vPrograms = _programManager.GetPartyWiseKnittingBalance(model.ProcessRefId, PortalContext.CurrentUser.CompId, model.Program.PartyId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "PartyWiseKnittingReciveBalance.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("PartyWiseKnitDset", vPrograms) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.3, MarginRight = 0.3, MarginBottom = 0.5 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult RunningOrderSweingPlanStatus(ReportType reportType)
        {
            DataTable sweingOrderPlanStatus = _planningReportManager.GetSweingOrderPlanStatus( PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "SweingOrderPlanStatustReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderPalnStatusDSet", sweingOrderPlanStatus) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1};
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

    }
}