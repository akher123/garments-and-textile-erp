using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IVoucherDetailRepository:IRepository<Acc_VoucherDetail>
    {
        int UpdateVoucherDetaiByCurrency(Acc_Currency model);
    }
}
