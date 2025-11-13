using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;


namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IBbLcRepository : IRepository<CommBbLcInfo>
    {
        CommBbLcInfo GetBbLcInfoById(int? id);
        CommBbLcInfo GetBbLcIdByBbLcNo(string bbLcNo);
        List<CommBbLcInfo> GetAllBbLcInfos();
        List<CommBbLcInfo> GetAllBbLcInfosByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcInfo bbLcInfo);
        List<CommBbLcInfo> GetBbLcInfoBySearchKey(int searchByCountry, string searchByBbLcInfo);
    }
}
