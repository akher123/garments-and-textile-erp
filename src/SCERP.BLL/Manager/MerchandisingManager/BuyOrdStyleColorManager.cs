using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BuyOrdStyleColorManager : IBuyOrdStyleColorManager
    {
        private readonly IBuyOrdStyleColorRepository _ordStyleColorRepository;
        private readonly string _compId;
        public BuyOrdStyleColorManager(IBuyOrdStyleColorRepository ordStyleColorRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _ordStyleColorRepository = ordStyleColorRepository;
        }

        public List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId)
        {
            return _ordStyleColorRepository.GetBuyOrdStyleColor(orderStyleRefId, _compId);
        }

        public VBuyOrdStyleColor GetBuyOrdStyleColorById(long id)
        {
            return _ordStyleColorRepository.GetBuyOrdStyleColorById(id);
        }
      

    }
}
