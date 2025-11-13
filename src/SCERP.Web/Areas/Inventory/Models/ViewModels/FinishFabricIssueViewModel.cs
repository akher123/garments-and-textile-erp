using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class FinishFabricIssueViewModel : ProSearchModel<FinishFabricIssueViewModel>
    {
        public Inventory_FinishFabricIssue FinishFabricIssue { get; set; }
        public List<Inventory_FinishFabricIssue> FinishFabricIssues { get; set; }
        public VwFinishFabricIssueDetail FinishFabricIssueDetail { get; set; }
        public Dictionary<string, VwFinishFabricIssueDetail> FinishFabricIssueDetails { get; set; }
        public List<Party> Parties { get; set; }
        public List<Pro_Batch> Batches { get; set; }
        public IEnumerable Items { get; set; }
        public string Key { get; set; }

        public FinishFabricIssueViewModel()
        {
            FinishFabricIssue=new Inventory_FinishFabricIssue();
            FinishFabricIssues=new List<Inventory_FinishFabricIssue>();
            FinishFabricIssueDetail=new VwFinishFabricIssueDetail();
            Items=new List<Object>();
            Parties=new List<Party>();
            FinishFabricIssueDetails=new Dictionary<string, VwFinishFabricIssueDetail>();
        }

        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }
        }
        public IEnumerable<SelectListItem> ApprovedStatusSelectListItem
        {
            get { return new SelectList(new[] { new { Value = true, Text = "Approved" }, new { Value = false, Text = "Panding" } }, "Value", "Text"); }

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