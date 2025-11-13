using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.HRMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.TrackingRepository
{
    public class HouseKeepingItemRepository : Repository<HouseKeepingItem>, IHouseKeepingItemRepository
    {
        public HouseKeepingItemRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
