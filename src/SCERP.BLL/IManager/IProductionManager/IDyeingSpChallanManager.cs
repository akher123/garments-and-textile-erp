using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IDyeingSpChallanManager 
    {
       List<PROD_DyeingSpChallan> GetAllDyeingSpChallanByPaging(int pageIndex, string sort, string sortdir, DateTime? toDate, DateTime? fromDate, string searchString, string compId, out int totalRecords);
       PROD_DyeingSpChallan GetDyeingSpChallanByDyeingSpChallanId(long dyeingSpChallanId, string compId);
       string GetDyeingSpChallanRefId();
       int EditDyeingSpChallan(PROD_DyeingSpChallan model);
       int SaveDyeingSpChallan(PROD_DyeingSpChallan model);
       int DeleteDyeingSpChallan(long dyeingSpChallanId, string compId);
       int UpdateApprovedSp(long dyeingSpChallanId);
       List<PROD_DyeingSpChallan> GetAllDyeingSpChallanList(string compId, bool isActive);
       IEnumerable DyeingSpChallanAutocomplite(string searchString, string compId);
       IEnumerable<dynamic> GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId);
        DataTable GetDyeingSpChallanByStyle(string orderStyleRefId);
    }
}
