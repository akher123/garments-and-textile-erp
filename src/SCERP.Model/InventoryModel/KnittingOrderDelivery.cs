using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class KnittingOrderDelivery
    {
        public string ProgramRefId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ColorName { get; set; }
        public string ColorRefId { get; set; }
        public string SizeName { get; set; }
        public string SizeRefId { get; set; }
        public string FSizeName { get; set; }
        public string FinishSizeRefId { get; set; }
        public string GSM { get; set; }
        public string StLength { get; set; }
        public double ProductionQty { get; set; }
        public double IQty { get; set; }
        public decimal OrdQty { get; set; }
        public double Qty { get; set; }
        public int TRollQty { get; set; }
        public int RollQty { get; set; }
        public int IRollQty { get; set; }
        public double BalanceQty { get { return  Math.Round(IQty, 2)-Convert.ToDouble(Math.Round(OrdQty, 2)) ; } }
        public double SIH { get { return Math.Round(IQty, 2)- Convert.ToDouble(Math.Round(ProductionQty, 2)) ; } }

        public double RollBalance { get { return TRollQty - IRollQty; } }
    }
}
