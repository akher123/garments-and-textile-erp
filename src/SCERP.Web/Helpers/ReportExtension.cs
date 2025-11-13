using Microsoft.Reporting.WebForms;
using SCERP.BLL.Manager.CommonManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.DAL;
using SCERP.Model.CommonModel;
using SCERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Spell;
using SCERP.Common;

namespace SCERP.Web.Helpers
{
    public static class ReportExtension
    {
        public static FileContentResult ToFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, DeviceInformation deviceInfos)
        {
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = deviceInfos ?? new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return new FileContentResult(renderedBytes, mimeType);
        }

        public static FileContentResult ToFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, DeviceInformation deviceInfos, List<ReportParameter> parameters)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            localReport.SetParameters(parameters);
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = deviceInfos ?? new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return new FileContentResult(renderedBytes, mimeType);
        }

        public static FileContentResult ToFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));

            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return new FileContentResult(renderedBytes, mimeType);
        }

        public static FileContentResult ToFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, List<ReportParameter> parameters)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            localReport.SetParameters(parameters);
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return new FileContentResult(renderedBytes, mimeType);
        }

        public static FileContentResult ToFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, DeviceInformation deviceInfos,ref byte[] renderedBytesRef)
        {
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = deviceInfos ?? new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            renderedBytesRef = renderedBytes;
           
            return new FileContentResult(renderedBytes, mimeType);
        }

        public static string NumberToWord(this string inWord)
        {
            if (!string.IsNullOrEmpty(inWord))
            {
                string word= inWord.Replace("Taka", "").Replace("Only", "Kg").ToUpper();
                if (word.Contains("PAISA"))
                {
                    word = word.Replace("PAISA", "");
                }
                return word;
            }

            return "";
        }



        public static FileContentResult ToWhiteFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, DeviceInformation deviceInfos)
        {
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = deviceInfos ?? new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            // CODE TO SAVE THE REPORT FILE ON SERVER
            WriteFile(renderedBytes, reportType);
            return new FileContentResult(renderedBytes, mimeType);
        }


        public static FileContentResult ToWhiteFile(ReportType reportType, string path, List<ReportDataSource> reportDataSources, DeviceInformation deviceInfos, List<ReportParameter> parameters)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = path;
            reportDataSources.ForEach(x => localReport.DataSources.Add(x));
            localReport.SetParameters(parameters);
            string mimeType;
            string encoding;
            string fileNameExtension;

            DeviceInformation model = deviceInfos ?? new DeviceInformation();

            StringBuilder deviceInfoSb = new StringBuilder();

            deviceInfoSb.Append("<DeviceInfo>");
            deviceInfoSb.AppendFormat(String.Format("<OutputFormat>{0}</OutputFormat>", model.OutputFormat));
            deviceInfoSb.AppendFormat(String.Format("<PageWidth>{0}in</PageWidth>", model.PageWidth));
            deviceInfoSb.AppendFormat(String.Format("<PageHeight>{0}in</PageHeight>", model.PageHeight));
            deviceInfoSb.AppendFormat(String.Format("<MarginTop>{0}in</MarginTop>", model.MarginTop));
            deviceInfoSb.AppendFormat(String.Format("<MarginLeft>{0}in</MarginLeft>", model.MarginLeft));
            deviceInfoSb.AppendFormat(String.Format("<MarginRight>{0}in</MarginRight>", model.MarginRight));
            deviceInfoSb.AppendFormat(String.Format("<MarginBottom>{0}in</MarginBottom>", model.MarginBottom));
            deviceInfoSb.Append("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType.ToString("g"), deviceInfoSb.ToString(), out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            WriteFile(renderedBytes, reportType);
            return new FileContentResult(renderedBytes, mimeType);
        }

        private static void WriteFile(byte[] renderedBytes, ReportType reportType)
        {
            // CODE TO SAVE THE REPORT FILE ON SERVER
            if (File.Exists(HostingEnvironment.MapPath(AppConfig.ExportReportFillPath+"." + reportType)))
            {
                File.Delete(HostingEnvironment.MapPath(AppConfig.ExportReportFillPath+"." + reportType));
            }

            FileStream fileStream = new FileStream(HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + reportType), FileMode.Create);

            for (int i = 0; i < renderedBytes.Length; i++)
            {
                fileStream.WriteByte(renderedBytes[i]);
            }
            fileStream.Close();
        }



        public static FileContentResult ToSsrsFile(ReportType reportType, string reportName, List<ReportParameter> parameters)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportViewer = new ReportViewer();
            string reportServerUrl = ConfigurationManager.AppSettings["ReportServerURL"];
            string domain = ConfigurationManager.AppSettings["rsDomain"];
            string userName = ConfigurationManager.AppSettings["rsUserName"];
            string password = ConfigurationManager.AppSettings["rsPassword"];
            string reportPath = ConfigurationManager.AppSettings["ReportPath"];

            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);

            reportViewer.ServerReport.ReportServerCredentials = new ReportCredentials(userName, password, domain);
            reportViewer.ServerReport.ReportPath = string.Format(reportPath, reportName);
            reportViewer.ServerReport.SetParameters(parameters);
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            byte[] bytes = reportViewer.ServerReport.Render(reportType.ToString("g"), null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            return new FileContentResult(bytes, mimeType);
        }

        public static FileContentResult ToSsrsFile(ReportType reportType, string reportName)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportViewer = new ReportViewer();
            string reportServerUrl = ConfigurationManager.AppSettings["ReportServerURL"];
            string domain = ConfigurationManager.AppSettings["rsDomain"];
            string userName = ConfigurationManager.AppSettings["rsUserName"];
            string password = ConfigurationManager.AppSettings["rsPassword"];
            string reportPath = ConfigurationManager.AppSettings["ReportPath"];
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportServerCredentials = new ReportCredentials(userName, password, domain);
            reportViewer.ServerReport.ReportPath = string.Format(reportPath, reportName);
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            byte[] bytes = reportViewer.ServerReport.Render(reportType.ToString("g"), null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            return new FileContentResult(bytes, mimeType);
        }
    }
}
