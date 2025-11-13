using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model.PayrollModel;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class AdvanceIncomeTaxRepository:Repository<AdvanceIncomeTax>,IAdvanceIncomeTaxRepository
    {
        public AdvanceIncomeTaxRepository(SCERPDBContext context) : base(context)
        {


        }
    }
}
