using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeePresentAddressRepository : IRepository<EmployeePresentAddress>
    {
        EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeIdGuid);
        List<EmployeePresentAddress> GetEmployeePresentAddressesByEmployeeGuidId(Guid employeeGuid);
        EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeIdGuid, int id);
        EmployeePresentAddress GetLatestEmployeePresentAddressByEmployeeGuidId(Guid employeeId);
    }
}
