using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class SampleOrderViewModel:ProSearchModel<OM_SampleOrder>
    {
        public SampleOrderViewModel()
        {
            SampleOrders=new List<OM_SampleOrder>();
            SampleOrder=new OM_SampleOrder();
            Buyers = new List<OM_Buyer>();
            Merchandisers = new List<OM_Merchandiser>();
        }

       
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable<OM_Merchandiser> Merchandisers { get; set; }
        public List<OM_SampleOrder> SampleOrders { get; set; }
        public OM_SampleOrder SampleOrder { get; set; }

        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerId", "BuyerName").ToList(); }
        }

        public List<SelectListItem> MerchandiserSelectListItem
        {
            get { return new SelectList(Merchandisers, "MerchandiserId", "EmpName").ToList(); }
        }


       
    }
}