using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmColorViewModel:OM_Color
    {
        public List<OM_Color> OmColors { get; set; }
   
        public OmColorViewModel()
        {
            OmColors=new List<OM_Color>();
         
        }

    }
}