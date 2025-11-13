using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface ICostSheetMasterManager
    {
       List<OM_CostSheetMaster> GetCostSheetMasterByPaging(long buyerId, string orderNo, string color, int pageIndex, string sort, string sortdir, out int totalRecords);
       string GetCostSheetMasterRefId();
       OM_CostSheetMaster GetCostSheetMasterById(long costSheetMasterId, string compId);
       List<OM_CostSheetTemplate> GetCostSheetTemplateDetailByCostSheetMasterId(long costSheetMasterId, string compId);
        int SaveCostSheetMaster(OM_CostSheetMaster model);
        int EditCostSheetMaster(OM_CostSheetMaster model);
       int DeleteCostSheetMaster(long costSheetMasterId); 
    }
}
