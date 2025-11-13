using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CuttingSequenceRepository : Repository<PROD_CuttingSequence>, ICuttingSequenceRepository
    {
        public CuttingSequenceRepository(SCERPDBContext context) : base(context)
        {
        }

        public IEnumerable GetComponentsByColor(string colorRefId, string orderStyleRefId, string compId)
        {
           return Context.VwCuttingSequence.Where(x => x.CompId == compId && x.ColorRefId == colorRefId&&x.OrderStyleRefId==orderStyleRefId).Select(x=>new
           {
               ComponentRefId=x.ComponentRefId,
               ComponentName=x.ComponentName
           }).ToList();
        }

        public List<VwCuttingSequence> GetCuttingSequenceByParam(string compId,string colorRefId, string orderNo, string buyerRefId, string orderStyleRefId)
        {
            return Context.VwCuttingSequence.Where(
               x =>
                   x.CompId ==compId && (x.ColorRefId == colorRefId )
                 && x.OrderStyleRefId == orderStyleRefId).OrderBy(x=>x.SlNo).ToList();
        }

        public List<VwCuttingSequence> GetCuttingSequenceByPaging(string compId, string colorRefId, string orderNo, string buyerRefId,
            string orderStyleRefId)
        {
            return Context.VwCuttingSequence.Where(x =>x.CompId==compId
                && (x.BuyerRefId == buyerRefId)
                && (x.OrderNo == orderNo)
                && (x.OrderStyleRefId == orderStyleRefId)
                && (x.ColorRefId == colorRefId || String.IsNullOrEmpty(colorRefId))).ToList();
        }

        public IEnumerable GetCuttingSequenceOrderStyle(string compId, string orderStyleRefId, string orderNo)
        {
            return Context.VwCuttingSequence.Where(x => x.CompId == compId && x.OrderNo == orderNo && x.OrderStyleRefId == orderStyleRefId).Select(x => new { x.ComponentName ,x.ComponentRefId}).Distinct().ToList();
        }
    }
}
