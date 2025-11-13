using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IPayrollProcessRepository
    {
        int ProcessBonus(int bonusRuleId);
    }
}
