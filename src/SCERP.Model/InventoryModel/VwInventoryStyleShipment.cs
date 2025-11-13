using System;
using System.Collections.Generic;

namespace SCERP.Model.InventoryModel
{
   
    
    public partial class VwInventoryStyleShipment
    {
        public string BuyerName { get; set; }
        public string RefNo { get; set; }
        public string StyleName { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string DepoName { get; set; }
        public string Through { get; set; }
        public string ThroughCellNo { get; set; }
        public string DriverName { get; set; }
        public string DriverCellNo { get; set; }
        public string ShipmentMode { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public bool IsApproved { get; set; }
        public long StyleShipmentId { get; set; }
        public string Remarks { get; set; }
        public string OrderStyleRefId { get; set; }
        public string StyleShipmentRefId { get; set; }
        public DateTime? ShipDate { get; set; }
    }
}
