using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommImportDetails : ProSearchModel<CommImportDetails>
    {
        public int ImportDetailId { get; set; }
        public int ImportId { get; set; }
        public string CompId { get; set; }
        public string LCNo { get; set; }
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
        public Nullable<System.DateTime> DocsReceiveDateByCF { get; set; }
        public Nullable<System.DateTime> GoodsInHouseDate { get; set; }
        public string BillOfEntry { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public virtual CommImport CommImport { get; set; }
    }
}
