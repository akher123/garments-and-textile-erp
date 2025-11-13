using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface ICostSheetTemplateManager
    {
       List<OM_CostSheetTemplate> GetCostSheetTemplateByPaging(string particular, int pageIndex, string sort, string sortdir, out int totalRecords);
       OM_CostSheetTemplate GeTcostSheetTemplateByTemplateId(int templateId, string compId);
       bool IsCostSheetTemplateExist(OM_CostSheetTemplate model);
       int EditCostSheetTemplate(OM_CostSheetTemplate model);
       int SaveCostSheetTemplate(OM_CostSheetTemplate model);
       int DeleteCostSheetTemplate(int templateId, string compId);
       List<OM_CostSheetTemplate> GetCostSheetTemplateByItemTypeId(int itemTypeId, string compId); 
    }
}
