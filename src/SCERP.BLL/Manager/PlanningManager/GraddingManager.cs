using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class GraddingManager : IGraddingManager
    {
        private readonly IGraddingRepository _graddingRepository;
        private readonly IBundleCuttingRepository _bundleCuttingRepository;
        public GraddingManager(IGraddingRepository graddingRepository, IBundleCuttingRepository bundleCuttingRepository)
        {

            _graddingRepository = graddingRepository;
            _bundleCuttingRepository = bundleCuttingRepository;
        }

        public int SaveGradding(PROD_CuttingGradding gradding)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                gradding.CompId = PortalContext.CurrentUser.CompId;

                saveIndex = UpdateBundaleByGradding(gradding);
                saveIndex += _graddingRepository.Save(gradding);
                transaction.Complete();
            }
            return saveIndex;
        }


        public int UpdateBundaleByGradding(PROD_CuttingGradding gradding)
        {
            int saveIndex = 0;
            var fromBundle = _bundleCuttingRepository.Filter(
             x =>
                 x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == gradding.CuttingBatchId &&
                 x.SizeRefId == gradding.FromSizeRefId).OrderByDescending(x => x.BundleEnd).FirstOrDefault();
            if (fromBundle == null)
            {
                throw new Exception(message: "Fro Size not exit of this job");
            }
            fromBundle.Quantity = fromBundle.Quantity - gradding.Quantity;
            fromBundle.BundleEnd = fromBundle.BundleEnd - gradding.Quantity;
            saveIndex += _bundleCuttingRepository.Edit(fromBundle);

            var toBundle = _bundleCuttingRepository.Filter(
              x =>
                  x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == gradding.CuttingBatchId &&
                  x.SizeRefId == gradding.ToSizeRefId).OrderByDescending(x => x.BundleEnd).FirstOrDefault();
            if (toBundle == null)
            {
                throw new Exception(message: "To Size not exit of this job");
            }
            toBundle.Quantity = toBundle.Quantity + gradding.Quantity;
            toBundle.BundleEnd = toBundle.BundleEnd + gradding.Quantity;
            saveIndex += _bundleCuttingRepository.Edit(toBundle);
            return saveIndex;
        }
        public int DeletUpdateBundaleByGradding(PROD_CuttingGradding gradding)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var fromBundle = _bundleCuttingRepository.Filter(
                    x =>
                        x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == gradding.CuttingBatchId &&
                        x.SizeRefId == gradding.FromSizeRefId).OrderByDescending(x => x.BundleEnd).FirstOrDefault();
                fromBundle.Quantity = fromBundle.Quantity + gradding.Quantity;
                fromBundle.BundleEnd = fromBundle.BundleEnd + gradding.Quantity;
                saveIndex += _bundleCuttingRepository.Edit(fromBundle);
                var toBundle = _bundleCuttingRepository.Filter(
                    x =>
                        x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == gradding.CuttingBatchId &&
                        x.SizeRefId == gradding.ToSizeRefId).OrderByDescending(x => x.BundleEnd).FirstOrDefault();
                toBundle.Quantity = toBundle.Quantity - gradding.Quantity;
                toBundle.BundleEnd = toBundle.BundleEnd - gradding.Quantity;
                saveIndex += _bundleCuttingRepository.Edit(toBundle);
                transaction.Complete();
            }
            return saveIndex;
        }
        public int EditGradding(PROD_CuttingGradding gradding)
        {

            var graddingObj = _graddingRepository.FindOne(x => x.CuttingGraddingId == gradding.CuttingGraddingId && x.CompId == PortalContext.CurrentUser.CompId);
            graddingObj.FromSizeRefId = gradding.FromSizeRefId;
            graddingObj.ToSizeRefId = gradding.ToSizeRefId;
            graddingObj.Quantity = gradding.Quantity;
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                saveIndex = UpdateBundaleByGradding(graddingObj);
                saveIndex += _graddingRepository.Edit(graddingObj);
                transaction.Complete();
            }
            return saveIndex;
        }

        public List<VwCuttingGraddding> GetGradingListByCuttingBatch(long cuttingBatchId)
        {
            return _graddingRepository.GetGradingListByCuttingBatch(cuttingBatchId, PortalContext.CurrentUser.CompId);
        }

        public PROD_CuttingGradding GetGraddingById(long cuttingGraddingId)
        {
            return _graddingRepository.FindOne(x => x.CuttingGraddingId == cuttingGraddingId);
        }

        public int DeleteGradding(long cuttingGraddingId)
        {
            int deleteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var gredding = _graddingRepository.FindOne(x => x.CuttingGraddingId == cuttingGraddingId);
                deleteIndex = DeletUpdateBundaleByGradding(gredding);
                deleteIndex += _graddingRepository.DeleteOne(gredding);
                transaction.Complete();
            }
            return deleteIndex;
        }
    }
}
