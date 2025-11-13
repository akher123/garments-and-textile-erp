using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class RedyeingFabricIssueViewModel : ProSearchModel<RedyeingFabricIssueViewModel>
    {
        public Inventory_RedyeingFabricIssue RedyeingFabricIssue { get; set; }
        public List<Inventory_RedyeingFabricIssue> RedyeingFabricIssues { get; set; }
        public VwRedyeingFabricIssueDetail RedyeingFabricIssueDetail { get; set; }
        public Dictionary<string, VwRedyeingFabricIssueDetail> RedyeingFabricIssueDetails { get; set; }
        public List<Party> Parties { get; set; }
        public List<Pro_Batch> Batches { get; set; }
        public IEnumerable Items { get; set; }
        public string Key { get; set; }

        public RedyeingFabricIssueViewModel()
        {
            RedyeingFabricIssue = new Inventory_RedyeingFabricIssue();
            RedyeingFabricIssues = new List<Inventory_RedyeingFabricIssue>();
            RedyeingFabricIssueDetail = new VwRedyeingFabricIssueDetail();
            Items=new List<Object>();
            Parties=new List<Party>();
            RedyeingFabricIssueDetails = new Dictionary<string, VwRedyeingFabricIssueDetail>();
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

      
        public IEnumerable<SelectListItem> ApprovedSpSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "Approved", Value = true }, new { Text = "Pending", Value = false } }, "Value", "Text");
            }
        }
    }
}