using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IFabricTypeManager
    {
       List<OM_FabricType> GetFabricTypes();
    }
}
