

namespace SCERP.Model.InventoryModel
{
   public partial  class Inventory_StockRegister
    {
        public long AdvanceStoreLadgerId { get; set; }
        public string CompId { get; set; }
        public int ItemId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public int TransactionType { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int StoreId { get; set; }
        public long SourceId { get; set; }
       public int ActionType { get; set; }
    }
}
