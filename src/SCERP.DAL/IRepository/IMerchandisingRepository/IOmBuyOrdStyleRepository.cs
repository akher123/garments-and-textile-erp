using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IOmBuyOrdStyleRepository:IRepository<OM_BuyOrdStyle>
    {
        string GetStyleRefNo(string compId);

        IQueryable<VOMBuyOrdStyle> GetBuyerOrderStyle(Expression<Func<VOMBuyOrdStyle, bool>> predicate);
        IQueryable<VOM_BuyOrdStyle> GetVBuyerOrderStyle(string compId);

        IQueryable<VwStyleFollowupStatus> GetStyleFollowupStatusesByPaging
            (string buyerRefId, string searchString);
        VOM_BuyOrdStyle GetBuyerOrderStyle(string compId,string orderStyleRefId);
        int CloseAllStyleByOrder(string orderNo, int activeStatus, string compId);
    }
}
