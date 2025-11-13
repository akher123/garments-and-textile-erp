using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.DAL.Repository.TaskManagementRepository
{
    public class ReportImageRepository : Repository<TmReportImageInfo>, IReportImageRepository
    {
        public ReportImageRepository(SCERPDBContext context)
            : base(context)
        {

        }
    }
}
