using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IMaterialReceiveAgainstPoRepository:IRepository<Inventory_MaterialReceiveAgainstPo>
    {
        List<VwMaterialReceiveAgainstPoDetail> VwMaterialReceiveAgainstPoDetail(long lecevedAgainstPoId);
        List<VwMaterialReceiveAgainstPo> GetMrrSummaryReport(string compId, DateTime? fromDate, DateTime? toDate, string searchString);

        DataTable GetAccessoriesStatusDataTable(string modelOrderStyleRefId, string compId);
        DataTable GetAccessoriesRcvDetailStatus(DateTime? fromDate, DateTime? toDate, string compId);
    }
}
