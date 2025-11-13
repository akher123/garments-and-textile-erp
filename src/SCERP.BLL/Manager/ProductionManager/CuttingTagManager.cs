using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttingTagManager : ICuttingTagManager
    {
        private readonly ICuttingTagRepository _cuttingTagRepository;

        private IComponentRepository _componentRepository;
        public CuttingTagManager(ICuttingTagRepository cuttingTagRepository, IComponentRepository componentRepository)
        {
            _cuttingTagRepository = cuttingTagRepository;
            _componentRepository = componentRepository;

        }

        public List<VwCuttingTag> GetAllCuttingTatByCuttingSequenceId(long cuttingSequenceId)
        {
            IQueryable<VwCuttingTag> cuttingTags = _cuttingTagRepository.GetVwCuttingTagByCuttingSequenceId(PortalContext.CurrentUser.CompId,cuttingSequenceId);
            return cuttingTags.ToList();
        }

        public int SaveCuttingTag(PROD_CuttingTag model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _cuttingTagRepository.Save(model);
        }

        public PROD_CuttingTag GetCuttingTagByCuttingTagId(long cuttingTagId)
        {
            return
                _cuttingTagRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagId == cuttingTagId);
        }

        public int EditCuttingTag(PROD_CuttingTag model)
        {
            var cuttingTag = _cuttingTagRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagId == model.CuttingTagId);

            cuttingTag.ComponentRefId = model.ComponentRefId;
            cuttingTag.IsSolid = model.IsSolid;
            cuttingTag.IsPrint = model.IsPrint;
            cuttingTag.IsEmbroidery = model.IsEmbroidery;
            return _cuttingTagRepository.Edit(cuttingTag);
        }

        public int DeleteCuttingTag(long cuttingTagId)
        {
            return
                _cuttingTagRepository.Delete(
                    x => x.CompId== PortalContext.CurrentUser.CompId && x.CuttingTagId == cuttingTagId);
        }

        public bool IsCuttingTagExist(PROD_CuttingTag model)
        {
            return _cuttingTagRepository.Exists(
            x =>
                x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagId != model.CuttingTagId && x.ComponentRefId==model.ComponentRefId && x.CuttingSequenceId==model.CuttingSequenceId);
        }

        public List<VwCuttingTag> GetAllCuttingTagSupplierByCuttingTagId(long cuttingTagId)
        {
            IQueryable<VwCuttingTag> cuttingTagSupplier = _cuttingTagRepository.GetVwCuttingTagSupplierByCuttingTagId(PortalContext.CurrentUser.CompId, cuttingTagId);
            return cuttingTagSupplier.ToList();
        }

        public object GetTagBySequence(string componentRefId, string orderStyleRefId)
        {
            return _cuttingTagRepository.GetTagBySequence(componentRefId, orderStyleRefId,
                PortalContext.CurrentUser.CompId);
        }
        public List<SpPrintEmbroiderySummary> GetPrintEmbroideryBalance(string cuttingBatchRefId,string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId)
        {
            List<SpPrintEmbroiderySummary> printEmbroideryBalanceList = _cuttingTagRepository.GetPrintEmbroideryBalance(cuttingBatchRefId ?? "000000", buyerRefId ?? "000", orderNo ?? "000000000000", orderStyleRefId ?? "0000000", colorRefId ?? "0000");
            return printEmbroideryBalanceList;
        }

        public object GetGetCuttingTagBySequence(string colorRefId, string componentRefId, string orderStyleRefId)
        {
            var cuttingTags =
                _cuttingTagRepository.GetWithInclude(x => x.CompId == PortalContext.CurrentUser.CompId,
                    "PROD_CuttingSequence")
                    .Where(
                        x =>
                            x.PROD_CuttingSequence.OrderStyleRefId == orderStyleRefId &&
                            x.PROD_CuttingSequence.ColorRefId == colorRefId &&
                            x.PROD_CuttingSequence.ComponentRefId == componentRefId);
           var components= _componentRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId);
            var tags = (from tag in cuttingTags
                join component in components on tag.ComponentRefId equals component.ComponentRefId
                select new {tag.CuttingTagId, component.ComponentName}).Distinct();
            return tags.ToList();


        }
    }
}
