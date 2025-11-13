using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IMessagingManager
    {
        int SaveChatMessage(Messaging messaging);
        List<MessagingNotification> GetChatMessage(Guid? userId);
        List<Dropdown> GetLoginusers(string compId);
    }
}
