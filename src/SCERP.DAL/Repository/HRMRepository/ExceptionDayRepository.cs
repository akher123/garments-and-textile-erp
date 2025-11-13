using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class ExceptionDayRepository : Repository<ExceptionDay>, IExceptionDayRepository
    {
        public ExceptionDayRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
