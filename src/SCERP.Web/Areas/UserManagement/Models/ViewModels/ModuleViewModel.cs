using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class ModuleViewModel : ProSearchModel<Module> 
    {
        
        public List<Module> Modules { get; set; }
    }
}