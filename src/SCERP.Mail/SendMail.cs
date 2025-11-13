using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCERP.Common.Mail;

namespace SCERP.Mail
{
    public class SendMail
    {
        public static int Send()
        {
            EmailSender emailSender = new EmailSender("smtpout.secureserver.net", 80, "info@soft-code.net", "info123");

            emailSender.From = "info@soft-code.net";
            emailSender.To = "kallol39@gmail.com";
            emailSender.Subject = "Test Mail - 1";
            emailSender.Body = "mail with attachment";
            //emailSender.AddAttachment("D:\\QA_TempFile\\Jobcard.pdf");
            emailSender.Send();
            return 1;
        }
    }
}
