using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common.Mail;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IEmailSendManager
    {
        bool SendEmail(string to, string subject, string body, string cc, string bcc);

        bool SendEmail(int reportId, byte[] renderedBytes);
        bool SendDbEmail(DbEmailModel dbEmail);
        DbEmailModel GetDbEmailByTemplateId(string tempId,string compId);
    }
}
