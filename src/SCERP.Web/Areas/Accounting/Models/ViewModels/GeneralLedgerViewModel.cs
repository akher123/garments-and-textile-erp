using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public partial class GeneralLedgerViewModel 
    {
        public List<GeneralLedgerViewModel> GeneralLedger { get; set; }
        public int TotalRecords { get; set; }
        public int GlId { get; set; }
        public string Particulars { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Balance { get; set; }
        public string VoucherNoShow { get; set; }
        public string VoucherNoMasterId { get; set; }
        public string VoucherRefNo { get; set; }
        public string VoucherDateShow { get; set; }
        public string CompanyName { get; set; }
        public string SectorName { get; set; }
        public string GLHeadName { get; set; }
        public string GLHeadNameNew { get; set; }
        public string DateBetween { get; set; }
        public int sectorId { get; set; }
    }
}