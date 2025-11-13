using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMisManager;
using SCERP.DAL.IRepository.IMisRepository;
using SCERP.Model.MisModel;

namespace SCERP.BLL.Manager.MisManager
{
    public class UserActivityManager : IUserActivityManager
    {
        private readonly IUserActivityRepository _userActivityRepository;
        public UserActivityManager(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public List<MIS_UserActivity> GetUserActivityList()
        {
            return _userActivityRepository.GetUserActivityList();
        }
    }
}
