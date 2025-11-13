using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class FabConsumptionViewModel:OM_CompConsumption
    {
        public string ButtonName
        {
            get
            {
               return (CompConsumptionId > 0) ? "Update" : "Add";
            }
        }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SearchItemKey { get; set; }
        public List<OM_Component> Components { get; set; }
        public List<VConsumption> Consumptions { get; set; }
        public List<VwCompConsumptionOrderStyle> OmBuyOrdStyles { get; set; }
        public List<OM_FabricType> FabricTypes { get; set; }
        public List<VCompConsumption> CompConsumptions { get; set; }
        public VOMBuyOrdStyle BuyOrdStyle { get; set; }
        public FabConsumptionViewModel()
        {
            BuyOrdStyle=new VOMBuyOrdStyle();
            CompConsumptions=new List<VCompConsumption>();
            Components = new List<OM_Component>();
            Consumptions = new List<VConsumption>();
            OmBuyOrdStyles = new List<VwCompConsumptionOrderStyle>();
            FabricTypes = new List<OM_FabricType>();
        }


        public IEnumerable<SelectListItem> CompSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }

        public IEnumerable<SelectListItem> FabTypeSelectListItem
        {
            get
            {
                return new SelectList(FabricTypes, "FabricType", "Description");
            }
        }
        public IEnumerable<SelectListItem> ConsumptionFabSelectListItem
        {
            get
            {
                return new SelectList(Consumptions, "ConsRefId", "ItemName");
            }
        }
    }
}