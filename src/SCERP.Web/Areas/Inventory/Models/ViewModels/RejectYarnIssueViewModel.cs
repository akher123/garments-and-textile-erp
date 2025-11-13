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
    public class RejectYarnIssueViewModel : ProSearchModel<InventoryReportViewModel>
    {
        public List<Inventory_RejectYarnIssue> RejectYarnIssues { get; set; }
        public Inventory_RejectYarnIssue RejectYarnIssue { get; set; }

        public Dictionary<string, SpRejectYarnDetail> RejectYarnDetails { get; set; }
     
        public List<Party> Parties { get; set; }

        public RejectYarnIssueViewModel()
        {
            RejectYarnDetails=new Dictionary<string, SpRejectYarnDetail>();
            RejectYarnIssue = new Inventory_RejectYarnIssue();
            RejectYarnIssue=new Inventory_RejectYarnIssue();
            Parties=new List<Party>();
        }

        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }

        }
    }
}