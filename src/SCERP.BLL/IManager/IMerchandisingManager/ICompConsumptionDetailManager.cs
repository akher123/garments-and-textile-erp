using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ICompConsumptionDetailManager
    {
        List<VCompConsumptionDetail> GetVCompConsumptionDetails(int? compomentSlNo, string consRefId, string orderStyleRefId);
        int UpdateCompConsDetail(VCompConsumptionDetail model,string updateKey);
        List<VCompConsumptionDetail> GetComConsumptionsFabric(string orderStyleRefId);

        int UpdateFabricSize(VCompConsumptionDetail model);
        int UpdateGrWidh(VCompConsumptionDetail model);
        int UpdateGrColor(VCompConsumptionDetail model);
        int UpdateFabricConsQty(string consRefId);

        int UpdateCollarCuffConsDetail(VCompConsumptionDetail model, string updateKey);
        int UpdateProductColor(VCompConsumptionDetail model);
    }
}
