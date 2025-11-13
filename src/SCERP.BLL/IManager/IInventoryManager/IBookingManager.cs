using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IBookingManager
    {
        List<Inventory_Booking> GetBookingByPaging(int pageIndex, string sort, string sortdir,DateTime? fromDate,DateTime? toDate,   string searchString,int storeId, out int totalRecords);
        int SaveBooking(Inventory_Booking booking);
        string GetNewBookingRefId();
        Inventory_Booking GetBookingByid(long bookingId);
        Dictionary<string, VwBookingDetail> GetVwBookingDetail(long bookingId);
        int DeleteBooking(long bookingId);
        List<VwBookingDetailReport> GetVwBookingDetaliReportById(long bookingId);
        List<VwBookingSummaryReport> GetVwBookingDetalSummary(DateTime? fromDate, DateTime? toDate, int storeId);
        IEnumerable GetBooking(int storeId);
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetVwYarnBookingDetail(string piBookingRefId);
        Inventory_Booking GetBookingByid(string piBookingRefId, int yarnStore);
        List<VwBookingDetailReport> GetVwBookingDetaliReport(int storeId);

        string GetNewAccBookingRefId();
    }
}
