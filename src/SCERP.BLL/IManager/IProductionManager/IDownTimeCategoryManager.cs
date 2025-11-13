using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IDownTimeCategoryManager
    {
        List<PROD_DownTimeCategory> GetDownTimeCategorys(string compId);
    }
}
