using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_ProcessReceiveDetail
    {
      
        public long ProcessReceiveDetailId { get; set; }
        public long ProcessReceiveId { get; set; }
          
        public long CuttingBatchId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public long CuttingTagId { get; set; }
        [Required(ErrorMessage = @"Required")]
        public int ReceivedQty { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = @"Required")]
        public int InvocieQty { get; set; }
        public int FabricReject { get; set; }
        public int ProcessReject { get; set; }
        public virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
        public virtual PROD_CuttingTag PROD_CuttingTag { get; set; }
        public virtual PROD_ProcessReceive PROD_ProcessReceive { get; set; }
      
    }
}
