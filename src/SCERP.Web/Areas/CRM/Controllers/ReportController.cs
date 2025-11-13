using System.Globalization;
using System.IO;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.Manager.CRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CRMModel;
using SCERP.Model.Custom;
using SCERP.Web.Areas.CRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.CRM.Controllers
{
    public class ReportController : BaseCrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult ProjectReport(ProjectDocumentInfoViewModel model)
        {
            ModelState.Clear();
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.Modules = ProjectDocumentInfoManager.GetAllModulesInfo();
            return View(model);
        }

        public ActionResult ProjectReportPrint(int moduleId, string fromDate, string toDate, string searchString)
        {
            DateTime? fromD = new DateTime(2001, 1, 1);
            DateTime? toD = new DateTime(2020, 1, 1);

            if (!string.IsNullOrEmpty(fromDate))
                fromD = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));

            if (!string.IsNullOrEmpty(toDate))
                toD = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            List<SPCRMDocumentationReport> reportdata = CRMReportManager.GetDocumentReport(moduleId, fromD, toD, searchString.Trim());

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/CRM/Reports"), "DocumentationReport.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("ReportDS", reportdata);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
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
    }
}
