using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeFamilyInfoRepository : Repository<EmployeeFamilyInfo>, IEmployeeFamilyInfoRepository
    {
        public EmployeeFamilyInfoRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<EmployeeFamilyInfo> GetEmployeeFamilyInfoByEmployeeGuidId(Guid employeeGuid)
        {
            IQueryable<EmployeeFamilyInfo> employeeFamilyInfos;
            try
            {
                employeeFamilyInfos =
                    Context.EmployeeFamilyInfos
                        .Where(x => x.IsActive)
                        .Where(x => (x.EmployeeId == employeeGuid || employeeGuid == null) && (x.IsActive == true))
                        .Include(x=>x.Gender)
                        .OrderByDescending(x => x.EmployeeFamilyInfoId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeFamilyInfos.ToList();
        }

        public EmployeeFamilyInfo GetEmployeeFamilyInfoById(Guid employeeId, int id)
        {
            IQueryable<EmployeeFamilyInfo> employeeFamilyInfos;
            try
            {
                employeeFamilyInfos =
                    Context.EmployeeFamilyInfos
                        .Where(x => (x.EmployeeId == employeeId) && (x.EmployeeFamilyInfoId == id) && (x.IsActive == true));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeFamilyInfos.ToList().FirstOrDefault();
        }

        
    }
}
