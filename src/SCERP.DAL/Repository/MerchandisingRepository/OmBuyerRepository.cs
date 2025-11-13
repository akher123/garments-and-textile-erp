using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmBuyerRepository : Repository<OM_Buyer>, IOmBuyerRepository
    {
        public OmBuyerRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewBuyerRefId(string compId)
        {
            string sqlQuery =
                String.Format("SELECT RIGHT('000'+ CAST( ISNULL(MAX(BuyerRefId),0)+1 as varchar(3) ),3) as BuyerRefId FROM OM_Buyer WHERE CompId='{0}'",
                    compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

        public string GetNewAgentRefId(string compid)
        {
            return "";
        }


        public object GetCuttingProcessStyleActiveBuyers()
        {
            string comId = PortalContext.CurrentUser.CompId;
            DateTime currentDate = DateTime.Now;
            var buyerList = (from B in Context.OM_Buyer
                           join A in Context.PROD_CuttingProcessStyleActive on B.BuyerRefId equals A.BuyerRefId 
                             where A.CompId == comId && B.CompId == comId && (currentDate>=A.StartDate )&&(currentDate<=A.EndDate||A.EndDate==null)
                           select new { B.BuyerRefId, B.BuyerName }).Distinct().ToList();
            return buyerList;
        }
    }
}
