using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class BulkBookingController : BaseController
    {
        private readonly IMerchandiserManager _merchandiserManager;
        private readonly IBulkBookingManager _bulkBookingManager;
        private readonly IBulkBookingDetailManager _bulkBookingDetailManager;
        private readonly IBulkBookingYarnDetailManager _bulkBookingYarnDetailManager;
        private readonly IOmBuyerManager _buyerManager;
        public BulkBookingController(IOmBuyerManager buyerManager, IMerchandiserManager merchandiserManager, IBulkBookingManager bulkBookingManager, IBulkBookingDetailManager bulkBookingDetailManager, IBulkBookingYarnDetailManager bulkBookingYarnDetailManager)
        {
            _merchandiserManager = merchandiserManager;
            _bulkBookingManager = bulkBookingManager;
            _bulkBookingDetailManager = bulkBookingDetailManager;
            _bulkBookingYarnDetailManager = bulkBookingYarnDetailManager;
            _buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-1,bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult Index(BulkBookingViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-1,bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult BulkBookingList(BulkBookingViewModel model)
        {
            ModelState.Clear();
            model.BulkBookings = _bulkBookingManager.GetBulkBookingList(model.SerarchString);
            return PartialView("~/Areas/Merchandising/Views/BulkBooking/_BulkBookingList.cshtml", model);
        }

        [AjaxAuthorize(Roles = "bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult Edit(BulkBookingViewModel model)
        {
            model.OmMerchandisers = _merchandiserManager.GetPermitedMerchandisers();
            if (model.BulkBooking.BulkBookingId > 0)
            {
                model.BulkBooking = _bulkBookingManager.GetBulkBookingById(model.BulkBooking.BulkBookingId);
            }
            else
            {
                model.BulkBooking.BookingDate = DateTime.Now;
                model.BulkBooking.BulkBookingRefId = _bulkBookingManager.GetNewRefId(PortalContext.CurrentUser.CompId);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult Save(BulkBookingViewModel model)
        {
            var saved = 0;
            try
            {
                saved = model.BulkBooking.BulkBookingId > 0 ? _bulkBookingManager.EditBulkBooking(model.BulkBooking) : _bulkBookingManager.SaveBulkBooking(model.BulkBooking);

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return saved > 0 ? Reload() : ErrorResult("Save Failed");

        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-3")]
        public ActionResult Delete(long bulkBookingId)
        {
            int delete = 0;
            try
            {
                delete = _bulkBookingManager.DeleteBulkBookingById(bulkBookingId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return delete > 0 ? Reload() : ErrorResult("Delete Failed");

        }

        [AjaxAuthorize(Roles = "bulkyarnbooking-1,bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult BulkBookingDetailLsit(BulkBookingViewModel model)
        {
            ModelState.Clear();
            model.BulkBooking = _bulkBookingManager.GetBulkBookingById(model.BulkBooking.BulkBookingId);
            model.BulkBookingDetails = _bulkBookingDetailManager.GetBulkBookingDetailList(model.BulkBooking.BulkBookingId);
            return PartialView("~/Areas/Merchandising/Views/BulkBooking/_FabricBookingLsit.cshtml", model);
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult EditDetail(BulkBookingViewModel model)
        {

            model.Buyers = _buyerManager.GetAllBuyers();
            if (model.BulkBookingDetail.BulkBookingDetailId > 0)
            {
                model.BulkBookingDetail =
                    _bulkBookingDetailManager.GetBulkBookingDetailId(model.BulkBookingDetail.BulkBookingDetailId);
            }
            else
            {

                model.BulkBookingDetail.SequenceNo = _bulkBookingDetailManager.GetNextSequenceNo(model.BulkBooking.BulkBookingId);
                model.BulkBookingDetail.BulkBookingId = model.BulkBooking.BulkBookingId;

            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult SaveDetail(BulkBookingViewModel model)
        {
            int saved = 0;
            try
            {
                saved = model.BulkBookingDetail.BulkBookingDetailId > 0 ? _bulkBookingDetailManager.EditBulkBookingDetail(model.BulkBookingDetail) : _bulkBookingDetailManager.SaveBulkBookingDetail(model.BulkBookingDetail);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            if (saved > 0)
            {
                model.BulkBooking = _bulkBookingManager.GetBulkBookingById(model.BulkBookingDetail.BulkBookingId);
                model.BulkBookingDetails = _bulkBookingDetailManager.GetBulkBookingDetailList(model.BulkBookingDetail.BulkBookingId);
                return PartialView("~/Areas/Merchandising/Views/BulkBooking/_FabricBookingLsit.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed");
            }
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-3")]
        public ActionResult DeleteDetail(BulkBookingViewModel model)
        {
            int delete = _bulkBookingDetailManager.DeleteBulkBookingDetailById(model.BulkBookingDetail.BulkBookingDetailId);
            if (delete > 0)
            {
                model.BulkBooking = _bulkBookingManager.GetBulkBookingById(model.BulkBookingDetail.BulkBookingId);
                model.BulkBookingDetails = _bulkBookingDetailManager.GetBulkBookingDetailList(model.BulkBookingDetail.BulkBookingId);
                return PartialView("~/Areas/Merchandising/Views/BulkBooking/_FabricBookingLsit.cshtml", model);
            }
            else
            {
                return ErrorResult("Delete Failed");
            }
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-1,bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult BulkBookingYarnDetailLsit(BulkBookingViewModel model)
        {
            ModelState.Clear();
            model.BulkBookingDetail = _bulkBookingDetailManager.GetBulkBookingDetailId(model.BulkBookingDetail.BulkBookingDetailId);
            model.BulkBookingYarnDetails = _bulkBookingYarnDetailManager.GetBulkBookingYarnList(model.BulkBookingDetail.BulkBookingDetailId);
            return PartialView("~/Areas/Merchandising/Views/BulkBooking/_BulkYarnBookingLsit.cshtml", model);
        }
        public ActionResult EditYarnDetail(BulkBookingViewModel model)
        {
            if (model.BulkBookingYarnDetail.BulkBookingYearnDetailId > 0)
            {
                model.BulkBookingYarnDetail = _bulkBookingYarnDetailManager.GetBulkBookingById(model.BulkBookingYarnDetail.BulkBookingYearnDetailId);
            }
            else
            {
                model.BulkBookingYarnDetail.BulkBookingDetailId = model.BulkBookingDetail.BulkBookingDetailId;
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-2,bulkyarnbooking-3")]
        public ActionResult SaveYarnDetail(BulkBookingViewModel model)
        {
            var saved = 0;
            try
            {
                saved = model.BulkBookingYarnDetail.BulkBookingYearnDetailId > 0 ? _bulkBookingYarnDetailManager.EditBulkBookingYarnDetail(model.BulkBookingYarnDetail) : _bulkBookingYarnDetailManager.SaveBulkBookingYarnDetail(model.BulkBookingYarnDetail);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            if (saved > 0)
            {
                ModelState.Clear();
                model.BulkBookingDetail = _bulkBookingDetailManager.GetBulkBookingDetailId(model.BulkBookingYarnDetail.BulkBookingDetailId);
                model.BulkBookingYarnDetails = _bulkBookingYarnDetailManager.GetBulkBookingYarnList(model.BulkBookingYarnDetail.BulkBookingDetailId);
                return PartialView("~/Areas/Merchandising/Views/BulkBooking/_BulkYarnBookingLsit.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed");
            }
        }
        [AjaxAuthorize(Roles = "bulkyarnbooking-3")]
        public ActionResult DeleteYarnDetail(BulkBookingViewModel model)
        {
            int delete = _bulkBookingYarnDetailManager.DeleteBulkBookingYarnDetailById(model.BulkBookingYarnDetail.BulkBookingYearnDetailId);
            if (delete > 0)
            {
                model.BulkBookingDetail = _bulkBookingDetailManager.GetBulkBookingDetailId(model.BulkBookingYarnDetail.BulkBookingDetailId);
                model.BulkBookingYarnDetails = _bulkBookingYarnDetailManager.GetBulkBookingYarnList(model.BulkBookingYarnDetail.BulkBookingDetailId);
                return PartialView("~/Areas/Merchandising/Views/BulkBooking/_BulkYarnBookingLsit.cshtml", model);
            }
            else
            {
                return ErrorResult("Delete Failed");
            }
        }
    }
}