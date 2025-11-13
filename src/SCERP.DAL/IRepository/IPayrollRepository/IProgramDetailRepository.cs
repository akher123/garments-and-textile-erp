using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IProgramDetailRepository:IRepository<PLAN_ProgramDetail>
    {
        IQueryable<VProgramDetail> GetVProgramDetails(Expression<Func<VProgramDetail, bool>> predicate);
        List<VProgramDetail> GetVProgramList(string prorgramRefId, string productionRefId, string pType);
    }
}
