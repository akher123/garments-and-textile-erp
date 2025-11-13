using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface IMailSendManager
    {
       List<VwMailSend> GetAllMailSendByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
       MailSend GetMailSendByMailSendId(int mailSendId, string compId);
       bool IsMailSendExist(MailSend model);
       int EditMailSend(MailSend model);
       int SaveMailSend(MailSend model); 
       int DeleteMailSend(int mailSendId);  
    }
}
