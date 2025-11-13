using SCERP.Model.CommercialModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ICashLCDyesChemicalManager
    {
        List<CommCashLCDyesChemical> GetAllCashLcsByPaging(CommCashLCDyesChemical model, out int totalRecords, string searchString);
        List<CommCashLCDyesChemical> GetAllCashLcs();
        CommCashLCDyesChemical GetCashLcById(int cashLcId);
        int SaveCashLc(CommCashLCDyesChemical model);
        int EditCashLc(CommCashLCDyesChemical model);
        int DeleteCashLc(int cashLcId);
        bool IsCashLcExist(CommCashLCDyesChemical model);
        string GetNewCashLcRefId(string prifix);
    }
}
