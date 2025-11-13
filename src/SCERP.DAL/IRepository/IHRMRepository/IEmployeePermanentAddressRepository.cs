using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeePermanentAddressRepository : IRepository<EmployeePermanentAddress>
    {
        EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeIdGuid);
        int SaveEmployeePermanentAddressInfo(EmployeePermanentAddress employeePermanentAddress);
        List<EmployeePermanentAddress> GetEmployeePermanentAddressesByEmployeeGuidId(Guid employeeGuid);
        EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeGuid, int id);
        EmployeePermanentAddress GetLatestEmployeePermanentAddressByEmployeeGuidId(Guid employeeId);

    }
}
