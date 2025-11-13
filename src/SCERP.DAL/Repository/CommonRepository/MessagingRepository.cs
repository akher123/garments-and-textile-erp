using SCERP.Common;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommonRepository
{
   public class MessagingRepository:Repository<Messaging>, IMessagingRepository
    {
        public MessagingRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<MessagingNotification> GetChatMessage(Guid? userId)
        {
           return Context.Database.SqlQuery<MessagingNotification>(string.Format("EXEC spGetChatMessage @ReceiverId='{0}'", userId)).ToList();
        }

        public List<Dropdown> GetLoginusers(string compId)
        {
            return Context.Database.SqlQuery<Dropdown>(string.Format("EXEC [dbo].[spGetLoginusers] @CompId='{0}'", compId)).ToList();
        }
    }
}
