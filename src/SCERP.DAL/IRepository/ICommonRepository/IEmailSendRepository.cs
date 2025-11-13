using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common.Mail;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.IRepository.ICommonRepository
{
    public interface IEmailSendRepository : IRepository<MailInformation>
    {
        MailInformation GetMailInfo();
        MailSend GetMailSendInfo(int reportId);
        int SendDbEmail(DbEmailModel dbEmail);
    }
}
