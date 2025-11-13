using System;
using System.ComponentModel.DataAnnotations;
namespace SCERP.Model.Production
{
    public class VwKnittingRoll
    {
        public long KnittingRollId { get; set; }
        public string RollRefNo { get; set; }
        [Required]
        public System.DateTime RollDate { get; set; }
        [Required]
        public long PartyId { get; set; }
        public long ProgramId { get; set; }
        [Required]
        public int MachineId { get; set; }
        public string SizeRefId { get; set; }
        public string ColorRefId { get; set; }
        public string FinishSizeRefId { get; set; }
        [Required]
        public string GSM { get; set; }
       // [Range(0, 200, ErrorMessage = @"Qty must be 0 to 200 kg")]
        public double Quantity { get; set; }

        public string Rmks { get; set; }
        [Required]
        public string CompId { get; set; }
        [Required]
        public string ItemCode { get; set; }
        public Nullable<double> RollLength { get; set; }
        [Required]
        public string CharllRollNo { get; set; }
        public string StLength { get; set; }
        [Required]
        public string ProgramRefId { get; set; }
        public string Buyer { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public string OrderStyleRefId { get; set; }
        public string SizeName { get; set; }
        public string PartyName { get; set; }
        public string ColorName { get; set; }
        public string FinishSizeName { get; set; }
        public string MachineName { get; set; }
        [Required]
        public string ItemName { get; set; }
        public bool? IsRejected { get; set; }
        public double? RejQuantity { get; set; }
        public string ComponentName { get; set; }
        public string ComponentRefId { get; set; }
    }
}
