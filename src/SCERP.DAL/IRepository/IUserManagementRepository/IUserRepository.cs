using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.DAL.IRepository.IUserManagementRepository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(User user);
        User GetUserById(int id);
        User GetUserByUserName(string userName);
        List<User> GetAllUsers();
        List<User> GetUsersByPaging(int pageIndex, string sort, string sortdir ,string searchString,out int totalRecords);
        User GetUserByEmployeeId(Guid employeeGuid);
        bool CheckValidUserEmailAddress(string email, out string password);
        bool GetValidEmailByUserName(string userName, out string email, out string password);

        int GetLoginStatus(User user);

        int SaveUser(User user);
        int ChangeCurrentPassword(string currentPassword, User user);

        int ChangePassword(string newPassword, int id);

        void ExecuteEmail(string filePath);
        bool SaveUserLogTime(UserLogTime userLogTime);
        bool UpdateUserLogTime(Guid? userId);
    }
}