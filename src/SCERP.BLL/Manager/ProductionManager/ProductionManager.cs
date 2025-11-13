using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class ProductionManager : IProductionManager
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProgramDetailRepository _programDetailRepository;
        private readonly IProductionDetailRepository _productionDetailRepository;
      
        private readonly string _compId;
        public ProductionManager(IProductionRepository productionRepository, IProgramDetailRepository programDetailRepository, IProductionDetailRepository productionDetailRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _programDetailRepository = programDetailRepository;
           
            _productionRepository = productionRepository;
            _productionDetailRepository = productionDetailRepository;
        }

        public List<VProductionDetail> GetProgramDetailsByProcessRefId(string processRefId, string programRefId, string pType)
        {

            return _programDetailRepository.GetVProgramDetails(x => x.CompId == _compId && x.ProcessRefId == processRefId && x.PrgramRefId == programRefId && x.MType == pType).ToList().Select(x => new VProductionDetail()
            {
                ProrgramRefId = x.PrgramRefId,
                ItemCode = x.ItemCode,
                ItemName = x.ItemName,
                SizeRefId = x.SizeRefId,
                ColorRefId = x.ColorRefId,
                ColorName = x.ColorName,
                SizeName = x.SizeName,
                MeasurementUinitId = x.MeasurementUinitId,
                UnitName = x.UnitName,
                ProcessRefId = x.ProcessRefId,
                Qty = x.Quantity,
                PType = x.MType
            }).ToList();
        }


        public List<VwProduction> GetProductionByPaging(PROD_Production model)
        {

            var productionList = _productionRepository.GetVwProductionList(x => x.CompId == _compId && x.ProrgramRefId == model.ProrgramRefId && x.PType == model.PType && x.ProcessRefId == model.ProcessRefId);
            return productionList.ToList();
        }
        public PROD_Production GetProductionById(long productionId)
        {
            return _productionRepository.FindOne(x => x.ProductionId == productionId);
        }
        public List<VProductionDetail> GetProductionDetailsByProductionIed(long productionId)
        {
            return _productionDetailRepository.GetVProductionDetails(x => x.ProductionId == productionId && x.CompId == _compId).ToList().Select(
                x =>
                {
                    x.EntryQty = x.Qty.GetValueOrDefault();
                    return x;
                }).ToList();
        }
        public string GetProductionRefId(string prifix)
        {
            string max = _productionRepository.Filter(x => x.CompId == _compId).Max(x => x.ProductionRefId.Substring(1, 8));
            return prifix+max.IncrementOne().PadZero(8);
            
        }

        public int SaveProduction(PROD_Production production)
        {
            production.CompId = _compId;
            production.UserId = PortalContext.CurrentUser.UserId;
            return _productionRepository.Save(production);
        }

        public int EditProduction(PROD_Production production)
        {
            var edited = 0;
            using (var transaction = new TransactionScope())
            {
                _productionDetailRepository.Delete(x => x.ProductionId == production.ProductionId && x.ProductionRefId == production.ProductionRefId && x.CompId == _compId);
                _productionRepository.Delete(x => x.ProductionId == production.ProductionId && x.CompId == _compId && x.ProductionRefId == production.ProductionRefId);
                edited = SaveProduction(production);
                transaction.Complete();

            }
            return edited;
        }

        public List<VProgramDetail> GetVProgramLis(string prorgramRefId, string productionRefId, string pType)
        {

            return _programDetailRepository.GetVProgramList(prorgramRefId, productionRefId, pType);
        }

        public int DeleteProduction(PROD_Production model)
        {
            var deleteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                deleteIndex += _productionDetailRepository.Delete(x => x.ProductionId == model.ProductionId);
                deleteIndex += _productionRepository.Delete(x => x.ProductionId == model.ProductionId);

                transaction.Complete();
            }
            return deleteIndex;
        }
    }
}
