using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.Production
{
    public partial class PROD_CuttingBatch
    {
        public PROD_CuttingBatch()
        {
            this.PROD_RollCutting=new HashSet<PROD_RollCutting>();
            this.PROD_PartCutting = new HashSet<PROD_PartCutting>();
            this.PROD_LayCutting = new HashSet<PROD_LayCutting>();
            this.PROD_BundleCutting = new HashSet<PROD_BundleCutting>();
            this.PROD_CuttingGradding = new HashSet<PROD_CuttingGradding>();
            this.PROD_RejectAdjustment = new HashSet<PROD_RejectAdjustment>();
            this.PROD_ProcessReceiveDetail = new HashSet<PROD_ProcessReceiveDetail>();
            this.PROD_RejectReplacement = new HashSet<PROD_RejectReplacement>();
        }
        public long CuttingBatchId { get; set; }
        public string CompId { get; set; }
        public string ApprovalStatus { get; set; }
        public string CuttingBatchRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public Nullable<System.DateTime> CuttingDate { get; set; }
        //[Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string BuyerRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string JobNo { get; set; }
        [Required]
        public string StyleRefId { get; set; }
        public string FIT { get; set; }
           //[Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string OrderNo { get; set; }
        public string Rmks { get; set; }
        public string CuttingStatus { get; set; }
        //[Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string OrderStyleRefId { get; set; }
        //[Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string ColorRefId { get; set; }
        //[Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string ComponentRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public int? MachineId { get; set; }
        public double? MarkerEffPct { get; set; }
        public double? ConsPerDzn { get; set; }

        public virtual ICollection<PROD_RollCutting> PROD_RollCutting { get; set; }
        public virtual ICollection<PROD_PartCutting> PROD_PartCutting { get; set; }
        public virtual ICollection<PROD_LayCutting> PROD_LayCutting { get; set; }
        public virtual ICollection<PROD_BundleCutting> PROD_BundleCutting { get; set; }
        public virtual ICollection<PROD_CuttingGradding> PROD_CuttingGradding { get; set; }
        public virtual ICollection<PROD_RejectAdjustment> PROD_RejectAdjustment { get; set; }
        public virtual ICollection<PROD_RejectReplacement> PROD_RejectReplacement { get; set; }
        
        public virtual ICollection<PROD_ProcessReceiveDetail> PROD_ProcessReceiveDetail { get; set; }
        
    }
}
