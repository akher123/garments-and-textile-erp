using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class BulkBookingViewModel : SearchModel<BulkBookingViewModel>
    {
        public List<OM_BulkBooking> BulkBookings { get; set; }
        public OM_BulkBooking BulkBooking { get; set; }
        public IEnumerable<OM_Merchandiser> OmMerchandisers { get; set; }
        public List<OM_BulkBookingDetail> BulkBookingDetails { get; set; }
        public OM_BulkBookingDetail BulkBookingDetail { get; set; }
        public OM_BulkBookingYarnDetail BulkBookingYarnDetail { get; set; }
        public List<OM_BulkBookingYarnDetail> BulkBookingYarnDetails { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public string SerarchString { get; set; }
        public BulkBookingViewModel()
        {
            BulkBookings=new List<OM_BulkBooking>();
            BulkBooking=new OM_BulkBooking();
            OmMerchandisers = new List<OM_Merchandiser>();
            BulkBookingDetails=new List<OM_BulkBookingDetail>();
            BulkBookingDetail=new OM_BulkBookingDetail();
            BulkBookingYarnDetail=new OM_BulkBookingYarnDetail();
            BulkBookingYarnDetails=new List<OM_BulkBookingYarnDetail>();
            Buyers=new List<OM_Buyer>();
        }
        public IEnumerable<SelectListItem> MerchandisersSelectListItem
        {
            get
            {
                return new SelectList(OmMerchandisers, "MerchandiserId", "EmpName");
            }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }

    }
}