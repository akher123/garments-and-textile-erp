using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IWorkShiftRepository : IRepository<WorkShift>
    {
        WorkShift GetWorkShiftById(int? id);
        List<WorkShift> GetAllWorkShiftsByPaging(int startPage, int pageSize,out int totalRecords, WorkShift workShift);
        List<WorkShiftRosterDetail> GetWorkShiftRoster(WorkShiftRoster shiftRoster);
        int SaveWorkShiftRoster(WorkShiftRoster shiftRoster);
        int ChangeWorkShiftRoster(WorkShiftRoster shiftRoster);
    }
}