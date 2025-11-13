using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class BatchRollManager : IBatchRollManager
    {
        private readonly IBatchRollRepository _batchRollRepository;

        public BatchRollManager(IBatchRollRepository batchRollRepository)
        {
            _batchRollRepository = batchRollRepository;
        }

        public List<VwBatchRoll> GetBatchRollByBatchId(long batchId)
        {
            return _batchRollRepository.GetBatchRollByBatchId(batchId);
        }

        public PROD_BatchRoll GetBatchRollById(long batchId, long knittingRollId)
        {
            return _batchRollRepository.FindOne(x => x.BatchId == batchId && x.KnittingRollId == knittingRollId);
        }

        public int SaveBatchRoll(List<PROD_BatchRoll> batchRollS)
        {
            var knittingRollIssueId=  batchRollS.First().KnittingRollIssueId;
            _batchRollRepository.Delete(x => x.KnittingRollIssueId == knittingRollIssueId);
            return _batchRollRepository.SaveList(batchRollS);
        }

        public VwBatchRoll GetBatchRollById(long batchRollId)
        {
            return _batchRollRepository.GetBatchRollById(batchRollId);
        }

        public int DeleteRoll(long batchRollId)
        {
            PROD_BatchRoll batchRoll = _batchRollRepository.FindOne(x => x.BatchRollId == batchRollId);
           return _batchRollRepository.DeleteOne(batchRoll);
        }
    }
}
