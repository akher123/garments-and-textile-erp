using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.CommonModel;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class ColorViewModel:Color
    {
        public List<Color> Colors { get; set; }

        public ColorViewModel()
        {
            Colors=new List<Color>();
        }
    }
}