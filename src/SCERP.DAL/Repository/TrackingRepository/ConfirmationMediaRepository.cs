using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;


namespace SCERP.DAL.Repository.TrackingRepository
{
    public class ConfirmationMediaRepository : Repository<TrackConfirmationMedia>, IConfirmationMediaRepository
    {
        public ConfirmationMediaRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
