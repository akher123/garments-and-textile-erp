using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Data;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeAppointmentManager
    {
        string GetEmployeeAppointmentInfo(Guid employeeId, string userName, DateTime prepareDate);
        string GetEmployeeAppointmentInfoNew(Guid employeeId, string userName, DateTime prepareDate);
        string GetEmployeeAppointmentInfoStaffNew(Guid employeeId, string userName, DateTime prepareDate);
        string GetFinalSettlementInfo(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        string GetFinalSettlementInfo08PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        string GetFinalSettlementInfo10PMNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        string GetFinalSettlementInfo10PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        string GetFinalSettlementInfoOriginalNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
    }
}