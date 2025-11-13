using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class SalesContactViewModel
    {
        public List<CommSalseContact> SalseContacts { get; set; }
        public CommSalseContact SalseContact { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public List<CommBank> Banks { get; set; }
        public SalesContactViewModel()
        {
            SalseContacts=new List<CommSalseContact>();
            SalseContact=new CommSalseContact();
            Buyers = new List<OM_Buyer>();
            Banks = new List<CommBank>();
        }

        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerId", "BuyerName").ToList(); }
        }

        public List<SelectListItem> BankSelectListItem
        {
            get { return new SelectList(Banks, "BankId", "BankName").ToList(); }
        }
    }
}