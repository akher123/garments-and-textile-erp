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
    public interface IOmStyleRepository:IRepository<OM_Style>
    {
        IQueryable<VStyle> GetVStyles(Expression<Func<VStyle, bool>> predicate);
        string GetNewStyleRefId(string compId);

    }
}
