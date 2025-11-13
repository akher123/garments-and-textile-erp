using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IMaintenance
{
   public interface IReturnableChallanReceiveMasterRepository:IRepository<Maintenance_ReturnableChallanReceiveMaster>
    {
       DataTable GetReturnableChallanReceive(long returnableChallanReceiveMasterId, string compId); 
    }
}
