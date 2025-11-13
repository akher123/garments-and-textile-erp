<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SCERP.Web.Helpers.ReportDataSourceModel>" %>

<%@ Import Namespace="System.IO" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>
<html>
<head runat="server" >
    <meta name="viewport" content="width=device-width" />
    <title></title>    
    <script runat="server">
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string path =Server.MapPath(Model.Path);
                var rdc = new ReportDataSource(Model.DataSetName, Model.DataSource);
                ReportViewer1.LocalReport.ReportPath = path;
                ReportViewer1.Width = Unit.Percentage(1000);
                ReportViewer1.Height = Unit.Percentage(900); 
                if (Model.ReportParameters != null && Model.ReportParameters.Any())
                {
                    ReportViewer1.LocalReport.SetParameters(Model.ReportParameters);
                }
              
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdc);
                ReportViewer1.LocalReport.Refresh();
            }
        }
    </script>
    <style type="text/css">
        #form1 {
            width:100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div style="clear: both">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true" Width="549px" Height="404px">
        </rsweb:ReportViewer>        
    </div>
    </form>

</body>

</html>

