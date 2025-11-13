using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class MachineActionManager : IMachineActionManager
    {
        private IMachineActionRepsitory _machineActionRepsitory;

        public MachineActionManager(IMachineActionRepsitory machineActionRepsitory)
        {
            _machineActionRepsitory = machineActionRepsitory;
        }
        public List<TrackMachineAction> GetAllMachineActionByPaging(TrackMachineAction model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var machineList =
                _machineActionRepsitory.Filter(
                    x => x.IsActive == true && (x.MachineActionName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = machineList.Count();
            switch (model.sort)
            {
                case "MachineActionName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            machineList = machineList
                                 .OrderByDescending(r => r.MachineActionName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            machineList = machineList
                                 .OrderBy(r => r.MachineActionName).ThenBy(r => r.MachineActionName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    machineList = machineList
                        .OrderByDescending(r => r.MachineActionId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return machineList.ToList();
        }
        public List<TrackMachineAction> GetAllMachineAction()
        {
            return _machineActionRepsitory.All().OrderBy(y => y.MachineActionName).ToList();
        }
        public bool IsMachineActonExist(TrackMachineAction model)
        {
            return _machineActionRepsitory.Exists(
             x =>
                 x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.MachineActionId != model.MachineActionId && x.MachineActionName == model.MachineActionName);
        }
        public int SaveMachineAction(TrackMachineAction model)
        {
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _machineActionRepsitory.Save(model);
        }
        public int EditMachineAction(TrackMachineAction model)
        {
            TrackMachineAction machineAction = _machineActionRepsitory.FindOne(x => x.IsActive == true && x.MachineActionId == model.MachineActionId);
            machineAction.MachineActionName = model.MachineActionName;
            machineAction.Remarks = model.Remarks;
            machineAction.EditedBy = PortalContext.CurrentUser.UserId;
            machineAction.EditedDate = DateTime.Now;
            return _machineActionRepsitory.Edit(machineAction);
        }
        public int DeleteMachineAction(int machineActionId)
        {
            TrackMachineAction machineAction = _machineActionRepsitory.FindOne(x => x.IsActive == true && x.MachineActionId == machineActionId);
            machineAction.EditedBy = PortalContext.CurrentUser.UserId;
            machineAction.EditedDate = DateTime.Now;
            machineAction.IsActive = false;
            return _machineActionRepsitory.Edit(machineAction);
        }
        public TrackMachineAction GetMachineActionByMachineActionId(int machineActionId)
        {
            return _machineActionRepsitory.FindOne(x => x.IsActive == true && x.MachineActionId == machineActionId);
        }
    }
}
