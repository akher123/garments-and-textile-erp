using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.Model.Production
{
    public class PROD_ProcessReceive
    {
        public PROD_ProcessReceive()
        {
            this.PROD_ProcessReceiveDetail = new Collection<PROD_ProcessReceiveDetail>();
        }
        public long ProcessReceiveId { get; set; }
        public string RefNo { get; set; }
        public string CompId { get; set; }
        public string ProcessRefId { get; set; }
        public string InvoiceNo { get; set; }
        [Required(ErrorMessage = @"Required")]
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string GateEntryNo { get; set; }
        [Required(ErrorMessage = @"Required")]
        public Nullable<System.DateTime> GateEntrydate { get; set; }
        public Nullable<System.Guid> ReceivedBy { get; set; }
        [Required(ErrorMessage = @"Required")]
        public long PartyId { get; set; }
        public string Remarks { get; set; }

        public long? VoucherMasterId { get; set; }
        public string Posted { get; set; }
        public virtual Party Party { get; set; }

        public virtual ICollection<PROD_ProcessReceiveDetail> PROD_ProcessReceiveDetail { get; set; }
    }
}
