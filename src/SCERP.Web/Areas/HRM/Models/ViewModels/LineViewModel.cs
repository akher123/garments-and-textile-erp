using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class LineViewModel : Line
    {
        public LineViewModel()
        {
            Lines = new List<Line>();
            IsSearch = true;
        }

        public List<Line> Lines { get; set; }

    
        public string SearchByLineName
        {
            get;
            set;
        }

    

    }
}