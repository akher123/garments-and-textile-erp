using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeePersonalInfoViewModel : Employee
    {

        public List<MaritalState> MaritalStates { get; set; }
        public List<SelectListItem> MaritalStateSelectListItem
        {
            get { return new SelectList(MaritalStates, "MaritalStateId", "Title").ToList(); }

        }

        public List<BloodGroup> BloodGroups { get; set; }
        public List<SelectListItem> BloodGroupSelectListItem
        {
            get { return new SelectList(BloodGroups, "Id", "GroupName").ToList(); }

        }
           
    }
}