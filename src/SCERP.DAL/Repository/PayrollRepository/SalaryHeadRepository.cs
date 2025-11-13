using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class SalaryHeadRepository:Repository<SalaryHead>,ISalaryHeadRepository
    {
        public SalaryHeadRepository(SCERPDBContext context) : base(context)
        {

        }

        public SalaryHead GetSalaryHead()
        {
            return Context.SalaryHeads.FirstOrDefault();
        }
    }
}
