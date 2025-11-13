using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IBookingRepository:IRepository<Inventory_Booking>
    {
        List<VwBookingDetail> GetVwBookingDetail(long bookingId, string compId);

        List<VwBookingDetailReport> GetVwBookingDetaliReportById(long bookingId, string compId);


        List<VwBookingSummaryReport> GetVwBookingSummaryReport(DateTime? fromDate, DateTime? toDate, string compId, int storeId);

        List<VwBookingDetailReport> GetVwBookingDetaliReport(string compId, int yarn);
     
    }
}
