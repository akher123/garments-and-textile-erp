using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class YarnConsumptionRepository :Repository<OM_YarnConsumption>, IYarnConsumptionRepository
    {
        public YarnConsumptionRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VYarnConsumption> GetVYarnConsumptions(Expression<Func<VYarnConsumption, bool>> predicates)
        {
            return Context.VYarnConsumptions.Where(predicates);
        }

        public string GetNewYCRef(string compId)
        {

            var sqlQuery = String.Format("Select  substring(MAX(YCRef),2,10) from OM_YarnConsumption where CompId='{0}'",
                compId);
            var issueReceiveNo =
              Context.Database.SqlQuery<string>(sqlQuery)
                  .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "Y" + GetRefNumber(maxNumericValue,9); // Y
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
