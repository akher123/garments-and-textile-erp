using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.CommonManager
{
    public class MessagingManager: IMessagingManager
    {
        private readonly IMessagingRepository repository;

        public MessagingManager(IMessagingRepository repository)
        {
            this.repository = repository;
        }

        public List<MessagingNotification> GetChatMessage(Guid? userId)
        {
            List<MessagingNotification> messagings = repository.GetChatMessage(userId);
            return messagings;
        }

        public List<Dropdown> GetLoginusers(string compId)
        {
            return repository.GetLoginusers( compId);
        }

        public int SaveChatMessage(Messaging messaging)
        {
           return repository.Save(messaging);
        }
    }
}
