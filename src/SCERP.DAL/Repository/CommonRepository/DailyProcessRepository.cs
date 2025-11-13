using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class DailyProcessRepository : Repository<object>, IDailyProcessRepository
    {
        public DailyProcessRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public int SendSmsTnaNotificationAlert()
        {
            string procedureName = "[dbo].[spSmsSendTnaAlert]";
            return Context.Database.ExecuteSqlCommand(procedureName);
        }
    }
}
