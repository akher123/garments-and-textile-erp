using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BulkBookingDetailManager : IBulkBookingDetailManager
    {
        private readonly IBulkBookingDetailRepository _bulkBookingDetailRepository;
        public BulkBookingDetailManager(IBulkBookingDetailRepository bulkBookingDetailRepository)
        {
            _bulkBookingDetailRepository = bulkBookingDetailRepository;
        }

        public List<OM_BulkBookingDetail> GetBulkBookingDetailList(long bulkBookingId)
        {
            return _bulkBookingDetailRepository.Filter( x => x.CompId == PortalContext.CurrentUser.CompId && x.BulkBookingId == bulkBookingId).ToList();
        }

        public OM_BulkBookingDetail GetBulkBookingDetailId(long bulkBookingDetailId)
        {
            return _bulkBookingDetailRepository.FindOne(x => x.BulkBookingDetailId == bulkBookingDetailId);
        }

        public int EditBulkBookingDetail(OM_BulkBookingDetail bulkBookingDetail)
        {
           var bkBookingDetail=   _bulkBookingDetailRepository.FindOne(x => x.BulkBookingDetailId == bulkBookingDetail.BulkBookingDetailId);
            bkBookingDetail.SequenceNo = bulkBookingDetail.SequenceNo;
            bkBookingDetail.BuyerRefId = bulkBookingDetail.BuyerRefId;
            bkBookingDetail.OrderNo = bulkBookingDetail.OrderNo;
            bkBookingDetail.StyleNo = bulkBookingDetail.StyleNo;
            bkBookingDetail.Fabrication = bulkBookingDetail.Fabrication;
            bkBookingDetail.ItemName = bulkBookingDetail.ItemName;
            bkBookingDetail.GSM = bulkBookingDetail.GSM;
            bkBookingDetail.ShipDate = bulkBookingDetail.ShipDate;
            return _bulkBookingDetailRepository.Edit(bkBookingDetail);
        }

        public int SaveBulkBookingDetail(OM_BulkBookingDetail bulkBookingDetail)
        {
            bulkBookingDetail.CompId = PortalContext.CurrentUser.CompId;
            return _bulkBookingDetailRepository.Save(bulkBookingDetail);
        }
        public int GetNextSequenceNo(long bulkBookingId)
        {
            int sequenceNo = 0;
            var bulkBookingLsit = _bulkBookingDetailRepository.Filter(x => x.BulkBookingId == bulkBookingId).ToList();
            if (bulkBookingLsit.Any())
            {
                sequenceNo=bulkBookingLsit.Max(x => x.SequenceNo);
            }
            return sequenceNo + 1;
        }
        public int DeleteBulkBookingDetailById(long bulkBookingDetailId)
        {
            return _bulkBookingDetailRepository.Delete(x => x.BulkBookingDetailId == bulkBookingDetailId);
        }
    }
}
