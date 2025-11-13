using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttingTagManager
    {
       List<VwCuttingTag> GetAllCuttingTatByCuttingSequenceId(long cuttingSequenceId);
       int SaveCuttingTag(PROD_CuttingTag model);
       PROD_CuttingTag GetCuttingTagByCuttingTagId(long cuttingTagId);
       int EditCuttingTag(PROD_CuttingTag model); 
       int DeleteCuttingTag(long cuttingTagId);

       bool IsCuttingTagExist(PROD_CuttingTag model);
       List<VwCuttingTag> GetAllCuttingTagSupplierByCuttingTagId(long cuttingTagId);
       object GetTagBySequence(string componentRefId, string orderStyleRefId);

       List<SpPrintEmbroiderySummary> GetPrintEmbroideryBalance(string cuttingBatchRefId,string buyerRefId,string orderNo,string orderStyleRefId,string colorRefId);
       object GetGetCuttingTagBySequence(string colorRefId, string componentRefId, string orderStyleRefId);
    } 
}
