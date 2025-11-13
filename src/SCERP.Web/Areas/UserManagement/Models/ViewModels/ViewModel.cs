using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class ViewModel
    {
        public string UserName { get; set; }
        public List<int> ModuleFeatureId { set; get; }
        public List<string> AccessLevel { get; set; }
    }
}