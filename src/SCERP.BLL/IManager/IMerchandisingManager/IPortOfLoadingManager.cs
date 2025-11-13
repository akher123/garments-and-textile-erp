using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IPortOfLoadingManager
    {
       List<OM_PortOfLoading> GetPortOfLoading();
       List<OM_PortOfLoading> GetPortOfLoadingsByPaging(OM_PortOfLoading model, out int totalRecords);
       string GetNewPortOfLoadingfId();

       OM_PortOfLoading GetPortOfLoadingById(int p);
       int EditPortOfLoading(OM_PortOfLoading model);
       int SavePortOfLoading(OM_PortOfLoading model);
       int DeletePortOfLoading(string portOfLoadingRefId);
       bool CheckExistingPortOfLoading(OM_PortOfLoading model);
    }
}
