using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class TargetProductionRepository :Repository<PLAN_TargetProduction>, ITargetProductionRepository
    {
        public TargetProductionRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwTargetProduction> VwGetTargetProduction(Expression<Func<VwTargetProduction, bool>> predicate)
        {
            return Context.VwTargetProductions.Where(predicate);
        }

        public List<VwTargetProduction> GetMontylyActiveTargetProductionList(int monthId, int yearId, string compId)
        {

           List<VwTargetProduction>tarteProductions=Context.VwTargetProductions.SqlQuery( String.Format("select * from VwTargetProduction where (MONTH(StartDate)='{0}' OR MONTH(EndDate)='{0}') and YEAR(StartDate)='{1}' and CompId='{2}'", monthId, yearId, compId)).ToList();
        
//            string sqlQury= @"INSERT INTO PLAN_TargetProductionDetail (CompId, TargetProductionId, TargetDate, TargetQty)
//            SELECT        '001' AS CompId, '{0}' AS TargetProductionId, WorkingDate AS TargetDate, {1}/(SELECT count(*)  FROM  PLAN_WorkingDay as WD
//                                  WHERE (DayStatus = 1) AND (WorkingDate >= '{2}') AND (WorkingDate <= '{3}')) AS TargetQty
//            FROM            PLAN_WorkingDay
//            WHERE        (DayStatus = 1) AND (Convert(date,WorkingDate) >= Convert(date,'{2}')) AND (Convert(date,WorkingDate) <=Convert(date,'{3}'))";
//            string sqlDeleteQury = @"delete from  PLAN_TargetProductionDetail  where TargetProductionId='{0}'";
//            foreach (var target in tarteProductions)
//            {
//              Context.Database.ExecuteSqlCommand(String.Format(sqlDeleteQury, target.TargetProductionId));
//                Context.Database.ExecuteSqlCommand(String.Format(sqlQury, target.TargetProductionId,
//                    target.TotalTargetQty, target.StartDate, target.EndDate));
//            }
           
            return tarteProductions;

        }
    }
}
