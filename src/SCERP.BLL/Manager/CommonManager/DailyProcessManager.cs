using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.DAL.IRepository.ICommonRepository;

namespace SCERP.BLL.Manager.CommonManager
{
    public class DailyProcessManager : IDailyProcessManager
    {
        private readonly IDailyProcessRepository _dailyProcessRepository;

        public DailyProcessManager(IDailyProcessRepository dailyProcessRepository)
        {
            _dailyProcessRepository = dailyProcessRepository;
        }

        public int SendSmsTnaNotificationAlert()
        {
           return _dailyProcessRepository.SendSmsTnaNotificationAlert();
        }
    }
}
