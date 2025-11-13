using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface IEmailTemplateUserManager
    {
        List<EmailTemplateUser> GetEmailTemplateUserUsers(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
        EmailTemplateUser GetEmailTemplateUseById(int emailTamplateUserId);
        int EditEmailTemplateUser(EmailTemplateUser emailTemplateUser);
        int SaveEmailTamplateUser(EmailTemplateUser emailTemplateUser);
       List<string> GetEmailTemplateUsersPhoneNumbers(string templateRefId, string compId);

    }
}
