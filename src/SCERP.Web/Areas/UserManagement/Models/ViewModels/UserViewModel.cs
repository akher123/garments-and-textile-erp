using SCERP.Common;
using SCERP.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserViewModel : User
    {
        public string SearchByUser { get; set; }
        public List<User> Users { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        // [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "password doesn't match")]

        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public List<Company> Companies { get; set; }
        public UserViewModel()
        {
            Users = new List<User>();
            Companies = new List<Company>();
        }

        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get
            {
                return new SelectList(Companies, "CompanyRefId", "Name", PortalContext.CurrentUser.CompId);
            }
        }

    }
}