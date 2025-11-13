using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserPermissionForEmployeeLevelManager : BaseManager, IUserPermissionForEmployeeLevelManager
    {
        private IUserPermissionForEmployeeLevelRepository UserPermissionForEmployeeLevelRepository { get; set; }

        public UserPermissionForEmployeeLevelManager(SCERPDBContext context)
        {
            UserPermissionForEmployeeLevelRepository = new UserPermissionForEmployeeLevelRepository(context);
        }

        public List<UserPermissionForEmployeeLevel> GetUserPermissionForEmployeeLevel(string userName)
        {
            try
            {
                return UserPermissionForEmployeeLevelRepository.GetUserPermissionForEmployeeLevel(userName);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public object UserPermissionForEmployeeLevelByUserName(string userName)
        {

            try
            {
                return UserPermissionForEmployeeLevelRepository.GetUserPermissionForEmployeeLevel(userName).Select(x => new
                {
                    EmployeeTypeId = x.EmployeeTypeId,
                    Title = x.EmployeeType.Title,
                    UserName = x.UserName
                }).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int SaveUserPermissionForEmployeeLevelManager(List<int> selectedEmployeeTypes, string userName)
        {
            var saveIndex = 0;
            try
            {
               
                var userPermission = UserPermission(selectedEmployeeTypes, userName);

                var existingUserPermisson =
                    UserPermissionForEmployeeLevelRepository.Filter(x => x.UserName == userName && x.IsActive).ToList();
                var finalUserPermissionForEmployeeLevels = new List<UserPermissionForEmployeeLevel>(userPermission);
                if (!existingUserPermisson.Any())
                {
                    saveIndex = UserPermissionForEmployeeLevelRepository.SaveList(userPermission);
                }
                else
                {
                    foreach (var user in existingUserPermisson)
                    {
                        var isExist =
                            userPermission.Exists(
                                x => x.UserName == user.UserName && x.EmployeeTypeId == user.EmployeeTypeId);
                        if (isExist)
                        {
                            finalUserPermissionForEmployeeLevels.RemoveAll(x => x.UserName == user.UserName && x.EmployeeTypeId == user.EmployeeTypeId);
                        }
                        else
                        {
                           saveIndex+= UserPermissionForEmployeeLevelRepository.Delete( x =>x.UserName == user.UserName && x.EmployeeTypeId == user.EmployeeTypeId);
                        }

                    }
                    saveIndex = finalUserPermissionForEmployeeLevels.Any() ? UserPermissionForEmployeeLevelRepository.SaveList(finalUserPermissionForEmployeeLevels) : 1;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                throw new Exception("User not saved successfylly");
            }
            return saveIndex;
        }

        private static List<UserPermissionForEmployeeLevel> UserPermission(IEnumerable<int> selectedEmployeeTypes, string userName)
        {
            return selectedEmployeeTypes.Select(x => new UserPermissionForEmployeeLevel()
            {
                UserName = userName,
                EmployeeTypeId = x,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = PortalContext.CurrentUser.UserId,
            }).ToList();
        }
    }
}
