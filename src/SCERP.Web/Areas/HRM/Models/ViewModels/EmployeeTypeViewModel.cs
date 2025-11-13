using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeTypeViewModel : EmployeeType
    {

        public EmployeeTypeViewModel()
        {
            EmployeeTypes = new List<EmployeeType>();
            IsSearch = true;
        }

        public List<EmployeeType> EmployeeTypes { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}