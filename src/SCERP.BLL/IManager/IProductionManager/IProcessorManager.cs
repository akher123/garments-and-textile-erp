using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IProcessorManager
    {
       List<PROD_Processor> GetProcessorLsit();
       List<VProcessor> GetProcessorByPaging(PROD_Processor model, out int totalRecords);
       List<PLAN_Process> GetProcessList();
       int SaveProcessor(PROD_Processor processor);
       string GetProcessorNewRefId();
       int EditProcessor(PROD_Processor processor);
       bool CheckExistingProcessor(PROD_Processor model);
       PROD_Processor GetProcessorById(int processorId);
       int DeleteProcessor(string processorRefId);


       List<PROD_Processor> GetProcessorByProcessRefId(string knitting, string compId);
    }
}
