using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class ReceiveAgainstPoViewModel : ProSearchModel<ReceiveAgainstPoViewModel>
    {
        public Inventory_MaterialReceiveAgainstPo ReceiveAgainstPo { get; set; }
        public VwMaterialReceiveAgainstPoDetail PoDetail { get; set; }
        public List<Inventory_MaterialReceiveAgainstPo> ReceiveAgainstPos { get; set; }
        public Dictionary<string,VwMaterialReceiveAgainstPoDetail> Dictionary { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public IEnumerable StoreList { get; set; }
        public string Key { get; set; }
        public string RType { get; set; }
        public string PiBookingRefId { get; set; }
        public List<OM_Buyer> OmBuyers { get; set; }
        public IEnumerable PiBookings { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable OrderList { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public ReceiveAgainstPoViewModel()
        {
            OmBuyers = new List<OM_Buyer>();
            ReceiveAgainstPos=new List<Inventory_MaterialReceiveAgainstPo>();
            ReceiveAgainstPo = new Inventory_MaterialReceiveAgainstPo();
            PoDetail = new VwMaterialReceiveAgainstPoDetail();
            Dictionary = new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
            SupplierCompanies = new List<Mrc_SupplierCompany>();
            StoreList = new List<object>();
            PiBookings = new List<object>();
            StyleList = new List<object>();
            OrderList = new List<object>();
        }

        public IEnumerable<SelectListItem> RTypeSelectListItem
        {
            get
            {
                return new SelectList(new[]
                {
                  //  new { RType = SCERP.Common.RType.PI, Text = "PI" },
                    new { RType = SCERP.Common.RType.BOOKING, Text = "Booking" }, 
                    new { RType = SCERP.Common.RType.YARNDYED, Text = "DYEING YARN" },
                    new { RType = SCERP.Common.RType.COLLAR_CUTT_PROGRAMWISEYARNRETURN, Text = "COLLAR CUTT PROGRAM WISE YARN RETURN" },
                    new { RType = SCERP.Common.RType.KNITTING_PROGRAMWISEYARNRETURN, Text = "KNITTING PROGRAM WISE YARN RETURN" },
                    new { RType = SCERP.Common.RType.RECEIVEWITHOUTBOOKIN, Text = "N/A" }
                }, "RType", "Text");
            }
        }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get
            {
                return new SelectList(SupplierCompanies, "SupplierCompanyId", "CompanyName");
            }
        }

        public IEnumerable<SelectListItem> StoreTypeSelectListItem
        {
            get
            {
                return new SelectList(StoreList, "StoreId", "Name");
            }

        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> BuyerRefSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
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
        public IEnumerable<SelectListItem> PiBookingSelectListItem
        {
            get
            {
                return new SelectList(PiBookings, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> PiNoSelectListItem
        {
            get
            {
                return new SelectList(PiBookings, "PiRefId", "PiNo");
            }
        }
    }
}