using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface IEmployeeSalaryProcessConfirmManager
    {
        string ConfirmSalary(Acc_VoucherMaster voucherMaster);
    }
}
