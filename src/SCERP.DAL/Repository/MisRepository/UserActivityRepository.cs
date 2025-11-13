using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMisRepository;
using SCERP.Model.MisModel;

namespace SCERP.DAL.Repository.MisRepository
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly SCERPDBContext _context;
        public UserActivityRepository(SCERPDBContext context)
        {
            _context = context;
        }

        public List<MIS_UserActivity> GetUserActivityList()
        {
            return _context.Database.SqlQuery<MIS_UserActivity>("exec spMisUserActivity").ToList();
        }

    }
}
