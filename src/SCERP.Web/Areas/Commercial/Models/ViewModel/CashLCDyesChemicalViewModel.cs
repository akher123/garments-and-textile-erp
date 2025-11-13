using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class CashLCDyesChemicalViewModel : CommCashLCDyesChemical
    {
        public CashLCDyesChemicalViewModel()
        {
            CommCashLcs = new List<CommCashLCDyesChemical>();
            CommCashLc = new CommCashLCDyesChemical();
            Banks = new List<CommBank>();
        }

        public List<CommCashLCDyesChemical> CommCashLcs { get; set; }
        public CommCashLCDyesChemical CommCashLc { get; set; }
        public List<CommBank> Banks { get; set; }
        public List<SelectListItem> BankSelectListItem
        {
            get { return new SelectList(Banks, "BankId", "BankName").ToList(); }
        }

        public IEnumerable<SelectListItem> ShipModeSelectListItem
        {
            get
            {
                return new SelectList(new[] { "SEA", "AIR" });
            }
        }
    }
}
