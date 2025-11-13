using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeFamilyInfoViewModel : EmployeeFamilyInfo
    {
        public EmployeeFamilyInfoViewModel()
        {
            EmployeeFamilyInfos = new List<EmployeeFamilyInfo>();
        }

        public List<EmployeeFamilyInfo> EmployeeFamilyInfos { get; set; }

        public List<Gender> Genders { get; set; }
        public List<SelectListItem> GenderSelectListItem
        {
            get { return new SelectList(Genders, "GenderId", "Title").ToList(); }

        }

    }

}