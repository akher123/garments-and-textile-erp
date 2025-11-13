using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IBulkBookingYarnDetailManager
    {
       List<OM_BulkBookingYarnDetail> GetBulkBookingYarnList(long bulkBookingDetailId);
       OM_BulkBookingYarnDetail GetBulkBookingById(long bulkBookingYearnDetailId);
       int EditBulkBookingYarnDetail(OM_BulkBookingYarnDetail bulkBookingYarnDetail);
       int SaveBulkBookingYarnDetail(OM_BulkBookingYarnDetail bulkBookingYarnDetail);
       int DeleteBulkBookingYarnDetailById(long bulkBookingYearnDetailId);
    }
}
