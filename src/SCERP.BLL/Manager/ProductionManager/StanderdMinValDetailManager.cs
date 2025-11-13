using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.ProductionRepository;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class StanderdMinValDetailManager : IStanderdMinValDetailManager
    {
        private IStanderdMinValDetailRepository _standerdMinValDetailRepository;
        public StanderdMinValDetailManager(IStanderdMinValDetailRepository standerdMinValDetailRepository)
        {
            _standerdMinValDetailRepository = standerdMinValDetailRepository;
        }
    }
}
