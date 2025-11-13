using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;

namespace SCERP.BLL.Manager.HRMManager
{
    public class WeekendManager:BaseManager,IWeekendManager
    {
        public WeekendManager(SCERPDBContext context)
        {
            WeekendRepository=new WeekendRepository(context);
        }

        public IWeekendRepository WeekendRepository { get; set; }
    }
}
