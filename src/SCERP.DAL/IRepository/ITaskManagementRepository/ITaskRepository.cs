using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.DAL.IRepository.ITaskManagementRepository
{
   public interface ITaskRepository:IRepository<TmTask>
    {
       IQueryable<vwTmTaskInformation> GetvwTmTaskInformation(Expression<Func<vwTmTaskInformation, bool>> predicates);
    }
}
