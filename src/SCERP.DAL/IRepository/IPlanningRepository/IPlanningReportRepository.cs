using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface IPlanningReportRepository
    {
        List<PLAN_TNAReport> GetTNAReportData(string buyerRefId, string orderNo, string orderStyleRefId,Guid userId);
        List<PLAN_TNAReport> GetTnaResponsePersonReportData(string person);
        List<PLAN_TNAHorizontal> GetTnaGroupUpdateReport(PLAN_TNAHorizontal tnaHorizonal);
        DataTable GetStyleWiseSmv(string compId);
        DataTable GetMachineCapacity(string compId,string processRefId);
        DataTable GetStyleWiseSmvDetal(string compId, DateTime? fromDate, DateTime? toDate);

        DataTable GetSweingOrderPlanStatus(string compId);
    }
}
