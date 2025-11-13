using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IProductionManager
    {
        List<VProductionDetail> GetProgramDetailsByProcessRefId(string processRefId, string programRefId, string pType);
        List<VwProduction> GetProductionByPaging(PROD_Production model);
        PROD_Production GetProductionById(long productionId);
        List<VProductionDetail> GetProductionDetailsByProductionIed(long productionId);
        string GetProductionRefId(string prifix);
        int SaveProduction(PROD_Production production);
        int EditProduction(PROD_Production production);
        List<VProgramDetail> GetVProgramLis(string prorgramRefId,string productionRefId, string pType);
        int DeleteProduction(PROD_Production model);
    }
}
