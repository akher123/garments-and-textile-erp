using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class GroupChallanViewModel : ProSearchModel<Inventory_GroupChallan>
    {
        public Inventory_GroupChallan GroupChallan { get; set; }
        public List<Inventory_GroupChallan> GroupChallans { get; set; }
        public List<Party> Parties { get; set; }
    
        public GroupChallanViewModel()
        {
            
            this.GroupChallans = new List<Inventory_GroupChallan>();
            this.GroupChallan=new Inventory_GroupChallan();
            this.Parties = new List<Party>();
          
        }

        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }

        }
    }
}