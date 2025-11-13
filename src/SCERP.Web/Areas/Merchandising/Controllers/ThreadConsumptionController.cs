using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ThreadConsumptionController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IThreadConsumptionManager _threadConsumptionManager;
        private readonly IBuyOrdStyleSizeManager _buyOrdStyleSizeManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public ThreadConsumptionController(IThreadConsumptionManager threadConsumptionManager, IOmBuyerManager buyerManager, IBuyOrdStyleSizeManager buyOrdStyleSizeManager, IOmBuyOrdStyleManager buyOrdStyle)
        {
            _threadConsumptionManager = threadConsumptionManager;
            _buyerManager = buyerManager;
            _buyOrdStyleSizeManager = buyOrdStyleSizeManager;
            _buyOrdStyle = buyOrdStyle;
        }
         [AjaxAuthorize(Roles = "threadconsumption-1,threadconsumption-2,threadconsumption-3")]
        public ActionResult Index(ThreadConsumptionViewModel model)
        {
             ModelState.Clear();
             model.Consumptions = _threadConsumptionManager.GetThreadConsumptionsByPaging(PortalContext.CurrentUser.CompId,model.SearchString);
            return View(model);
        }
        [AjaxAuthorize(Roles = "threadconsumption-2,threadconsumption-3")]
        public ActionResult Edit(ThreadConsumptionViewModel model)
        {
            ModelState.Clear();
            
            if (model.Consumption.ThreadConsumptionId>0)
            {
                model.Consumption =
                    _threadConsumptionManager.GetThreadConsumptionById(model.Consumption.ThreadConsumptionId);
                model.ConsumptionDetails =
                    model.Consumption.OM_ThreadConsumptionDetail.ToDictionary(
                        x => Convert.ToString(x.ThreadConsumptionDetailId), x => x);
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.Consumption.BuyerRefId);
                model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.Consumption.OrderNo);
                model.SizeList = _buyOrdStyleSizeManager.GetBuyOrdStyleSize(model.Consumption.OrderStyleRefId);
            }
            else
            {
                 model.Consumption.EntryDate=DateTime.Now;
            }
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }
        [AjaxAuthorize(Roles = "threadconsumption-2,threadconsumption-3")]
        public ActionResult Save(ThreadConsumptionViewModel model)
        {
            int saveIndex = 0;
            model.Consumption.OM_ThreadConsumptionDetail=model.ConsumptionDetails.Select(x => x.Value).ToList();
            model.Consumption.CompId = PortalContext.CurrentUser.CompId;
            model.Consumption.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            model.Consumption.IsApproved = false;
            saveIndex = model.Consumption.ThreadConsumptionId>0 ? _threadConsumptionManager.EditThreadConsumption(model.Consumption) : _threadConsumptionManager.SaveThreadConsumption(model.Consumption);
            if (saveIndex>0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return ErrorResult("Save Failed");
            }
           
        }
          [AjaxAuthorize(Roles = "threadconsumption-3")]
        public ActionResult Delete(int threadConsumptionId)
        {
            int deleted = _threadConsumptionManager.DeleteThreadConsumption(threadConsumptionId);
            return deleted>0 ? Reload() : ErrorResult("Delete Failed");
        }
          [AjaxAuthorize(Roles = "threadconsumption-2,threadconsumption-3")]
        public JsonResult ApproveThreadConsumption(int threadConsumptionId)
        {
            int approved = _threadConsumptionManager.ApproveThreadConsumption(threadConsumptionId);
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }
        public ActionResult AddNewRow([Bind(Include = "ConsumptionDetail")]ThreadConsumptionViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.ConsumptionDetails.Add(model.Key, model.ConsumptionDetail);
            return PartialView("~/Areas/Merchandising/Views/ThreadConsumption/_ThreadConsDetail.cshtml", model);
        }


        public JsonResult GetSizeListByOrderStyleRefId(string orderStyleRefId)
        {
          var sieList=  _buyOrdStyleSizeManager.GetBuyOrdStyleSize(orderStyleRefId);
            return Json(sieList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetThreadConsumptionsReport(long threadConsumptionId)
        {

            DataTable dataTable = _threadConsumptionManager.GetThreadConsumptionsReportDataTable(threadConsumptionId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC/"), "ThreadConsumptionSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ThreadConsDataSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .5 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

	}
}