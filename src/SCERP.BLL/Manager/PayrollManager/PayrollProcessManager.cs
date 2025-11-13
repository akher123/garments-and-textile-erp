using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model.PayrollModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class PayrollProcessManager : IPayrollProcessManager
    {
        private readonly IPayrollProcessRepository payrollProcessRepository;
        public PayrollProcessManager(IPayrollProcessRepository payrollProcessRepository)
        {
            this.payrollProcessRepository = payrollProcessRepository;
        }

        public int ProcessBonus(int bonusRuleId)
        {
            int processed = payrollProcessRepository.ProcessBonus(bonusRuleId);
            return processed;
        }
    }
}
