using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IConsumptionDetailManager
    {
       List<VConsumptionDetail> GetVConsumptionDetails(string consRefId);
       List<OM_Color> GetGColorList(string orderStyleRefId);
       List<OM_Size> GetGSizeList(string orderStyleRefId);
       int UpdateConsDetail(VConsumptionDetail model);
       int UpdateConsDetailByPcolor(VConsumptionDetail model);
       int UpdateConsDetailByPSize(VConsumptionDetail model);
       DataTable GetVConsumptionDetailsByStyleRefId(string orderStyleRefId);

       List<SPOrderStyleDetailForBOM> GetOrderStyleDetailForBOM(string orderStyleRefId);
       DataTable GetAccessoriesConsumptionDetail(string orderStyleRefId);
       int UpdateRemarks(VConsumptionDetail model);
       int UpdateProductQty (VConsumptionDetail model);
        DataTable GetAccessoriesConsumptionDetailByOrder(string orderNo);
    }
}
