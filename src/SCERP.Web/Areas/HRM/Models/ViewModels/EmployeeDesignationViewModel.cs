using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeDesignationViewModel : EmployeeDesignation
    {
        public EmployeeDesignationViewModel()
        {
            EmployeeDesignations = new List<EmployeeDesignation>();
            IsSearch = true;
        }

        public List<EmployeeDesignation> EmployeeDesignations { get; set; }


        public int SearchByEmployeeTypeId
        {
            get;
            set;
        }

        public int SearchByEmployeeGradeId
        {
            get;
            set;
        }


        public string SearchByEmployeeDesignationTitle
        {
            get;
            set;
        }

    

    }
}