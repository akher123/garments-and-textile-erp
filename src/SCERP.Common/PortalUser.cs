using System;
using Org.BouncyCastle.Asn1.Crmf;
using SCERP.Common.PermissionModel;
namespace SCERP.Common
{
    public class PortalUser
    {
      
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool Validated
        {
            get;
            set;
        }

        public Nullable<System.Guid>  UserId
        {
            get;
            set;
        }

        public bool IsUserInRole(string role)
        {
            return System.Web.Security.Roles.Provider.IsUserInRole(Name, role);
        }

        public PermissionContext PermissionContext
        {
            get;
            set;
        }
   

        public string CompId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string CompanyLogo { get; set; }
        public string DomainName { get; set; }
        public string TnaResponsible { get; set; }
        public Guid SessionId { get;  set; }
        public bool IsSystemUser { get; set; }
    }
}
