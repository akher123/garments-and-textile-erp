using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class WeekendRepository:Repository<Weekend>,IWeekendRepository
    {
        public WeekendRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
