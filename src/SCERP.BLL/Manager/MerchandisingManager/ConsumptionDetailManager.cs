using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ConsumptionDetailManager : IConsumptionDetailManager
    {
        private readonly string _compId;
        private readonly IConsumptionDetailRepository _consumptionDetailRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private ICompConsumptionDetailRepository _compConsumptionDetailRepository;
        public ConsumptionDetailManager(IConsumptionRepository consumptionRepository, ICompConsumptionDetailRepository compConsumptionDetailRepository, IConsumptionDetailRepository consumptionDetailRepository)
        {
            _compConsumptionDetailRepository = compConsumptionDetailRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _consumptionRepository = consumptionRepository;
            _consumptionDetailRepository = consumptionDetailRepository;
        }

        public List<VConsumptionDetail> GetVConsumptionDetails(string consRefId)
        {
            return _consumptionDetailRepository.GetVConsumptionDetails(consRefId, _compId);
        }

        public List<OM_Color> GetGColorList(string orderStyleRefId)
        {
            var colordt = _consumptionDetailRepository.GetGColorList(orderStyleRefId, _compId);
            return colordt.ToList<OM_Color>();
        }

        public List<OM_Size> GetGSizeList(string orderStyleRefId)
        {
            var sizedt = _consumptionDetailRepository.GetGSizeList(orderStyleRefId, _compId);
            return sizedt.ToList<OM_Size>();
        }

        public int UpdateConsDetail(VConsumptionDetail model)
        {

            var updateIndex = 0;
            using (var transction =new TransactionScope())
            {
                var consDetails = GetConsumptionDetails(model);
                consDetails = consDetails.Select(x =>
                {
                    {
                        x.PPQty = model.PDzCons / 12M;
                        x.PAllow = model.PAllow??0.0M;
                       // x.TotalQty = (x.QuantityP * (model.PDzCons / 12M)) +(x.QuantityP * model.PAllow.GetValueOrDefault() * 0.01M);
                        x.TotalQty = x.QuantityP * ((model.PDzCons / 12M)) * (1 + (model.PAllow*.01M));
                    }
                    return x;
                }).ToList();

                updateIndex = consDetails.Sum(consDetail => _consumptionDetailRepository.Edit(consDetail));
                var totalQty = _consumptionDetailRepository.Filter(x => x.ConsRefId == model.ConsRefId&&x.CompId==_compId)
                  .Sum(x => x.TotalQty)??0;
                var consumption =
                    _consumptionRepository.FindOne(
                        x => x.ConsRefId == model.ConsRefId && x.CompId == _compId);
                consumption.Quantity = totalQty;
                updateIndex += _consumptionRepository.Edit(consumption);
                transction.Complete();
            }
           
            return updateIndex;
        }

        public int UpdateConsDetailByPcolor(VConsumptionDetail model)
        {

          var comConsumptionDtls=_compConsumptionDetailRepository.Filter( x => x.ConsRefId == model.ConsRefId && x.CompID == _compId).ToList();
          if (comConsumptionDtls.Any())
            {
                comConsumptionDtls = comConsumptionDtls.Select(x =>
                {
                    {
                        x.PColorRefId = model.PColorRefId;
                    }
                    return x;
                }).ToList();
                comConsumptionDtls.Sum(comConsumptionDtl => _compConsumptionDetailRepository.Edit(comConsumptionDtl));
            }
            var consDetails = GetConsumptionDetails(model);
            consDetails = consDetails.Select(x =>
            {
                {
                    x.PColorRefId = model.PColorRefId;
                }
                return x;
            }).ToList();

            return consDetails.Sum(consDetail => _consumptionDetailRepository.Edit(consDetail));
        }
        public int UpdateConsDetailByPSize(VConsumptionDetail model)
        {
            var consDetails = GetConsumptionDetails(model);
            consDetails = consDetails.Select(x =>
            {
                {
                    x.PSizeRefId = model.PSizeRefId;
                }
                return x;
            }).ToList();

            return consDetails.Sum(consDetail => _consumptionDetailRepository.Edit(consDetail));
        }

        public DataTable GetVConsumptionDetailsByStyleRefId(string orderStyleRefId)
        {
            return _consumptionDetailRepository.GetVConsumptionDetailsByStyleRefId(orderStyleRefId, _compId);
        }

        public DataTable GetVConsumptionDetailsByStyleRefId(string orderStyleRefId, string fabricGroupCode)
        {
            return _consumptionDetailRepository.GetVConsumptionDetailsByStyleRefId(orderStyleRefId, _compId);
        }

        public List<SPOrderStyleDetailForBOM> GetOrderStyleDetailForBOM(string orderStyleRefId)
        {
            return _consumptionDetailRepository.GetOrderStyleDetailForBOM(orderStyleRefId, _compId);
        }

        public DataTable GetAccessoriesConsumptionDetail(string orderStyleRefId)
        {
            return _consumptionDetailRepository.GetAccessoriesConsumptionDetail(orderStyleRefId, _compId);
        }

        public int UpdateRemarks(VConsumptionDetail model)
        {
            var consDetails = GetConsumptionDetails(model);
            consDetails = consDetails.Select(x =>
            {
                {
                    x.Remarks = model.Remarks;
                }
                return x;
            }).ToList();

            return consDetails.Sum(consDetail => _consumptionDetailRepository.Edit(consDetail));
        }

        public int UpdateProductQty(VConsumptionDetail model)
        {
            var consDetails = GetConsumptionDetails(model);
            consDetails = consDetails.Select(x =>
            {
                {
                    x.QuantityP = model.QuantityP;
                }
                return x;
            }).ToList();

            return consDetails.Sum(consDetail => _consumptionDetailRepository.Edit(consDetail));
        }

        public DataTable GetAccessoriesConsumptionDetailByOrder(string orderNo)
        {
            return _consumptionDetailRepository.GetAccessoriesConsumptionDetailByOrder(orderNo, _compId);
        }


        private List<OM_ConsumptionDetail> GetConsumptionDetails(VConsumptionDetail model)
        {
            var consDetails = new List<OM_ConsumptionDetail>();
            if (model.ConsTypeRefId == Convert.ToString((int)ConsType.General))
            {
                consDetails =
                    _consumptionDetailRepository.Filter(
                        x =>
                            x.ConsRefId == model.ConsRefId && x.CompId == _compId &&
                            (x.GSizeRefId == model.GSizeRefId || model.GSizeRefId == null)).ToList();
            }
            else if (model.ConsTypeRefId == Convert.ToString((int)ConsType.ColorSize))
            {
                consDetails =
                    _consumptionDetailRepository.Filter(
                        x =>
                            x.ConsRefId == model.ConsRefId && x.CompId == _compId &&
                            (x.GSizeRefId == model.GSizeRefId || model.GSizeRefId == null) &&
                            (x.GColorRefId == model.GColorRefId || model.GColorRefId == null)).ToList();
            }
            else if (model.ConsTypeRefId == Convert.ToString((int)ConsType.Color))
            {
                consDetails =
                    _consumptionDetailRepository.Filter(
                        x =>
                            x.ConsRefId == model.ConsRefId && x.CompId == _compId &&
                            (x.GColorRefId == model.GColorRefId || model.GColorRefId == null)).ToList();
            }
            else if (model.ConsTypeRefId == Convert.ToString((int)ConsType.Size))
            {
                consDetails =
                    _consumptionDetailRepository.Filter(
                        x =>
                            x.ConsRefId == model.ConsRefId && x.CompId == _compId &&
                            (x.GSizeRefId == model.GSizeRefId || model.GSizeRefId == null)).ToList();
            }
            return consDetails;
        }




        
    }
}
