using System;
using System.Collections.Generic;
namespace SCERP.Model.Production
{
    public partial class VwSewingInputProcess
    {
        public long SewingInputProcessId { get; set; }
        public string SewingInputProcessRefId { get; set; }
        public int InputQuantity { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public int LineId { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<System.DateTime> OnlyDate { get; set; }
        public string CompId { get; set; }
        public System.Guid PreparedBy { get; set; }
        public string Remarks { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string MachineName { get; set; }
        public int? HourId { get; set; }
        public string HourName { get; set; }
        public string BatchNo { get; set; }
        public string JobNo { get; set; }
        public string OrderShipRefId { get; set; }
        public bool? Locked { get; set; }
        public System.Guid? LockedBy { get; set; }
    }
}
