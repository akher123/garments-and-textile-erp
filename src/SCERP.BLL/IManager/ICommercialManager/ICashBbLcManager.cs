using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    
    public interface ICashBbLcManager
    {
        List<CommCashBbLcInfo> GetAllCashBbLcInfosByPaging(out int totalRecords, CommCashBbLcInfo model);

        List<CommCashBbLcInfo> GetAllCashBbLcInfos();

        CommCashBbLcInfo GetCashBbLcInfoById(int? id);

        CommCashBbLcInfo GetCashBbLcIdByBbLcNo(string bbLcNo);

        int SaveCashBbLcInfo(CommCashBbLcInfo bblcInfo);

        int EditCashBbLcInfo(CommCashBbLcInfo bbLcInfo);

        int DeleteCashBbLcInfo(CommCashBbLcInfo bbLcInfo);

        bool CheckExistingCashBbLcInfo(CommCashBbLcInfo bblcInfo);

        List<CommCashBbLcInfo> GetCashBbLcInfoBySearchKey(int searchByCountry, string searchByCashBbLcInfo);
    }
}
