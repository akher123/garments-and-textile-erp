using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IThreadConsumptionManager
    {
       List<VwThreadConsumption> GetThreadConsumptionsByPaging(string compId, string searchString);
       int SaveThreadConsumption(OM_ThreadConsumption consumption);
       int EditThreadConsumption(OM_ThreadConsumption consumption);
       int DeleteThreadConsumption(int threadConsumptionId);
       OM_ThreadConsumption GetThreadConsumptionById(int threadConsumptionId);
       DataTable GetThreadConsumptionsReportDataTable(long threadConsumptionId);
       int ApproveThreadConsumption
           (int threadConsumptionId);
    }
}
