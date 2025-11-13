using System;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class StanderdMinValueRepository :Repository<PROD_StanderdMinValue>, IStanderdMinValueRepository
    {
        public StanderdMinValueRepository(SCERPDBContext context) : base(context)
        {
        }

 

        public string GetStanderdMinValueRefId(string compId)
        {
            var sqlQuery = String.Format("Select  substring(MAX(StanderdMinValueRefId),4,7)  from PROD_StanderdMinValue  where CompId='{0}'",
                compId);
            var issueReceiveNo =
              Context.Database.SqlQuery<string>(sqlQuery)
                  .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "SMV" + GetRefNumber(maxNumericValue, 4);
            return irNo;
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
    }
}
