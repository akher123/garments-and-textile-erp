using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager.CommonManager;
using System.Linq;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;
using System.Net.Mail;
using SCERP.DAL.Repository;
using SCERP.Model.CommonModel;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserManager : BaseManager, IUserManager
    {
        public IUserRepository _userRepository { get; set; }
        public EmailSendManager _emailSendManager = null;
        
        public UserManager(SCERPDBContext context)
        {
            _userRepository = new UserRepository(context);
            _emailSendManager=new EmailSendManager(new EmailSendRepository(context),new Repository<EmailTemplateUser>(context));
           
        }
        public bool UpdateUserLogTime(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID)&& PortalContext.CurrentUser==null)
            {
                return true;
            }
            else
            {
                Guid? userLogTimeId = PortalContext.CurrentUser.SessionId;
                return _userRepository.UpdateUserLogTime(userLogTimeId);
            }
         
        }
        public bool SaveUserLogTime(UserLogTime userLogTime)
        {
            userLogTime.UserLogTimeId = PortalContext.CurrentUser.SessionId;
            userLogTime.SessionId = PortalContext.CurrentUser.SessionId.ToString();
            userLogTime.UserId = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            return _userRepository.SaveUserLogTime(userLogTime);
        }
        public List<User> GetUsersByPaging(int pageIndex, string sort, string sortdir,string searchString, out int totalRecords)
        {
            List<User> users = null;
            try
            {
                users = _userRepository.GetUsersByPaging(pageIndex, sort, sortdir, searchString, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return users;
        }


        public User GetUser(User user)
        {
            User anUser = null;
            try
            {
                anUser = _userRepository.GetUser(user);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                anUser = null;
            }
            return anUser;
        }

        public int GetLoginStatus(User user)
        {
            return _userRepository.GetLoginStatus(user);
        }
        public bool IsUserExist(User user)
        {
            return _userRepository.Exists(x => x.IsActive == true && x.UserName == user.UserName && x.Id !=user.Id);
        }

        public void ExecuteEmail(string pathName)
        {
            _userRepository.ExecuteEmail(pathName);
        }

        public bool IsUserIdExist(string userId)
        {
            return _userRepository.Exists(x => x.IsActive == true && x.UserName == userId);
        }
        public bool IsEmailExist(string email)
        {
            return _userRepository.Exists(x => x.IsActive == true && x.EmailAddress == email);
        }

        public string GetEmailByUserId(string userId)
        {
            return
                _userRepository.FindOne(x => x.IsActive == true && x.UserName == userId).EmailAddress;
        }

        public string GetUserIdByEmail(string email)
        {
            return
                _userRepository.FindOne(x => x.IsActive == true && x.EmailAddress == email).UserName;
        }


        public User GetUserById(int id)
        {
            User user = null;
            try
            {
                user = _userRepository.GetUserById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                user = null;
            }

            return user;
        }

        public User GetUserByEmployeeId(Guid employeeId)
        {
            User user = null;
            try
            {
                user = _userRepository.GetUserByEmployeeId(employeeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                user = null;
            }

            return user;
        }
        public List<User> GetAllUsers()
        {
            List<User> users = null;
            try
            {
                users = _userRepository.GetAllUsers();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                users = null;
            }
            return users;
        }

        public int EditUser(User model)
        {
            var editedUser = 0;
            try
            {
                var user = _userRepository.FindOne(x => x.IsActive == true && x.Id == model.Id);
                user.EDT = DateTime.Now;
                user.EmailAddress = model.EmailAddress;
                user.TnaResponsible = model.TnaResponsible;
                user.EditedBy = PortalContext.CurrentUser.UserId;
                editedUser = _userRepository.Edit(user);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedUser = 0;
            }

            return editedUser;
        }

        public int SaveUser(User user)
        {

            user.CDT = DateTime.Now;
            user.CreatedBy = PortalContext.CurrentUser.UserId;
            user.IsActive = true;
            user.Contact.ContactId = Guid.NewGuid();
            user.CompId = user.Contact.CompId;
            return _userRepository.SaveUser(user);
        }

        public int DeleteUser(User user)
        {
            var deletedUser = 0;
            try
            {
                user.EDT = DateTime.Now;
                user.EditedBy = PortalContext.CurrentUser.UserId;
                user.IsActive = false;
                deletedUser = _userRepository.Edit(user);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedUser = 0;
            }

            return deletedUser;
        }

        public bool SendForgotPassword(string userId, string email)
        {
            var user =_userRepository.FindOne(x => x.UserName == userId && x.EmailAddress == email && x.IsActive == true);
            if (user == null)
            {
                return false;
            }

            Random random = new Random();
            int randomNumber = random.Next(10000);
            string newpassword = user.UserName + randomNumber;
            _userRepository.ChangePassword(newpassword, user.Id);
            const string subject = "GTex ERP Password Recovery";
            string body = string.Format("Dear User, your user name for GTex ERP is: {0}, and  password is: {1}", user.UserName, newpassword);
            return _emailSendManager.SendEmail(user.EmailAddress, subject, body, null, null);
        }

        public int ChangeCurrentPassword(string currentPassword, User user)
        {
            return _userRepository.ChangeCurrentPassword(currentPassword, user);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public List<User> GetActiveUsers(string empName)
        {
            Guid? userId = PortalContext.CurrentUser.UserId;
            List<User> users = _userRepository
                .GetWithInclude(x => x.IsActive == true &&x.EmployeeId!= userId
               , "Employee").Where(x=>x.Employee.Name.Replace(" ", "")
                                  .ToLower()
                                  .Contains(empName.Replace(" ", "").ToLower())).ToList();
            return users;
        }

        public object GetUsersByCompany(string compId,bool isSystemUser,string userName)
        {
            if (!isSystemUser)
            {
                var users = _userRepository.Filter(x => x.CompId == compId && x.IsActive == true&&x.UserName!= userName).OrderBy(x => x.UserName).ToList();
                return users.Select(x => new { Text = x.UserName, Value = x.UserName });
            }
            else
            {
                var users = _userRepository.Filter(x => x.CompId == compId && x.IsActive == true).OrderBy(x => x.UserName).ToList();
                return users.Select(x => new { Text = x.UserName, Value = x.UserName });
            }
          
        }

        public object GetData(string key)
        {
            if (key=="A")
            {
                var data = new
                {
                    Value1 = AppConfig.SSRS_USER,
                    Value2 = AppConfig.SSRS_CRED,
                    Value3 = AppConfig.SSRS_CON,
                };
                return data;
            }
            else
            {
                return "Hello User";
            }
          

        }

    
    }
}
