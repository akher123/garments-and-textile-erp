using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class RollCuttingManager : IRollCuttingManager
    {
        private readonly IRollCuttingRepository _rollCuttingRepository;
        public RollCuttingManager(IRollCuttingRepository rollCuttingRepository)
        {
            _rollCuttingRepository = rollCuttingRepository;
        }
        public List<VwRollCutting> GetRollCuttingByCuttingBatchRefId(string cuttingBatchRefId)
        {
           
                IQueryable<VwRollCutting> rollCuttings= _rollCuttingRepository.GetRollCuttingByCuttingBatchRefId(PortalContext.CurrentUser.CompId,  cuttingBatchRefId);
                return rollCuttings.ToList();
        }

        public int SaveRollCutting(PROD_RollCutting model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _rollCuttingRepository.Save(model);
        }

        public int DeleteRollCutting(int rollCuttingId, string cuttingBatchRefId)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
             saveIndex= _rollCuttingRepository.Delete(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchRefId == cuttingBatchRefId);

                transaction.Complete();
            }
            return saveIndex;
        }
    }
}
