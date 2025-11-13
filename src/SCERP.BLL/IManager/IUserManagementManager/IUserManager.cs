using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IUserManager
    {
        User GetUser(User user);

        User GetUserById(int id);

        List<User> GetAllUsers();

        int EditUser(User model); 

        int SaveUser(User user);

        int DeleteUser(User user);

        List<User> GetUsersByPaging(int startPage, string sort, string sortdir,string searchString, out int totalRecords);

        User GetUserByEmployeeId(Guid employeeId);

        bool SendForgotPassword(string userId, string email);
        int ChangeCurrentPassword(string currentPassword, User model);
        int GetLoginStatus(User user);
        bool IsEmailExist(string email);
        string GetEmailByUserId(string userId);
        string GetUserIdByEmail(string email);
        bool IsUserIdExist(string userId);
        bool IsUserExist(User user);
        void ExecuteEmail(string filePath);
        List<User> GetActiveUsers(string empName);
        object GetUsersByCompany(string compId, bool isSystemUser, string userName);
        object GetData(string key);
        bool SaveUserLogTime(UserLogTime userLogTime);
        bool UpdateUserLogTime(string sessionID);
    }
}
