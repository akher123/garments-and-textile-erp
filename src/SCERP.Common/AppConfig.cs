using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public static class AppConfig
    {
        public static double MaxUploadFileSizeMB
        {
            get
            {
                var files = ConfigurationManager.AppSettings["MaxUploadFileSizeMB"];

                if (String.IsNullOrEmpty(files))
                {
                    files = @"30";
                }

                return Convert.ToDouble(files);
            }
        }

        public static int  PageSize
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PageSize"])) return 0;
                var pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["PageSize"]);
                return pageSize >= 0 ? pageSize : 0;
            }
        }
        public static string ReportServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportServerAddress"];
            }
        }

        public static string ExportReportFillPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ExportReportFillPath"];
            }
        }

        ///SMS Gateway
        ///

        public static string SmsGatewayUserId
        {
            get
            {
                return ConfigurationManager.AppSettings["SmsGatewayUserId"];
            }
        }

        public static string SmsGatewayPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmsGatewayPassword"];
            }
        }

        public static string SmsSenderName
        {
            get
            {
                return ConfigurationManager.AppSettings["SmsSenderName"];
            }
        }

        public static string SmsGatewayUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SmsGatewayUrl"];
            }
        }
        public static string HostingServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["AppHostingAddress"];
            }
        }
        public static string SSRS_USER
        {
            get
            {
                return ConfigurationManager.AppSettings["rsUserName"];
            }
        }
        public static string SSRS_CRED
        {
            get
            {
                return ConfigurationManager.AppSettings["rsPassword"];
            }
        }
        public static string SSRS_CON
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SCERPDBContext"].ConnectionString;
            }
        }
        public static bool IsEnableUserLogTime
        {
            get
            {
                return Convert.ToBoolean( ConfigurationManager.AppSettings["IsEnableUserLogTime"]);
            }
        }
        public static bool IsSetCustomProductionDate
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsSetCustomProductionDate"]);
            }
        }
        public static DateTime ProductionDate
        {
            get
            {
                return DateTime.Parse(ConfigurationManager.AppSettings["ProductionDate"]);
            }
        }
    }
}
