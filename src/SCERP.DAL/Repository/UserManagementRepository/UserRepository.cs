using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using SCERP.Common;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public User GetUser(User user)
        {
            return Context.Users.FirstOrDefault(x => x.UserName == user.UserName && x.IsActive == true);
        }

        public int SaveUser(User user)
        {
            //var userNameParam = new SqlParameter { ParameterName = "UserName", Value = user.UserName };
            //var emailAddressParam = new SqlParameter { ParameterName = "EmailAddress", Value = user.EmailAddress };
            //var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = user.EmployeeId };
            //var passwordParam = new SqlParameter { ParameterName = "Password", Value = user.Password };
            //var cdtParam = new SqlParameter { ParameterName = "CDT", Value = user.CDT };
            //var createdByParam = new SqlParameter { ParameterName = "CreatedBy", Value = user.CreatedBy };
            //var isActiveParam = new SqlParameter { ParameterName = "IsActive", Value = user.IsActive };

            //return (Context.Database.SqlQuery<int>("SPInsertUser  @UserName, @EmailAddress, @EmployeeId, @Password, @CDT, @CreatedBy, @IsActive",
            //    userNameParam, emailAddressParam, employeeIdParam, passwordParam, cdtParam, createdByParam, isActiveParam)).ToList()[0];
            string salt = Guid.NewGuid().ToString();
            var has = Context.Database.SqlQuery<byte[]>("select HASHBYTES('SHA2_512', '" + user.Password + salt + "')").First();
            user.sort = salt;
            user.PasswordHash = has;
            user.ContactId = user.Contact.ContactId;
            Context.Users.Add(user);
            return Context.SaveChanges();
        }

        public int ChangeCurrentPassword(string currentPassword, User user)
        {
            user.Id = PortalContext.CurrentUser.Id;

            var userNameParam = new SqlParameter { ParameterName = "LoginName", Value = user.UserName };
            var passwordParam = new SqlParameter { ParameterName = "Password", Value = currentPassword };

            int index = (Context.Database.SqlQuery<int>("SPGetUserLoginStatus @LoginName, @Password", userNameParam, passwordParam)).ToList()[0];
            if (index > 0)
            {
                var userIdParam = new SqlParameter { ParameterName = "Id", Value = user.Id };
                var newPasswordParam = new SqlParameter { ParameterName = "Password", Value = user.Password };
                return (Context.Database.SqlQuery<int>("SPPasswordChange @Id, @Password", userIdParam, newPasswordParam)).ToList()[0];
            }
            return 0;
        }

        public int ChangePassword(string newpassword, int id)
        {
            var userIdParam = new SqlParameter { ParameterName = "Id", Value = id };
            var passwordParam = new SqlParameter { ParameterName = "Password", Value = newpassword };
            return (Context.Database.SqlQuery<int>("SPPasswordChange @Id, @Password", userIdParam, passwordParam)).ToList()[0];
        }

        public void ExecuteEmail(string filePath)
        {
            string sqlQuery = @"EXEC msdb.dbo.sp_send_dbmail @profile_name = 'softcodedbmail', @recipients = 'akher.ice07@gmail.com', @body = 'User has successfully loged in', @subject = '" + PortalContext.CurrentUser.Name + " has loged In', @body_format='HTML'; ";
            sqlQuery = @"EXEC msdb.dbo.sp_send_dbmail
  @profile_name='softcodedbmail',
  @recipients='akher.ice07@gmail.com',
  @subject='Test Attachment with Two Files',
  @body='One files have been included in this email.',
  @file_attachments='" + filePath + "'";
            Context.Database.ExecuteSqlCommand(sqlQuery);
        }

        public int GetLoginStatus(User user)
        {
            string userName = user.UserName;
            if (String.IsNullOrEmpty(userName))
                userName = string.Empty;
            var userNameParam = new SqlParameter { ParameterName = "LoginName", Value = userName };
            string password = user.Password;
            if (String.IsNullOrEmpty(password))
                password = string.Empty;
            var passwordParam = new SqlParameter { ParameterName = "Password", Value = password };
            return (Context.Database.SqlQuery<int>("SPGetUserLoginStatus @LoginName, @Password", userNameParam, passwordParam)).ToList()[0];

        }

        public User GetUserById(int id)
        {
            return Context.Users.Where(x => x.Id == id && x.IsActive == true).Include(y => y.Employee).FirstOrDefault();
        }

        public User GetUserByEmployeeId(Guid employeeGuid)
        {
            return Context.Users.FirstOrDefault(x => x.EmployeeId == employeeGuid && x.IsActive == true);
        }

        public List<User> GetAllUsers()
        {
            return Context.Users.Where(x => x.IsActive == true).Include(y => y.Employee).OrderBy(z => z.UserName).ToList();
        }

        public List<User> GetUsersByPaging(int pageIndex, string sort, string sortdir,string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<User> users = Context.Users.Where(x => x.IsActive == true && (
                (x.UserName.Contains(searchString) || String.IsNullOrEmpty(searchString))||(x.Employee.EmployeeCardId.Contains(searchString)||String.IsNullOrEmpty(searchString)))).Include(y => y.Employee);
            totalRecords = users.Count();
            switch (sort)
            {
                case "UserName":
                    switch (sortdir)
                    {
                        case "DESC":
                            users = users
                                .OrderByDescending(r => r.UserName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            users = users
                                .OrderBy(r => r.UserName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "EmployeeName":
                    switch (sortdir)
                    {
                        case "DESC":
                            users = users
                                .OrderByDescending(r => r.Employee.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            users = users
                                .OrderBy(r => r.Employee.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "EmployeeCardId":
                    switch (sortdir)
                    {
                        case "DESC":
                            users = users
                                .OrderByDescending(r => r.Employee.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            users = users
                                .OrderBy(r => r.Employee.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    users = users
                        .OrderByDescending(r => r.UserName)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return users.ToList();

       
        }


        public User GetUserByUserName(string userName)
        {
            return Context.Users.FirstOrDefault(x => x.UserName == userName && x.IsActive == true);
        }

        public bool CheckValidUserEmailAddress(string email, out string password)
        {
            var temp = (from p in Context.EmployeePresentAddresses
                        join q in Context.Users on p.EmployeeId equals q.EmployeeId
                        where p.EmailAddress == email
                        select q).SingleOrDefault();
            if (temp != null)
            {
                password = temp.Password;
                return true;
            }
            else
            {
                password = "";
                return false;
            }
        }

        public bool GetValidEmailByUserName(string userName, out string email, out string password)
        {
            email = "";
            password = "";

            var user = Context.Users.FirstOrDefault(p => p.IsActive == true && p.UserName == userName);

            if (user == null)
            {
                return false;
            }

            var temp = (from p in Context.Users
                        join q in Context.EmployeePresentAddresses on p.EmployeeId equals q.EmployeeId
                        where p.UserName == userName
                        select new
                        {
                            //p.Password,
                            //p.UserName,
                            q.EmailAddress
                        }
                        ).SingleOrDefault();

            if (temp != null)
            {
                email = temp.EmailAddress;
                //password = temp.Password;
                return true;
            }
            return false;
        }

        public bool SaveUserLogTime(UserLogTime userLogTime)
        {
            Context.UserLogTimes.Add(userLogTime);
            return Context.SaveChanges()>0;
        }

        public bool UpdateUserLogTime(Guid? userLoginId)
        {
            UserLogTime userLogTime=Context.UserLogTimes.FirstOrDefault(x=>x.UserLogTimeId== userLoginId);
            if (userLogTime!=null)
            {
                userLogTime.LogoutTime = DateTime.Now;
                userLogTime.Offline = false;
                Context.Entry(userLogTime).State = EntityState.Modified;
                return Context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
          
        }
    }
}
