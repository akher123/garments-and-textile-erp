using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeGradeViewModel : EmployeeGrade
    {
        public EmployeeGradeViewModel()
        {
            EmployeeGrades = new List<EmployeeGrade>();
            EmployeeTypes=new List<EmployeeType>();
            IsSearch = true;
        }

        public List<EmployeeType> EmployeeTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        public List<EmployeeGrade> EmployeeGrades { get; set; }

        public string SearchByEmployeeGrade{get; set;}

        public int SearchByEmployeeType { get; set;}
    }
}
