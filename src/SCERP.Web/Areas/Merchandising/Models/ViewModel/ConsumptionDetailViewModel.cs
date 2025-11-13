using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ConsumptionDetailViewModel : VConsumptionDetail
    {

        public string OrderStyleRefId { get; set; }
        public List<VConsumptionDetail> ConsumptionDetails { get; set; }
        public List<OM_ConsumptionType> ConsumptionTypes { get; set; }
        public IEnumerable GSizes { get; set; }
        public List<OM_Color> GColors { get; set; }
        public string ComponentName { get; set; }
        public ConsumptionDetailViewModel()
        {
            ConsumptionTypes = new List<OM_ConsumptionType>();
            ConsumptionDetails = new List<VConsumptionDetail>();
            GSizes = new List<OM_Size>();
            GColors = new List<OM_Color>();
        }
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
        public decimal? TotalConsQty { get { return ConsumptionDetails.Sum(x => x.TotalQty); } }
        public decimal? TotalQuantityP { get { return ConsumptionDetails.Sum(x => x.QuantityP); } }
    }
}