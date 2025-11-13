using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttFabRejectManager
    {
       List<VwCuttFabReject> GetCuttFabRejectsByPaging(string searchString, int pageIndex, out int totalRecord);
        PROD_CuttFabReject GetCuttFabRejectById(int cuttFabRejectId);
        int SaveFabReject(PROD_CuttFabReject cuttFabReject);
        VwCuttFabReject GetVwCuttFabRejectById(int cuttFabRejectId);
        int DeleteCuttFabReject(int cuttFabRejectId);
    }
}
