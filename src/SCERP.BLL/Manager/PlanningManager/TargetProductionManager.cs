using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class TargetProductionManager : ITargetProductionManager
    {
        private readonly ITargetProductionRepository _targetProductionRepository;
        private readonly ITargetProductionDetailRepository _targetProductionDetail;
        public TargetProductionManager(
            ITargetProductionRepository targetProductionRepository

            , ITargetProductionDetailRepository targetProductionDetail)
        {
            _targetProductionRepository = targetProductionRepository;

            _targetProductionDetail = targetProductionDetail;

        }

        public List<VwTargetProduction> GetTargetProductionList(string compId, string buyerRefId, string orderNo, string orderStyleRefId)
        {

            return _targetProductionRepository.VwGetTargetProduction(x => x.CompId == compId &&
                    ((x.BuyerRefId == buyerRefId || String.IsNullOrEmpty(buyerRefId)) && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo)) && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId)))).ToList();
        }

        public string GetnewTargetProductionRefId(string compId)
        {

            var maxTargetProductionRefId =
                _targetProductionRepository.Filter(x => x.CompId == compId).Max(x => x.TargetProductionRefId);
            var targetProductionRefId = maxTargetProductionRefId.IncrementOne().PadZero(10);
            return targetProductionRefId;
        }

        public PLAN_TargetProduction GetTargetProductionById(long targetProductionId)
        {

            var targetProduction = _targetProductionRepository.FindOne(x => x.TargetProductionId == targetProductionId);
            return targetProduction ?? new PLAN_TargetProduction();
        }

        public int SaveTargetProduction(PLAN_TargetProduction planTargetProduction)
        {
            int index = 0;
            index= _targetProductionRepository.Save(planTargetProduction);
            index +=_targetProductionDetail.SaveTargetDetail(planTargetProduction);
            return index;
        }

        public int DeleteTargetProduction(long targetProductionId)
        {
            int deletIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var compId = PortalContext.CurrentUser.CompId;
                deletIndex =
                    _targetProductionDetail.Delete(x => x.CompId == compId && x.TargetProductionId == targetProductionId);
                deletIndex +=
                    _targetProductionRepository.Delete(
                        x => x.CompId == compId && x.TargetProductionId == targetProductionId);
                transaction.Complete();
            }

            return deletIndex;

        }

        public List<VwTargetProduction> GetActiveTargetProductionList(int lineId,string compId)
        {
            return
                _targetProductionRepository.VwGetTargetProduction(x => x.CompId == compId && x.LineId == lineId)
                    .ToList();
        }

        public List<VwTargetProduction> GetMontylyActiveTargetProductionList(int monthId, int yearId, string compId)
        {
            return
              _targetProductionRepository.GetMontylyActiveTargetProductionList(monthId,yearId,compId).ToList();
        }
    }
}
