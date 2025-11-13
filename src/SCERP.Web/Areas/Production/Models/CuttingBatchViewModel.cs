using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class CuttingBatchViewModel : ProSearchModel<CuttingBatchViewModel>
    {
       
        public CuttingBatchViewModel()
        {
            CuttingBatch=new PROD_CuttingBatch();
            RollCuttings = new List<VwRollCutting>();
            LayCuttings = new List<VwLayCutting>();
            LayCutting = new PROD_LayCutting();
            CuttingBatchList = new List<VwCuttingBatch>();
            CuttingBatches = new List<PROD_CuttingBatch>();
            CuttingBatch=new PROD_CuttingBatch();
             RollCutting = new VwRollCutting();
            LayCutting=new PROD_LayCutting();
            BundleCutting=new PROD_BundleCutting();
            Buyers = new List<OM_Buyer>();
        
            Sizes=new List<OM_Size>();
            Colors=new List<OM_Color>();
            Color=new OM_Color();
            Component=new OM_Component();
            Components=new List<OM_Component>();
            BundleCuttings=new List<VwBundleCutting>();
            PartCutting=new PROD_PartCutting();
            PartCuttings = new List<VwPartCutting>();
            Machines = new List<Production_Machine>();
            SpCuttingJobCards=new Dictionary<string, List<string>>();
            RatioDictionary = new Dictionary<string, PROD_LayCutting>();
            VwRollCuttingDictionary=new Dictionary<string, VwRollCutting>();
            StyleList = new List<object>();
            OrderList = new List<object>();
            VwCuttingApprovalList = new List<VwCuttingApproval>();
            OrderShips=  new List<object>();
          
        }
        public int? TotalBody { get; set; }
        public int TotalPly { get { return CuttingBatchList.Sum(x => x.PLY); }}
        public int? RowNo { get; set; }
        public List<VwCuttingApproval> VwCuttingApprovalList { get; set; }
        public Dictionary<string, PROD_LayCutting> RatioDictionary { get; set; }
        public Dictionary<string, List<string>> SpCuttingJobCards { get; set; }
        public Dictionary<string,VwRollCutting> VwRollCuttingDictionary { get; set; }
        public Dictionary<string, PROD_CuttingGradding> VwCuttingGraddings { get; set; }
        public IEnumerable OrderShips { get; set; }
        public string OrderStyleRefId { get; set; }
     
        public VwRollCutting RollCutting { get; set; }
        public List<VwRollCutting> RollCuttings { get; set; } 
        public List<VwLayCutting> LayCuttings { get; set; }
        public List<VwBundleCutting> BundleCuttings { get; set; }
        public PROD_LayCutting LayCutting { get; set; }
        public List<VwCuttingBatch> CuttingBatchList { get; set; }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public List<PROD_CuttingBatch> CuttingBatches { get; set; }
        public PROD_BundleCutting BundleCutting { get; set; }
        public IEnumerable Buyers { get; set; }
        public List<OM_Size> Sizes { get; set; }
        public IEnumerable Colors { get; set; }
        public OM_Color Color { get; set; }
        public IEnumerable Components { get; set; }
        public OM_Component Component { get; set; }
        public List<VwPartCutting> PartCuttings { get; set; }
        public PROD_PartCutting PartCutting { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable OrderList { get; set; }
        public string ApprovalStatus { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }

        public IEnumerable<SelectListItem> OrderShipSelectListItem
        {
            get
            {
                return new SelectList(OrderShips, "OrderShipRefId", "CountryName");
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
        public IEnumerable<SelectListItem> MachineSelectListItem
        {
            get
            {
                return new SelectList(Machines, "MachineId", "Name");
            }
        }
        public IEnumerable<SelectListItem> ApprovalStatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "Pending", Value = "P" }, new { Text = "Approved", Value = "A" } },  "Value","Text");
            }
        }
    }
}