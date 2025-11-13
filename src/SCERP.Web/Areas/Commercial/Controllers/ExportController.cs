using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class ExportController : BaseController
    {
        private readonly IExportManager _exportManager;
        private readonly ILcManager _lcManager;
        private readonly ICountryManager _countryManager;
        private readonly IOmColorManager _omColorManager;
        private readonly IOmSizeManager _omSizeManager;
        private readonly ISalesContactManager _salesContactManager;

        public ExportController(IExportManager exportManager, ILcManager lcManager, ICountryManager countryManager, IOmSizeManager omSizeManager, IOmColorManager omColorManager,ISalesContactManager salesContactManager)
        {
            _exportManager = exportManager;
            _lcManager = lcManager;
            _countryManager = countryManager;
            _omColorManager = omColorManager;
            _omSizeManager = omSizeManager;
            _salesContactManager = salesContactManager;
        }

        public ActionResult Index(ExportViewModel model)
        {
            ModelState.Clear();
            int totalRecords;
            model.CommExports = _exportManager.GetExportByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(ExportViewModel model)
        {
            ModelState.Clear();

            if (model.CommExport.ExportId > 0)
            {
                model.CommExport = _exportManager.GetExportById(model.CommExport.ExportId);
                model.CommExport.InvoiceValue = model.CommExport.InvoiceValue == null ? 0 : model.CommExport.InvoiceValue;
                model.CommExport.RealizedValue = model.CommExport.RealizedValue == null ? 0 : model.CommExport.RealizedValue;
                model.CommExport.ShortFallAmount = model.CommExport.InvoiceValue - model.CommExport.RealizedValue;
            }
            else
            {
                model.CommExport.ExportRefId = _exportManager.GetNewExportRefId();
            }
            
            model.CommLcInfos = _lcManager.GetAllLcInfos();
            model.CommSalesContacts = _salesContactManager.GetSalesContacts(model.CommExport.LcId);
            return View(model);
        }

        public ActionResult Save(ExportViewModel model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.CommExport.ExportId > 0 ? _exportManager.EditExport(model.CommExport) : _exportManager.SaveExport(model.CommExport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }

        public ActionResult Delete(ExportViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = _exportManager.DeleteExport(model.CommExport.ExportId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }

        public void GetExcel(ExportViewModel model)
        {
            var branches = _exportManager.GetExportLsit(model.FromDate, model.ToDate, model.SearchString);
            const string fileName = "ExportList";
            var boundFields = new List<BoundField>
            {
                new BoundField() {HeaderText = @"LCNO", DataField = "COMMLcInfo.LcNo"},
                new BoundField() {HeaderText = @"ExportNo", DataField = "ExportNo"},
                new BoundField() {HeaderText = @"ExportDate", DataField = "ExportDate"},
                new BoundField() {HeaderText = @"InvoiceNo", DataField = "InvoiceNo"},
                new BoundField() {HeaderText = @"InvoiceValue", DataField = "InvoiceValue"},
                new BoundField() {HeaderText = @"BankRefNo", DataField = "BankRefNo"},
                new BoundField() {HeaderText = @"BankRefDate", DataField = "BankRefDate"},
                new BoundField() {HeaderText = @"RealizedValue", DataField = "RealizedValue"},
                new BoundField() {HeaderText = @"RealizedDate", DataField = "RealizedDate"},
                new BoundField() {HeaderText = @"BillOfLadingNo", DataField = "BillOfLadingNo"},
                new BoundField() {HeaderText = @"BillOfLadingDate", DataField = "BillOfLadingDate"},
                new BoundField() {HeaderText = @"SBNo", DataField = "SBNo"},
                new BoundField() {HeaderText = @"SBNoDate", DataField = "SBNoDate"},
            };

            ReportConverter.CustomGridView(boundFields, branches, fileName);
        }



        /* Commercial Invoice */

        public ActionResult Detail(ExportViewModel model)
        {
            ModelState.Clear();

            model.BuyerOrders = _exportManager.GetBuyerOrderbyExportId(model.CommExport.ExportId);
            model.ExportId = model.CommExport.ExportId;

            List<CommExportDetail> exportDetail = _exportManager.GetExportDetailByExportId(model.CommExport.ExportId);

            foreach (var t in exportDetail)
            {
                model.CommExportDetail.Add(t.OrderStyleRefId, t);
            }
            return View(model);
        }

        public ActionResult AddNewRow(ExportViewModel model)
        {
            if (model.ExportDetail.CartonQuantity > 0 || model.ExportDetail.ItemQuantity > 0)
            {
                var expDetail = new CommExportDetail
                {
                    OrderStyleRefId = model.ExportDetail.OrderStyleRefId,
                    CartonQuantity = model.ExportDetail.CartonQuantity,
                    ItemQuantity = model.ExportDetail.ItemQuantity,
                    Rate = model.ExportDetail.Rate,
                    ItemDescription = model.ExportDetail.ItemDescription,
                    ExportCode = model.ExportDetail.ExportCode,
                };
                model.CommExportDetail.Add(model.ExportDetail.OrderStyleRefId, expDetail);
            }

            if (model.CommExportDetail != null && model.ExportDetail.Rate == null)
            {
                return ErrorResult("Please Insert a valid Commercial Invoice !");
            }
            else
            {
                return PartialView("~/Areas/Commercial/Views/Export/_AddNewRow.cshtml", model);
            }
        }

        public ActionResult SaveDetail(ExportViewModel model)
        {
            var saveIndex = 0;

            try
            {
                foreach (var t in model.CommExportDetail)
                {
                    var detail = new CommExportDetail
                    {
                        ExportId = model.ExportId,
                        OrderStyleRefId = t.Value.OrderStyleRefId,
                        CartonQuantity = t.Value.CartonQuantity,
                        ItemQuantity = t.Value.ItemQuantity,
                        Rate = t.Value.Rate,
                        ItemDescription = t.Value.ItemDescription,
                        ExportCode = t.Value.ExportCode
                    };

                    var isExist = _exportManager.IsExistExportDetail(detail);
                    saveIndex = isExist ? _exportManager.EditExportDetail(detail) : _exportManager.SaveExportDetail(detail);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }

        public JsonResult GetStyleNoByOrderNo(string orderNo)
        {
            var style = _exportManager.GetStyleNoByOrderNo(orderNo);
            return Json(style, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesContacByLcId(int LcId)
        {
            var style = _salesContactManager.GetSalesContacts(LcId);
            return Json(style, JsonRequestBehavior.AllowGet);
        }

        /* Packing List Detail */

        public ActionResult DetailPacking(ExportViewModel model)
        {
            ModelState.Clear();

            model.BuyerOrders = _exportManager.GetBuyerOrderbyExportId(model.CommExport.ExportId);
            model.ExportId = model.CommExport.ExportId;

            List<CommPackingListDetail> packingListDetail = _exportManager.GetPackingDetailByExportId(model.CommExport.ExportId);

            foreach (var t in packingListDetail)
            {
                model.CommPackingListDetail.Add(t.OrderStyleRefId, t);
            }

            return View(model);
        }

        public ActionResult AddPackNewRow(ExportViewModel model)
        {
            if (model.PackingListDetail.CartonQuantity > 0 || model.PackingListDetail.CartonCapacity > 0)
            {
                var packDetail = new CommPackingListDetail
                {
                    OrderStyleRefId = model.ExportDetail.OrderStyleRefId,
                    Block = model.PackingListDetail.Block,
                    CountryName = model.PackingListDetail.CountryName,
                    ColorName = model.PackingListDetail.ColorName,
                    SizeName = model.PackingListDetail.SizeName,
                    CartonQuantity = model.PackingListDetail.CartonQuantity,
                    CartonCapacity = model.PackingListDetail.CartonCapacity,
                    CartonFrom = model.PackingListDetail.CartonFrom,
                    CartonTo = model.PackingListDetail.CartonFrom + 10,
                    ContainerNo = model.PackingListDetail.ContainerNo
                };
                model.CommPackingListDetail.Add(model.ExportDetail.OrderStyleRefId, packDetail);
            }

            if (model.PackingListDetail != null && model.PackingListDetail.CartonQuantity == null)
            {
                return ErrorResult("Please Insert a valid Packing Information !");
            }
            else
            {
                return PartialView("~/Areas/Commercial/Views/Export/_AddPackNewRow.cshtml", model);
            }
        }

        public ActionResult SavePackingDetail(ExportViewModel model)
        {
            var saveIndex = 0;

            try
            {
                foreach (var t in model.CommPackingListDetail)
                {
                    var pack = new CommPackingListDetail
                    {
                        ExportId = model.ExportId,
                        OrderStyleRefId = t.Value.OrderStyleRefId,

                        Block = t.Value.Block,
                        CountryName = t.Value.CountryName,
                        ColorName = t.Value.ColorName,
                        SizeName = t.Value.SizeName,
                        CartonQuantity = t.Value.CartonQuantity,
                        CartonCapacity = t.Value.CartonCapacity,
                        CartonFrom = t.Value.CartonFrom,
                        ContainerNo = t.Value.ContainerNo
                    };

                    var isExist = false;
                    saveIndex = isExist ? _exportManager.EditPackDetail(pack) : _exportManager.SavePackDetail(pack);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }

        public ActionResult TagCountrySearch(string term)
        {
            List<string> tags = _countryManager.GetAllCountries().Select(p => p.CountryName).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagColorSearch(string term)
        {
            List<string> tags = _omColorManager.GetOmColors().Select(p => p.ColorName).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagSizeSearch(string term)
        {
            List<string> tags = _omSizeManager.GetOmSizes().Select(p => p.SizeName).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CommercialInvoice(ExportViewModel model)
        {
            return View(model);
        }

        public ActionResult PackingList(ExportViewModel model)
        {
            return View(model);
        }
    }
}