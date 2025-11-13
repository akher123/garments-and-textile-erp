using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class TmModuleViewModel:TmModule
    {
         public TmModuleViewModel()
        {
            Modules=new List<TmModule>();
        }
         public List<TmModule> Modules { get; set; } 
    }
}