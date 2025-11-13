using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class BookingRepository :Repository<Inventory_Booking>, IBookingRepository
    {
        public BookingRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwBookingDetail> GetVwBookingDetail(long bookingId, string compId)
        {
            return
                Context.Database.SqlQuery<VwBookingDetail>("Select * from VwBookingDetail where CompId='" + compId +
                                                           "' and BookingId='" + bookingId + "'").ToList();
        }

        public List<VwBookingDetailReport> GetVwBookingDetaliReportById(long bookingId,string compId)
        {
            return Context.VwBookingDetailReports.Where(x => x.BookingId == bookingId && x.CompId == compId).ToList();
        }

        public List<VwBookingSummaryReport> GetVwBookingSummaryReport(DateTime? fromDate, DateTime? toDate, string compId, int storeId)
        {
           return Context.VwBookingSummaryReports.Where(
                x =>
                    x.CompId == compId &&(x.StoreId==storeId||storeId==0)&&
                    ((x.BookingDate >= fromDate || fromDate == null) && (x.BookingDate <= toDate || toDate == null))).ToList();
        }

        public List<VwBookingDetailReport> GetVwBookingDetaliReport(string compId, int storeId)
        {
            return Context.VwBookingDetailReports.Where(x => x.CompId == compId && x.StoreId == storeId).ToList();
        }

     
    }
}
