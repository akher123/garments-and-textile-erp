using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager;
using SCERP.Common;
using SCERP.Common.PermissionModel;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Security;

namespace SCERP.Web.Provider
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public Manager Manager { get { return Manager.Instance; } }

        public IUserManager UserManager
        {
            get { return Manager.UserManager; }
        }

        public IUserPermissionForDepartmentLevelManager UserPermissionForDepartmentLevelManager
        {
            get { return Manager.UserPermissionForDepartmentLevelManager; }
        }

        public IUserPermissionForEmployeeLevelManager UserPermissionForEmployeeLevelManager
        {
            get { return Manager.UserPermissionForEmployeeLevelManager; }
        }
        public IEmployeeCompanyInfoManager EmployeeCompanyInfoManager
        {
            get { return Manager.EmployeeCompanyInfoManager; }
        }
        #region Unused Override
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion Unused Override
        public override bool ValidateUser(string username, string password)
        {
            //1) GetUser from DB
            //2) If user exists then Add to PortalUser else false

            var user = new User { UserName = username, Password = password };

            int userLoginStatus = UserManager.GetLoginStatus(user);
            if (userLoginStatus > 0)
            {
                var currentUser = UserManager.GetUser(user);

                if (currentUser != null && currentUser.Id > 0)
                {
                    var portalUser = new PortalUser
                    {
                        Name = currentUser.UserName,
                        Id = currentUser.Id,
                        UserId = currentUser.EmployeeId,
                        TnaResponsible = currentUser.TnaResponsible,
                        IsSystemUser = currentUser.IsSystemUser,
                        CompId = currentUser.CompId,
                        Validated = true,
                        
                    };
                    Company company = EmployeeCompanyInfoManager.GetCompanyRefIdByEmployeeId(portalUser.UserId);
                    portalUser.CompId = company.CompanyRefId;
                    portalUser.CompanyId = company.Id;
                    portalUser.CompanyName = company.Name;
                    portalUser.FullAddress = company.FullAddress;
                    portalUser.CompanyLogo = company.ImagePath ?? "--";
                    portalUser.DomainName = company.DomainName;
                    portalUser.SessionId =Guid.NewGuid();
                    PortalContext.CurrentUser = portalUser;
                

                    /*Real Data*/

                    var companies = new List<UserCompany>();

                    var branches = new List<UserBranch>();

                    var units = new List<UserUnit>();

                    var departments = new List<UserDepartment>();

                    var employeeTypes = new List<UserEmployeeType>();

                    var userPermissionForDepartmentLevels = UserPermissionForDepartmentLevelManager.GetUserPermissionForDepartmentLevel(username);
                    GetUserPermissionForDepartmentLevels(userPermissionForDepartmentLevels, ref companies, ref branches, ref units, ref departments);

                    var userPermissionForEmployeeLevels = UserPermissionForEmployeeLevelManager.GetUserPermissionForEmployeeLevel(username);
                    GetUserPermissionForEmployeeLevels(userPermissionForEmployeeLevels, ref employeeTypes);

                    PermissionContext permissionContext = GetUserPermissionContext(companies, branches, units, departments, employeeTypes);


                    PortalContext.CurrentUser.PermissionContext = permissionContext;
                  
                    return true;
                }
            }

            return false;
        }

        private void GetUserPermissionForDepartmentLevels(List<UserPermissionForDepartmentLevel> userPermissionForDepartmentLevels, ref List<UserCompany> companies, ref List<UserBranch> branches, ref List<UserUnit> units, ref List<UserDepartment> departments)
        {
            //Must use for loop, not for each loop in this scenario

            for (var i = 0; i < userPermissionForDepartmentLevels.Count; i++)
            {
                departments.Add(new UserDepartment()
                {
                    BranchUnitDepartmentId = userPermissionForDepartmentLevels[i].BranchUnitDepartmentId,
                    BranchUnitId = userPermissionForDepartmentLevels[i].BranchUnitId,
                    DepartmentId = userPermissionForDepartmentLevels[i].BranchUnitDepartment.UnitDepartment.DepartmentId,
                    DepartmentName = userPermissionForDepartmentLevels[i].BranchUnitDepartment.UnitDepartment.Department.Name
                });

                units.Add(new UserUnit()
                {
                    BranchUnitId = userPermissionForDepartmentLevels[i].BranchUnitId,
                    BranchId = userPermissionForDepartmentLevels[i].BranchUnit.BranchId,
                    UnitId = userPermissionForDepartmentLevels[i].BranchUnit.UnitId,
                    UnitName = userPermissionForDepartmentLevels[i].BranchUnit.Unit.Name,
                    Department = departments[i],
                    Departments = departments
                });

                branches.Add(new UserBranch()
                {
                    BranchId = userPermissionForDepartmentLevels[i].BranchId,
                    CompanyId = userPermissionForDepartmentLevels[i].CompanyId,
                    BranchName = userPermissionForDepartmentLevels[i].Branch.Name,
                    Unit = units[i],
                    Units = units
                });

                companies.Add(new UserCompany()
                {
                    CompanyId = userPermissionForDepartmentLevels[i].CompanyId,
                    CompanyName = userPermissionForDepartmentLevels[i].Company.Name,
                    Branch = branches[i],
                    Branches = branches
                });
            }
        }

        private void GetUserPermissionForEmployeeLevels(List<UserPermissionForEmployeeLevel> userPermissionForEmployeeLevels, ref List<UserEmployeeType> employeeTypes)
        {

            for (var i = 0; i < userPermissionForEmployeeLevels.Count; i++)
            {
                employeeTypes.Add(new UserEmployeeType()
                {
                    Id = userPermissionForEmployeeLevels[i].EmployeeTypeId,
                    Title = userPermissionForEmployeeLevels[i].EmployeeType.Title
                });
            }
        }

        private PermissionContext GetUserPermissionContext(List<UserCompany> companies, List<UserBranch> branches, List<UserUnit> units, List<UserDepartment> departments, List<UserEmployeeType> employeeTypes)
        {
            var companyList = (from UserCompany company in companies
                               select new
                               {
                                   companyId = company.CompanyId,
                                   companyName = company.CompanyName
                               }).Distinct();

            var branchList = (from UserBranch branch in branches
                              select new
                              {
                                  branchId = branch.BranchId,
                                  companyId = branch.CompanyId,
                                  branchName = branch.BranchName
                              }).Distinct();


            var unitList = (from UserUnit unit in units
                            select new
                            {
                                branchUnitId = unit.BranchUnitId,
                                branchId = unit.BranchId,
                                unitId = unit.UnitId,
                                unitName = unit.UnitName
                            }).Distinct();
            unitList = unitList.ToList();


            var departmentList = (from UserDepartment department in departments
                                  select new
                                  {
                                      branchUnitDepartmentId = department.BranchUnitDepartmentId,
                                      branchUnitId = department.BranchUnitId,
                                      departmentId = department.DepartmentId,
                                      departmentName = department.DepartmentName
                                  }).Distinct();

            departmentList = departmentList.ToList();

            var employeeTypeList = (from UserEmployeeType employeeType in employeeTypes
                                    select new
                                    {
                                        id = employeeType.Id,
                                        title = employeeType.Title
                                    }).Distinct();

            employeeTypeList = employeeTypeList.ToList();


            var permissionContext = new PermissionContext
            {
                CompanyList = companyList.Select(x => new UserCompany() { CompanyId = x.companyId, CompanyName = x.companyName }).ToList(),
                BranchList = branchList.Select(x => new UserBranch() { CompanyId = x.companyId, BranchId = x.branchId, BranchName = x.branchName }).ToList(),
                UnitList = unitList.Select(x => new UserUnit() { BranchUnitId = x.branchUnitId, BranchId = x.branchId, UnitName = x.unitName }).ToList(),
                DepartmentList = departmentList.Select(x => new UserDepartment() { BranchUnitDepartmentId = x.branchUnitDepartmentId, BranchUnitId = x.branchUnitId, DepartmentName = x.departmentName }).ToList(),
                EmployeeTypeList = employeeTypeList.Select(x => new UserEmployeeType() { Id = x.id, Title = x.title }).ToList(),
            };

            return permissionContext;
        }
    }

}
