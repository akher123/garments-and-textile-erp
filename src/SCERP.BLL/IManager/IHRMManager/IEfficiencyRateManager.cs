using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEfficiencyRateManager
    {
       List<EfficiencyRate> GetAllEfficiencyByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EfficiencyRate efficiencyRate);
       EfficiencyRate GetEficiencyRateById(int efficiencyRateId);
       bool IsExistEfficiencyRate(EfficiencyRate efficiencyRate);
       int EditEfficiencyRate(EfficiencyRate efficiencyRate);
       int SaveEfficiencyRate(EfficiencyRate efficiencyRate);
       int DeleteEfficiencyRate(int efficiencyRateId); 
    }
}
   