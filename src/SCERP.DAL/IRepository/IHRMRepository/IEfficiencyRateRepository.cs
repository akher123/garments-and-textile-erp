using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEfficiencyRateRepository : IRepository<EfficiencyRate>
    {
        List<EfficiencyRate> GetAllEfficiencyByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EfficiencyRate efficiencyRate);  
    } 
}
