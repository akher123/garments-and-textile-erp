using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.DAL.Repository.TaskManagementRepository
{
    public class TaskRepository : Repository<TmTask>, ITaskRepository
    {
        public TaskRepository(SCERPDBContext context) : base(context)
        {
        }
        public IQueryable<vwTmTaskInformation> GetvwTmTaskInformation(Expression<Func<vwTmTaskInformation, bool>> predicates)
        {
            return Context.VwTmTaskInformations.Where(predicates);
        }
    }
}
