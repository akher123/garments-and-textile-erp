using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingManager _bookingManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly IMerchandiserManager _merchandiserManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        public BookingController(ISupplierCompanyManager supplierCompanyManager, IBookingManager bookingManager, IOmBuyerManager omBuyerManager, IMerchandiserManager merchandiserManager)
        {
            _bookingManager = bookingManager;
            _omBuyerManager = omBuyerManager;
            _merchandiserManager = merchandiserManager;
            _supplierCompanyManager = supplierCompanyManager;

        }
        [AjaxAuthorize(Roles = "accessoriesbooking-1,accessoriesbooking-2,accessoriesbooking-3")]
        public ActionResult Index(BookingViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.Booking.StoreId = (int)StoreType.Acessories;
            model.Bookings = _bookingManager.GetBookingByPaging(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.Booking.StoreId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "accessoriesbooking-2,accessoriesbooking-3")]
        public ActionResult Edit(BookingViewModel model)
        {
            ModelState.Clear();
            if (model.Booking.BookingId > 0)
            {
                Inventory_Booking booking = _bookingManager.GetBookingByid(model.Booking.BookingId);
                model.Booking = booking;
                model.BookingDetailsDictionary = _bookingManager.GetVwBookingDetail(model.Booking.BookingId);
            }
            else
            {
                model.Booking.BookingRefId = _bookingManager.GetNewAccBookingRefId();
                model.Booking.BookingDate = DateTime.Now;
            }

            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            model.OmMerchandisers = _merchandiserManager.GetMerchandisers();
            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            return View(model);
        }
        [AjaxAuthorize(Roles = "accessoriesbooking-2,accessoriesbooking-3")]
        public ActionResult Save(BookingViewModel model)
        {
            int savedIndex = 0;
            try
            {
                string compId = PortalContext.CurrentUser.CompId;
                model.Booking.CompId = compId;
                model.Booking.StoreId = (int)StoreType.Acessories;
                model.Booking.Inventory_BookingDetail = model.BookingDetailsDictionary.Select(x => x.Value).Select(x => new Inventory_BookingDetail()
                {
                    CompId = compId,
                    ItemId = x.ItemId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    Rate = x.Rate,
                    Quantity = x.Quantity
                }).ToList();
                if (model.Booking.BookingId == 0)
                {
                    model.Booking.BookingRefId = _bookingManager.GetNewAccBookingRefId();
                }
                savedIndex = _bookingManager.SaveBooking(model.Booking);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Save Failed");
        }

        public ActionResult AddNewRow(BookingViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.BookingDetailsDictionary.Add(model.Key, model.BookingDetail);
            return PartialView("~/Areas/Inventory/Views/Booking/_AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "accessoriesbooking-3")]
        public ActionResult Delete(long bookingId)
        {
            var deleleteIndex = _bookingManager.DeleteBooking(bookingId);
            return deleleteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }

        public ActionResult BookingPdfReport(long bookingId)
        {
            List<VwBookingDetailReport> bookingDetail = _bookingManager.GetVwBookingDetaliReportById(bookingId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "BookingReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { new ReportParameter("HostingServerAddress",AppConfig.HostingServerAddress), new ReportParameter("ImagePath",PortalContext.CurrentUser.CompanyLogo) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BookingDSet", bookingDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }


        public ActionResult BookingSummary(BookingViewModel model)
        {
            model.Booking.StoreId = (int) StoreType.Acessories;
            List<VwBookingSummaryReport> bookingDetail = _bookingManager.GetVwBookingDetalSummary(model.FromDate, model.ToDate, model.Booking.StoreId);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
            if (model.FromDate == null && model.ToDate == null)
            {
                fromDateParameter = new ReportParameter("FromDate", "ALL");
                toDateParameter = new ReportParameter("ToDate", "ALL");
            }
            else
            {
                fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "BookingSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BookingSummaryDSet", bookingDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }
    }
}