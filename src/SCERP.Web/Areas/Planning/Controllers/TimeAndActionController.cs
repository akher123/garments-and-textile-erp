using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TimeAndActionController : BaseController
    {
       
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;

        private readonly IOmBuyerManager _buyerManager;
        private readonly IUserMerchandiserManager _userMerchandiser;
        public TimeAndActionController(ITimeAndActionManager timeAndActionManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IOmBuyerManager buyerManager, IUserMerchandiserManager userMerchandiser)
        {
            _timeAndActionManager = timeAndActionManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;
            _buyerManager = buyerManager;
            _userMerchandiser = userMerchandiser;

        }
        [AjaxAuthorize(Roles = "tnaxl-1,tnaxl-2,tnaxl-3")]
        public ActionResult Index(TimeAndActionViewModel model)
        {
        
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.IsMerchandiser = !_userMerchandiser.IsUserMerchandiser(PortalContext.CurrentUser.UserId);
            DataTable table = _timeAndActionManager.GetStyleWiseTna(model.OrderNo ?? "-1", model.OrderStyleRefId ?? "-1", model.BuyerRefId, PortalContext.CurrentUser.CompId, model.SearchString, model.ActivitySearchKey, model.FromDate);
            model.Tnas = table.Todynamic();
            return View(model);
        }
        [HttpPost]
        // [AjaxAuthorize(Roles = "tnaxl-2,tnaxl-3")]
        public ActionResult UpdateTna(int tnaRowId, string key, string value)
        {
            int updated = _timeAndActionManager.UpdateTna(PortalContext.CurrentUser.CompId, tnaRowId, key, value);
            return Json(true);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult AddRow(TimeAndActionViewModel model)
        {

            int updated = _timeAndActionManager.AddRows(model.RowNumber, model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            return Reload();
        }

        [HttpPost]
        [AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult RemoveRow(TimeAndActionViewModel model)
        {
            var orderStyle = _omBuyOrdStyleManager.GetBuyOrdStyleByRefId(model.OrderStyleRefId);
            if (orderStyle.TnaMode.Equals("L"))
            {
                return ErrorResult("Tna Already Locked By Merchandiser Manager");
            }
            else
            {
                int updated = _timeAndActionManager.RemoveRow(model.RowNumber, model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
                return Reload();
            }

        }
        [HttpPost]
        [AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult CopyAndPast(TimeAndActionViewModel model)
        {
            bool isExist = _timeAndActionManager.IsExistTnA(model.CopyOrderStyleRefId);
            if (!isExist)
            {
                var orderStyle = _omBuyOrdStyleManager.GetBuyOrdStyleByRefId(model.OrderStyleRefId);
                if (orderStyle.TnaMode.Equals("L"))
                {
                    return ErrorResult("Tna Already Locked By Merchandiser Manager");
                }
                else
                {
                    bool saved = _timeAndActionManager.TnACopyAndPast(model.OrderStyleRefId, model.CopyOrderStyleRefId);
                    if (saved)
                    {
                        return Reload();
                    }
                    else
                    {
                        return ErrorResult("TNA Copy and Past Failed !");
                    }
                }
            }
            else
            {
                return ErrorResult("TNA Already Exist");
            }

        }


        public ActionResult TnAReport(string orderStyleRefId, ReportType reportType)
        {
            DataTable reportData = _timeAndActionManager.GetTnAReport(orderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TNA_REPORT.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("TNADS", reportData) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult TnAReportOutput(TimeAndActionViewModel model)
        {
            DataTable reportData = _timeAndActionManager.GetStyleWiseTna(model.OrderNo ?? "-1", model.OrderStyleRefId ?? "-1", model.BuyerRefId, PortalContext.CurrentUser.CompId, model.SearchString, model.ActivitySearchKey, model.FromDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TNA_REPORT_OUTPUT.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("TNADS", reportData) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.Excel, path, reportDataSources, deviceInformation);

        }
        public ActionResult TnaStatus(TimeAndActionViewModel model, string indicationKey = "D")
        {
            string color = "";
            string status = "";
            indicationKey = indicationKey ?? "0";
            switch (indicationKey)
            {
                case "Y":
                    color = "yellow";
                    status = "Having only 5 to begin";
                    break;
                case "A":
                    color = "palevioletred";
                    status = "Having only 3 to begin";
                    break;
                case "R":
                    color = "red";
                    status = "Not started as per plan";
                    break;
                default:
                    color = "#b30000";
                    status = "running 3 days late";
                    break;

            }
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.IndicationKey = indicationKey;
            ViewBag.Color = color;
            ViewBag.Status = status;
            model.Tnas = _timeAndActionManager.GetTnaStatus(PortalContext.CurrentUser.CompId, indicationKey, model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
            return View(model);
        }

        public ActionResult TnaApprove(TimeAndActionViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.IsLocked = model.IsLocked ?? "U";
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.OrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyles(model.PageIndex, model.BuyerRefId, model.OrderNo, model.OrderStyleRefId, model.IsLocked, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult TnaApproved(int orderStyleId)
        {
            int index = 0;
            index = _omBuyOrdStyleManager.TnaApproved(orderStyleId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);

        }

        public ActionResult HorizontalTna(TimeAndActionViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            List<OM_TnaActivity> activities = _timeAndActionManager.GetTaActivityList();
            var tnas = _timeAndActionManager.GetHorizontalTna(model.OrderNo ?? "-1", model.OrderStyleRefId ?? "-1", model.BuyerRefId ?? "-1", PortalContext.CurrentUser.CompId);
            model.Tnas = tnas.Todynamic();
            model.Heads = new List<string>() { "MERCHANTDISER", "BUYER", "ORDER", "STYLE", "QTY", "SHIPMENT" };
            model.Heads.AddRange(activities.Select(x => x.Name));
            model.Columns = new List<object>() { new { data = "Merchandiser", readOnly = true, renderer = "html" }, new { data = "Buyer", readOnly = true }, new { data = "Order", readOnly = true }, new { data = "Style", readOnly = true, renderer = "html" }, new { data = "Quantity", readOnly = true }, new { data = "ShipDate", readOnly = true } };
            model.Columns.AddRange(activities.Select(x => new { data = x.ShortName, readOnly = true, renderer = "html" }).ToList());
            return View(model);
        }

        public JsonResult GetTnaActivity(string searchString)
        {
            List<string> ativities = _timeAndActionManager.GetTnaActivity(searchString);
            return Json(ativities, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTnaResponsible(string searchString)
        {
            List<string> ativities = _timeAndActionManager.GetTnaResponsible(searchString);
            return Json(ativities, JsonRequestBehavior.AllowGet);
        }
    }
}