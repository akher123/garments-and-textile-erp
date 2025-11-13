using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MisModel;

namespace SCERP.BLL.IManager.IMisManager
{
   public interface IUserActivityManager
    {
       List<MIS_UserActivity> GetUserActivityList();
    }
}
