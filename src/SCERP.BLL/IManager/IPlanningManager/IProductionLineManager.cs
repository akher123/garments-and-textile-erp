using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IProductionLineManager
    {
        List<PLAN_ProductionLine> GetProductionLine(int startPage, int pageSize, out int totalRecords, PLAN_ProductionLine model, SearchFieldModel searchFieldModel);

        List<PLAN_ProductionLine> GetProductionLineBySearchKey(SearchFieldModel searchFieldModel);
        int DeleteProductionLine(int productionLineId);
        PLAN_ProductionLine GetProductionLineById(int productionLineId);
        int EditProductionLine(PLAN_ProductionLine model);
        int SaveProductionLine(PLAN_ProductionLine model);
        List<PLAN_ProductionLine> GetProductionLineByBranchUnitDepartmentId(int branchUnitDepartmentId);
    }
}
