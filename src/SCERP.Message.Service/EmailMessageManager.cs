using SCERP.Message.Service.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Message.Service
{
    public class EmailMessageManager
    {
        MessageQueue messageQueue = null;
       
        public void AddEmail(EmailMessage emailMessage)
        {
            if (!MessageQueue.Exists(EmailManager.QueueName))
            {
                messageQueue = MessageQueue.Create(EmailManager.QueueName);
            }
            else
            {
                messageQueue = new MessageQueue(EmailManager.QueueName);
            }
            messageQueue.Send(emailMessage);
        }
    }
}
