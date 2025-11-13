using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ConsumptionViewModel:OM_Consumption
    {
        public bool IsShowReport { get; set; }
        public string ButtonName { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SearchItemKey { get; set; }
        public List<OM_ConsumptionGroup> ConsumptionGroups { get; set; }
        public List<VConsumption> Consumptions { get; set; }
        public List<VwConsuptionOrderStyle> OmBuyOrdStyles { get; set; }
        public List<OM_ConsumptionType> ConsumptionTypes { get; set; }
        public DataTable ConsumptionDetailDataTable { get; set; }
        public VOMBuyOrdStyle BuyOrdStyle { get; set; }
        public ConsumptionViewModel()
        {
            ConsumptionGroups=new List<OM_ConsumptionGroup>();
            Consumptions = new List<VConsumption>();
            OmBuyOrdStyles = new List<VwConsuptionOrderStyle>();
            ConsumptionTypes=new List<OM_ConsumptionType>();
            ConsumptionDetailDataTable = new DataTable();
            BuyOrdStyle=new VOMBuyOrdStyle();
        }


        public IEnumerable<SelectListItem> ConsGroupSeasonsSelectListItem
        {
            get
            {
                return new SelectList(ConsumptionGroups, "ConsGroup", "ConsGroupName");
            }
        }

        public IEnumerable<SelectListItem> ConsTypeSelectListItem
        {
            get
            {
                return new SelectList(ConsumptionTypes, "ConsTypeRefId", "ConsTypeName");
            }
        }

    }
}