using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class VoucherList:SearchModel<VoucherList>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string VoucherType { get; set; }
        public long? VoucherNo { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
        public bool Reconciled { get; set; }
        public string VoucherRefNo { get; set; }

        [DataType(DataType.Date)]

        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
    }
}
