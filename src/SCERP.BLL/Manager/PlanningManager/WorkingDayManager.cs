using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class WorkingDayManager : IWorkingDayManager
    {
        private readonly IWorkingDayRepository _workingDayRepository;
        public WorkingDayManager(IWorkingDayRepository workingDayRepository)
        {
            _workingDayRepository = workingDayRepository;
        }

        public List<PLAN_WorkingDay> GetWorkingDay(int workingDayStatus, DateTime? fromDate, DateTime? toDate, int index, out int totalRecords)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var pageSize = AppConfig.PageSize;
            IQueryable<PLAN_WorkingDay> workingDays = _workingDayRepository.Filter(x => x.IsActive && x.CompId == compId && (x.DayStatus == workingDayStatus || workingDayStatus==0) && ((x.WorkingDate >= fromDate || fromDate == null) && (x.WorkingDate <= toDate || toDate == null)));
            totalRecords = workingDays.Count();
            workingDays = workingDays
                       .OrderBy(r => r.WorkingDate)
                       .Skip(index * pageSize)
                       .Take(pageSize);
            return workingDays.ToList();
        }

        public int CreateWorkingDays(int cureentYar)
        {
            string compId = PortalContext.CurrentUser.CompId;
           bool isWorkingDayExist=   _workingDayRepository.Exists(x => x.WorkingDate.Year == cureentYar);
            int saveIndex = 0;
           if (!isWorkingDayExist)
            {
                List<DateTime> dates = new DateTime(cureentYar, 1, 1).Range(new DateTime(cureentYar, 12, 31)).ToList();
                List<PLAN_WorkingDay> workingDays = dates.Select(x => new PLAN_WorkingDay()
                {
                    WorkingDate = x,
                    CompId = compId,
                    Remarks = "Working Day",
                    IsActive = true,
                    DayStatus = (int)WorkingDayStatus.WorkingDay
                }).ToList();
                 saveIndex = _workingDayRepository.SaveList(workingDays);
            }
          else
          {
              throw new Exception("Working day of this " + cureentYar + " yar is created !");
          }
      
            return saveIndex;
        }

        public PLAN_WorkingDay GetWorkingDayById(long workingDayId)
        {   var compId = PortalContext.CurrentUser.CompId;
            return _workingDayRepository.FindOne(x => x.IsActive && x.CompId == compId && x.WorkingDayId == workingDayId);
        }

        public int EditWorkingDays(PLAN_WorkingDay workingDay)
        {
             var compId = PortalContext.CurrentUser.CompId;
            var workingday=   _workingDayRepository.FindOne( x => x.CompId == compId && x.IsActive && x.WorkingDayId == workingDay.WorkingDayId);
            workingday.DayStatus = workingDay.DayStatus;
            workingday.Remarks = workingDay.Remarks;

            return _workingDayRepository.Edit(workingday);
        }
    }
}
