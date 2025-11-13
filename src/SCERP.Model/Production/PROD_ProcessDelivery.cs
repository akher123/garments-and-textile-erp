using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.Model.Production
{
   public class PROD_ProcessDelivery
    {
        public PROD_ProcessDelivery()
        {
            this.PROD_ProcessDeliveryDetail = new HashSet<PROD_ProcessDeliveryDetail>();
        }

        public long ProcessDeliveryId { get; set; }
        public long PartyId { get; set; }
        public string RefNo { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ProcessRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string InvoiceNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime InvDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
     
        public string CompId { get; set; }
        public System.Guid PreparedBy { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public string Remarks { get; set; }
        public virtual Party Party { get; set; }

        public virtual ICollection<PROD_ProcessDeliveryDetail> PROD_ProcessDeliveryDetail { get; set; }
    }
}
