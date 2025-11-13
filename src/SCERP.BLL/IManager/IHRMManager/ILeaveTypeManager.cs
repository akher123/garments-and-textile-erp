using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ILeaveTypeManager
    {

        int SaveLeaveType(LeaveType leaveType);

        int EditLeaveType(LeaveType leaveType);

        LeaveType GetLeaveTypeById(int? id);

        int DeleteLeaveType(LeaveType leaveType);

        List<LeaveType> GetAllLeaveTypes(int startPage, int pageSize, out int totalRecords, LeaveType leaveType);
        bool IsLeaveTypeExist(LeaveType model);
        List<LeaveType> GetLeaveTypeBySearchKey(LeaveType leaveType);
        List<LeaveType> GetAllLeaveTypes();

    }
}
