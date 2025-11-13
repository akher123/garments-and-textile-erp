using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeePermanentAddressRepository : Repository<EmployeePermanentAddress>, IEmployeePermanentAddressRepository
    {
        public EmployeePermanentAddressRepository(SCERPDBContext context)
            : base(context)
        {
           
        }

        public EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeId)
        {
            var employee = Context.EmployeePermanentAddresses.Where(x => x.EmployeeId == employeeId && x.IsActive == true).AsNoTracking().FirstOrDefault();
            return employee;
        }

        public int SaveEmployeePermanentAddressInfo(EmployeePermanentAddress  employeePermanentAddress)
        {
            var saved = 0;

            try
            {
                Context.EmployeePermanentAddresses.Add(employeePermanentAddress);
                saved = Context.SaveChanges();
            }
            catch (Exception)
            {
                saved = 0;
            }

            return saved;
        }

        public List<EmployeePermanentAddress> GetEmployeePermanentAddressesByEmployeeGuidId(Guid employeeGuid)
        {
            List<EmployeePermanentAddress> permanentAddresses = null;

            try
            {
                permanentAddresses = Context.EmployeePermanentAddresses
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
            return permanentAddresses;
        }

        public EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeIdGuid, int id)
        {
            EmployeePermanentAddress employeePermanentAddress;
            try
            {
                employeePermanentAddress = Context.EmployeePermanentAddresses.Where(x => x.EmployeeId == employeeIdGuid && x.Id == id && x.IsActive == true).AsNoTracking().FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeePermanentAddress;
        }

        public EmployeePermanentAddress GetLatestEmployeePermanentAddressByEmployeeGuidId(Guid employeeId)
        {
            IQueryable<EmployeePermanentAddress> employeePermanentAddresses;
            try
            {
                employeePermanentAddresses =
                    Context.EmployeePermanentAddresses
                        .Where(x => x.IsActive && x.EmployeeId == employeeId).OrderByDescending(x => x.Id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeePermanentAddresses.ToList().FirstOrDefault();
        }

        
    }
}
