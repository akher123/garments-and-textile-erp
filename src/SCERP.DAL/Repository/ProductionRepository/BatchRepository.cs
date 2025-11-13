using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class BatchRepository : Repository<Pro_Batch>, IBatchRepository
   {
        public BatchRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VProBatch> GetBachList()
        {
            return Context.VProBatches;
        }

        public string GetNewBtRefNo(string prefix)
        {
            var btchRefNo = Context.Pro_Batch.Where(x => x.BtRefNo.Substring(0, 2) == prefix).Max(x => x.BtRefNo.Substring(2, 8)) ?? "0";
            var maxNumericValue = Convert.ToInt32(btchRefNo);
            var btRefNo = prefix + GetRefNumber(maxNumericValue, 6);
            return btRefNo;
        }

        public List<VwProdBatchDetail> GetBachByBatchId(long batchId, string compId)
        {
            return Context.BatchDetail.Where(x => x.CompId == compId && x.BatchId == batchId).ToList();
        }

        public string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }
   
   }
}
