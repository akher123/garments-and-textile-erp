using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IKnittingRollRepository : IRepository<PROD_KnittingRoll>
    {
        VwKnittingRoll GetKnittingRollById(long knittingRollId);
        IQueryable<VwKnittingRoll> GetKnittingRolls(Expression<Func<VwKnittingRoll, bool>> predicate);
        DataTable MachineWiseKnitting(DateTime? rolldate, string kType, string compId);
        DataTable GetDailyKnittingRollSummaryByDate(DateTime dateTime, string compId);
        List<VwKnittingRoll> AutocompliteKnittingRoll(string orderStyleRefId, string compId);
    } 
}
