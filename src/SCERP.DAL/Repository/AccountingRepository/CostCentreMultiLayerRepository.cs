using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class CostCentreMultiLayerRepository : Repository<Acc_CostCentreMultiLayer>, ICostCentreMultiLayerRepository
    {
        public CostCentreMultiLayerRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<Acc_CostCentreMultiLayer> GetAllCostCentreMultiLayers()
        {
            return Context.Acc_CostCentreMultiLayer.Where(p => p.IsActive == true).ToList();
        }

        public int GetMaxItemId(int parentId)
        {
            return Context.Acc_CostCentreMultiLayer.Max(p => p.ItemId) + 1;
        }

        public Acc_CostCentreMultiLayer GetMultiLayerById(int id)
        {
            return Context.Acc_CostCentreMultiLayer.FirstOrDefault(p => p.Id == id);
        }

        public bool ExistCostCentre(int id)
        {
            return Context.Acc_VoucherToCostcentre.Any(p => p.IsActive && p.CostCentreId == id);
        }
    }
}
