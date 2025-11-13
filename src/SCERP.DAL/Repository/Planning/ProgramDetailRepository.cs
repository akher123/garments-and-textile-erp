using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.Planning
{
    public class ProgramDetailRepository :Repository<PLAN_ProgramDetail>, IProgramDetailRepository
    {
        public ProgramDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VProgramDetail> GetVProgramDetails(Expression<Func<VProgramDetail, bool>> predicate)
        {
            return Context.VProgramDetails.Where(predicate);
        }

        public List<VProgramDetail> GetVProgramList(string prorgramRefId, string productionRefId, string pType)
        {
            var sqlQuery = String.Format( "select * from VProgramDetail as Pg where Pg.PrgramRefId='{0}'" +
                               "  and   Pg.MType='{1}' " +
                               "and Pg.ItemCode+pg.ColorRefId+Pg.SizeRefId  " +
                               "not in (select Pd.ItemCode+pd.ColorRefId+pd.SizeRefId " +
                               "from PROD_ProductionDetaill as Pd where pd.ProductionRefId='{2}')", prorgramRefId, pType, productionRefId);
            return Context.VProgramDetails.SqlQuery(sqlQuery).ToList();
        }
    }
}
