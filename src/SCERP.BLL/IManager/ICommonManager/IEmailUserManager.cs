using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IEmailUserManager  
    {
        List<EmailUser> GetEmailUsers(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
        EmailUser GetEmailUserById(int emailUserEmailUserId);
        string GetNewRefId(string currentUserCompId);
        int EditEmailUser(EmailUser emailUser);
        int SaveEmailUser(EmailUser emailUser);
        List<EmailUser> GetAllEmailUsers(string compId);
    }
}
