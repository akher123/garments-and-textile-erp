using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface ICostCentreRepository : IRepository<Acc_CostCentre>
    {
        Acc_CostCentre GetCostCentreById(int? id);
        List<Acc_CostCentre> GetAllCostCentres(int page, int records, string sort);
        IQueryable<Acc_CostCentre> GetAllCostCentres();
        IQueryable<Acc_CompanySector> GetAllCompanySector();
        Acc_CostCentreMultiLayer GetNewCostCentreById(int? id);
    }
}
