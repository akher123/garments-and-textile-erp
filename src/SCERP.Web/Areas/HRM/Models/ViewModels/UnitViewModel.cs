using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class UnitViewModel:Unit
    {
        public UnitViewModel()
        {
            Units=new List<Unit>();
            IsSearch = true;
        }
        public List<Unit> Units { get; set; }
        public string SearchByName { get; set; }
    }
}