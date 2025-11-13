using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ReligionViewModel : Religion
    {

        public ReligionViewModel()
        {
            Religions = new List<Religion>();
            IsSearch = true;
        }

        public List<Religion> Religions { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}