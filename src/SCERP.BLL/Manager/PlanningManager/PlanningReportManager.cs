using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class PlanningReportManager : IPlanningReportManager
    {
        private readonly IPlanningReportRepository planningReportRepository;

        public PlanningReportManager(IPlanningReportRepository planningReportRepository)
        {
            this.planningReportRepository = planningReportRepository;
        }

        public List<PLAN_TNAReport> GetTNAReportData(string buyerRefId,string orderNo,string orderStyleRefId)
        {
            var userId = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            return planningReportRepository.GetTNAReportData(buyerRefId, orderNo, orderStyleRefId, userId);
        }

        public List<PLAN_TNAReport> GetTnaResponsePersonReportData(string person)
        {
            return planningReportRepository.GetTnaResponsePersonReportData(person);
        }

        public List<PLAN_TNAHorizontal> GetTnaGroupUpdateReport(PLAN_TNAHorizontal tnaHorizonal)
        {
            return planningReportRepository.GetTnaGroupUpdateReport(tnaHorizonal);
        }

        public DataTable GetStyleWiseSmv()
        {
            return planningReportRepository.GetStyleWiseSmv(PortalContext.CurrentUser.CompId);
        }

        public DataTable GetMachineCapacity(string processRefId)
        {
            return planningReportRepository.GetMachineCapacity(PortalContext.CurrentUser.CompId, processRefId);
        }

        public DataTable GetStyleWiseSmvDetal(DateTime? fromDate, DateTime? toDate)
        {
            return planningReportRepository.GetStyleWiseSmvDetal(PortalContext.CurrentUser.CompId, fromDate,toDate);
        }

        public DataTable GetSweingOrderPlanStatus(string compId)
        {
            return planningReportRepository.GetSweingOrderPlanStatus(compId);
        }
    }
}