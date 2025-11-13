using System;
using System.Linq;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model;
using SCERP.Common.Mail;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class EmailSendRepository : Repository<MailInformation>, IEmailSendRepository
    {
        public EmailSendRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public MailInformation GetMailInfo()
        {
            MailInformation mailInfo = Context.MailInformations.FirstOrDefault(p => p.IsActive == true);
            return mailInfo;
        }

        public MailSend GetMailSendInfo(int reportId)
        {
            MailSend mailSendinfo = Context.MailSends.FirstOrDefault(p => p.MailSendId == reportId);
            return mailSendinfo;
        }

        public int SendDbEmail(DbEmailModel dbEmail)
        {
            //var softcodedbmail = new SqlParameter { ParameterName = "@profile_name", Value = "softcodedbmail" };
            //var recipients = new SqlParameter { ParameterName = "@recipients", Value = dbEmail.Recipients };
            //var copyRecipients = new SqlParameter { ParameterName = "@copy_recipients", Value = dbEmail.CopyRecipients };

            //var subject = new SqlParameter { ParameterName = "@subject", Value = dbEmail.Subject };
            //var body = new SqlParameter { ParameterName = "@body", Value = dbEmail.Body };
            //var fileattachments = new SqlParameter { ParameterName = "@file_attachments", Value = dbEmail.FileAttachments };
            //object plist = new[] { softcodedbmail, recipients, copyRecipients, subject, body, fileattachments };
            //return Context.Database.ExecuteSqlCommand("msdb.dbo.sp_send_dbmail @profile_name,@recipients,@copy_recipients,@subject,@body,@file_attachments", plist);
            string sqlQuery = String.Format(@"EXEC msdb.dbo.sp_send_dbmail 
                                            @profile_name='softcodedbmail',
                                            @recipients='{0}',
                                            @copy_recipients = '{1}',
                                            @subject='{2}',
                                            @body='{3}',
                                            @file_attachments='{4}'", dbEmail.Recipients, dbEmail.CopyRecipients, dbEmail.Subject, dbEmail.Body, dbEmail.FileAttachments);
            return Context.Database.ExecuteSqlCommand(sqlQuery);
        }
    }
}
