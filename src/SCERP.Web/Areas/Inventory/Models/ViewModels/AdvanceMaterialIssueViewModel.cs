using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class AdvanceMaterialIssueViewModel : ProSearchModel<AdvanceMaterialIssueViewModel>
    {
        public VwAdvanceMaterialIssue MaterialIssue { get; set; }
        public VwAdvanceMaterialIssueDetail MaterialIssueDetail { get; set; }
        public List<VwAdvanceMaterialIssue> MaterialIssues { get; set; }
        public Dictionary<string, VwAdvanceMaterialIssueDetail> MaterialIssueDetails { get; set; }
        public string Key { get; set; }
        public decimal SinH { get; set; }
        public IEnumerable StoreList { get; set; }
        public List<Party> Parties { get; set; }
        public List<PLAN_Process> Processes { get; set; }
        public List<OM_Buyer> OmBuyers { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable OrderList { get; set; }
        public string OrderRefId { get; set; }
        [Required]
        public override string SearchString { get; set; }
        public List<ProgramYarnWithdrow> ProgramYarnWithdrows { get; set; }
        public AdvanceMaterialIssueViewModel()
        {
            Processes=new List<PLAN_Process>();
            Parties=new List<Party>();
            StoreList = new List<Object>();
            MaterialIssue = new VwAdvanceMaterialIssue();
            MaterialIssues=new List<VwAdvanceMaterialIssue>();
            MaterialIssueDetail = new VwAdvanceMaterialIssueDetail();
            MaterialIssueDetails=new Dictionary<string, VwAdvanceMaterialIssueDetail>();
            OmBuyers = new List<OM_Buyer>();
            StyleList = new List<object>();
            OrderList = new List<object>();
            ProgramYarnWithdrows = new List<ProgramYarnWithdrow>();
        }
        public IEnumerable<SelectListItem> StoreTypeSelectListItem
        {
            get
            {
                return new SelectList(StoreList, "StoreId", "Name");
            }

        }
        public IEnumerable<SelectListItem> ProcessSelectListItem
        {
            get
            {
                return new SelectList(new List<PLAN_Process>()
                {
                    new PLAN_Process(){ProcessRefId = ProcessType.YARNDYEING,ProcessName="YARNDYEING"},
                    new PLAN_Process(){ProcessRefId = ProcessType.KNITTING,ProcessName="KNITTING"},
                    new PLAN_Process(){ProcessRefId = ProcessType.COLLARCUFF,ProcessName="COLLARCUFF"}
                }, "ProcessRefId", "ProcessName");
            }

        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }

        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> WrapperSelectListItem
        {
            get
            {
                return new SelectList(new[]{"BAG","PACKET"});
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }

        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
    }
}