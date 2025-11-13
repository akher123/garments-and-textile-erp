using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;


namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IBbLcManager
    {
        List<CommBbLcInfo> GetAllBbLcInfosByPaging(out int totalRecords, CommBbLcInfo model);

        List<CommBbLcInfo> GetAllBbLcInfos();

        CommBbLcInfo GetBbLcInfoById(int? id);

        CommBbLcInfo GetBbLcIdByBbLcNo(string bbLcNo);

        int SaveBbLcInfo(CommBbLcInfo bblcInfo);

        int EditBbLcInfo(CommBbLcInfo bbLcInfo);

        int DeleteBbLcInfo(CommBbLcInfo bbLcInfo);

        bool CheckExistingBbLcInfo(CommBbLcInfo bblcInfo);

        List<CommBbLcInfo> GetBbLcInfoBySearchKey(int searchByCountry, string searchByBbLcInfo);
    }
}
