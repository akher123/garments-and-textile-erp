using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMarketingManager;
using SCERP.Web.Areas.Marketing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Marketing.Controllers
{
    public class MarketingReportsController : Controller
    {

        private readonly IMarketingInstituteManager _marketingInstituteManager;

        private readonly IMarketingInquiryManager _marketingInquiryManager;

        public MarketingReportsController(IMarketingInstituteManager marketingInstituteManager, IMarketingInquiryManager marketingInquiryManager)
        {
            _marketingInstituteManager = marketingInstituteManager;
            _marketingInquiryManager = marketingInquiryManager;
        }

        // GET: Marketing/MarketingReports
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ClientListIndex(MarketingInstituteViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.MarketingInstitutes = _marketingInstituteManager.GetMarketingtInstitute(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult ClientList(ReportType reportType)
        {
            var marketingInstitutes = _marketingInstituteManager.GetAllMarketingtInstitute();
            string path = Path.Combine(Server.MapPath("~/Areas/Marketing/Reports"), "ClientList.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ClientDataSet", marketingInstitutes) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = 1, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 1 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }

        public ActionResult MarketingInquiryIndex(MarketingInquiryViewModel model)
        {

            ModelState.Clear();
            int totalRecords = 0;
            model.MarketingInstitute = _marketingInstituteManager.GetAllMarketingtInstitute();
            model.MarketingInquiries = _marketingInquiryManager.GetMarketingInquiry(model.PageIndex, model.sort, model.sortdir, model.SearchString, model.MarketingInquiriy.MarketingPersonId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult MarketingReportPdf(MarketingInquiryViewModel model)
        {
            MarketingInquiryViewModel model2 = new MarketingInquiryViewModel();
            MarketingInquiryViewModel model3 = new MarketingInquiryViewModel();

            model.MarketingInstitute = _marketingInstituteManager.GetAllMarketingtInstitute();
            model.MarketingInquiries = _marketingInquiryManager.GetAllMarketingInquiry();

            var inquiryList =

            from inq in model.MarketingInquiries
            join ins in model.MarketingInstitute
                 on inq.InstituteId equals ins.InstituteId
                 where inq.InquiryDate>=model.FromDate && inq.InquiryDate<=model.ToDate
                 orderby inq.InquiryDate
            select new
            {
                inq.InquiryId,
                inq.InquiryDate,
                ins.InstituteName,
                //ins.DecisionMaker,
                DecisionMaker= inq.InquiryContactPerson,
                inq.Mobile,
                inq.Email,
                inq.Telephone,
                inq.Remarks,
                inq.FurtherContactType,
                inq.InstituteId,

            };

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Marketing/Reports"), "MarketingVisitReport.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("MarketingRepoet");

            ReportParameter From = new ReportParameter();

            ReportParameter To = new ReportParameter();

            From = new ReportParameter("FromDate", model.FromDate.ToString());

            To = new ReportParameter("ToDate", model.ToDate.ToString());


            ReportDataSource rd = new ReportDataSource("ImsDbSet", inquiryList);
            lr.SetParameters(new ReportParameter[] { From, To });
            //lr.SetParameters(To);
            lr.DataSources.Add(rd);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>14.0in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0..1in</MarginTop>" +
                "  <MarginLeft>.1in</MarginLeft>" +
                "  <MarginRight>.1in</MarginRight>" +
                "  <MarginBottom>0.1in</MarginBottom>" +
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