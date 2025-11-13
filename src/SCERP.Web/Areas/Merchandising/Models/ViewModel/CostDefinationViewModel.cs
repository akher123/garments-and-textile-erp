using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class CostDefinationViewModel : OM_CostDefination
    {
        public List<OM_CostDefination> CostDefinations { get; set; }
        public CostDefinationViewModel()
        {

            CostDefinations = new List<OM_CostDefination>();
        }

        public  List<Dropdown> CostGroups
        {
            get
            {
                return new List<Dropdown>()
                {
                    new Dropdown() {Id = "FAB", Value = "FABRIC"},
                    new Dropdown() {Id = "ACC", Value = "ACCESSORIES"},
                    new Dropdown() {Id = "EMB", Value = "EMBELLISHMENT"},
                    new Dropdown() {Id = "OTC", Value = "OTHER COST"},
                };
            }
        }

        public IEnumerable<SelectListItem> CostGroupsSelectListItem
        {

            get
            {

                return new SelectList(CostGroups, "Id", "Value");
            }
        }
    }
}