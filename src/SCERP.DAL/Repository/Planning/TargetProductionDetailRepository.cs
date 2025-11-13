using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.Planning
{
    public class TargetProductionDetailRepository :Repository<PLAN_TargetProductionDetail>, ITargetProductionDetailRepository
    {
        public TargetProductionDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpPlaningTargetProductionDetail> GetSpPlaningTargetProductionDetail()
        {
            return Context.Database.SqlQuery<SpPlaningTargetProductionDetail>("SpPlaningTargetProductionDetail").ToList();
        }

        public int SaveTargetDetail(PLAN_TargetProduction target)
        {
            string sqlQury = @"INSERT INTO PLAN_TargetProductionDetail (CompId, TargetProductionId, TargetDate, TargetQty)
            SELECT        '001' AS CompId, '{0}' AS TargetProductionId, WorkingDate AS TargetDate, {1}/(SELECT count(*)  FROM  PLAN_WorkingDay as WD
                                  WHERE (DayStatus = 1) AND (WorkingDate >= '{2}') AND (WorkingDate <= '{3}')) AS TargetQty
            FROM            PLAN_WorkingDay
            WHERE        (DayStatus = 1) AND (Convert(date,WorkingDate) >= Convert(date,'{2}')) AND (Convert(date,WorkingDate) <=Convert(date,'{3}'))";
            string sqlDeleteQury = @"delete from  PLAN_TargetProductionDetail  where TargetProductionId='{0}'";
          
                Context.Database.ExecuteSqlCommand(String.Format(sqlDeleteQury, target.TargetProductionId));
              return  Context.Database.ExecuteSqlCommand(String.Format(sqlQury, target.TargetProductionId,
                    target.TotalTargetQty, target.StartDate, target.EndDate));
            
           
        }
    }
}
