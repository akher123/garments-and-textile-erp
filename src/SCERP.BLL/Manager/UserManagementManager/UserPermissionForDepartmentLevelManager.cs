using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Process.TreeView;
using SCERP.Common;
using SCERP.Common.PermissionModel;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserPermissionForDepartmentLevelManager : BaseManager, IUserPermissionForDepartmentLevelManager
    {
        private readonly IUserPermissionForDepartmentLevelRepository _userPermissionForDepartmentLevelRepository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IBranchUnitDepartmentRepository _branchUnitDepartmentRepository = null;
        private IBranchRepository _branchRepository = null;

        public UserPermissionForDepartmentLevelManager(SCERPDBContext context)
        {
            _userPermissionForDepartmentLevelRepository = new UserPermissionForDepartmentLevelRepository(context);
            _branchUnitDepartmentRepository = new BranchUnitDepartmentRepository(context);
            _companyRepository = new CompanyRepository(context);
            _branchRepository=new BranchRepository(context);
        }
        public List<UserPermissionForDepartmentLevel> GetUserPermissionForDepartmentLevel(string userName)
        {
            try
            {
                return _userPermissionForDepartmentLevelRepository.GetUserPermissionForDepartmentLevel(userName);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        public List<UserCompany> GetUserCompanyList()
        {
            List<UserCompany> userCompanyList;
            try
            {
                var companies = _companyRepository.Filter(x => x.IsActive).ToList();
                var branchUnitDepartments = _branchUnitDepartmentRepository.GetBranchUnitDepartment();
                var branchs = _branchRepository.Filter(x => x.IsActive).ToList();
                var userPermissionTreeView = new UserPermissionTreeView(companies, branchUnitDepartments, branchs);
                userCompanyList = userPermissionTreeView.GetUserCompanyList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return userCompanyList;
        }

        public int SaveUserPermissionForDepartmentLevel(List<string> companyBranchUnitDepartment, string userName)
        {
            var saveIndex = 0;
            try
            {

                var userPermissions = GetUserPermissions(companyBranchUnitDepartment, userName);
                var finalUserPermissions = new List<UserPermissionForDepartmentLevel>(userPermissions);
                var existingUserPermissionList = _userPermissionForDepartmentLevelRepository.Filter(x => x.IsActive && x.UserName == userName).ToList();

                if (!existingUserPermissionList.Any())
                {
                    saveIndex += _userPermissionForDepartmentLevelRepository.SaveUserPermissionForDepartmentLevel(userPermissions);
                }
                else
                {
                    foreach (var permission in existingUserPermissionList)
                    {
                        var isExist = IsExistUserPermission(userPermissions, permission);
                        if (isExist)
                        {
                            Remove(finalUserPermissions, permission);
                        }
                        else
                        {
                            saveIndex += DeleteUserPermission(permission);
                        }

                    }
                }
                saveIndex = finalUserPermissions.Any() ? _userPermissionForDepartmentLevelRepository.SaveUserPermissionForDepartmentLevel(finalUserPermissions) : 1;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saveIndex;
        }

        private static bool IsExistUserPermission(List<UserPermissionForDepartmentLevel> userPermissions, UserPermissionForDepartmentLevel permission)
        {
            return userPermissions.Exists(
                x => x.IsActive &&
                     x.UserName == permission.UserName && x.CompanyId == permission.CompanyId &&
                     x.BranchId == permission.BranchId && x.BranchUnitId == permission.BranchUnitId &&
                     x.BranchUnitDepartmentId == permission.BranchUnitDepartmentId);
        }

        private static void Remove(List<UserPermissionForDepartmentLevel> finalUserPermissions, UserPermissionForDepartmentLevel permission)
        {
            finalUserPermissions.RemoveAll(x => x.IsActive &&
                                                x.UserName == permission.UserName &&
                                                x.CompanyId == permission.CompanyId &&
                                                x.BranchId == permission.BranchId &&
                                                x.BranchUnitId == permission.BranchUnitId &&
                                                x.BranchUnitDepartmentId == permission.BranchUnitDepartmentId);
        }

        private int DeleteUserPermission(UserPermissionForDepartmentLevel permission)
        {
            return _userPermissionForDepartmentLevelRepository
                .Delete(x => x.IsActive &&
                             x.UserName == permission.UserName &&
                             x.CompanyId == permission.CompanyId &&
                             x.BranchId == permission.BranchId &&
                             x.BranchUnitId == permission.BranchUnitId &&
                             x.BranchUnitDepartmentId == permission.BranchUnitDepartmentId);
        }

        private static List<UserPermissionForDepartmentLevel> GetUserPermissions(IEnumerable<string> companyBranchUnitDepartment, string userName)
        {
            return companyBranchUnitDepartment.Select(x => new UserPermissionForDepartmentLevel
            {
                UserName = userName,
                CompanyId = Convert.ToInt32(x.Split('-')[0]),
                BranchId = Convert.ToInt32(x.Split('-')[1]),
                BranchUnitId = Convert.ToInt32(x.Split('-')[2]),
                BranchUnitDepartmentId = Convert.ToInt32(x.Split('-')[3]),
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = PortalContext.CurrentUser.UserId,
            }).ToList();
        }

        private int EditPermission(UserPermissionForDepartmentLevel permission)
        {
            var userPermission =
                      _userPermissionForDepartmentLevelRepository.FindOne(x =>
                              x.UserName == permission.UserName && x.CompanyId == permission.CompanyId &&
                              x.BranchId == permission.BranchId && x.BranchUnitId == permission.BranchUnitId &&
                              x.BranchUnitDepartmentId == permission.BranchUnitDepartmentId);
            userPermission.CompanyId = permission.CompanyId;
            userPermission.BranchId = permission.BranchId;
            userPermission.BranchUnitId = permission.BranchUnitId;
            userPermission.BranchUnitDepartmentId = permission.BranchUnitDepartmentId;
            userPermission.EditedBy = PortalContext.CurrentUser.UserId;
            permission.EditedDate = DateTime.Now;
            return _userPermissionForDepartmentLevelRepository.Edit(userPermission);
        }
        public object GetUserPermissionSector(string userName)
        {
            object userUserPermission;
            try
            {
                userUserPermission =
                   _userPermissionForDepartmentLevelRepository.Filter(x => x.UserName == userName && x.IsActive).ToList()
                       .Select(x => new
                       {
                           CompanySelector = x.CompanyId,
                           BranchSelector = x.CompanyId + "-" + x.BranchId,
                           UnitSelector = x.CompanyId + "-" + x.BranchId + "-" + x.BranchUnitId,
                           DepartmentSelector = x.CompanyId + "-" + x.BranchId + "-" + x.BranchUnitId + "-" + x.BranchUnitDepartmentId
                       }).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return userUserPermission;
        }
    }
}
