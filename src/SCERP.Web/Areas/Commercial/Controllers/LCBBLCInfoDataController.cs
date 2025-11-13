using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class LCBBLCInfoDataController : BaseController
    {
        // GET: Commercial/ImportDocsData
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;

        private readonly IOmBuyerManager _buyerManager;
        private readonly IUserMerchandiserManager _userMerchandiser;
        public LCBBLCInfoDataController(ITimeAndActionManager timeAndActionManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IOmBuyerManager buyerManager, IUserMerchandiserManager userMerchandiser)
        {
            _timeAndActionManager = timeAndActionManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;
            _buyerManager = buyerManager;
            _userMerchandiser = userMerchandiser;

        }
        //[AjaxAuthorize(Roles = "tnaxl-1,tnaxl-2,tnaxl-3")]
        public ActionResult Index(TimeAndActionViewModel model)
        {

            ModelState.Clear();
            dynamic tna = _timeAndActionManager.GetStyleWiseTna(model.OrderNo, model.OrderStyleRefId,model.BuyerRefId, PortalContext.CurrentUser.CompId,model.SearchString,model.ActivitySearchKey,model.FromDate);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            model.IsMerchandiser = _userMerchandiser.IsUserMerchandiser(PortalContext.CurrentUser.UserId);
            model.Tnas = tna;
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
        //[AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult AddRow(TimeAndActionViewModel model)
        {
            int updated = _timeAndActionManager.AddRows(model.RowNumber, model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            return Reload();
        }

        [HttpPost]
        //[AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult RemoveRow(TimeAndActionViewModel model)
        {
            int updated = _timeAndActionManager.RemoveRow(model.RowNumber, model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            return Reload();
        }
        [HttpPost]
        //[AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult CopyAndPast(TimeAndActionViewModel model)
        {
            bool isExist = _timeAndActionManager.IsExistTnA(model.CopyOrderStyleRefId);
            if (!isExist)
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
            else
            {
                return ErrorResult("TNA Already Exist");
            }

        }


        public ActionResult TnAReport(string orderStyleRefId)
        {
            DataTable reportData = _timeAndActionManager.GetTnAReport(orderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "TNA_REPORT.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("TNADS", reportData) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

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

    }
}