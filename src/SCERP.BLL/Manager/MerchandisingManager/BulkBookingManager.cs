using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BulkBookingManager : IBulkBookingManager
    {
        private readonly IBulkBookingRepository _bulkBookingRepository;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        public BulkBookingManager(IUserMerchandiserRepository userMerchandiserRepository,IBulkBookingRepository bulkBookingRepository)
        {
            _bulkBookingRepository = bulkBookingRepository;
            _userMerchandiserRepository = userMerchandiserRepository;
        }

        public List<OM_BulkBooking> GetBulkBookingList(string serarchString)
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit = _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == PortalContext.CurrentUser.CompId).Select(x => x.MerchandiserRefId).ToArray();
            return _bulkBookingRepository.GetWithInclude(x => x.CompId == PortalContext.CurrentUser.CompId && (x.BulkBookingRefId.Contains(serarchString) || serarchString==null), "OM_Merchandiser").Where(x => permitedMerchandiserLsit.Contains(x.OM_Merchandiser.EmpId)).ToList();
        }

        public OM_BulkBooking GetBulkBookingById(long bulkBookingId)
        {
            return _bulkBookingRepository.FindOne(x => x.BulkBookingId == bulkBookingId);
        }

        public int SaveBulkBooking(OM_BulkBooking bulkBooking)
        {
            bulkBooking.CompId = PortalContext.CurrentUser.CompId;
           return _bulkBookingRepository.Save(bulkBooking);
        }

        public int EditBulkBooking(OM_BulkBooking bulkBooking)
        {
            var bkBooking=_bulkBookingRepository.FindOne(x => x.BulkBookingId == bulkBooking.BulkBookingId);
            bkBooking.MerchadiserId = bulkBooking.MerchadiserId;
            bkBooking.BookingDate = bulkBooking.BookingDate;
            bkBooking.Attention = bulkBooking.Attention;
            bkBooking.Note = bulkBooking.Note;
            return _bulkBookingRepository.Edit(bkBooking);
        }

        public string GetNewRefId(string compId)
        {
          
            string refNo = _bulkBookingRepository.Filter(x => x.CompId ==compId).Max(x => x.BulkBookingRefId.Substring(2, 8)) ?? "0";
            string newRefNo = "BK" + refNo.IncrementOne().PadZero(6);
            return newRefNo;
        }

        public int DeleteBulkBookingById(long bulkBookingId)
        {
          return  _bulkBookingRepository.Delete(x => x.BulkBookingId == bulkBookingId);
        }
    }
}
