using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmBuyOrdStyleRepository : Repository<OM_BuyOrdStyle>, IOmBuyOrdStyleRepository
    {
        public OmBuyOrdStyleRepository(SCERPDBContext context) : base(context)
        {

        }


        public string GetStyleRefNo(string compId)
        {
            var sqlQuery = String.Format("Select  substring(MAX(OrderStyleRefId),3,7 )from OM_BuyOrdStyle where CompId='{0}'", compId);
            var oldRefId = Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(oldRefId);
            var orderStyleRefId = "ST" + GetRefNumber(maxNumericValue, 5);
            return orderStyleRefId;
        }

        public IQueryable<VOMBuyOrdStyle> GetBuyerOrderStyle(Expression<Func<VOMBuyOrdStyle, bool>> predicate)
        {
            return Context.VOMBuyOrdStyles.Where(predicate);
        }

        public IQueryable<VOM_BuyOrdStyle> GetVBuyerOrderStyle(string compId)
        {
            return Context.VOM_BuyOrdStyles.Where(x => x.CompId == compId);
        }

        public IQueryable<VwStyleFollowupStatus> GetStyleFollowupStatusesByPaging(
            string buyerRefId, string searchString)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var employeeId = PortalContext.CurrentUser.UserId;
            return Context.Database.SqlQuery<VwStyleFollowupStatus>("exec spStyleActivity '" + compId + "','" + employeeId + "','" + buyerRefId + "','" + searchString + "'").AsQueryable();
        }

        public VOM_BuyOrdStyle GetBuyerOrderStyle(string compId, string orderStyleRefId)
        {
            return Context.VOM_BuyOrdStyles.FirstOrDefault(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId);
        }


        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }

        public int CloseAllStyleByOrder(string orderNo, int activeStatus, string compId)
        {
            string sql = String.Format(@"update  OM_BuyOrdStyle set ActiveStatus={0} WHERE OrderNo='{1}' and CompId='{2}'", activeStatus, orderNo, compId);
            return Context.Database.ExecuteSqlCommand(sql);
        }
    }
}
