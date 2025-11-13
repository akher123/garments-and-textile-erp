using SCERP.Model.MerchandisingModel;
using System.Collections.Generic;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IMCostingManager
    {
        int SaveCost(OM_Costing model);
      
        OM_Costing GetCostById(int costingId);
        int DeleteCost(int costingId);
        List<OM_Costing> GetCostByPaging(OM_Costing costing, int pageIndex, string sort, string sortdir, out int totalRecords);
        int UpdateCosting(int costingId, string fieldName, string value);
        OM_Costing GetUpdatedCosting(int costingId);
    }

}
