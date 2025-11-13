using System;

namespace SCERP.Model.MerchandisingModel
{
    public class OM_Costing 
    {
        public int CostingId { get; set; }
        public string BuyerName { get; set; }
        public string BuyingHouse { get; set; }
        public string ItemName { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Fabrication { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ShipDate { get; set; }
        public double? Yarn { get; set; }
        public double? Knitting { get; set; }
        public double? Dyeing { get; set; }
        public double? Aop { get; set; }
        public double? PrintCost { get; set; }
        public double? Embroidery { get; set; }
        public double? TrimsAccss { get; set; }
        public double? Cm { get; set; }
        public double? CommercialCharge { get; set; }
        public double? Others { get; set; }
        public double? ProcessLoss { get; set; }
        public double? ConsDzn { get; set; }
        public double? PriceDzn { get; set; }
        public double? Total { get; set; }
        public double? CostPerPcs { get; set; }
        public double? Target { get; set; }
        public string Remarks { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }

    }
}
