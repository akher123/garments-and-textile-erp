using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class MachineLogManager : IMachineLogManager
    {
        public IMachineLogRepository _MachineLogRepository;
        public MachineLogManager(IMachineLogRepository machineLogRepository)
        {
            _MachineLogRepository = machineLogRepository;
        }

        public List<TrackMachineLog> GetAllMachineLogByPaging(TrackMachineLog model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            var machineLogList =_MachineLogRepository.Filter(
                x =>x.IsActive==true && ((x.Production_Machine.Name.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                    || (x.Employee.Name.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                    || (x.TrackMachineAction.MachineActionName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                    )).Include(x => x.Employee).Include(x => x.Production_Machine).Include(x => x.TrackMachineAction);
            totalRecords = machineLogList.Count();
            switch (model.sort)
            {
                case "Production_Machine.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            machineLogList = machineLogList
                                 .OrderByDescending(r => r.Production_Machine.Name).ThenBy(r => r.Production_Machine.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            machineLogList = machineLogList
                                 .OrderBy(r => r.Production_Machine.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "Employee.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            machineLogList = machineLogList
                                 .OrderByDescending(r => r.Employee.Name).ThenBy(r=>r.Employee.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            machineLogList = machineLogList
                                 .OrderBy(r => r.MachineId).ThenBy(r => r.Employee.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    machineLogList = machineLogList
                        .OrderByDescending(r => r.MachineLogId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return machineLogList.ToList();
        }
        public string GetNewMachineLogRefId()
        {
            var maxRefId = _MachineLogRepository.All().Max(x => x.MachineLogRefId);
            return maxRefId.IncrementOne().PadZero(10);
        }
        public bool IsMachineLogExist(TrackMachineLog model)
        {
            DateTime actionDate = Convert.ToDateTime(model.ActionDate);
            DateTime actionDateTime = DateTime.Parse(actionDate.ToString("yyyy-MM-dd ") + model.ActionTime);
 
            return _MachineLogRepository.Exists(
                 x =>
                     x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.MachineLogId != model.MachineLogId && x.MachineId == model.MachineId && x.MachineActionId == model.MachineActionId && x.EmployeeId == model.EmployeeId && x.ActionDate == actionDateTime);
        }
        public int SaveMachineLog(TrackMachineLog model)
        {
            DateTime actionDate = Convert.ToDateTime(model.ActionDate);
            model.ActionDate = DateTime.Parse(actionDate.ToString("yyyy-MM-dd ") + model.ActionTime); 
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            model.MachineLogRefId = GetNewMachineLogRefId();
            return _MachineLogRepository.Save(model);
        }

        public int EditMachineLog(TrackMachineLog model)
        {
            TrackMachineLog machineLog = _MachineLogRepository.FindOne(x => x.IsActive == true && x.MachineLogId == model.MachineLogId);
            DateTime actionDate = Convert.ToDateTime(model.ActionDate);
            DateTime actionDateTime = DateTime.Parse(actionDate.ToString("yyyy-MM-dd ") + model.ActionTime);
            machineLog.ActionDate = actionDateTime;
            machineLog.MachineId = model.MachineId;
            machineLog.EmployeeId = model.EmployeeId;
            machineLog.MachineActionId = model.MachineActionId;
            machineLog.Remarks = model.Remarks;
            machineLog.CreatedBy = PortalContext.CurrentUser.UserId;
            machineLog.CreatedDate = DateTime.Now;
            return _MachineLogRepository.Edit(machineLog);
        }
        public int DeleteMachineLog(long machineLogId)
        {
            TrackMachineLog machineLog= _MachineLogRepository.FindOne(x =>x.IsActive==true && x.MachineLogId == machineLogId);
            machineLog.IsActive = false;
            machineLog.EditedBy = PortalContext.CurrentUser.UserId;
            machineLog.EditedDate = DateTime.Now; 
            return _MachineLogRepository.Edit(machineLog);
        }
        public TrackMachineLog GetMachineLogByMachineLogId(long machineLogId)
        {
            return _MachineLogRepository.FindOne(x => x.IsActive == true && x.MachineLogId == machineLogId);
        }
        
    }
}
