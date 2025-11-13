using SCERP.DAL.IRepository.IPayrollRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class PayrollProcessRepository : IPayrollProcessRepository
    {
        private readonly SCERPDBContext context; 
        public PayrollProcessRepository(SCERPDBContext context)
        {
            this.context = context;

        }
        public int ProcessBonus(int bonusRuleId)
        {
            return context.Database.ExecuteSqlCommand("exec [dbo].[Utility_BonusProcess] {0}", bonusRuleId);
        }
    }
}
