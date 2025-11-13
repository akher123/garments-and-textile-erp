using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BulkBookingYarnDetailManager : IBulkBookingYarnDetailManager
    {
        private readonly IBulkBookingYarnDetailRepository _bulkBookingYarnDetailRepository;
        public BulkBookingYarnDetailManager(IBulkBookingYarnDetailRepository bulkBookingYarnDetailRepository)
        {
            _bulkBookingYarnDetailRepository = bulkBookingYarnDetailRepository;
        }

        public List<OM_BulkBookingYarnDetail> GetBulkBookingYarnList(long bulkBookingDetailId)
        {
            return
                _bulkBookingYarnDetailRepository.Filter(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.BulkBookingDetailId == bulkBookingDetailId)
                    .ToList();
        }

        public OM_BulkBookingYarnDetail GetBulkBookingById(long bulkBookingYearnDetailId)
        {
            return _bulkBookingYarnDetailRepository.FindOne(x => x.BulkBookingYearnDetailId == bulkBookingYearnDetailId);
        }

        public int EditBulkBookingYarnDetail(OM_BulkBookingYarnDetail bulkBookingYarnDetail)
        {
            var bkYarnDetail = _bulkBookingYarnDetailRepository.FindOne(x => x.BulkBookingYearnDetailId == bulkBookingYarnDetail.BulkBookingYearnDetailId);
            bkYarnDetail.ItemName = bulkBookingYarnDetail.ItemName;
            bkYarnDetail.ColorName = bulkBookingYarnDetail.ColorName;
            bkYarnDetail.CountName = bulkBookingYarnDetail.CountName;
            bkYarnDetail.OrdQty = bulkBookingYarnDetail.OrdQty;
            bkYarnDetail.ConsQty = bulkBookingYarnDetail.ConsQty;
            bkYarnDetail.Remarks = bulkBookingYarnDetail.Remarks;
            return _bulkBookingYarnDetailRepository.Edit(bkYarnDetail);
        }

        public int SaveBulkBookingYarnDetail(OM_BulkBookingYarnDetail bulkBookingYarnDetail)
        {
            bulkBookingYarnDetail.CompId = PortalContext.CurrentUser.CompId;
            return _bulkBookingYarnDetailRepository.Save(bulkBookingYarnDetail);
        }

        public int DeleteBulkBookingYarnDetailById(long bulkBookingYearnDetailId)
        {
            return _bulkBookingYarnDetailRepository.Delete(x => x.BulkBookingYearnDetailId == bulkBookingYearnDetailId);
        }
    }
}
