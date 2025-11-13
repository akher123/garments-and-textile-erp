using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IHourManager
    {
       List<PROD_Hour> GetAllHour();

       List<PROD_Hour> GetAllHourByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString);

       PROD_Hour GethourById(int hourId, string compId);
       bool IsHourExist(PROD_Hour model);
       int EditHour(PROD_Hour model);
       int SaveHour(PROD_Hour model);
       int DeleteHour(long hourId);
       string GetNewHourRefId(); 
    }
}
