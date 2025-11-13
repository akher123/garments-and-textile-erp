using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.DAL.Repository.TrackingRepository
{
    public class VehicleRepository : Repository<TrackVehicle>, IVehicleRepository
    {
        public VehicleRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
