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
    public static class EmailManager
    {
        public static string QueueName = ConfigurationManager.AppSettings["QueueName"];
        public static bool IsProcessing = false;

        public static void ProcessEmail()
        {
            if(!MessageQueue.Exists(QueueName))
            {
                IsProcessing = false;
                return;
            }

            IsProcessing = true;

            
            while (IsProcessing)
            {

                MessageQueue messageQueue = new MessageQueue(QueueName);
                var message = messageQueue.GetMessageEnumerator2();

                if (message.MoveNext())
                {
                    messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(EmailMessage) });
                    EmailMessage emailMessage = (EmailMessage)messageQueue.Receive().Body;
                }
                else
                {
                    IsProcessing = false;
                }
            }
        }

        public static void AddEmail(EmailMessage emailMessage)
        {
            MessageQueue messageQueue;

            if (!MessageQueue.Exists(QueueName))
            {
                messageQueue = MessageQueue.Create(QueueName);
            }
            else
            {
                messageQueue = new MessageQueue(QueueName);
            }
            messageQueue.Send(emailMessage);

            if (!IsProcessing)
            {
                var taskManageProcess = new Task(() => ProcessEmail());

                taskManageProcess.Start();
            }
        }
    }
}
