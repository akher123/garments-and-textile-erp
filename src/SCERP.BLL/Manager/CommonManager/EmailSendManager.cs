using System;
using System.Collections.Generic;
using System.Net.Mail;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common.Mail;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model;
using System.Net;
using SCERP.Model.CommonModel;
using System.IO;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository;

namespace SCERP.BLL.Manager.CommonManager
{
    public class EmailSendManager : IEmailSendManager
    {
        private IEmailSendRepository _emailSendRepository = null;
        private IRepository<EmailTemplateUser> _emailTemplateUseRepository;

        public EmailSendManager(IEmailSendRepository emailSendRepository, IRepository<EmailTemplateUser> emailTemplateUseRepository)
        {
            _emailTemplateUseRepository = emailTemplateUseRepository;
            _emailSendRepository = emailSendRepository;
        }

        public bool SendEmail(string to, string subject, string body, string cc, string bcc)
        {
            bool retVal = false;

            MailMessage message = new MailMessage();
            MailInformation mailInfo = _emailSendRepository.GetMailInfo();

            string sendingMail = mailInfo.SendingMailAddress;
            string password = mailInfo.Password;
            string smtpClient = mailInfo.SmtpClient;
            int port = Convert.ToInt32(mailInfo.Port ?? "25");
            string name = "SCERP Authority";

            if (to.Contains(";"))
            {
                string[] ToArry = to.Split(';');
                foreach (string s in ToArry)
                {
                    message.To.Add(s);
                }
            }
            else
                message.To.Add(to);

            if (!string.IsNullOrEmpty(cc))
            {
                if (cc.Contains(";"))
                {
                    string[] ToArry = cc.Split(';');
                    foreach (string s in ToArry)
                    {
                        message.CC.Add(s);
                    }
                }
                else
                    message.CC.Add(cc);
            }

            if (!string.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);

            if (string.IsNullOrEmpty(sendingMail))
                sendingMail = "info@soft-code.net";

            message.Subject = subject;
            message.From = new MailAddress(sendingMail, name);
            message.Body = body;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(smtpClient, port);

            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(sendingMail, password);
            smtp.Send(message);

            retVal = true;

            return retVal;
        }

        public bool SendEmail(int reportId, byte[] renderedBytes)
        {
            bool retVal = false;

            MailInformation mailInfo = _emailSendRepository.GetMailInfo();
            MailSend mailsendInfo = _emailSendRepository.GetMailSendInfo(reportId);

            using (FileStream fs = new FileStream(mailsendInfo.FileName, FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }
           
            EmailSender emailSender = new EmailSender(mailInfo.SmtpClient, Convert.ToInt32(mailInfo.Port), mailInfo.SendingMailAddress, mailInfo.Password);

            emailSender.From = mailInfo.SendingMailAddress;
            emailSender.To = mailsendInfo.MailAddress;

            emailSender.Subject = mailsendInfo.MailSubject;
            emailSender.Body = mailsendInfo.MailBody;

            emailSender.AddAttachment(mailsendInfo.FileName);
            emailSender.Send();

            retVal = true;
            return retVal;
        }

        public bool SendDbEmail(DbEmailModel dbEmail)
        {
           
           return _emailSendRepository.SendDbEmail(dbEmail)>0;
        }

        public DbEmailModel GetDbEmailByTemplateId(string tempId,string compId)
        {
            List<EmailTemplateUser> templateUsers = _emailTemplateUseRepository.GetWithInclude(x => x.CompId == compId&&x.EmailTemplate.EmailTemplateRefId==tempId, "EmailUser", "EmailTemplate").ToList();
            DbEmailModel emailModel=new DbEmailModel();
            if (templateUsers.Any())
            {
                emailModel.Recipients = String.Join(";", templateUsers.Select(x => x.EmailUser.EmailAddress));
                emailModel.CopyRecipients = String.Join(";", templateUsers.Select(x => x.SendingType));
              
            }
            return emailModel;
        }
    }
}
