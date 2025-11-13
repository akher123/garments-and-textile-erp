using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ILeaveTypeRepository : IRepository<LeaveType>
    {
        LeaveType GetLeaveTypeById(int? id);
        List<LeaveType> GetAllLeaveTypes(int startPage, int pageSize, out int totalRecords, LeaveType leaveType);
        List<LeaveType> GetLeaveTypeBySearchKey(LeaveType leaveType);
    }
}
