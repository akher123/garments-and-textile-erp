using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ICashLcManager
    {
        List<CommCashLc> GetAllCashLcsByPaging(CommCashLc model, out int totalRecords,string searchString);
        List<CommCashLc> GetAllCashLcs();
        CommCashLc GetCashLcById(int cashLcId);
        int SaveCashLc(CommCashLc model);
        int EditCashLc(CommCashLc model);
        int DeleteCashLc(int cashLcId);
        bool IsCashLcExist(CommCashLc model);
        string GetNewCashLcRefId(string prifix);
    }
}
