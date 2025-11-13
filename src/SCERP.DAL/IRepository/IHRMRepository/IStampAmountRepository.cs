using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IStampAmountRepository : IRepository<StampAmount>
    {
        StampAmount GetStampAmountById(int? id);
        List<StampAmount> GetAllStampAmounts();
        List<StampAmount> GetAllStampAmountsByPaging(int startPage, int pageSize, out int totalRecords, StampAmount stampAmount);
        List<StampAmount> GetStampAmountBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);
        StampAmount GetLatestStampAmountInfo();
        int UpdateLatestStampInfoDate(StampAmount stampAmount);
    }
}
