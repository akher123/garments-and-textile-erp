using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{


    public class CostOrdStyleViewModel : VCostOrderStyle
    {
        public string ButtinName { get; set; }
        public List<OM_CostDefination> CostDefinations { get; set; }
        public List<VCostOrderStyle> VCostOrderStyles { get; set; }
        public List<VOMBuyOrdStyle> OmBuyOrdStyles { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public CostOrdStyleViewModel()
        {
            CostDefinations=new List<OM_CostDefination>();
            OmBuyOrdStyles=new List<VOMBuyOrdStyle>();
            VCostOrderStyles=new List<VCostOrderStyle>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
        }
        public IEnumerable<SelectListItem> ConsDefinationSeasonsSelectListItem
        {
            get
            {
                return new SelectList(CostDefinations, "CostRefId", "CostName");
            }
        }
        private List<Dropdown> CostGroups
        {
            get
            {
                return new List<Dropdown>()
                {
                    new Dropdown() {Id = "FAB", Value = "FABRIC"},
                    new Dropdown() {Id = "ACC", Value = "ACCESSORIES"},
                    new Dropdown() {Id = "EMB", Value = "EMBELLISHMENT"},
                    new Dropdown() {Id = "OTC", Value = "OTHER COST"},
                };
            }
        }
        public IEnumerable<SelectListItem> CostGroupsSelectListItem
        {

            get
            {

                return new SelectList(CostGroups, "Id", "Value");
            }
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
    }
}