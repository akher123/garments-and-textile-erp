using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface ISalaryMappingRepository : IRepository<Acc_SalaryMapping>
    {
        List<SalaryHead> GetAllSalaryHead();

        int GetAccountId(decimal AccountCode);

        List<SalaryHead> GetSalaryHead();

        string GetAccountNamesById(int Id);
    }
}
