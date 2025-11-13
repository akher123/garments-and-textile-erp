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
    public class LayCuttingManager : ILayCuttingManager
    {
        private readonly ILayCuttingRepository _layCuttingRepository;
        public LayCuttingManager(ILayCuttingRepository layCuttingRepository)
        {
            _layCuttingRepository = layCuttingRepository;
        }
        public List<PROD_LayCutting> GetLayCuttingByCuttingBatchId(long cuttingBatchRefId)
        {


            return
                _layCuttingRepository.Filter(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchRefId).ToList();
        }

        public int SaveLayCutting(PROD_LayCutting model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            PROD_LayCutting layCutting =  _layCuttingRepository.FindOne(x => x.CompId == model.CompId && x.SizeRefId == model.SizeRefId && x.CuttingBatchRefId == model.CuttingBatchRefId)??new PROD_LayCutting();
            layCutting.SizeRefId = model.SizeRefId;
            layCutting.Ratio = model.Ratio;
            layCutting.CuttingBatchRefId = model.CuttingBatchRefId;
            layCutting.CompId = model.CompId;
            if (String.IsNullOrEmpty(layCutting.LaySl))
            {
                string laySl = _layCuttingRepository.Filter(x => x.CompId == model.CompId && x.CuttingBatchRefId == model.CuttingBatchRefId).Max(x => x.LaySl) ?? "0";
                layCutting.LaySl = laySl.IncrementOne().PadZero(2);
            }
        
            return _layCuttingRepository.Save(layCutting);
        }

        public int DeleteLayCutting(string cuttingBatchRefId, int layCuttingId)
        {
            int saveIndex = 0;
            using (var transaction=new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                List<PROD_LayCutting> layCuttings = _layCuttingRepository.Filter(x => x.CompId == compId && x.CuttingBatchRefId == cuttingBatchRefId && x.LayCuttingId != layCuttingId).ToList();

                layCuttings = layCuttings.Select((x, index) =>
                {
                    int laySl = index + 1;
                    x.LaySl = Convert.ToString(laySl).PadZero(2);
                    return x;
                }).ToList();
                _layCuttingRepository.Delete(x => x.CompId == compId && x.CuttingBatchRefId == cuttingBatchRefId);
                saveIndex= _layCuttingRepository.SaveList(layCuttings);
                transaction.Complete();
                
            }

            return saveIndex;
        }
    }
}
