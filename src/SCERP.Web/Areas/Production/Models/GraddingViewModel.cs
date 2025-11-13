using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class GraddingViewModel : SearchModel<GraddingViewModel>
    {
        public GraddingViewModel()
        {
            CuttingBatch=new VwCuttingBatch();
            CuttingGraddings=new List<PROD_CuttingGradding>();
            CuttingBatches=new List<VwCuttingBatch>();
            Sizes = new List<VBuyOrdStyleSize>();
            Colors = new List<OM_Color>();
            Buyers=new List<OM_Buyer>();
            StyleList = new List<object>();
            OrderList = new List<object>();
            Gradding=new PROD_CuttingGradding();
            Graddings = new List<VwCuttingGraddding>();
            Components = new List<OM_Component>();
            CuttingBatchList=new List<PROD_CuttingBatch>();
        }

        public List<PROD_CuttingBatch> CuttingBatchList { get; set; }
        public IEnumerable Components { get; set; }
        public long CuttingBatchId { get; set; }
        public List<VwCuttingGraddding> Graddings { get; set; }
        public PROD_CuttingGradding Gradding { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public List<VBuyOrdStyleSize> Sizes { get; set; }
        public IEnumerable Colors { get; set; }
        public VwCuttingBatch CuttingBatch { get; set; }
        public List<PROD_CuttingGradding> CuttingGraddings { get; set; }
        public List<VwCuttingBatch> CuttingBatches { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable OrderList { get; set; }
 
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get
            {
                return new SelectList(Sizes, "SizeRefId", "SizeName");
            }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }

        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
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
                return new SelectList(CuttingBatchList, "CuttingBatchId", "JobNo");
            }
        }
    }
}