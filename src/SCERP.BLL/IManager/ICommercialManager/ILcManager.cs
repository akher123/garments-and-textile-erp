using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ILcManager
    {
        List<COMMLcInfo> GetAllLcInfosByPaging(int startPage, int pageSize, out int totalRecords, COMMLcInfo lcInfo);

        List<COMMLcInfo> GetAllLcInfos();

        COMMLcInfo GetLcInfoById(int? id);
        List<COMMLcInfo> GetLcInfoByLcId(int? id);
        int SaveLcInfo(COMMLcInfo lcInfo);

        int EditLcInfo(COMMLcInfo lcInfo);

        int DeleteLcInfo(COMMLcInfo commLcInfo);

        bool CheckExistingLcInfo(COMMLcInfo lcInfo);

        List<COMMLcInfo> GetLcInfoBySearchKey(int searchByCountry, string searchByCommLcInfo);

        COMMLcInfo GetLcInfoByLcNo(string lcNo);

        int UpdateChashIncentive(COMMLcInfo commLcInfo);

        List<COMMLcInfo> GetLcInfosByPaging(int pageIndex, int pageSize, out int totalRecords, long? buyerId, DateTime? fromDate, DateTime? toDate, string searchString, string completeStatus);

        List<COMMLcInfo> GetCashIncentiveReport(long? buyerId, DateTime? fromDate, DateTime? toDate, string searchString);
        List<COMMLcInfo> GetCashIncentiveByDateReport(DateTime? fromDate, DateTime? toDate, string searchString);
        List<COMMLcInfo> GetAllGroupLcs(string compId);
        List<CommBank> GetBankInfo(string bankType);
        List<VwCommLcInfo> GetLcInfos(int pageIndex,  out int totalRecords, string modelRStatus, int? receivingBankId,long?buyerId,string searchString);
    }
}