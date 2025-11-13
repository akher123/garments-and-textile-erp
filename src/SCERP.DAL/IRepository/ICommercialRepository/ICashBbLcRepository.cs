using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    //class ICashBbLcRepository
    //{
    //}
    public interface ICashBbLcRepository : IRepository<CommCashBbLcInfo>
    {
        CommCashBbLcInfo GetCashBbLcInfoById(int? id);
        CommCashBbLcInfo GetCashBbLcIdByBbLcNo(string bbLcNo);
        List<CommCashBbLcInfo> GetAllCashBbLcInfos();
        List<CommCashBbLcInfo> GetAllCashBbLcInfosByPaging(int startPage, int pageSize, out int totalRecords, CommCashBbLcInfo cashBbLcInfo);
        List<CommCashBbLcInfo> GetCashBbLcInfoBySearchKey(int searchByCountry, string searchByCashBbLcInfo);
    }
}
