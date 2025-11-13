using SCERP.Message.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Message.Service
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract]
        string SendMail(EmailMessage emailMessage);
    }
}
