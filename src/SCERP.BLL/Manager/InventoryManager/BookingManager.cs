using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public static class RefPrifix
    {
        public const string AccessoriesBooking = "AB";
    }

    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDetailRepository _bookingDetailRepository;
       
        public BookingManager(IBookingRepository bookingRepository, IBookingDetailRepository bookingDetailRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingDetailRepository = bookingDetailRepository;
        }
        public List<Inventory_Booking> GetBookingByPaging(int pageIndex, string sort, string sortdir,DateTime? fromDate,DateTime? toDate, string searchString,int storeId, out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            string compId = PortalContext.CurrentUser.CompId;
            IQueryable<Inventory_Booking> bookings = _bookingRepository.GetWithInclude(x => x.CompId == compId&&x.StoreId==storeId 
                &&((x.OrderNo.Trim().ToLower().Contains(searchString.Trim().ToLower())||String.IsNullOrEmpty(searchString))
                || (x.StyleNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.BookingRefId.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.OM_Buyer.BuyerName.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.OM_Merchandiser.EmpName.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.Mrc_SupplierCompany.CompanyName.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString)))
               &&((x.BookingDate >= fromDate || fromDate == null) && (x.BookingDate <= toDate || toDate == null)), "OM_Buyer", "Mrc_SupplierCompany", "OM_Merchandiser");
            totalRecords = bookings.Count();
            switch (sort)
            {
                case "OrderNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            bookings = bookings
                                  .OrderByDescending(r => r.OrderNo)
                                  .Skip(pageIndex * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            bookings = bookings
                                  .OrderBy(r => r.OrderNo)
                                  .Skip(pageIndex * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "StyleNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            bookings = bookings
                             .OrderByDescending(r => r.StyleNo)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            bookings = bookings
                                 .OrderBy(r => r.StyleNo)
                                 .Skip(pageIndex * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
              
                default:
                    bookings = bookings
                      .OrderByDescending(r => r.BookingId)
                      .Skip(pageIndex * pageSize)
                      .Take(pageSize);
                    break;

            }
            return bookings.ToList();

        }

        public int SaveBooking(Inventory_Booking booking)
        {

            string compId = PortalContext.CurrentUser.CompId;
            Inventory_Booking bookingObj = _bookingRepository.FindOne(x => x.BookingId == booking.BookingId && x.CompId == compId) ?? new Inventory_Booking();
            bookingObj.BookingRefId = bookingObj.BookingId > 0 ? bookingObj.BookingRefId : booking.BookingRefId;
            bookingObj.CompId = compId;
            bookingObj.BookingId = booking.BookingId;
            bookingObj.MarchandiserId = booking.MarchandiserId;
            bookingObj.SupplierId = booking.SupplierId;
            bookingObj.BuyerId = booking.BuyerId;
            bookingObj.OrderNo = booking.OrderNo;
            bookingObj.StyleNo = booking.StyleNo;
            bookingObj.OrderQty = booking.OrderQty;
            bookingObj.PiNo = booking.PiNo;
            bookingObj.StoreId = booking.StoreId;
            bookingObj.BookingDate = booking.BookingDate;
            bookingObj.Remarks = booking.Remarks;
            bookingObj.Inventory_BookingDetail = booking.Inventory_BookingDetail;
            _bookingDetailRepository.Delete(x => x.CompId == compId && x.BookingId == booking.BookingId);
            return _bookingRepository.Save(bookingObj);
        }

        public string GetNewBookingRefId()
        {
            string compId = PortalContext.CurrentUser.CompId;
              int yarnStoreId=(int)StoreType.Yarn;
              var refIdDesgit = _bookingRepository.Filter(x => x.CompId == compId & x.StoreId == yarnStoreId).Max(x => x.BookingRefId.Substring(2)) ?? "0";
            string bookingRefId = "BK" + refIdDesgit.IncrementOne().PadZero(6);
            return bookingRefId;
        }

        public Inventory_Booking GetBookingByid(long bookingId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _bookingRepository.FindOne(x => x.CompId == compId && x.BookingId == bookingId);
        }

        public Dictionary<string, VwBookingDetail> GetVwBookingDetail(long bookingId)
        {
              string compId = PortalContext.CurrentUser.CompId;
              List<VwBookingDetail> vwBookingDetail = _bookingRepository.GetVwBookingDetail(bookingId, compId);
              return vwBookingDetail.ToDictionary(x => Convert.ToString(x.BookingDetailId), x => x);

        }

        public int DeleteBooking(long bookingId)
        {
            var deleteIndx = 0;
            string compId = PortalContext.CurrentUser.CompId;
            using (var transaction=new TransactionScope())
            {
                 deleteIndx+= _bookingDetailRepository.Delete(x => x.BookingId == bookingId && x.CompId == compId);
                 deleteIndx+= _bookingRepository.Delete(x => x.BookingId == bookingId && x.CompId == compId);
                transaction.Complete();
            }
            return deleteIndx;
        }

        public List<VwBookingDetailReport> GetVwBookingDetaliReportById(long bookingId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _bookingRepository.GetVwBookingDetaliReportById(bookingId, compId);
        }

        public List<VwBookingSummaryReport> GetVwBookingDetalSummary(DateTime? fromDate, DateTime? toDate, int storeId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _bookingRepository.GetVwBookingSummaryReport(fromDate, toDate, compId,storeId);

          
        }

        public IEnumerable GetBooking(int storeId)
        {   string compId = PortalContext.CurrentUser.CompId;
         var bookings=   _bookingRepository.Filter(x => x.CompId == compId&&x.StoreId==storeId)
                .OrderByDescending(x=>x.BookingId).Select(x=>new
            {
                Value=x.BookingRefId,
                Text = x.BookingRefId+"(Order :"+x.OrderNo+",Style No :"+x.StyleNo+")"
            });
            return bookings;
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetVwYarnBookingDetail(string piBookingRefId)
        {
             string compId = PortalContext.CurrentUser.CompId;
             if (!String.IsNullOrEmpty(piBookingRefId))
            {
                var booking = _bookingRepository.FindOne(x => x.CompId == compId && x.BookingRefId == piBookingRefId);
                List<VwBookingDetail> bookingDetails = _bookingRepository.GetVwBookingDetail(booking.BookingId, compId);
                return bookingDetails.ToDictionary(x => Convert.ToString(x.BookingDetailId), x => new VwMaterialReceiveAgainstPoDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId,
                    SizeName = x.SizeName,
                    ColorName = x.ColorName,
                    SizeRefId = x.SizeRefId,
                    FColorRefId = x.FColorRefId,
                    FColorName = x.FColorName,
                    ReceivedRate = x.Rate,
                    ReceivedQty = x.Quantity,
                }); 
            }
            return new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
         
        }

        public Inventory_Booking GetBookingByid(string piBookingRefId, int yarnStore)
        { string compId = PortalContext.CurrentUser.CompId;
            return
                _bookingRepository.FindOne(
                    x => x.CompId == compId && x.BookingRefId == piBookingRefId && x.StoreId == yarnStore);
        }

        public List<VwBookingDetailReport> GetVwBookingDetaliReport(int storeId)
        {
           return _bookingRepository.GetVwBookingDetaliReport(PortalContext.CurrentUser.CompId,storeId);
        }

        public string GetNewAccBookingRefId()
        {


            int accessoryesStoreId=(int)StoreType.Acessories;
            string compId = PortalContext.CurrentUser.CompId;
            var refIdDesgit = _bookingRepository.Filter(x => x.CompId == compId & x.StoreId == accessoryesStoreId).Max(x => x.BookingRefId.Substring(2)) ?? "0";
            string bookingRefId = RefPrifix.AccessoriesBooking + refIdDesgit.IncrementOne().PadZero(6);
            return bookingRefId;
        }
    }


}


