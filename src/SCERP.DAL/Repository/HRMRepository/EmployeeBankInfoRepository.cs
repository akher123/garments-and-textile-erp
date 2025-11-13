using System;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeBankInfoRepository:Repository<EmployeeBankInfo>,IEmployeeBankInfoRepository
    {
        public EmployeeBankInfoRepository(SCERPDBContext context) : base(context)
        {
        }

        public EmployeeBankInfo GetLatestEmployeeBankInfoByEmployeeGuidId(Guid employeeId)
        {
            IQueryable<EmployeeBankInfo> employeeBankInfos;
            try
            {
                employeeBankInfos =
                    Context.EmployeeBankInfoes.Where(x => x.IsActive && x.EmployeeId == employeeId).OrderByDescending(x => x.BankName);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeBankInfos.ToList().FirstOrDefault();
        }
    }
}
