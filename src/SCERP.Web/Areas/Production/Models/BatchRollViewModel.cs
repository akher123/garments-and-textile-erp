using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class BatchRollViewModel :VProBatch
    {
      
        public List<VProBatch> Batches { get; set; }
        public List<VwProdBatchDetail> BatchDetails { get; set; }
        public List<VwBatchRoll> BatchRolls { get; set; }
        public VwBatchRoll VwBatchRoll { get; set; }
        public VProBatch VProBatch { get; set; }
        public List<PROD_KnittingRollIssue> KnittingRollIssues { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public BatchRollViewModel()
        {
            Batches=new List<VProBatch>();
            BatchDetails=new List<VwProdBatchDetail>();
            BatchRolls = new List<VwBatchRoll>();
            VwBatchRoll = new VwBatchRoll();
            VProBatch=new VProBatch();
        
            KnittingRollIssues=new List<PROD_KnittingRollIssue>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
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
                return new SelectList(GetBatchStatusList(), "Value", "Text");
            }
        }

        private IEnumerable GetBatchStatusList()
        {
            var batchStatusList = (from BatchStatus batchStatus in Enum.GetValues(typeof(BatchStatus))
                                   select new { Value = (int)batchStatus, Text = batchStatus.ToString() }).ToList();
            return batchStatusList;
        }
        public IEnumerable<SelectListItem> KnittingRollIssuesSelectListItem
        {
            get
            {
                return new SelectList(KnittingRollIssues, "KnittingRollIssueId", "IssueRefNo");
            }

        }
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
    }
}