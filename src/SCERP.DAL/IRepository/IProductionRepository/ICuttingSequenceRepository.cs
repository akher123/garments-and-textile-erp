using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ICuttingSequenceRepository:IRepository<PROD_CuttingSequence>
    {
       IEnumerable GetComponentsByColor(string colorRefId, string orderStyleRefId, string compId);
     
       List<VwCuttingSequence> GetCuttingSequenceByParam
           
           (string compId,  string colorRefId, string orderNo, string buyerRefId, string orderStyleRefId);

       List<VwCuttingSequence> GetCuttingSequenceByPaging(string compId, string colorRefId, string orderNo, string buyerRefId, string orderStyleRefId);
       IEnumerable GetCuttingSequenceOrderStyle(string compId, string orderStyleRefId, string orderNo);
    }
}
