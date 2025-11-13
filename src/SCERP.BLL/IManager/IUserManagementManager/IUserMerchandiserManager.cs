using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IUserMerchandiserManager
    {
        List<UserMerchandiser> GetPermitedUserMerchandiser(Guid employeeId);
        int SaveUserUserMerchandiser(List<string> merchandiserIdList, Guid employeeId);
        bool IsUserMerchandiser(Guid? userId);
    }
}
