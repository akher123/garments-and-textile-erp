using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface ICostDefinationManager
    {
       List<OM_CostDefination> GetCostDefinationPaging(OM_CostDefination model, out int totalRecords);
       OM_CostDefination GetCostDefinationById(int costDefinationId);
       string GetNewCostRefId();
       int EditCostDefination(OM_CostDefination model);
       int SaveCostDefination(OM_CostDefination model);
       int DeleteCostDefination(string costRefId);
       List<OM_CostDefination> GetCostDefination();
       bool CheckExistingCostDefination(OM_CostDefination model);
       List<OM_CostDefination> GetCostDefinationByCostGroup(string costGroupId, string compId);
    }
}
