using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class KnittingRollIssueViewModel : ProSearchModel<KnittingRollIssueViewModel>
    {
        public PROD_KnittingRollIssue KnittingRollIssue { get; set; }
        public List<PROD_KnittingRollIssue> KnittingRollIssues { get; set; }
        public Dictionary<string,VwKnittingRollIssueDetail> Dictionary { get; set; }
        public Dictionary<string,PROD_KnittingRollIssueDetail> RollIssueDetails { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public List<PLAN_Program> Programs { get; set; }
        public KnittingRollIssueViewModel()
        {
            KnittingRollIssues=new List<PROD_KnittingRollIssue>();
            Dictionary = new Dictionary<string, VwKnittingRollIssueDetail>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            Programs=new List<PLAN_Program>();
            KnittingRollIssue=new PROD_KnittingRollIssue();
            RollIssueDetails=new Dictionary<string, PROD_KnittingRollIssueDetail>();
        }

        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ProgramsSelectListItem
        {
            get
            {
                return new SelectList(Programs, "ProgramRefId", "ProgramRefId");
            }
        }
        public IEnumerable<SelectListItem> ChallanTypesSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Value = 1, Text = "Knitting Challan" }, new { Value = 2, Text = "Collar&Cuff Challan" }, new { Value = 3, Text = "Rejection Challan" } }, "Value", "Text");
            }
        }
    }
}