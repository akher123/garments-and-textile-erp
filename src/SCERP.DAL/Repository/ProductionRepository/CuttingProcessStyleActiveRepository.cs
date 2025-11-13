using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CuttingProcessStyleActiveRepository : Repository<PROD_CuttingProcessStyleActive>, ICuttingProcessStyleActiveRepository
    {
        public CuttingProcessStyleActiveRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwCuttingProcessStyleActive> GetCuttingProcessStyleActiveByPaging(string compId, string buyerRefId, string orderNo, string orderStyleRefId)
        {
            return Context.VwCuttingProcessStyleActives.Where(x => x.CompId == compId
                && ((x.BuyerRefId == buyerRefId || String.IsNullOrEmpty(buyerRefId))
                && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo))
                && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId)))).OrderBy(x => x.EndDate).ThenBy(x => x.OrderNo).ThenBy(x => x.StyleName).AsNoTracking().ToList();
        }

        public IEnumerable GetOrderByBuyer(string compId, string buyerRefId)
        {
            DateTime todate = DateTime.Now;
            var orderLsit = Context.VwCuttingProcessStyleActives.Where(x => ((todate>=x.StartDate&& (todate<= x.EndDate || x.EndDate == null))) && x.CompId == compId && x.BuyerRefId == buyerRefId).Select(x => new
                {
                     OrderNo = x.OrderNo,
                     RefNo = x.OrderRefNo,
                     
                }).Distinct().ToList();
            return orderLsit;
        }

        public IEnumerable GetStyleByOrderNo(string compId, string orderNo)
        {
            DateTime todate = DateTime.Now;
            var orderLsit = Context.VwCuttingProcessStyleActives.Where(x => ((todate >=x.StartDate &&( todate <=x.EndDate || x.EndDate == null))) && x.CompId == compId && x.OrderNo == orderNo).Select(x => new
            {
                OrderStyleRefId = x.OrderStyleRefId,
                StyleNo = x.StyleName
            }).Distinct().ToList();
            return orderLsit;
        }

        public VwCuttingProcessStyleActive GetVwStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId)
        {
          return  Context.VwCuttingProcessStyleActives.FirstOrDefault(
                x => x.CuttingProcessStyleActiveId == cuttingProcessStyleActiveId);
        }
    }
}
