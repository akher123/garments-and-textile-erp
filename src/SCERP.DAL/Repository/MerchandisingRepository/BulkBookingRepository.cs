using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BulkBookingRepository :Repository<OM_BulkBooking>, IBulkBookingRepository
    {
        public BulkBookingRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
