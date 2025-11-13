using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EducationLevelViewModel:EducationLevel
    {
        
        public EducationLevelViewModel()
        {
            EducationLevels = new List<EducationLevel>();
            IsSearch = true;
        }

        public List<EducationLevel> EducationLevels { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}