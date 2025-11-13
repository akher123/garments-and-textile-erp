using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IPlanningReportManager
    {
        List<PLAN_TNAReport> GetTNAReportData(string buyerRefId, string orderNo, string orderStyleRefId);
        List<PLAN_TNAReport> GetTnaResponsePersonReportData(string person);
        List<PLAN_TNAHorizontal> GetTnaGroupUpdateReport(PLAN_TNAHorizontal tnaHorizonal);
        DataTable GetStyleWiseSmv();
        DataTable GetMachineCapacity(string processRefId);
        DataTable GetStyleWiseSmvDetal(DateTime? fromDate, DateTime? toDate);
        DataTable GetSweingOrderPlanStatus(string compId);
    }
}
