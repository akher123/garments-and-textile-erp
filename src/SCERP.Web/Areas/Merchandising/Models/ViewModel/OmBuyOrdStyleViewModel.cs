using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmBuyOrdStyleViewModel:OM_BuyOrdStyle
    {
        public DataTable AssortTable { get; set; }
        public VBuyerOrder BuyerOrder { get; set; }
        public List<VOMBuyOrdStyle> OmBuyOrdStyles { get; set; }
        public List<OM_Style> OmStyles { get; set; }
        public List<OM_Brand> OmBrands { get; set; }
        public List<OM_Category> OmCategories { get; set; }
        public List<OM_Season> OmSeasons { get; set; }
        public List<OM_BuyOrdStyleColor> OmBuyOrdStyleColors { get; set; }
        public List<OM_BuyOrdStyleSize> OmBuyOrdStyleSizes { get; set; }
        public VOMBuyOrdStyle VomBuyOrdStyle { get; set; }
        public OmBuyOrdStyleViewModel()
        {
            VomBuyOrdStyle=new VOMBuyOrdStyle();
            AssortTable=new DataTable();
            BuyerOrder = new VBuyerOrder();
            OmBuyOrdStyles = new List<VOMBuyOrdStyle>();
            OmStyles=new List<OM_Style>();
            OmBrands=new List<OM_Brand>();
            OmCategories=new List<OM_Category>();
            OmSeasons=new List<OM_Season>();
            OmBuyOrdStyleColors=new List<OM_BuyOrdStyleColor>();
            OmBuyOrdStyleSizes=new List<OM_BuyOrdStyleSize>();
        }

        public IEnumerable<SelectListItem> StyleSelectListItem
        {
            get
            {
                return new SelectList(OmStyles, "StylerefId", "StyleName");
            }
        }

        public IEnumerable<SelectListItem> BrandSelectListItem
        {
            get
            {
                return new SelectList(OmBrands, "BrandRefId", "BrandName");
            }
        }

        public IEnumerable<SelectListItem> CategorieSelectListItem
        {
            get
            {
                return new SelectList(OmCategories, "CatRefId", "CatName");
            }
        }
        public IEnumerable<SelectListItem> SeasonsSelectListItem
        {
            get
            {
                return new SelectList(OmSeasons, "SeasonRefId", "SeasonName");
            }
        }


        public IEnumerable<SelectListItem> LcStatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Id = "1", Value = "Not Received" }, new { Id = "2", Value = "Received" } }, "Id", "Value");
            }
        }
    }
}