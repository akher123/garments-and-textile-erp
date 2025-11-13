using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class AccCurrencyRepository :Repository<Acc_Currency>, IAccCurrencyRepository
    {
        public AccCurrencyRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
