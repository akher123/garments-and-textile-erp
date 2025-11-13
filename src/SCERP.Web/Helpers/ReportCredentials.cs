using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;

namespace SCERP.Web.Helpers
{
    public class ReportCredentials : IReportServerCredentials
    {
        public ReportCredentials(string userName, string password, string domain)
        {
            UserName = userName;
            Password = password;
            Domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(UserName, Password, Domain);
           
            }
        }

        private string UserName { get; set; }
        private string Password { get; set; }
        private string Domain { get; set; }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = password = authority = null;
            return false;
        }
    }
}
