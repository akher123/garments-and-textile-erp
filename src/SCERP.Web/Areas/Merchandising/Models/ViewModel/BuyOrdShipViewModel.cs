using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class BuyOrdShipViewModel:OM_BuyOrdShip
    {
        public VOMBuyOrdStyle OrdStyle { get; set; }
        public List<OM_ItemMode> ItemModes { get; set; }
        public OM_BuyOrdShipDetail OrdShipDetail { get; set; }
        public List<OM_PortOfLoading> PrOmPortOfLoadings { get; set; }
        public List<VwBuyOrdShip> OmBuyOrdShips { get; set; }
        public List<VBuyOrdStyleColor> BuyOrdStyleColors { get; set; }
        public List<VBuyOrdStyleSize> BuyOrdStyleSizes { get; set; }
        public DataTable AssortTable { get; set; }
        public List<Country> Countries { get; set; }
        public BuyOrdShipViewModel()
        {
            OrdStyle=new VOMBuyOrdStyle();
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