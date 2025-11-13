using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IMaintenance
{
   public interface IReturnableChallanReceiveMasterManager
    {
       List<Maintenance_ReturnableChallanReceiveMaster> GetChallanReceiveMasterByPaging(int pageIndex, string sort, string sortdir, string searchString,string compId,string challanType, out int totalRecords);
       int EditChallanReceiveMaster(Maintenance_ReturnableChallanReceiveMaster model);
       int SaveChallanReceiveMaster(Maintenance_ReturnableChallanReceiveMaster model);
       Maintenance_ReturnableChallanReceiveMaster GetChallanReceiveMasterByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId);
       int DeleteReturnableChallanReceiveMaster(long returnableChallanReceiveMasterId, string compId);

       DataTable GetReturnableChallanReceive(long returnableChallanReceiveMasterId, string compId); 
    }
}
