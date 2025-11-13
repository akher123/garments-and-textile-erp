using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttingProcessStyleActiveManager : ICuttingProcessStyleActiveManager
    {
        private readonly ICuttingProcessStyleActiveRepository _cuttingProcessStyleActiveRepository;
        private readonly ITimeAndActionManager timeAndActionManager;
        public CuttingProcessStyleActiveManager(ICuttingProcessStyleActiveRepository cuttingProcessStyleActiveRepository, ITimeAndActionManager timeAndActionManager)
        {
            _cuttingProcessStyleActiveRepository = cuttingProcessStyleActiveRepository;
            this.timeAndActionManager = timeAndActionManager;
        }
        public int SaveCuttingProcessStyleActive(PROD_CuttingProcessStyleActive model)
        {
            int saved = _cuttingProcessStyleActiveRepository.Save(model);
            if (saved > 0)
            {
                timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.BULKCUTTING, model.StartDate, model.OrderStyleRefId, model.CompId);
            };
            return saved;
        }

        public List<VwCuttingProcessStyleActive> GetCuttingProcessStyleActiveByPaging(int pageIndex, string sort, string sortdir, out int totalRecords,
            string buyerRefId, string orderNo, string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            List<VwCuttingProcessStyleActive> CuttingProcessStyleActiveList = _cuttingProcessStyleActiveRepository.GetCuttingProcessStyleActiveByPaging(compId, buyerRefId, orderNo, orderStyleRefId);
            totalRecords = CuttingProcessStyleActiveList.Count();
            return CuttingProcessStyleActiveList;
        }

        public PROD_CuttingProcessStyleActive GetStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return
                _cuttingProcessStyleActiveRepository.FindOne(
                    x => x.CompId == compId && x.CuttingProcessStyleActiveId == cuttingProcessStyleActiveId);
        }

        public int EditCuttingProcessStyleActive(PROD_CuttingProcessStyleActive model)
        {
            var cuttingProcessStyleActive = _cuttingProcessStyleActiveRepository.FindOne(x => x.CompId == model.CompId && x.CuttingProcessStyleActiveId == model.CuttingProcessStyleActiveId);
            cuttingProcessStyleActive.BuyerRefId = model.BuyerRefId;
            cuttingProcessStyleActive.OrderNo = model.OrderNo;
            cuttingProcessStyleActive.OrderStyleRefId = model.OrderStyleRefId;
            cuttingProcessStyleActive.StartDate = model.StartDate;
            cuttingProcessStyleActive.EndDate = model.EndDate;
            int edited= _cuttingProcessStyleActiveRepository.Edit(cuttingProcessStyleActive);
            if (edited > 0)
            {
                timeAndActionManager.UpdateActualEndDate(TnaActivityKeyValue.BULKCUTTING, model.EndDate, model.OrderStyleRefId, model.CompId);
            };
            return edited;
        }

        public bool IsCuttingProcessStyleActiveExist(PROD_CuttingProcessStyleActive model)
        {
            return _cuttingProcessStyleActiveRepository.Exists(x => x.CompId == model.CompId && x.CuttingProcessStyleActiveId != model.CuttingProcessStyleActiveId && x.OrderStyleRefId == model.OrderStyleRefId);
        }

        public int DeleteCuttingProcessStyleActive(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _cuttingProcessStyleActiveRepository.Delete(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId);
        }

        public IEnumerable GetOrderByBuyer(string buyerRefId)
        {
            return _cuttingProcessStyleActiveRepository.GetOrderByBuyer(PortalContext.CurrentUser.CompId, buyerRefId);

        }

        public IEnumerable GetStyleByOrderNo(string orderNo)
        {
            return _cuttingProcessStyleActiveRepository.GetStyleByOrderNo(PortalContext.CurrentUser.CompId, orderNo);
        }

        public VwCuttingProcessStyleActive GetVwStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId)
        {
            return _cuttingProcessStyleActiveRepository.GetVwStyleInCuttingByCuttingProcessStyleActiveId(cuttingProcessStyleActiveId);
        }
    }
}
