using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class SubjectViewModel:TmSubject
    {
        public List<TmSubject> Subjects { get; set; }
        public TmSubject Subject { get; set; }
        public List<TmModule> Modules { get; set; } 
        public SubjectViewModel()
        {
            Subjects=new List<TmSubject>();
            Subject=new TmSubject();
            Modules=new List<TmModule>();
        }
        public IEnumerable<SelectListItem> ModuleSelectListItems
        {
            get
            {
                return new SelectList(Modules, "ModuleId", "ModuleName");
            }
        }
    }
}