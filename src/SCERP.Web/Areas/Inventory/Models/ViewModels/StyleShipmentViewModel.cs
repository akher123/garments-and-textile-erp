using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class StyleShipmentViewModel : SearchModel<StyleShipmentViewModel>
    {
        public string searchKey { get; set; }
        public StyleShipmentViewModel()
        {
            StyleShipment=new Inventory_StyleShipment();
            StyleShipments = new List<VwInventoryStyleShipment>();
            Dictionary = new Dictionary<string, Dictionary<string, SpInventoryStyleShipment>>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            DictionaryList=new Dictionary<string, Dictionary<string, Dictionary<string, SpInventoryStyleShipment>>>();
        }
        public Inventory_StyleShipment StyleShipment { get; set; }
        public List<VwInventoryStyleShipment> StyleShipments { get; set; }
        public Dictionary<string,Dictionary<string, Dictionary<string, SpInventoryStyleShipment>>> DictionaryList { get; set; }
        public Dictionary<string, Dictionary<string, SpInventoryStyleShipment>> Dictionary { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
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
        public IEnumerable<SelectListItem> StyleModeSelectListItem
        {
            get
            {
                return new SelectList(new[]{"SEA","AIR"});
            }
        }
    }
}