using System.Collections.Generic;
using System.Linq;
using SCERP.Common.PermissionModel;
using SCERP.Model;

namespace SCERP.BLL.Process.TreeView
{
    public class UserPermissionTreeView
    {
        private readonly IList<Company> _companies;
        private readonly List<BranchUnitDepartment> _branchUnitDepartments;
        private readonly List<UserCompany> _userCompanies;
        private readonly List<Branch> _branches;
        public UserPermissionTreeView(IList<Company> companies, List<BranchUnitDepartment> branchUnitDepartments,List<Branch>branches )
        {
            _companies = companies;
            _branchUnitDepartments = branchUnitDepartments;
            _userCompanies = new List<UserCompany>();
            _branches = branches;
        }
        public List<UserCompany> GetUserCompanyList()
        {
            foreach (UserCompany userCompany in GetCompanyList())
            {
                userCompany.Branches = GetUserBranchList(userCompany.CompanyId).ToList();
                _userCompanies.Add(userCompany);
            }
            return _userCompanies;
        }

        private IEnumerable<UserCompany> GetCompanyList()
        {
            return _companies
                .Select(x => new { CompanyName = x.Name, CompanyId = x.Id })
                .Distinct()
                .Select(x => new UserCompany() { CompanyName = x.CompanyName, CompanyId = x.CompanyId });
        }
        //private IEnumerable<UserBranch> GetUserBranchList(int companyId)
        //{
        //    var userBranchList =
        //          _branchUnitDepartments.Where(x => x.BranchUnit.Branch.CompanyId == companyId)
        //              .Select(
        //                  x => new { BranchId = x.BranchUnit.BranchId, BranchName = x.BranchUnit.Branch.Name })
        //                  .Distinct()
        //               .Select(
        //                  x => new UserBranch() { BranchId = x.BranchId, BranchName = x.BranchName }).ToList();

        //    foreach (var userBranch in userBranchList.Where(userBranch => userBranch != null))
        //    {
        //        userBranch.Units = GetUserUnitList(userBranch.BranchId).ToList();
        //        yield return userBranch;
        //    }
        //}
        private IEnumerable<UserBranch> GetUserBranchList(int companyId)
        {
            var userBranchList =
                  _branches.Where(x => x.CompanyId == companyId)
                      .Select(
                          x => new { BranchId = x.Id, BranchName = x.Name })
                          .Distinct()
                       .Select(
                          x => new UserBranch() { BranchId = x.BranchId, BranchName = x.BranchName }).ToList();

            foreach (var userBranch in userBranchList.Where(userBranch => userBranch != null))
            {
                userBranch.Units = GetUserUnitList(userBranch.BranchId).ToList();
                yield return userBranch;
            }
        }

        private IEnumerable<UserUnit> GetUserUnitList(int bracnhId)
        {
            var userUnitList = _branchUnitDepartments.Where(x => x.BranchUnit.BranchId == bracnhId)
                .Select(x => new  { BranchUnitId = x.BranchUnitId, UnitName = x.BranchUnit.Unit.Name }).Distinct()
                  .Select(x => new UserUnit() { BranchUnitId = x.BranchUnitId, UnitName = x.UnitName });
            foreach (UserUnit userUnit in userUnitList.Where(userUnit => userUnit != null))
            {
                userUnit.Departments = GetUserDepartmentList(userUnit.BranchUnitId).ToList();
                yield return userUnit;
            }
        }

        private IEnumerable<UserDepartment> GetUserDepartmentList(int branchUnitId)
        {

            var userDepartmentList = _branchUnitDepartments.Where(x => x.BranchUnitId == branchUnitId)
                  .Select(
                      x =>
                          new 
                          {
                              BranchUnitDepartmentId = x.BranchUnitDepartmentId,
                              DepartmentName = x.UnitDepartment.Department.Name
                          }).Distinct().Select(
                      x =>
                          new UserDepartment()
                          {
                              BranchUnitDepartmentId = x.BranchUnitDepartmentId,
                              DepartmentName = x.DepartmentName
                          });

            return userDepartmentList;

        }
    }
}
