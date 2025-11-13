using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface ICuttingTagRepository : IRepository<PROD_CuttingTag>
    {
        IQueryable<VwCuttingTag> GetVwCuttingTagByCuttingSequenceId(string compId, long cuttingSequenceId);
        IQueryable<VwCuttingTag> GetVwCuttingTagSupplierByCuttingTagId(string compId, long cuttingTagId);
        object GetTagBySequence(string componentRefId, string orderStyleRefId, string compId);
        List<SpPrintEmbroiderySummary> GetPrintEmbroideryBalance(string cuttingBatchRefId,string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId);
    }
}
