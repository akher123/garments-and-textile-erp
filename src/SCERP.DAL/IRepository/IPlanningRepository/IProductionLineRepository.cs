using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface IProductionLineRepository : IRepository<PLAN_ProductionLine>
    {
        List<PLAN_ProductionLine> GetProductionLine(int startPage, int pageSize, out int totalRecords, PLAN_ProductionLine model, SearchFieldModel searchFieldModel);
        List<PLAN_ProductionLine> GetProductionLineBySearchKey(SearchFieldModel searchFieldModel);
        PLAN_ProductionLine GetProductionLineById(int productionLineId);
        List<PLAN_ProductionLine> GetProductionLineByBranchUnitDepartmentId(int branchUnitDepartmentId);
    }
}
