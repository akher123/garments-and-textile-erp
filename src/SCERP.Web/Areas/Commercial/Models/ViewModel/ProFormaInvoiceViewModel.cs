using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class ProFormaInvoiceViewModel:ProSearchModel<ProFormaInvoiceViewModel>
    {

        public List<ProFormaInvoice> ProFormaInvoices { get; set; }
        public ProFormaInvoice ProFormaInvoice { get; set; }
        public ProFormaInvoiceViewModel()
        {
            this.ProFormaInvoices = new List<ProFormaInvoice>();
            this.ProFormaInvoice = new ProFormaInvoice();
            this.SupplierCompanies = new List<Mrc_SupplierCompany>();
        }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get
            {
                return new SelectList(SupplierCompanies, "SupplierCompanyId", "CompanyName", ProFormaInvoice.SupplierId);
            }
        }
    }
}