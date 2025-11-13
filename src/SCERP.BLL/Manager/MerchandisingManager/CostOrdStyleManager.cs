using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class CostOrdStyleManager : ICostOrdStyleManager
    {
        private readonly ICostOrdStyleRepository _costOrdStyleRepository;
        private readonly string _compId;
        public CostOrdStyleManager(ICostOrdStyleRepository costOrdStyleRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _costOrdStyleRepository = costOrdStyleRepository;
        }

        public List<VCostOrderStyle> GetCostCostOrdStyle(string orderStyleRefId)
        {
            return _costOrdStyleRepository.GetVCostCostOrdStyle(x => x.OrderStyleRefId == orderStyleRefId).ToList();
        }

        public VCostOrderStyle GetCostCostOrderById(long costOrderStyleId)
        {
            return _costOrdStyleRepository.GetVCostCostOrdStyle(x => x.CompId == PortalContext.CurrentUser.CompId).FirstOrDefault(x=>x.CostOrderStyleId==costOrderStyleId);
        }

        public int SaveCostOrdStyle(OM_CostOrdStyle model)
        {
            model.CompId = _compId;
           
            return _costOrdStyleRepository.Save(model);
        }

        public int EditCostOrdStyle(OM_CostOrdStyle model)
        {
            var costOrderStyle = _costOrdStyleRepository.FindOne(x => x.CostOrderStyleId == model.CostOrderStyleId&&x.CompId==_compId);
            costOrderStyle.CostRate = model.CostRate;
            costOrderStyle.CostRefId = model.CostRefId;
            costOrderStyle.CostDate = model.CostDate;
            costOrderStyle.Unit = model.Unit;
            costOrderStyle.Qty = model.Qty;
            return _costOrdStyleRepository.Edit(costOrderStyle);
        }

        public int DeleteCostOrdStyle(long costOrderStyleId)
        {
            return _costOrdStyleRepository.Delete(x => x.CostOrderStyleId == costOrderStyleId&&x.CompId==_compId);
        }

        public bool IsPreCostExit(string orderStyleRefId)
        {
            return _costOrdStyleRepository.Exists(x => x.OrderStyleRefId == orderStyleRefId);
        }

        public int SaveCostOrdStyleList(List<OM_CostOrdStyle> costOrdStyles)
        {
            return _costOrdStyleRepository.SaveList(costOrdStyles);
        }
    }
}
