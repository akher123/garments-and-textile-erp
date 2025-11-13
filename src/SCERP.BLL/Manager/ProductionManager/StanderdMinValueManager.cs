using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.DAL.Repository.ProductionRepository;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class StanderdMinValueManager : IStanderdMinValueManager
    {
        private readonly IStanderdMinValueRepository _standerdMinValueRepository;
        private readonly IStanderdMinValDetailRepository _standerdMinValDetailRepository;
        private readonly string _compId;
        private readonly ISubProcessRepository _subProcessRepository;
        public StanderdMinValueManager(IStanderdMinValueRepository standerdMinValue, IStanderdMinValDetailRepository standerdMinValDetail, ISubProcessRepository subProcess)
        {


            _compId = PortalContext.CurrentUser.CompId;
            _standerdMinValueRepository = standerdMinValue;
            _standerdMinValDetailRepository = standerdMinValDetail;
            _subProcessRepository = subProcess;
        }

        public List<PROD_StanderdMinValue> GetStanderdMinValuesByStyleOrder(string orderStyleRefId)
        {
            return _standerdMinValueRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList();
        }
        public string GetStanderdMinValueRefId()
        {
            return _standerdMinValueRepository.GetStanderdMinValueRefId(_compId);
        }

        public List<PROD_SubProcess> GetSubProcessList()
        {
            return _subProcessRepository.Filter(x => x.CompId == _compId).ToList(); // 
        }

        public int SaveSmv(PROD_StanderdMinValue model)
        {
            var nsmv = model.PROD_StanderdMinValDetail.Sum(x => x.StMvD);
            model.StMv = nsmv == 0 ? model.StMv : nsmv;
            model.CompId = _compId;
            return _standerdMinValueRepository.Save(model);
        }

        public PROD_StanderdMinValue GetSmvById(long standerdMinValueId)
        {
            var smv = _standerdMinValueRepository.FindOne(x => x.StanderdMinValueId == standerdMinValueId);

            return smv;
        }

        public int EditSmv(PROD_StanderdMinValue standerdMinValue)
        {
            var saveIndex = 0;
            using (var transction = new TransactionScope())
            {
                var smv = _standerdMinValueRepository.FindOne(x => x.CompId == _compId && x.StanderdMinValueId == standerdMinValue.StanderdMinValueId);
                var nsmv = standerdMinValue.PROD_StanderdMinValDetail.Sum(x => x.StMvD);
                smv.StMv = nsmv == 0 ? standerdMinValue.StMv : nsmv;
                saveIndex += _standerdMinValueRepository.Edit(smv);

                _standerdMinValDetailRepository.Delete(x => x.CompId == _compId && x.StanderdMinValueId == standerdMinValue.StanderdMinValueId);
                saveIndex += _standerdMinValDetailRepository.SaveList(standerdMinValue.PROD_StanderdMinValDetail.ToList());

                transction.Complete();
            }
            return saveIndex;
        }
        public bool IsSmvCreated(string orderStyleRefId)
        {
            return _standerdMinValueRepository.Exists(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId);
        }
        public int DeleteStanderdMinValue(long standerdMinValueId)
        {
            var deleteIndex = 0;
            using (var transction = new TransactionScope())
            {
                deleteIndex += _standerdMinValDetailRepository.Delete(x => x.CompId == _compId && x.StanderdMinValueId == standerdMinValueId);
                deleteIndex += _standerdMinValueRepository.Delete(x => x.CompId == _compId && x.StanderdMinValueId == standerdMinValueId);
                transction.Complete();
            }
            return deleteIndex;
        }

        public Dictionary<string, VwStanderdMinValDetail> GetVwSmvDetails(long standerdMinValueId)
        {

            List<VwStanderdMinValDetail> standerdMinDetails = _standerdMinValDetailRepository.GetVwSmvDetails(standerdMinValueId, _compId);
            return standerdMinDetails.ToDictionary(x => Convert.ToString(x.StanderdMinValDetailId), x => x);

        }
    }
}
