using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IConsumptionRepository:IRepository<OM_Consumption>
    {
        IQueryable<VConsumption> GetConsumptions(Expression<Func<VConsumption, bool>> predicate);
        string GetNewConsRefId(string compId);
        int UpdateFabricConsuption(string consRefId,string compId);

        IQueryable<VwConsuptionOrderStyle> GetVwConsuptionOrderStyle(Expression<Func<VwConsuptionOrderStyle, bool>> where);
        IQueryable<VwConsuptionOrderStyle> GetVwConsuptionStyle(string compId,Guid? employeeId,string searchString);
    }
}
