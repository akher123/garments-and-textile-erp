using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.Repository;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IStylePaymentRepository : IRepository<Acc_StylePayment>
    {
        IQueryable<VStylePayment> GetStylePaymentView(Expression<Func<VStylePayment, bool>> predicate);
    }
}
