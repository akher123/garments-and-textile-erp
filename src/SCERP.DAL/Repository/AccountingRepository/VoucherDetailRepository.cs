using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class VoucherDetailRepository : Repository<Acc_VoucherDetail>, IVoucherDetailRepository
    {
        public VoucherDetailRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public int UpdateVoucherDetaiByCurrency(Acc_Currency model)
        {
            var updateQueryString = string.Format("update Acc_VoucherDetail set FirstCurValue='" + model.FirstCurValue +
                                       "' ,SecendCurValue='" + model.SecendCurValue + "',ThirdCurValue='" +
                                       model.ThirdCurValue + "'" +
                                       "where  FirstCurValue<='" + 0.00 + "' or  SecendCurValue<='" + 0.00 + "'or   ThirdCurValue<='" + 0.00 + "'");
            var efrows = Context.Database.ExecuteSqlCommand(updateQueryString);
            return efrows;
        }
    }
}


