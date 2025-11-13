using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeePresentAddressRepository : Repository<EmployeePresentAddress>, IEmployeePresentAddressRepository
    {
        public EmployeePresentAddressRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeId)
        {
            EmployeePresentAddress employee = Context.EmployeePresentAddresses.Where(x => x.EmployeeId == employeeId && x.IsActive == true).AsNoTracking().FirstOrDefault();
            return employee;
        }

        public List<EmployeePresentAddress> GetEmployeePresentAddressesByEmployeeGuidId(Guid employeeGuid)
        {
            List<EmployeePresentAddress> presentAddresses = null;

            try
            {
                presentAddresses = Context.EmployeePresentAddresses
                    .Where(x => x.EmployeeId == employeeGuid && x.IsActive == true).AsNoTracking()
                     .Include(x => x.Country)
                     .Include(x => x.District)
                     .Include(x => x.PoliceStation)
                     .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return presentAddresses;
        }


        public EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeIdGuid, int id)
        {
            EmployeePresentAddress employeePresentAddress;
            try
            {
                employeePresentAddress = Context.EmployeePresentAddresses.Where(x => x.EmployeeId == employeeIdGuid && x.Id==id && x.IsActive == true).AsNoTracking().FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeePresentAddress;
        }

        public EmployeePresentAddress GetLatestEmployeePresentAddressByEmployeeGuidId(Guid employeeId)
        {
            IQueryable<EmployeePresentAddress> employeePresentAddresses;
            try
            {
                employeePresentAddresses =
                    Context.EmployeePresentAddresses
                        .Where(x => x.IsActive && x.EmployeeId == employeeId).OrderByDescending(x => x.Id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeePresentAddresses.ToList().FirstOrDefault();
        }


        
    }
}
