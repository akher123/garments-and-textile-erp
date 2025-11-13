using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class EmbWorkOrderViewModel : ProSearchModel<EmbWorkOrderViewModel>
    {
        public List<OM_EmbWorkOrder> EmbWorkOrders { get; set; }
        public OM_EmbWorkOrder EmbWorkOrder { get; set; }
        public OM_EmbWorkOrderDetail EmbWorkOrderDetail { get; set; }
        public dynamic EmbWorkOrderDetails { get; set; }
        public List<PLAN_Process> Process { get; set; }
        public List<Party> Parties { get; set; }
        public IEnumerable<OM_Merchandiser> Merchandisers { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public List<OM_Component> Components { get; set; }
        public IEnumerable Colors { get; set; }
        public IEnumerable Sizes { get; set; }

        public EmbWorkOrderViewModel()
        {
            EmbWorkOrders = new List<OM_EmbWorkOrder>();
            EmbWorkOrder = new OM_EmbWorkOrder();
            Process = new List<PLAN_Process>();
            Parties=new List<Party>();
            Merchandisers=new List<OM_Merchandiser>();
            EmbWorkOrderDetail=new OM_EmbWorkOrderDetail();
            EmbWorkOrderDetails=new List<object>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            Components = new List<OM_Component>();
            Colors = new List<object>();
            Sizes = new List<object>();
        }


        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }
        }
        public IEnumerable<SelectListItem> MerchandiserSelectListItem
        {
            get
            {
                return new SelectList(Merchandisers, "EmpId", "EmpName");
            }
        }

        public IEnumerable<SelectListItem>ProcessSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "PRINTING", Value = "005" }, new { Text = "EMBROIDARY", Value = "006" }, new { Text = "AOP", Value = "011" } }, "Value", "Text");
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
        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get
            {
                return new SelectList(Sizes, "SizeRefId", "SizeName");
            }
        }
    }
}