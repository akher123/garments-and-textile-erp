using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MisModel;

namespace SCERP.DAL.IRepository.IMisRepository
{
    public interface IUserActivityRepository
    {
        List<MIS_UserActivity> GetUserActivityList();
    }
}
