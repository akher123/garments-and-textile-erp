using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IPortOfLoadingRepository:IRepository<OM_PortOfLoading>
   {
       string GetNewPortOfLoadingfId(string compId);
   }
}
