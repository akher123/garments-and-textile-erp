using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IDepreciationChartRepository : IRepository<Acc_DepreciationChart>
    {
        Acc_DepreciationChart GetDepreciationById(int? id);
        List<Acc_DepreciationChart> GetAllDepreciation(int page, int records, string sort);
        IQueryable<Acc_DepreciationChart> GetAllDepreciations();
        IQueryable<Acc_ControlAccounts> GetAllControlAccounts();
        string GetControlName(decimal ControlCode);
    }
}
