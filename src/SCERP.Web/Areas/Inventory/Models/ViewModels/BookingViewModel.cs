using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class BookingViewModel : ProSearchModel<BookingViewModel>
    {
        public string Key { get; set; }
        public IEnumerable StoreList { get; set; }
        public Inventory_Booking Booking { get; set; }
        public VwBookingDetail BookingDetail { get; set; }
        public List<Inventory_Booking>Bookings { get; set; }
        public List<OM_Buyer> OmBuyers { get; set; }
        public IEnumerable<OM_Merchandiser> OmMerchandisers { get; set; }
        public Dictionary<string, VwBookingDetail> BookingDetailsDictionary { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public BookingViewModel()
        {
            StoreList = new List<Object>();
            OmMerchandisers=new List<OM_Merchandiser>();
            OmBuyers = new List<OM_Buyer>();
            Booking=new Inventory_Booking();
            Bookings=new List<Inventory_Booking>();
            BookingDetail = new VwBookingDetail();
            BookingDetailsDictionary = new Dictionary<string, VwBookingDetail>();
            SupplierCompanies=new List<Mrc_SupplierCompany>();
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> MerchandisersSelectListItem
        {
            get
            {
                return new SelectList(OmMerchandisers, "MerchandiserId", "EmpName");
            }
        }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get
            {
                return new SelectList(SupplierCompanies, "SupplierCompanyId", "CompanyName");
            }
        }

        public IEnumerable<SelectListItem> StoreTypeSelectListItem
        {
            get
            {
                return new SelectList(StoreList, "StoreId", "Name");
            }

        }
    }
}