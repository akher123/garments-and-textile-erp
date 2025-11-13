using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class BatchRollRepository :Repository<PROD_BatchRoll>, IBatchRollRepository
    {
        public BatchRollRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwBatchRoll> GetBatchRollByBatchId(long batchId)
        {
           return Context.VwBatchRolls.Where(x => x.BatchId == batchId).ToList();
        }

        public VwBatchRoll GetBatchRollById(long batchRollId)
        {
            return Context.VwBatchRolls.FirstOrDefault(x => x.BatchRollId == batchRollId);
        }
    }
}
