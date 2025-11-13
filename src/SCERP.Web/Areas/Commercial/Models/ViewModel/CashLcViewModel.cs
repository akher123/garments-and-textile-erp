using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class CashLcViewModel : CommCashLc
    {
        public CashLcViewModel()
        {
            CommCashLcs = new List<CommCashLc>();
            CommCashLc = new CommCashLc();
            Banks = new List<CommBank>();
            PrintFormatStatuses = new List<PrintFormatType>();
        }

        public List<CommCashLc> CommCashLcs { get; set; }
        public CommCashLc CommCashLc { get; set; }
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

        public List<Mrc_SupplierCompany> Suppliers { get; set; }
        public List<SelectListItem> SupplierSelectListItem
        {
            get { return new SelectList(Suppliers, "SupplierCompanyId", "CompanyName").ToList(); }
        }

        public IEnumerable LcTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> LcTypeSelectListItem
        {
            get { return new SelectList(LcTypes, "Id", "Name"); }
        }

        public IEnumerable Lcs { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> LcSelectListItem
        {
            get { return new SelectList(Lcs, "LcId", "LcNo"); }
        }

        public IEnumerable PrintFormatStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PrintFormatStatusSelectListItems
        {
            get { return new SelectList(PrintFormatStatuses, "Id", "Name"); }
        }

        public int PrintFormatId { get; set; }
    }
}