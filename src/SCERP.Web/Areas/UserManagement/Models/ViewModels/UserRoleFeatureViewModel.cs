using SCERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserRoleFeatureViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string CompId { get; set; }
        public List<int> ModuleFeatureId { set; get; }
        public List<string> AccessLevel { get; set; }
        public string NewUserName { get; set; }
        public List<Company> Companies { get; set; }
        public List<User> Users { get; set; }
        public List<ModuleFeatureTreeView> ModuleFeatureTreeViews { get; set; }
        public UserRoleFeatureViewModel()
        {
            this.Companies = new List<Company>();
            this.Users = new List<User>();
            this.ModuleFeatureTreeViews = new List<ModuleFeatureTreeView>();
        }
        public IEnumerable<SelectListItem> UserSelectListItem
        {
            get
            {
                return new SelectList(Users, "UserName", "UserName", UserName);
            }
        }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get
            {
                return new SelectList(Companies, "CompanyRefId", "Name", CompId);
            }
        }
    }
}