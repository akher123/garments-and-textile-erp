using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{ 
    public partial class PROD_RollCutting
    {
        public long RollCuttingId { get; set; }
        public string CompId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string RollNo { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string ColorRefId { get; set; }
        public string BatchNo { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> RollSart { get; set; }
        public Nullable<int> RollEnd { get; set; }
        public string Combo { get; set; }
        public string RollName { get; set; }
        public  virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
    }
}
