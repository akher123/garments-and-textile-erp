using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class OpeningBalanceViewModel : Acc_OpeningClosing
    {
        public OpeningBalanceViewModel()
        {
            CompanySectors = new List<Acc_CompanySector>();
        }

        public List<Acc_OpeningClosing> OpeningBalances { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Balance { get; set; }


        public string AccountHead { get; set; }
        public List<Acc_CompanySector> CompanySectors { get; set; }

        public IEnumerable<SelectListItem> CompanySectorSelectListItem
        {
            get { return new SelectList(CompanySectors, "Id", "SectorName"); }
        }
    }
}