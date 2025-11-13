using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class EmbroideryProcessController : BaseController
    {
        private readonly IProcessDeliveryManager _processDeliveryManager;
        private readonly ICuttingTagManager _cuttingTagManager;
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly ITimeAndActionManager _timeAndActionManager;
        public EmbroideryProcessController(ITimeAndActionManager timeAndActionManager, IOmBuyOrdStyleManager buyOrdStyleManager, IPartyManager partyManager, ICuttingTagManager cuttingTagManager, IProcessDeliveryManager processDeliveryManager)
        {
            _processDeliveryManager = processDeliveryManager;
            _cuttingTagManager = cuttingTagManager;
            _partyManager = partyManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            this._timeAndActionManager = timeAndActionManager;
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-1,embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult Index(ProcessDeliveryViewModel model)
        {
            const string factoryType = "F";
            ModelState.Clear();
            var totalRecords = 0;
            model.PartyList = _partyManager.GetParties(factoryType);
            model.Delivery.ProcessRefId = ProcessCode.EMBROIDARY;
            model.Deliveries = _processDeliveryManager.GetProcessDelivery(model.PageIndex, model.Delivery.ProcessRefId, model.Delivery.PartyId, model.SearchString, model.Delivery.OrderStyleRefId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View("~/Areas/Production/Views/EmbroideryProcess/Index.cshtml", model);
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult Edit(ProcessDeliveryViewModel model)
        {
            ModelState.Clear();
            const string factoryType = "F";
            model.Delivery.InvDate = DateTime.Now;
            model.PartyList = _partyManager.GetParties(factoryType);
            model.Delivery.RefNo = _processDeliveryManager.GetEmbroideryDeliveryRefNo();
            if (model.Delivery.ProcessDeliveryId > 0)
            {
                model.CuttingJobDictionary = _processDeliveryManager.GetSizeNameDictionry(model.Delivery.ProcessDeliveryId);
                model.Delivery = _processDeliveryManager.GetProcessDeliveryById(model.Delivery.ProcessDeliveryId);
                model.DoDictionary = _processDeliveryManager.GetProcessDeliveryDictionary(model.Delivery.ProcessDeliveryId);
                model.Buyers = _processDeliveryManager.GetBuyerByPartyId(model.Delivery.PartyId);
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.Delivery.BuyerRefId);
                model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.Delivery.OrderNo);
                const bool isEmbroidery = true;
                const bool isPrintable = false;
                model.ComponentRefId = "001";// "Body sequence"
                model.CuttiongProcesses = _processDeliveryManager.GetPartyWiseCuttingDeliveryProcess(model.Delivery.PartyId, model.Delivery.OrderStyleRefId, model.ColorRefId, model.ComponentRefId, isPrintable, isEmbroidery, ProcessCode.EMBROIDARY);

            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult Save(ProcessDeliveryViewModel model)
        {
            int saveIndex = 0;
            try
            {
                model.Delivery.ProcessRefId = ProcessCode.EMBROIDARY;
                foreach (var detail in model.DoDictionary.Select(x => x.Value).SelectMany(dictionary => dictionary.Select(x => x.Value)))
                {
                    model.Delivery.PROD_ProcessDeliveryDetail.Add(detail);
                }
                if (model.Delivery.PROD_ProcessDeliveryDetail.Any())
                {
                    if (model.Delivery.ProcessDeliveryId > 0)
                    {

                        saveIndex = _processDeliveryManager.EditProcessDelivery(model.Delivery);
                    }
                    else
                    {
                        model.Delivery.RefNo = _processDeliveryManager.GetEmbroideryDeliveryRefNo();
                        saveIndex = _processDeliveryManager.SaveProcessDelivery(model.Delivery);
                    }

                }
                else
                {
                    return ErrorResult("Not Enough Delivery Quantity");
                }

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            if (saveIndex > 0)
            {
                return RedirectToAction("index");
            }
            else
            {

                return ErrorResult("Failed To Save Embroidery Delivery ");
            }


        }
        [AjaxAuthorize(Roles = "embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult CuttingJobQty(ProcessDeliveryViewModel model)
        {
            ModelState.Clear();
            model.CuttingJobDictionary = _processDeliveryManager.GetJobWiserBalanceDelivery(model.CuttingBatch.CuttingBatchId, model.CuttingTagId, ProcessCode.EMBROIDARY);

            var sizeList = model.CuttingJobDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
            model.DeliveryDetails = new Dictionary<string, PROD_ProcessDeliveryDetail>();
            foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
            {
                model.DeliveryDetails.Add(size.Value, new PROD_ProcessDeliveryDetail()
                {
                    SizeRefId = size.Value,
                    ProcessDeliveryId = model.Delivery.ProcessDeliveryId,
                    ColorRefId = model.CuttingBatch.ColorRefId,
                    ComponentRefId = model.CuttingBatch.ComponentRefId,
                    CuttingTagId = model.CuttingTagId,
                    CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                    CompId = PortalContext.CurrentUser.CompId,
                });
            }
            return PartialView("~/Areas/Production/Views/EmbroideryProcess/_CuttingJob.cshtml", model);
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult AddProcessDeliveryDetail(ProcessDeliveryViewModel model)
        {
            List<string> sizeNames = model.CuttingJobDictionary.Where(x => x.Key == "Size").Select(x => x.Value).First();
            model.CuttingJobDictionary.Add(model.CuttingBatch.CuttingBatchRefId, sizeNames);
            model.DoDictionary.Add(model.CuttingBatch.CuttingBatchRefId + "-" + model.CuttingTagId + "*" + model.CuttingBatch.ColorRefId, model.DeliveryDetails);
            return PartialView("~/Areas/Production/Views/EmbroideryProcess/_ProcessDeliveryDetail.cshtml", model);
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-3")]
        public ActionResult Delete(long processDeliveryId)
        {
            int deleted = 0;
            deleted = _processDeliveryManager.DeleteProcessDelivery(processDeliveryId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Embroidery Delivery ");
        }

        public ActionResult GetTagBySequence(string componentRefId, string orderStyleRefId)
        {
            var taglist = _cuttingTagManager.GetTagBySequence(componentRefId, orderStyleRefId);
            return Json(taglist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBuyerByPartyId(long partyId)
        {
            List<OM_Buyer> buyerlist = _processDeliveryManager.GetBuyerByPartyId(partyId);
            return Json(buyerlist, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "embroiderydelivery-2,embroiderydelivery-3")]
        public ActionResult GetPartyWiseCuttingProcess([Bind(Include = "Delivery,ColorRefId,ComponentRefId")] ProcessDeliveryViewModel model)
        {
            const bool isEmbroidery = true;
            const bool isPrintable = false;
            model.CuttiongProcesses = _processDeliveryManager.GetPartyWiseCuttingDeliveryProcess(model.Delivery.PartyId, model.Delivery.OrderStyleRefId, model.ColorRefId, model.ComponentRefId, isPrintable, isEmbroidery, ProcessCode.EMBROIDARY);

            return PartialView("~/Areas/Production/Views/EmbroideryProcess/_PartyWiseCuttingProcess.cshtml", model);
        }

    }
}