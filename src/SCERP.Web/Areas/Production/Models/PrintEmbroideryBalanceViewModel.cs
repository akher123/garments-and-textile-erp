using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class PrintEmbroideryBalanceViewModel : SpPrintEmbroiderySummary
    {
        public PrintEmbroideryBalanceViewModel()
        { 
            CuttingBatch=new PROD_CuttingBatch();
            PrintEmbroideryBalanceList = new List<SpPrintEmbroiderySummary>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            Colors = new List<object>();
        }
        public bool IsSearch { get; set; }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public List<SpPrintEmbroiderySummary> PrintEmbroideryBalanceList { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }
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
                return new SelectList(StyleList, "OrderStyleRefId", "StyleNo");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
    }
}