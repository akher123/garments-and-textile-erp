using Microsoft.Owin.Security.OAuth;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager;
using SCERP.Model;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SCERP.Web.Provider
{
    public class GTexAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public Manager Manager { get { return Manager.Instance; } }

        public IUserManager UserManager
        {
            get { return Manager.UserManager; }
        }
        public IEmployeeCompanyInfoManager EmployeeCompanyInfoManager
        {
            get { return Manager.EmployeeCompanyInfoManager; }
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = new User { UserName = context.UserName, Password = context.Password };

             int userLoginStatus = UserManager.GetLoginStatus(user);
            if (userLoginStatus > 0)
            {
                var loginuser = UserManager.GetUser(user);
                if (loginuser == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                Company company = EmployeeCompanyInfoManager.GetCompanyRefIdByEmployeeId(loginuser.EmployeeId);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
  
                identity.AddClaim(new Claim(ClaimTypes.Name, loginuser.UserName));
                identity.AddClaim(new Claim("preferred_username", loginuser.UserName));
                identity.AddClaim(new Claim("user_id", loginuser.EmployeeId.ToString()));
                identity.AddClaim(new Claim("user_compId", company.CompanyRefId));

                context.Validated(identity);
            }
        }
    }
}