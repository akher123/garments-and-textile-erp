using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IMaterialReceivedRepository:IRepository<Inventory_MaterialReceived>
   {
       DataTable GetMaterialReceivedDataTable(DateTime? fromDate, DateTime? toDate, string challanNo,string registerType, string compId); 
   }
}
