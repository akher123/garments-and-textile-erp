using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommImportDocsData
    {
        public int ImportDocsDataId { get; set; }
        public string BBLCNo { get; set; }
        public Nullable<System.DateTime> LCDate { get; set; }
        public Nullable<decimal> LcValue { get; set; }
        public Nullable<decimal> DocsValue { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCountry { get; set; }
        public string Item { get; set; }
        public Nullable<decimal> LCQty { get; set; }
        public Nullable<decimal> DocsQty { get; set; }
        public Nullable<System.DateTime> DocsReceiveDate { get; set; }
        public string IncomingPort { get; set; }
        public string DocsReceiverAgent { get; set; }
        public string DocsReceiverPerson { get; set; }
        public Nullable<System.DateTime> GoodsInHouseDate { get; set; }
    }
}
