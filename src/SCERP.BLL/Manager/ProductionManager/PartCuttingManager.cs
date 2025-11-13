using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class PartCuttingManager : IPartCuttingManager
    {
        private readonly IPartCuttingRepository _partCuttingRepository;
        public PartCuttingManager(IPartCuttingRepository partCuttingRepository)
        {
            _partCuttingRepository = partCuttingRepository;
        }
        public List<VwPartCutting> GetAllPartCutting(string cuttingBatchRefId)
        {
            IQueryable<VwPartCutting> partCuttings = _partCuttingRepository.GetVwPartCuttingLsit(PortalContext.CurrentUser.CompId, cuttingBatchRefId);
            return partCuttings.ToList();
        }

        public int SavePartCutting(PROD_PartCutting model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            string laySl =
                _partCuttingRepository.Filter(
                    x => x.CompId == model.CompId && x.CuttingBatchRefId == model.CuttingBatchRefId).Max(x => x.PartSL) ?? "0";
            model.PartSL = laySl.IncrementOne().PadZero(2);
            return _partCuttingRepository.Save(model);
        }

        public int DeletePartCutting(int partCuttingId, string cuttingBatchRefId)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                List<PROD_PartCutting> partCuttings = _partCuttingRepository.Filter(x => x.CompId == compId && x.CuttingBatchRefId == cuttingBatchRefId && x.PartCuttingId != partCuttingId).ToList();

                partCuttings = partCuttings.Select((x, index) =>
                {
                    int partCuttingSl = index + 1;
                    x.PartSL = Convert.ToString(partCuttingSl).PadZero(2);
                    return x;
                }).ToList();
                _partCuttingRepository.Delete(x => x.CompId == compId && x.CuttingBatchRefId == cuttingBatchRefId);
                saveIndex = _partCuttingRepository.SaveList(partCuttings);
                transaction.Complete();
            }
            return saveIndex;
        }

        public PROD_PartCutting GetPartCuttingByCuttingBatchId(long cuttingBatchId)
        {
            return
                _partCuttingRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchId);
        }
    }
}
