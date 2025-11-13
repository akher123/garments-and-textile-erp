using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Common.PermissionModel;
using SCERP.Model;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserPermissionForDepartmentLevelViewModel : UserPermissionForDepartmentLevel
    {
        public List<UserCompany> UserCompanies { get; set; }
        public List<User> Users { get; set; }
        public List<string> CompanyBranchUnitDepartment { get; set; }
        public UserPermissionForDepartmentLevelViewModel()
        {
            Users=new List<User>();
            CompanyBranchUnitDepartment=new List<string>();
            UserCompanies = new List<UserCompany>();
        }
        public IEnumerable<SelectListItem> UserSelectListItem
        {
            get
            {
                return new SelectList(Users, "UserName", "UserName");
            }
        }
    }
}