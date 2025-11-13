using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class CommTNAViewModel : CommTNA
    {

        public CommTNAViewModel()
        {

            CommTNA = new CommTNA();
            CommTNAs =new List<CommTNA>();
            CommSalesContacts = new List<CommSalseContact>();
            
        }

        public int RowNumber { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SalesContactSelectListItem
        {
            get { return new SelectList(CommSalesContacts, "SalseContactId", "LcNo"); }
        }

        public List<CommSalseContact> CommSalesContacts { get; set; }
        public CommTNA CommTNA { get; set; }
        public List<CommTNA> CommTNAs { get; set; }
    }
}