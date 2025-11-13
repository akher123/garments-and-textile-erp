using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Data;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeAppointmentRepository
    {
        DataTable GetEmployeeAppointmentInfo(Guid employeeId, string userName, DateTime prepareDate);
        DataTable GetEmployeeAppointmentInfoNew(Guid employeeId, string userName, DateTime prepareDate);
        DataTable GetFinalSettlementInfo(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        DataTable GetFinalSettlementInfo08PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        DataTable GetFinalSettlementInfo10PMNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        DataTable GetFinalSettlementInfo10PM(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
        DataTable GetFinalSettlementInfoOriginalNoWeekend(Guid employeeId, string userName, DateTime prepareDate, decimal? othersDeduction);
    }
}