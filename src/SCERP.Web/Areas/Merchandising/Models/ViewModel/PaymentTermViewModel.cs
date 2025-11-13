using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class PaymentTermViewModel : OM_PaymentTerm
    {
        public List<OM_PaymentTerm> OmPaymentTerms { get; set; }
     
        public PaymentTermViewModel()
        {
           
            OmPaymentTerms=new List<OM_PaymentTerm>();
        }
    }
}