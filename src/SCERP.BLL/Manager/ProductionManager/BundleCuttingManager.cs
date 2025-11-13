using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class BundleCuttingManager : IBundleCuttingManager
    {
        private readonly IBundleCuttingRepository _bundleCuttingRepository;
        public BundleCuttingManager(IBundleCuttingRepository bundleCuttingRepository)
        {
            _bundleCuttingRepository = bundleCuttingRepository;
        }
        public List<VwBundleCutting> GetBundleCuttingByCuttingBatchRefId(string cuttingBatchRefId)
        {
            IQueryable<VwBundleCutting> bunlCuttings = _bundleCuttingRepository.GetVwBundleCuttingByCuttingBatchRefId(PortalContext.CurrentUser.CompId,
               cuttingBatchRefId);
            return bunlCuttings.ToList();
        }

       
    }
}
