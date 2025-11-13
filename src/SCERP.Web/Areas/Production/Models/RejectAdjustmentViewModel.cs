using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class RejectAdjustmentViewModel : SearchModel<RejectAdjustmentViewModel>
    {
        public RejectAdjustmentViewModel()
        {
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            Colors = new List<object>();
       
            Components = new List<OM_Component>();
            RejectAdjustmentDictionary=new Dictionary<string, List<string>>();
            RejectAdjustments=new Dictionary<string, PROD_RejectAdjustment>();
           CuttingBatches=new List<PROD_CuttingBatch>();
        }


   
        public Dictionary<string, List<string>> RejectAdjustmentDictionary { get; set; }
        public Dictionary<string,PROD_RejectAdjustment> RejectAdjustments { get; set; }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public OM_Component Component { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable Colors { get; set; }
        public List<VwCuttingSequence> VwCuttingSequences { get; set; }
        public List<PROD_CuttingBatch> CuttingBatches { get; set; }

        public IEnumerable Components { get; set; }
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
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }

        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }
        public IEnumerable<SelectListItem> JobNOSelectListItem
        {
            get
            {
                return new SelectList(CuttingBatches, "CuttingBatchId", "JobNo");
            }
        }
    }
}