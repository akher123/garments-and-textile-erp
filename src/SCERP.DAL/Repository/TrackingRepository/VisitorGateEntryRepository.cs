using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.DAL.Repository.TrackingRepository
{
    public class VisitorGateEntryRepository : Repository<TrackVisitorGateEntry>, IVisitorGateEntryRepository
    {
        public VisitorGateEntryRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
