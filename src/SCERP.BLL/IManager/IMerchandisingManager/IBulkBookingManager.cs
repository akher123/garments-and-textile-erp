using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IBulkBookingManager
    {
        List<OM_BulkBooking> GetBulkBookingList(string serarchString);
        OM_BulkBooking GetBulkBookingById(long bulkBookingId);
        int SaveBulkBooking(OM_BulkBooking bulkBooking);
        int EditBulkBooking(OM_BulkBooking bulkBooking);
        string GetNewRefId(string compId);
        int DeleteBulkBookingById(long bulkBookingId);
    }
}
