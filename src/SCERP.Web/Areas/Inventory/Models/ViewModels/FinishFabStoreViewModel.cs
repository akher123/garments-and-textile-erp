using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class FinishFabStoreViewModel : ProSearchModel<FinishFabStoreViewModel>
    {
        public FinishFabStoreViewModel()
        {
            this.FinishFabStores=new List<Inventory_FinishFabStore>();
            this.FinishFabbricStore = new Inventory_FinishFabStore();
            this.FabDictionary = new Dictionary<string, SpInvFinishFabStore>();
        }
        public List<Inventory_FinishFabStore> FinishFabStores { get; set; }
        public Inventory_FinishFabStore FinishFabbricStore { get; set; }
        public Dictionary<string, SpInvFinishFabStore> FabDictionary { get; set; }
       
    }
}