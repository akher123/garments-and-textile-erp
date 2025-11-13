using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.ICommonRepository
{
    public interface IDailyProcessRepository:IRepository<object>
    {
        int SendSmsTnaNotificationAlert();
    }
}
