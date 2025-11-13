using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IDyeingJobOrderManager
    {
        List<PROD_DyeingJobOrder> GetDyeingJobOrderByPaging(string searchString, int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, long partyId, out int totalRecord);
        string GetJobRefId(string compId);
        PROD_DyeingJobOrder GetDyeingJobOrderById(long dyeingJobOrderId);
        int SaveDyeingJobOrder(PROD_DyeingJobOrder jobOrder);
        List<VwDyeingJobOrderDetail> GetDyeingJobOrderDetails(long dyeingJobOrderId);
        int EditDyeingJobOrder(PROD_DyeingJobOrder jobOrder);
        int DeleteDyeingJobOrderDetail(long dyeingJobOrderId);
        DataTable DyeingJobOrderReportDataTable(long dyeingJobOrderId);
        List<Dropdown> GetDyeingJobOrderByPartyId(long partyId);
        List<VwDyeingJobOrderDetail> LoadKnittingRollIssueChallan(string challanRefId);
        List<Dropdown> GetKnittingRollIssueChallan(string orderStyleRefId);
    }
}
