using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface ICostCentreMultiLayerRepository : IRepository<Acc_CostCentreMultiLayer>
    {

        List<Acc_CostCentreMultiLayer> GetAllCostCentreMultiLayers();

        Acc_CostCentreMultiLayer GetMultiLayerById(int id);

        int GetMaxItemId(int parentId);

        bool ExistCostCentre(int id);
    }
}
