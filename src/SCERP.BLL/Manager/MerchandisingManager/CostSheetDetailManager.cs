using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class CostSheetDetailManager: ICostSheetDetailManager
   {
       private readonly ICostSheetDetailRepository _costSheetDetailRepository;

       public CostSheetDetailManager(ICostSheetDetailRepository costSheetDetailRepository)
       {
           _costSheetDetailRepository = costSheetDetailRepository;
       }

   }
}
