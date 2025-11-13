using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IEmployeeSalaryProcessConfirmRepository : IRepository<EmployeeSalary_Processed>
    {
        IQueryable<EmployeeSalary_Processed_Temp> GetEmployeeSalaryTemp();
        int TruncateTempTable();
        string SaveMaster(Acc_VoucherMaster voucherMaster);
        string SaveDetail(Acc_VoucherDetail voucherDetail);
    }
}
