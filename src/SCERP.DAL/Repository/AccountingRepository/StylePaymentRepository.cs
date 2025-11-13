using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class StylePaymentRepository: Repository<Acc_StylePayment>, IStylePaymentRepository
    {
        public StylePaymentRepository(SCERPDBContext context) : base(context)
        {

        }
        public IQueryable<VStylePayment> GetStylePaymentView(Expression<Func<VStylePayment, bool>> predicate)
        {
            return Context.VStylePayments.Where(predicate);
        }
    }
}
