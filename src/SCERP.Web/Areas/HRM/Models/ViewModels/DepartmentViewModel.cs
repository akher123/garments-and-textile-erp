using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{  
    public class DepartmentViewModel : Department
    {
        public DepartmentViewModel()
        {
            Departments = new List<Department>();
            IsSearch = true;
        }

        public List<Department> Departments { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}