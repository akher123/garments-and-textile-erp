using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class DyeingJobOrderController : BaseController
    {
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IDyeingJobOrderManager _dyeingJobOrderManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        private readonly IProcessorManager _processorManager;
        public DyeingJobOrderController(IProcessorManager processorManager,IDyeingJobOrderManager dyeingJobOrderManager, IPartyManager partyManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager buyOrdStyle)
        {
            _dyeingJobOrderManager = dyeingJobOrderManager;
            _partyManager = partyManager;
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
            _processorManager = processorManager;
        }
        public ActionResult Index(DyeingJobOrderViewModel model)
        {
            ModelState.Clear();
            int totalRecord = 0;
            model.JobOrders = _dyeingJobOrderManager.GetDyeingJobOrderByPaging(model.SearchString, model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.JobOrder.PartyId, out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }
        public ActionResult Edit(DyeingJobOrderViewModel model)
        {
            ModelState.Clear();
            if (model.JobOrder.DyeingJobOrderId>0)
            {
                model.JobOrder = _dyeingJobOrderManager.GetDyeingJobOrderById(model.JobOrder.DyeingJobOrderId);
                model.ChallanNo = model.JobOrder.WorkOrderNo;
                List<VwDyeingJobOrderDetail> dyeingJobOrdersDetails =_dyeingJobOrderManager.GetDyeingJobOrderDetails(model.JobOrder.DyeingJobOrderId);
                model.Dictionary = dyeingJobOrdersDetails.ToDictionary(x => Convert.ToString(x.DyeingJobOrderDetailId), x => x);
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.JobOrder.BuyerRefId);
                model.StyleList = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.JobOrder.OrderNo);
                model.Dropdowns = _dyeingJobOrderManager.GetKnittingRollIssueChallan(model.JobOrder.OrderStyleRefId);
            }
            else
            {
                model.JobOrder.JobRefId = _dyeingJobOrderManager.GetJobRefId(PortalContext.CurrentUser.CompId);
                model.JobOrder.JobDate=DateTime.Now;
                model.JobOrder.DeliveryDate = DateTime.Now.AddDays(5);
                model.JobOrder.JobType = "IN";
                model.JobOrder.PartyId =1;
            }
            model.Parties = _partyManager.GetParties("P");
            model.BuyerList = _buyerManager.GetAllBuyers();
            model.ProcessList = _processorManager.GetProcessList();

            return View(model);
        }
        public ActionResult Save(DyeingJobOrderViewModel model)
        {
            try
            {
                model.JobOrder.PROD_DyeingJobOrderDetail = model.Dictionary.Select(x => new PROD_DyeingJobOrderDetail()
                {
                    ItemId = x.Value.ItemId,
                    ComponentRefId = x.Value.ComponentRefId,
                    ColorRefId = x.Value.ColorRefId,
                    MdSizeRefId = x.Value.MdSizeRefId,
                    FdSizeRefId = x.Value.FdSizeRefId,
                    CompId = PortalContext.CurrentUser.CompId,
                    Gsm = x.Value.Gsm,
                    Quantity = x.Value.Quantity,
                    GreyWit = x.Value.GreyWit,
                    Rate = x.Value.Rate,
                    Remarks = x.Value.Remarks,
                }).ToList();

                model.JobOrder.CompId = PortalContext.CurrentUser.CompId;
                int saveIndex = 0;
                model.JobOrder.WorkOrderNo = model.JobOrder.WorkOrderNo??model.ChallanNo;
                saveIndex = model.JobOrder.DyeingJobOrderId>0 ? _dyeingJobOrderManager.EditDyeingJobOrder(model.JobOrder) : _dyeingJobOrderManager.SaveDyeingJobOrder(model.JobOrder);
                if (saveIndex > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return ErrorResult("Job order save failed");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        public ActionResult AddJobDetailRow(DyeingJobOrderViewModel model)
        {
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.Dictionary.Add(key, model.JobOrderDetail);
            return View("~/Areas/Production/Views/DyeingJobOrder/_NewRow.cshtml", model);
        }
        public ActionResult Delete(long dyeingJobOrderId)
        {
            int deleted = 0;

            try
            {
               deleted= _dyeingJobOrderManager.DeleteDyeingJobOrderDetail(dyeingJobOrderId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Delete Failed");
            }
            return Reload();
        }


        public ActionResult DyeingJobOrderReport(long dyeingJobOrderId)
        {
            var rType = ReportType.PDF;
            DataTable dataTable = _dyeingJobOrderManager.DyeingJobOrderReportDataTable(dyeingJobOrderId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DyeingJobOrderReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DyeingJobOrderDset", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(rType, path, reportDataSources, deviceInformation);
        }

        public ActionResult LoadKnittingRollIssueChallan(string challanRefId)
        {
            DyeingJobOrderViewModel model = new DyeingJobOrderViewModel();
            List<VwDyeingJobOrderDetail> dyeingJobOrdersDetails = _dyeingJobOrderManager.LoadKnittingRollIssueChallan(challanRefId);
            foreach (var item in dyeingJobOrdersDetails)
            {
                model.Dictionary.Add(Guid.NewGuid().ToString(), item);
            }
            return View("~/Areas/Production/Views/DyeingJobOrder/_NewRow.cshtml", model);
        }

        public ActionResult GetKnittingRollIssueChallan(string orderStyleRefId)
        {
            List<Dropdown> rollIsseChallans = _dyeingJobOrderManager.GetKnittingRollIssueChallan(orderStyleRefId);
            return Json(rollIsseChallans, JsonRequestBehavior.AllowGet);
        }
    }
}