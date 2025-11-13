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
    public class GreyIssueViewModel : ProSearchModel<GreyIssueViewModel>
    {
        public Inventory_GreyIssue GreyIssue { get; set; }
        public List<Inventory_GreyIssue> GreyIssues { get; set; }
        public List<Party> Parties { get; set; }
        public Inventory_GreyIssueDetail GreyIssueDetail { get; set; }
        public Dictionary<string, KnittingOrderDelivery> KnittingOrderDelivery { get; set; }
        public GreyIssueViewModel()
        {
            this.Parties = new List<Party>();
            this.GreyIssue = new Inventory_GreyIssue();
            this.GreyIssues = new List<Inventory_GreyIssue>();
            this.GreyIssueDetail=new Inventory_GreyIssueDetail();
            this.KnittingOrderDelivery = new Dictionary<string, KnittingOrderDelivery>();
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