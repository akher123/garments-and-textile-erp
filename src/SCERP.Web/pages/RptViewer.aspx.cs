using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using SCERP.Common;
using SCERP.Web.Helpers;

namespace SCERP.Web.pages
{
    public partial class RptViewer : System.Web.UI.Page
    {
     

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var reportParams = new List<ReportParameter>();
                    string reportId = Request.QueryString["id"].ToString();
                    Dictionary<string, string> myValues = new Dictionary<string, string>();

             
                    string reportServerUrl = ConfigurationManager.AppSettings["ReportServerURL"];
                    string domain = ConfigurationManager.AppSettings["rsDomain"];
                    string userName = ConfigurationManager.AppSettings["rsUserName"];
                    string password = ConfigurationManager.AppSettings["rsPassword"];
                    string reportPath = ConfigurationManager.AppSettings["ReportPath"];

                    reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
                    reportViewer.ServerReport.ReportServerCredentials = new ReportCredentials(userName, password, domain);
                    reportViewer.ServerReport.ReportPath = string.Format(reportPath, reportId);
                    ReportParameterInfoCollection parameters = reportViewer.ServerReport.GetParameters();
                    if (parameters["CompId"] != null)
                    {
                        reportParams.Add(new ReportParameter("CompId", PortalContext.CurrentUser.CompId));
                    }
                    if (parameters["HostingServerAddress"] != null)
                    {
                        reportParams.Add(new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress));
                    }
                    if (parameters.Count > 0)
                    {
                        reportViewer.ServerReport.SetParameters(reportParams);
                    }
                   
                    reportViewer.ProcessingMode = ProcessingMode.Remote;
                    // reportViewer.ProcessingMode =ProcessingMode.Local;
                    reportViewer.ShowCredentialPrompts = false;

                    reportViewer.ServerReport.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}