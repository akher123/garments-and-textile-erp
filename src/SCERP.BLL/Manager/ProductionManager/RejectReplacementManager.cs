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
    public class RejectReplacementManager : IRejectReplacementManager
    {
         private readonly IRejectReplacementRepository _rejectRepository;

         public RejectReplacementManager(IRejectReplacementRepository rejectAdjustmentRepository)
        {
            _rejectRepository = rejectAdjustmentRepository;
        }
       public Dictionary<string, List<string>> GetRejectReplacementByCuttingBatch(long cuttingBatchId)
        {
            var pivotTable = new Dictionary<string, List<string>>();
            List<SpProdJobWiseRejectAdjusment> rejectAdjusments = _rejectRepository.GetRejectReplacementByCuttingBatch(PortalContext.CurrentUser.CompId, cuttingBatchId);

            if (rejectAdjusments.Any())
            {
                List<string> sizeList = rejectAdjusments.Select(x => x.SizeName).ToList();
                sizeList.Add("Total");
                List<string> quantityList = rejectAdjusments.Select(x => Convert.ToString(x.Quantity)).ToList();
                quantityList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.Quantity)));
                List<string> rejectQtyList = rejectAdjusments.Select(x => Convert.ToString(x.RejectQty)).ToList();
                rejectQtyList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.RejectQty)));
                List<string> rejectPercent = rejectAdjusments.Select(x => String.Format("{0:0.00}" + " " + "%", (x.RejectQty * 100.00m) / x.Quantity)).ToList();
                rejectPercent.Add(String.Format("{0:0.00}" + " " + "%", rejectAdjusments.Sum(x => x.RejectQty) * 100.0m / rejectAdjusments.Sum(x => x.Quantity)));
                List<string> sizeRefIdList = rejectAdjusments.Select(x => Convert.ToString(x.SizeRefId)).ToList();
                List<string> okQtyList = rejectAdjusments.Select(x => Convert.ToString(x.Quantity)).ToList();
                okQtyList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.Quantity - x.RejectQty)));
                pivotTable = new Dictionary<string, List<string>>
            {
                {"Size", sizeList},
                {"Quantity", quantityList},
                {"Reject", rejectQtyList},
               // {"Reject(%)", rejectPercent},
                {"SizeRefId", sizeRefIdList},
                {"Final Quantity", okQtyList}
            };
            }

            return pivotTable;
        }

       public int SaveRejectReplacement(List<PROD_RejectReplacement> resReplacements)
        {
            long cuttingBatchId = resReplacements.First().CuttingBatchId;
            int saveIndex = 0;
            using (var transaction=new TransactionScope())
            {
                _rejectRepository.Delete(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchId);
                saveIndex = _rejectRepository.SaveList(resReplacements);
                transaction.Complete();
            }
            return saveIndex;
        }

        public int DeleteRejectReplacement(long cuttingBatchId)
        {
            return
                _rejectRepository.Delete(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchId);
        }

  
    }
}
