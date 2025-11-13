using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserPermissionForEmployeeLevelViewModel : UserPermissionForEmployeeLevel
    {
        public IList<EmployeeType> EmployeeTypes { get; set; }
        public List<User> Users { get; set; }
        public List<int> SelectedEmployeeTypes { get; set; }
        public UserPermissionForEmployeeLevelViewModel()
        {
            EmployeeTypes=new List<EmployeeType>();
            Users=new List<User>();
            SelectedEmployeeTypes=new List<int>();
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> UserSelectListItem
        {
            get
            {
                return new SelectList(Users, "UserName", "UserName");
            }
        }
    }
}