using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{

    public class ComponentViewModel:OM_Component
    {
        public List<OM_Component> Components { get; set; }
        public ComponentViewModel()
        {
            Components=new List<OM_Component>();
        }

        public IEnumerable<SelectListItem> ComponentTypeSelectListItem
        {
            get
            {
                return new SelectList(Enum.GetValues(typeof(ComponentType)).Cast<ComponentType>().Select(x=>new{Id=(int)x,Value=x.ToString()}),"Id","Value");
            }
        }
    }
}