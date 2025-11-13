using SCERP.Common;
using SCERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.ICommonRepository
{
    public interface IMessagingRepository : IRepository<Messaging>
    {
        List<MessagingNotification> GetChatMessage(Guid? userId);
        List<Dropdown> GetLoginusers(string compId);
    }
}
