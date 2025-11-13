using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeEducationInfoViewModel : EmployeeEducation
    {
        public EmployeeEducationInfoViewModel()
        {
            EmployeeEducations = new List<EmployeeEducation>();
        }

        public List<EmployeeEducation> EmployeeEducations { get; set; }

        public List<EducationLevel> EducationLevels { get; set; }
        public List<SelectListItem> EducationLevelSelectListItem
        {
            get { return new SelectList(EducationLevels, "Id", "Title").ToList(); }

        }

    }

}