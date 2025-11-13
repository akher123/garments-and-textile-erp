using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{

    public class YarnConsumptionManager : IYarnConsumptionManager
    {

        private class YearnProcess
        {
            private readonly OM_YarnConsumption _yarn;
            private readonly decimal _tQty;
            public YearnProcess(OM_YarnConsumption yarn, decimal tQty)
            {
                _tQty = tQty;
                _yarn = yarn;
            }

            public decimal GetNetYarnAmount()
            {
                var yarn =_yarn.CPercent.GetValueOrDefault() * _tQty * 0.01M;
                return yarn;
            }

            private decimal GetPLossYarnAmount()
            {


                var knitYearn = _yarn.PLoss.GetValueOrDefault() * .01M * _tQty;
                return knitYearn;
            }

            public decimal GetKYarnQty()
            {
                return GetNetYarnAmount() + GetPLossYarnAmount();
            }
        }
        private readonly IYarnConsumptionRepository _yarnConsumptionRepository;
        private readonly string _compId;
        public YarnConsumptionManager(IYarnConsumptionRepository yarnConsumptionRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _yarnConsumptionRepository = yarnConsumptionRepository;
        }

        public List<VYarnConsumption> GetVYarnConsumptions(string consRefId,string grColorRefId)
        {
            var vYarnConsumptions = _yarnConsumptionRepository.GetVYarnConsumptions(x => x.CompId == _compId && x.ConsRefId == consRefId && x.GrColorRefId == grColorRefId);
            return vYarnConsumptions.ToList();
        }

        public string GetNewYCRef()
        {
            return _yarnConsumptionRepository.GetNewYCRef(_compId);
        }
        public int SaveYarnConsumption(OM_YarnConsumption yarnConsumption, decimal tQty)
        {
            var yearnProcess = new YearnProcess(yarnConsumption, tQty);
        
            var model=new OM_YarnConsumption
            {
                YCRef = yarnConsumption.YCRef,
                ConsRefId = yarnConsumption.ConsRefId,
                GrColorRefId = yarnConsumption.GrColorRefId,
                KColorRefId = yarnConsumption.KColorRefId,
                KSizeRefId = yarnConsumption.KSizeRefId,
                KQty = yearnProcess.GetKYarnQty(),
                PLoss = yarnConsumption.PLoss,
                CPercent = yarnConsumption.CPercent,
                WMtr = yearnProcess.GetNetYarnAmount(),
                DReq = yarnConsumption.DReq,
                ItemCode = yarnConsumption.ItemCode,
                CompId = _compId
            };
            return _yarnConsumptionRepository.Save(model);
        }
        public int DeleteYarnCons(long yarnConsumptionId)
        {
            return _yarnConsumptionRepository.Delete(x => x.YarnConsumptionId == yarnConsumptionId&&x.CompId==_compId);
        }
        public List<VYarnConsumption> GetVYarnConsByOrderSyleRefId(string orderStyleRefId)
        {
            return _yarnConsumptionRepository.GetVYarnConsumptions( x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList()??new List<VYarnConsumption>();
        }

        public int UpdateYarnConsRate(List<VYarnConsumption> yarnConsumptions)
        {
            var updateInde = 0;
            foreach (var item in yarnConsumptions)
            {
                List<VYarnConsumption>exitstingList = _yarnConsumptionRepository.GetVYarnConsumptions(x => x.OrderStyleRefId + "I" + x.ItemCode + "C" + x.KColorRefId + "S" + x.KSizeRefId == item.ConsRefId && x.OrderStyleRefId == item.OrderStyleRefId && x.CompId == _compId).ToList();
                foreach (var ycons in exitstingList)
                {
                    var yconsumption = _yarnConsumptionRepository.FindOne(x =>x.YarnConsumptionId==ycons.YarnConsumptionId);
                    yconsumption.Rate = item.Rate;
                    yconsumption.PiRefId = item.PiRefId;
                    yconsumption.SupplierId = item.SupplierId;
                    updateInde += _yarnConsumptionRepository.Edit(yconsumption);
                }
            }
            return updateInde;
        }

        public VYarnConsumption GetYarnConsumptionById(long yarnConsumptionId)
        {
            return _yarnConsumptionRepository.GetVYarnConsumptions(x =>x.CompId == _compId).FirstOrDefault(x=> x.YarnConsumptionId == yarnConsumptionId);
        }

        public int EditYarnConsumption(OM_YarnConsumption yarnConsumption, decimal tQty)
        {
            var yearnProcess = new YearnProcess(yarnConsumption, tQty);
            var model= _yarnConsumptionRepository.FindOne(x => x.YarnConsumptionId == yarnConsumption.YarnConsumptionId);
            model.YCRef = yarnConsumption.YCRef;
            model.GrColorRefId = yarnConsumption.GrColorRefId;
            model.KColorRefId = yarnConsumption.KColorRefId;
            model.KSizeRefId = yarnConsumption.KSizeRefId;
            model.KQty = yearnProcess.GetKYarnQty();
            model.PLoss = yarnConsumption.PLoss;
            model.CPercent = yarnConsumption.CPercent;
            model.WMtr = yearnProcess.GetNetYarnAmount();
            model.DReq = yarnConsumption.DReq;
            model.ItemCode = yarnConsumption.ItemCode;
            model.YarnConsumptionId = yarnConsumption.YarnConsumptionId;
            model.CompId = _compId;
           return _yarnConsumptionRepository.Edit(model);
        }

        public List<VYarnConsumption> GetYarnConsSummaryByOrderSyleRefId(string orderStyleRefId)
        {
     
            List<VYarnConsumption> ylist= _yarnConsumptionRepository.GetVYarnConsumptions( x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList() ?? new List<VYarnConsumption>();
            return ylist.GroupBy(x => new { ConsRefId= x.OrderStyleRefId + "I" + x.ItemCode + "C" + x.KColorRefId??"0000" + "S" + x.KSizeRefId ?? "0000", x.ItemName, x.UnitName, x.ItemCode, x.KColorRefId, x.KColorName, x.KSizeRefId, x.KSizeName, x.Rate,x.SupplierId,x.PiRefId }).ToList()
                .Select(g => new VYarnConsumption()
                {
                    OrderStyleRefId = g.First().OrderStyleRefId,
                    ConsRefId = g.First().OrderStyleRefId + "I" + g.First().ItemCode + "C" + g.First().KColorRefId + "S" + g.First().KSizeRefId,
                    KQty = g.Sum(x => x.KQty??0),
                    WMtr = g.Sum(x => x.WMtr),
                    Rate = g.First().Rate,
                    ItemName = g.First().ItemName,
                    UnitName = g.First().UnitName,
                    KSizeRefId = g.First().KSizeRefId ?? "0000",
                    KColorRefId = g.First().KColorRefId ?? "0000",
                    KColorName = g.First().KColorName,
                    KSizeName = g.First().KSizeName,
                    SupplierId = g.First().SupplierId,
                    PiRefId = g.First().PiRefId,
                }).Where(h => h.KQty > 0).ToList();


        }
    }
}
