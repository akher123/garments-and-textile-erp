using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.DAL.IRepository;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserTnaResponsibleManager : IUserTnaResponsibleManager
    {
        private readonly IRepository<UserTnaResponsible> _userTnaResponsibleRepository;
        public UserTnaResponsibleManager(IRepository<UserTnaResponsible> userTnaResponsibleRepository)
        {
            _userTnaResponsibleRepository = userTnaResponsibleRepository;
        }
    }
}
