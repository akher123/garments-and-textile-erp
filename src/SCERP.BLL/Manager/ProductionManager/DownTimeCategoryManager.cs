using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class DownTimeCategoryManager : IDownTimeCategoryManager
    {
        private readonly IDownTimeCategoryRepository _downTimeCategoryRepository;
        public DownTimeCategoryManager(IDownTimeCategoryRepository downTimeCategoryRepository)
        {
            _downTimeCategoryRepository = downTimeCategoryRepository;
        }

        public List<PROD_DownTimeCategory> GetDownTimeCategorys(string compId)
        {
            return _downTimeCategoryRepository.Filter(x => x.CompId == compId).ToList();
        }
    }
}
