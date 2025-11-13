using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Model.HRMModel;
using System;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IWorkShiftManager
    {
        List<WorkShift> GetAllWorkShiftsByPaging(int startPage, int pageSize, WorkShift WorkShift, out int totalRecords);
        int SaveWorkShift(WorkShift aDepartment);
        IQueryable<WorkShift> GetAllWorkShifts();
        WorkShift GetWorkShiftById(int? id);
        int EditWorkShift(WorkShift department);
        int DeleteWorkShift(int id);
        List<WorkShift> GetAllWorkShiftsBySearchKey(string searchKey);
        bool IsWorkShiftExist(WorkShift workShift);
        List<WorkShiftRosterDetail> GetWorkShiftRoster(WorkShiftRoster shiftRoster);
        int SaveWorkShiftRoster(WorkShiftRoster shiftRoster);
        int ChangeWorkShiftRoster(WorkShiftRoster shiftRoster);
    }
}