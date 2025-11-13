using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class FabricTypeManager : IFabricTypeManager
    {
        private readonly IFabricTypeRepository _fabricTypeRepository;
        public FabricTypeManager(IFabricTypeRepository fabricTypeRepository)
        {
            _fabricTypeRepository = fabricTypeRepository;
        }

        public List<OM_FabricType> GetFabricTypes()
        {
            return _fabricTypeRepository.All().ToList();
        }
    }
}
