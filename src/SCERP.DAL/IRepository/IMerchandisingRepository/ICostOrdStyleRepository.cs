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
    public interface ICostOrdStyleRepository:IRepository<OM_CostOrdStyle>
    {
        IQueryable<VCostOrderStyle> GetVCostCostOrdStyle(Expression<Func<VCostOrderStyle, bool>> predicates);
    }
}
