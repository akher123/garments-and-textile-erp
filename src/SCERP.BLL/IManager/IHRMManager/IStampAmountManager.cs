using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IStampAmountManager
    {
        List<StampAmount> GetAllStampAmountsByPaging(int startPage, int pageSize, out int totalRecords, StampAmount stampAmount);

        List<StampAmount> GetAllStampAmounts();

        StampAmount GetStampAmountById(int? id);

        int SaveStampAmount(StampAmount stampAmount);

        int EditStampAmount(StampAmount stampAmount);

        int DeleteStampAmount(StampAmount stampAmount);

        bool CheckExistingStampAmount(StampAmount stampAmount);

        List<StampAmount> GetStampAmountBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);

        StampAmount GetLatestStampAmountInfo();

        int UpdateLatestStampInfoDate(StampAmount stampAmount);

    }
}
