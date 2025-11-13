using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BulkBookingYarnDetailRepository :Repository<OM_BulkBookingYarnDetail>, IBulkBookingYarnDetailRepository
    {
        public BulkBookingYarnDetailRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
