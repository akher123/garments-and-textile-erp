using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.X509.Qualified;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class BatchViewModel:VProBatch
    {
        public string Key { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string LoadingTime { get; set; }
        public string UnLoadingTime { get; set; }
        public List<Party> Parties { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public List<OM_Color> OmColors { get; set; }
        public List<OM_Size> OmSizes { get; set; }
        public List<Pro_Batch> Batches { get; set; }
        public List<VProBatch> VProBatches { get; set; }

        public BatchViewModel()
        {
            Batches=new List<Pro_Batch>();
            VProBatches=new List<VProBatch>();
            Parties=new List<Party>();
            Machines=new List<Production_Machine>();
            OmSizes=new List<OM_Size>();
            BatchDetailDictionary=new Dictionary<string, VwProdBatchDetail>();
            BuyerList = new List<OM_Buyer>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            ColorList = new List<object>();
            BatchDetail=new VwProdBatchDetail();
            Batch=new Pro_Batch();
            GroupList = new List<PROD_GroupSubProcess>();
            JobOrders = new List<Dropdown>();
        }
        public Pro_Batch Batch { get; set; }
        public VwProdBatchDetail BatchDetail { get; set; }
        public Dictionary<string, VwProdBatchDetail> BatchDetailDictionary { get; set; }
        public List<OM_Buyer> BuyerList { get; set; }
        public List<Dropdown> JobOrders { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable ColorList { get; set; }
        public List<PROD_GroupSubProcess> GroupList { get; set; }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }
        }
        public IEnumerable<SelectListItem> MachineSelectListItem
        {
            get { return new SelectList(Machines, "MachineId", "Name"); }
        }
        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get { return new SelectList(OmSizes, "SizeRefId", "SizeName"); }
        }
        public IEnumerable<SelectListItem> GroupSelectListItem
        {
            get { return new SelectList(GroupList, "GroupSubProcessId", "GroupName"); }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(BuyerList, "BuyerRefId", "BuyerName");
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
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ColorListSelectListItem
        {
            get { return new SelectList(ColorList, "ColorRefId", "ColorName"); }
        }
        public IEnumerable<SelectListItem> JobOrderListSelectListItem
        {
            get { return new SelectList(JobOrders, "Id", "Value"); }
        }

        public IEnumerable<SelectListItem> BatchTypeSelectListItem
        {
            get
            {
                return new SelectList(GetBatchTypeList(), "Value", "Text");
            }

        }
        private IEnumerable GetBatchTypeList()
        {
            var batchTypeList = (from BatchType s in Enum.GetValues(typeof(BatchType))
                                select new { Value = (int)s, Text = s.ToString() }).ToList();
            return batchTypeList;
        }

        public IEnumerable<SelectListItem> BatchStatuSelectListItem
        {
            get
            {
                return new SelectList(GetBatchStatusList(),"Value","Text");
            }
        }

        private IEnumerable GetBatchStatusList()
        {
            var batchStatusList = (from BatchStatus batchStatus in Enum.GetValues(typeof (BatchStatus))
                select new { Value = (int)batchStatus, Text = batchStatus.ToString()}).ToList();
            return batchStatusList;
        }
    }
}