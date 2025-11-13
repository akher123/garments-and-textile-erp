using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class VAccVoucherMaster : VoucherList
    {
        public string SectorName { get; set; }
        public int? SectorId { get; set; }
        public int ActiveCurrencyId { get; set; }
    }
}
