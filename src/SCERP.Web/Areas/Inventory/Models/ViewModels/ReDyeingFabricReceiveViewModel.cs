using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class ReDyeingFabricReceiveViewModel : ProSearchModel<ReDyeingFabricReceiveViewModel>
    {
        public List<Inventory_ReDyeingFabricReceive> FabricReceives { get; set; }
        public Inventory_ReDyeingFabricReceive ReDyeingFabricReceive { get; set; }
        public List<Party> Parties { get; set; }
        public List<Pro_Batch> Batches { get; set; }
        public IEnumerable Items { get; set; }
        public string Key { get; set; }
        public VwReDyeingFabricReceiveDetail ReDyeingFabricReceiveDetail { get; set; }
        public Dictionary<string, VwReDyeingFabricReceiveDetail> ReDyeingFabricReceiveDetails { get; set; }
        public ReDyeingFabricReceiveViewModel()
        {
            ReDyeingFabricReceive=new Inventory_ReDyeingFabricReceive();
            FabricReceives=new List<Inventory_ReDyeingFabricReceive>();
            Items = new List<Object>();
            Parties = new List<Party>();
            ReDyeingFabricReceiveDetails = new Dictionary<string, VwReDyeingFabricReceiveDetail>();
            ReDyeingFabricReceiveDetail = new VwReDyeingFabricReceiveDetail();
        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }
        }
      
        public IEnumerable<SelectListItem> BatchSelectListItem
        {
            get { return new SelectList(Batches, "BatchId", "BtRefNo"); }
        }

        public IEnumerable<SelectListItem> ItemSelectListItem
        {
            get { return new SelectList(Items, "BatchDetailId", "ItemName"); }
        }
    }
}