using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class BuyerOrderColorSizeViewModel
    {
        public OM_Size OmSize { get; set; }
        public VOMBuyOrdStyle OrdStyle { get; set; }
        public VBuyerOrder BuyerOrder { get; set; }
        public OM_BuyOrdStyleColor Color { get; set; }
        public OM_BuyOrdStyleSize Size { get; set; }
        public List<VBuyOrdStyleColor> BuyOrdStyleColors { get; set; }
        public List<VBuyOrdStyleSize> BuyOrdStyleSizes { get; set; }
        public OM_Color OmColor { get; set; }
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string SizeSearchSetring { get; set; }
          [Required(ErrorMessage = @"Required")]
        public string ColorSearchSetring { get; set; }
        public string ButtonName { get; set; }
        public BuyerOrderColorSizeViewModel()
        {
            OmSize=new OM_Size();
            OmColor=new OM_Color();
            BuyerOrder = new VBuyerOrder();
            OrdStyle = new VOMBuyOrdStyle();
            BuyOrdStyleColors = new List<VBuyOrdStyleColor>();
            BuyOrdStyleSizes = new List<VBuyOrdStyleSize>();
            Color=new OM_BuyOrdStyleColor();
            Size=new OM_BuyOrdStyleSize();
        }
    }
}