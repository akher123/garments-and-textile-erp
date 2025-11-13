using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;


namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class GenericNameViewModel : ProSearchModel<GenericNameViewModel>
    {
        public Inventory_GenericName GenericName { get; set; }
        public List<Inventory_GenericName> GenericNames { get; set; }

        public GenericNameViewModel()
        {
            GenericName=new Inventory_GenericName();
            GenericNames=new List<Inventory_GenericName>();
        }
    }
}