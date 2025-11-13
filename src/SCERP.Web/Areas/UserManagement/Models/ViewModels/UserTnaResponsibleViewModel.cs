using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class UserTnaResponsibleViewModel
    {
        public Guid EmployeeId { get; set; }
        public List<User> Users { get; set; }
        public IEnumerable<string> Responsibles { get; set; }
        public List<string> MerchandiserIdList { get; set; }
        public UserTnaResponsibleViewModel()
        {
            MerchandiserIdList = new List<string>();
            Users = new List<User>();
            Responsibles = new List<string>();
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