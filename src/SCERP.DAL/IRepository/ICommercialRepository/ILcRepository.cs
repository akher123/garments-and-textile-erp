using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface ILcRepository : IRepository<COMMLcInfo>
    {
        COMMLcInfo GetLcInfoById(int? id);
        List<COMMLcInfo> GetLcInfoByLcId(int? id);
        List<COMMLcInfo> GetAllLcInfos();
        COMMLcInfo GetLcInfoByLcNo(string lcNo);
        List<COMMLcInfo> GetAllLcInfosByPaging(int startPage, int pageSize, out int totalRecords, COMMLcInfo lcInfo);
        List<COMMLcInfo> GetLcInfoBySearchKey(int searchByCountry, string searchByLcInfo);
        List<CommBank> GetBankInfo(string bankType);
        IQueryable<VwCommLcInfo> GetLcInfos(string modelRStatus, int? receivingBankId, string searchString);
    }
}
