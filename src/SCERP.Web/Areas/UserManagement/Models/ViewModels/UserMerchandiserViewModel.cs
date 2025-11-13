using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserMerchandiserViewModel
    {
        public Guid EmployeeId { get; set; }
        public List<User> Users { get; set; }
        public IEnumerable<OM_Merchandiser> Merchandisers { get; set; }
        public List<string> MerchandiserIdList { get; set; }
        public UserMerchandiserViewModel()
        {
            MerchandiserIdList = new List<string>();
            Users=new List<User>();
            Merchandisers=new List<OM_Merchandiser>();
         
        }
        public IEnumerable<SelectListItem> UserSelectListItem
        {
            get
            {
                return new SelectList(Users, "EmployeeId", "UserName");
            }
        }
    }
}