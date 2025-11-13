using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttingTagSupplierManager : ICuttingTagSupplierManager
    {
        private readonly ICuttingTagSupplierRepository _cuttingTagSupplierRepository;
      
        public CuttingTagSupplierManager(ICuttingTagSupplierRepository cuttingTagSupplierRepository)
        {
            
            _cuttingTagSupplierRepository = cuttingTagSupplierRepository;
        }
        public PROD_CuttingTagSupplier GetCuttingTagByCuttingTagId(long cuttingTagSupplierId)
        {
            return
                _cuttingTagSupplierRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagSupplierId == cuttingTagSupplierId);
        }

        public int EditCuttingTagSupplier(PROD_CuttingTagSupplier model)
        {
            var cuttingTagSupplier = _cuttingTagSupplierRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagSupplierId == model.CuttingTagSupplierId);
            cuttingTagSupplier.PartyId = model.PartyId;
            cuttingTagSupplier.Rate = model.Rate;
            cuttingTagSupplier.DeductionAllowance = model.DeductionAllowance;
            cuttingTagSupplier.EditedBy =PortalContext.CurrentUser.UserId;
            cuttingTagSupplier.EditedDate = DateTime.Now;
            return _cuttingTagSupplierRepository.Edit(cuttingTagSupplier);
        }

        public int SaveCuttingTagSupplier(PROD_CuttingTagSupplier model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            return _cuttingTagSupplierRepository.Save(model);
        }

        public int DeleteCuttingTagSupplier(long cuttingTagSupplierId)
        {
            return
               _cuttingTagSupplierRepository.Delete(
                   x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagSupplierId == cuttingTagSupplierId);
        }

        public bool IsCuttingTagSupplierExist(PROD_CuttingTagSupplier model)
        {
            return _cuttingTagSupplierRepository.Exists(
            x =>
                x.CompId == PortalContext.CurrentUser.CompId && x.CuttingTagSupplierId != model.CuttingTagSupplierId && x.PartyId == model.PartyId && x.CuttingTagId==model.CuttingTagId && x.EmblishmentStatus==model.EmblishmentStatus );
        }

      
    }
}
