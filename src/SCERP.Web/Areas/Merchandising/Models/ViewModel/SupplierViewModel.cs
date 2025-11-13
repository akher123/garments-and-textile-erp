using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class SupplierViewModel:Mrc_SupplierCompany
    {
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }

        public SupplierViewModel()
        {
            SupplierCompanies=new List<Mrc_SupplierCompany>();
        }
    }
}