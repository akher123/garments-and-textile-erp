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
    public class EmployeeQuitInfoViewModel : Employee
    {

        public List<QuitType> QuitTypes { get; set; }
        public List<SelectListItem> QuitTypeSelectListItem
        {
            get { return new SelectList(QuitTypes, "QuitTypeId", "Type").ToList(); }

        }
           
    }
}