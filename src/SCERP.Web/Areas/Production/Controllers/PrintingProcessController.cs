using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class PrintingProcessController : BaseController
    {
        private readonly IProcessDeliveryManager _processDeliveryManager;
      
        private readonly ICuttingTagManager _cuttingTagManager;
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        public PrintingProcessController(IOmBuyOrdStyleManager buyOrdStyleManager,IPartyManager partyManager, ICuttingTagManager cuttingTagManager, IProcessDeliveryManager processDeliveryManager, IOmBuyerManager buyerManager)
        {
            _processDeliveryManager = processDeliveryManager;
            _cuttingTagManager = cuttingTagManager;
            _partyManager = partyManager;
            _buyOrdStyleManager = buyOrdStyleManager;
        }
        [AjaxAuthorize(Roles = "printingdelivery-1,printingdelivery-2,printingdelivery-3")]
        public ActionResult Index(ProcessDeliveryViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string factoryType = "F";
            model.PartyList = _partyManager.GetParties(factoryType);
            model.Delivery.ProcessRefId = ProcessCode.PRINTING;
            model.Deliveries = _processDeliveryManager.GetProcessDelivery(model.PageIndex, model.Delivery.ProcessRefId,model.Delivery.PartyId, model.SearchString,model.Delivery.OrderStyleRefId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View("~/Areas/Production/Views/PrintingProcess/Index.cshtml", model);
        }
              [AjaxAuthorize(Roles = "printingdelivery-2,printingdelivery-3")]
        public ActionResult Edit(ProcessDeliveryViewModel model)
        {
            ModelState.Clear();
            const string factoryType = "F";
            model.Delivery.InvDate = DateTime.Now;
            model.PartyList = _partyManager.GetParties(factoryType);
            model.Delivery.RefNo = _processDeliveryManager.GetPrintingDeliveryRefNo();
            if (model.Delivery.ProcessDeliveryId > 0)
            {
                model.CuttingJobDictionary = _processDeliveryManager.GetSizeNameDictionry(model.Delivery.ProcessDeliveryId);
                model.Delivery = _processDeliveryManager.GetProcessDeliveryById(model.Delivery.ProcessDeliveryId);
                model.DoDictionary = _processDeliveryManager.GetProcessDeliveryDictionary(model.Delivery.ProcessDeliveryId);
                model.Buyers = _processDeliveryManager.GetBuyerByPartyId(model.Delivery.PartyId);
                model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.Delivery.BuyerRefId);
                model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.Delivery.OrderNo);
                model.ComponentRefId = "001"; //Body Sequence
                const bool isEmbroidery = false;
                const bool isPrintable = true;
                model.CuttiongProcesses = _processDeliveryManager.GetPartyWiseCuttingDeliveryProcess(model.Delivery.PartyId, model.Delivery.OrderStyleRefId, model.ColorRefId, model.ComponentRefId, isPrintable, isEmbroidery, ProcessCode.PRINTING);
             
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "printingdelivery-2,printingdelivery-3")]
        public ActionResult Save(ProcessDeliveryViewModel model)
        {
            int saveIndex = 0;
            try
            {
                model.Delivery.ProcessRefId = ProcessCode.PRINTING;
                foreach (var dictionary in model.DoDictionary.Select(x => x.Value))
                {
                    foreach (var detail in dictionary.Select(x => x.Value))
                    {
                        detail.ProcessDeliveryDetailId = 0;
                        model.Delivery.PROD_ProcessDeliveryDetail.Add(detail);
                    }
                }
                if (model.Delivery.PROD_ProcessDeliveryDetail.Any())
                {
                    if (model.Delivery.ProcessDeliveryId > 0)
                    {
                     
                        saveIndex = _processDeliveryManager.EditProcessDelivery(model.Delivery);
                    }
                    else
                    {
                        model.Delivery.RefNo = _processDeliveryManager.GetPrintingDeliveryRefNo();
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
            if (saveIndex>0)
            {
                return RedirectToAction("index");
            }
            else
            {

                return ErrorResult("Failed To Save Printing Delivery ");
            }

            
        }
        [AjaxAuthorize(Roles = "printingdelivery-2,printingdelivery-3")]
        public ActionResult CuttingJobQty(ProcessDeliveryViewModel model)
        {
            ModelState.Clear();
            model.CuttingJobDictionary = _processDeliveryManager.GetJobWiserBalanceDelivery(model.CuttingBatch.CuttingBatchId,model.CuttingTagId, ProcessCode.PRINTING);
            var sizeList = model.CuttingJobDictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
            model.DeliveryDetails=new Dictionary<string, PROD_ProcessDeliveryDetail>();
            foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
            {
                model.DeliveryDetails.Add(size.Value, new PROD_ProcessDeliveryDetail()
                {
                    SizeRefId = size.Value,
                    ProcessDeliveryId = model.Delivery.ProcessDeliveryId,
                    ColorRefId = model.CuttingBatch.ColorRefId,
                    ComponentRefId=model.CuttingBatch.ComponentRefId,
                    CuttingTagId = model.CuttingTagId,
                    CuttingBatchId = model.CuttingBatch.CuttingBatchId,
                    CompId = PortalContext.CurrentUser.CompId,    
                });
            }
            return PartialView("~/Areas/Production/Views/PrintingProcess/_CuttingJob.cshtml", model);
        }
              [AjaxAuthorize(Roles = "printingdelivery-2,printingdelivery-3")]
        public ActionResult AddProcessDeliveryDetail(ProcessDeliveryViewModel model)
        {
            List<string> sizeNames = model.CuttingJobDictionary.Where(x => x.Key == "Size").Select(x => x.Value).First();
            model.CuttingJobDictionary.Add(model.CuttingBatch.CuttingBatchRefId, sizeNames);
            model.DoDictionary.Add(model.CuttingBatch.CuttingBatchRefId + "-" + model.CuttingTagId + "k" + model.CuttingBatch.ColorRefId, model.DeliveryDetails);
            return PartialView("~/Areas/Production/Views/PrintingProcess/_ProcessDeliveryDetail.cshtml", model);
        }
        [AjaxAuthorize(Roles = "printingdelivery-3")]
        public ActionResult Delete(long processDeliveryId)
        {
            int deleted = 0;
            deleted = _processDeliveryManager.DeleteProcessDelivery(processDeliveryId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Printing Delivery ");

            
        }
        public ActionResult GetTagBySequence(string componentRefId, string orderStyleRefId)
        {
            var taglist = _cuttingTagManager.GetTagBySequence(componentRefId, orderStyleRefId);
            return Json(taglist, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "printingdelivery-2,printingdelivery-3")]
        public ActionResult GetPartyWiseCuttingProcess(ProcessDeliveryViewModel model)
        {
            const bool isEmbroidery = false;
            const bool isPrintable = true;
           
            model.CuttiongProcesses = _processDeliveryManager.GetPartyWiseCuttingDeliveryProcess( model.Delivery.PartyId, model.Delivery.OrderStyleRefId, model.ColorRefId, model.ComponentRefId,isPrintable, isEmbroidery, ProcessCode.PRINTING);
            return PartialView("~/Areas/Production/Views/PrintingProcess/_PartyWiseCuttingProcess.cshtml", model);
        }
    }
}