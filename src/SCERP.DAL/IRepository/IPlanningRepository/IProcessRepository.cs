using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
   public interface IProcessRepository:IRepository<PLAN_Process>
   {
       string GetProcessById(string compId);
   }
}
