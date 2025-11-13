using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IConsumptionDetailRepository:IRepository<OM_ConsumptionDetail>
   {
       List<VConsumptionDetail> GetVConsumptionDetails(string consRefId, string compId);
       DataTable GetGColorList(string orderStyleRefId, string compId);
       DataTable GetGSizeList(string orderStyleRefId, string compId);

       DataTable GetVConsumptionDetailsByStyleRefId(string orderStyleRefId, string compId);
       DataTable GetAccessoriesConsumptionDetail(string orderStyleRefId, string compId);
       
       List<SPOrderStyleDetailForBOM> GetOrderStyleDetailForBOM(string orderStyleRefId, string compId);
       DataTable GetAccessoriesConsumptionDetailByOrder(string orderNo, string compId);
   }
}
