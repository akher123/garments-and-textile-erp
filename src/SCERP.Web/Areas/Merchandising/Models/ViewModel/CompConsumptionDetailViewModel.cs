using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class CompConsumptionDetailViewModel : VCompConsumptionDetail
    {
        public List<VCompConsumptionDetail> CompConsumptionDetails { get; set; }
        public string UpdateKey { get; set; }
        public List<OM_ConsumptionType> ConsumptionTypes { get; set; }
        public IEnumerable GSizes { get; set; }
        public List<OM_Color> GColors { get; set; }
        public CompConsumptionDetailViewModel()
        {
            ConsumptionTypes = new List<OM_ConsumptionType>();
            GSizes = new List<OM_Size>();
            GColors = new List<OM_Color>();
            CompConsumptionDetails = new List<VCompConsumptionDetail>();
        }

        public bool IsCollar { get; set; }
    
        public VCompConsumption CompConsumption { get; set; }
        public IEnumerable<SelectListItem> ConsTypeSelectListItem
        {
            get
            {
                return new SelectList(ConsumptionTypes, "ConsTypeRefId", "ConsTypeName");
            }
        }
        public IEnumerable<SelectListItem> GColorSelectListItem
        {
            get
            {
                return new SelectList(GColors, "ColorRefId", "ColorName");
            }
        }
        public IEnumerable<SelectListItem> GSizeSelectListItem
        {
            get
            {
                return new SelectList(GSizes, "SizeRefId", "SizeName");
            }
        }
    }
}