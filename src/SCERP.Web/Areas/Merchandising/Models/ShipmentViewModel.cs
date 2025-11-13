using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models
{
    public class ShipmentViewModel : ProSearchModel<ShipmentViewModel>
    {
        public OM_BuyOrdShip BuyOrdShip { get; set; }
        public string OrderShipRefId { get; set; }
        public int OrderShipId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public List<VOMBuyOrdStyle> BuyOrdStyles { get; set; }
        public IEnumerable<VBuyerOrder> BuyerOrders { get; set; }
         public List<OM_ItemMode> ItemModes { get; set; }
        public OM_BuyOrdShipDetail OrdShipDetail { get; set; }
        public List<OM_PortOfLoading> PrOmPortOfLoadings { get; set; }
        public List<VwBuyOrdShip> OmBuyOrdShips { get; set; }
        public List<VBuyOrdStyleColor> BuyOrdStyleColors { get; set; }
        public List<VBuyOrdStyleSize> BuyOrdStyleSizes { get; set; }
        public DataTable AssortTable { get; set; }
        public List<Country> Countries { get; set; }
        public ShipmentViewModel()
        {
            BuyOrdStyles=new List<VOMBuyOrdStyle>();
            BuyerOrders=new List<VBuyerOrder>();
            ItemModes=new List<OM_ItemMode>();
            PrOmPortOfLoadings=new List<OM_PortOfLoading>();
            OrdShipDetail=new OM_BuyOrdShipDetail();
            BuyOrdStyleSizes = new List<VBuyOrdStyleSize>();
            AssortTable=new DataTable();
            OmBuyOrdShips=new List<VwBuyOrdShip>();
            BuyOrdStyleColors = new List<VBuyOrdStyleColor>();
            Countries=new List<Country>();
        }
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
        public IEnumerable<SelectListItem> ModeRefIdSelectListItem
        {
            get
            {
                return new SelectList(ItemModes, "IModeRefId", "IModeName");
            }
        }

        public IEnumerable<SelectListItem> PortOfLoadingSelectListItem
        {
            get
            {
                return new SelectList(PrOmPortOfLoadings, "PortOfLoadingRefId", "PortOfLoadingName");
            }
        }

        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(BuyOrdStyleColors, "ColorRefId", "ColorName");
            }
        }

        public IEnumerable<SelectListItem> StyleSelectListItem
        {
            get
            {
                return new SelectList(BuyOrdStyleSizes, "SizeRefId", "SizeName");
            }
        }
    }
}