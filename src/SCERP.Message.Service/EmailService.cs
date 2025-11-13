using SCERP.Message.Service.Model;
using System;
using System.ServiceModel;

namespace SCERP.Message.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class EmailService : IEmailService
    {
        public string SendMail(EmailMessage emailMessage)
        {
            try
            {
                EmailManager.AddEmail(emailMessage);

                return "Email send successfully";
            }
            catch(Exception ex)
            {
                return "Error on sending message";
            }

        }
    }
}
