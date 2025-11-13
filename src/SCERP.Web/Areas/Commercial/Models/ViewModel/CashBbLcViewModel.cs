using SCERP.Model;
using SCERP.Model.CommercialModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class CashBbLcViewModel : CommCashBbLcInfo
    {
        public CashBbLcViewModel()
        {
            CashBbLcInfos = new List<CommCashBbLcInfo>();
            CashBbLcInfo = new CommCashBbLcInfo();
            CashBbLcDetail = new CommCashBbLcDetail();
            CommCashBbLcDetailsDct = new Dictionary<string, CommCashBbLcDetail>();
            Banks = new List<CommBank>();
            CommCashBbLcDetails = new List<CommCashBbLcDetail>();
            OmPaymentTerms = new List<OM_PaymentTerm>();
            IsSearch = true;
           
        }

        public string Key { get; set; }

        public string PaymentModeRefId { get; set; }

        public Dictionary<string, CommCashBbLcDetail> CommCashBbLcDetailsDct { get; set; }

        public IEnumerable<OM_PaymentTerm> OmPaymentTerms { get; set; }
        public CommCashBbLcInfo CashBbLcInfo { get; set; }

        public CommCashBbLcDetail CashBbLcDetail { get; set; }
        public List<CommBank> Banks { get; set; }

        public List<CommCashBbLcDetail> CommCashBbLcDetails { get; set; }

        public IEnumerable Lcs { get; set; }
        public IEnumerable LCTypeName { get; set; }

        public List<SelectListItem> BankSelectListItem
        {
            get { return new SelectList(Banks, "BankId", "BankName").ToList(); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> LcSelectListItem
        {
            get { return new SelectList(Lcs, "LcId", "LcNo"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> LcTypeNameSelectListItem
        {
            get { return new SelectList(LCTypeName, "LcTypeName", "LCType"); }
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

        public IEnumerable<SelectListItem> PayTermSelectListItem
        {
            get
            {
                return new SelectList(OmPaymentTerms, "PayTermRefId", "PayTerm");
            }
        }

        public IEnumerable PartialShip { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PartialShipmentSelectListItem
        {
            get { return new SelectList(PartialShip, "Id", "Name"); }
        }

        public List<CommCashBbLcInfo> CashBbLcInfos { get; set; }
    }
}