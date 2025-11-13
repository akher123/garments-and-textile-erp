using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttingSequenceManager
    {
       string GetNewComponentRefId();
       int SaveCuttingSequenceLis(List<PROD_CuttingSequence> cuttingSequences,long cuttingSequencId,string colorRefId);
       IEnumerable GetComponentsByColor(string colorRefId, string orderStyleRefId);

       List<VwCuttingSequence> GetCuttingSequenceByParam(string colorRefId,  string orderNo, string buyerRefId, string orderStyleRefId);
       List<VwCuttingSequence> GetCuttingSequenceByPaging   (int pageIndex, string sort, string sortdir, out int totalRecords, string colorRefId, string orderNo, string buyerRefId, string orderStyleRefId);
       IEnumerable GetCuttingSequenceOrderStyle(string orderStyleRefId, string orderNo);
       int DeleteCuttingSequence
           (long cuttingSequenceId);

   
    }
}
