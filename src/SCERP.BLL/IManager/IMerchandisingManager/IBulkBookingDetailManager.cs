using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IBulkBookingDetailManager
    {
       List<OM_BulkBookingDetail> GetBulkBookingDetailList(long bulkBookingId);
       OM_BulkBookingDetail GetBulkBookingDetailId(long bulkBookingDetailId);
       int EditBulkBookingDetail(OM_BulkBookingDetail bulkBookingDetail);
       int SaveBulkBookingDetail(OM_BulkBookingDetail bulkBookingDetail);
       int GetNextSequenceNo(long bulkBookingId);
       int DeleteBulkBookingDetailById(long bulkBookingDetailId);
    }
}
