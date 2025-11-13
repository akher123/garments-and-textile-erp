using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
    
    
    public partial class VwSewingInputProcessDetail
    {
      //  public long SewingInputProcessId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
     //   public int LineId { get; set; }
      //  public Nullable<System.DateTime> InputDate { get; set; }
        public int InputQuantity { get; set; }
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<int> BankQty { get; set; }
        public int TotalInput { get; set; }
        public int SizeRow { get; set; }
    }
}
