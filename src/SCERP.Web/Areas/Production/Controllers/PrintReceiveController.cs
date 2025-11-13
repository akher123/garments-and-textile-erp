using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class PrintReceiveController : BaseController
    {
        private  readonly  IProcessReceiveManager _processReceiveManager;
        private readonly IPartyManager _partyManager;
        
        private readonly ICuttingProcessStyleActiveManager _cuttingProcessStyleActive;
        private readonly IProductionReportManager _productionReportManager;
        private readonly IProcessDeliveryManager _processDeliveryManager;
        public PrintReceiveController(ICuttingProcessStyleActiveManager cuttingProcessStyleActive, IProcessDeliveryManager processDeliveryManager,IProductionReportManager productionReportManager,IPartyManager partyManager,IProcessReceiveManager processReceiveManager)
        {
            _productionReportManager = productionReportManager;
            this._processReceiveManager = processReceiveManager;
            _partyManager = partyManager;
            _processDeliveryManager = processDeliveryManager;
            _cuttingProcessStyleActive = cuttingProcessStyleActive;
        }
        [AjaxAuthorize(Roles = "printreceive-1,printreceive-2,printreceive-3")]
        public ActionResult Index(ProcessReceiveViewModel model)
        {
            ModelState.Clear();
            const string factoryType = "F";
            model.Parties = _partyManager.GetParties(factoryType);
            int totalRecords = 0;
            model.Receives = _processReceiveManager.GetProcessReceiveLsitByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, model.Receive.PartyId,ProcessCode.PRINTING,out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "printreceive-2,printreceive-3")]
        public ActionResult Save(ProcessReceiveViewModel model)
        {
            try
            {
                int saveIndex = 0;
             
                model.Receive.ProcessRefId = ProcessCode.PRINTING;
                model.Receive.CompId = PortalContext.CurrentUser.CompId;
                model.Receive.ReceivedBy = PortalContext.CurrentUser.UserId;
                foreach (var dictionary in model.DoDictionary.Select(x => x.Value))
                {
                    foreach (var detail in dictionary.Select(x => x.Value))
                    {
                        model.Receive.PROD_ProcessReceiveDetail.Add(detail);
                    }
                }
                if (model.Receive.ProcessReceiveId > 0)
                {
                    saveIndex =   _processReceiveManager.EditProcessReceive(model.Receive);
                }
                else
                {
                    const string printReceiveFor = "PR";
                    model.Receive.RefNo = _processReceiveManager.GetProcessReceiveRefNo(printReceiveFor, ProcessCode.PRINTING);
                    saveIndex = _processReceiveManager.SaveProcessReceive(model.Receive);
                }
                if (saveIndex > 0)
                {
                    return Reload();
                }
               
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            
            }
            return ErrorResult("Failed To Save Print Receive!");
            
        }
        [AjaxAuthorize(Roles = "printreceive-2,printreceive-3")]
        public ActionResult Edit(ProcessReceiveViewModel model)
        {
            ModelState.Clear();
            const string factoryType = "F";
            model.Receive.GateEntrydate = DateTime.Now;
            model.Receive.InvoiceDate = DateTime.Now;
            model.Parties = _partyManager.GetParties(factoryType);
            if (model.Receive.ProcessReceiveId>0)
            {
                model.Dictionary = _processReceiveManager.GetReceiveDictionary(model.Receive.ProcessReceiveId);
                model.Receive = _processReceiveManager.GetProcessReceiveById(model.Receive.ProcessReceiveId);
                model.DoDictionary =_processReceiveManager.GetProcessReceiveDetailDictionary(model.Receive.ProcessReceiveId);
                model.SearchString = model.SearchString ?? "C";
                model.ReceiveBalances = _processReceiveManager.GetProcessReceiveBalance(ProcessCode.PRINTING, model.Receive.PartyId, model.SearchString);
            }
            else
            {
                const string printReceiveFor = "PR"; 
                model.Receive.RefNo = _processReceiveManager.GetProcessReceiveRefNo(printReceiveFor, ProcessCode.PRINTING);
            }
         
            return View(model);
        }

        public ActionResult GetPrintingDeliveryJobs(ProcessReceiveViewModel model)
        {
            model.SearchString = model.SearchString ?? "C";
            List<SpPodProcessReceiveBalance> receiveBalanceList = _processReceiveManager.GetProcessReceiveBalance(ProcessCode.PRINTING, model.Receive.PartyId,model.SearchString);
            model.ReceiveBalances = receiveBalanceList;
            return PartialView("~/Areas/Production/Views/PrintReceive/_PrintingProcessDelivery.cshtml", model);
        }
        [AjaxAuthorize(Roles = "printreceive-2,printreceive-3")]
        public ActionResult GetPrintReciveBalance(ProcessReceiveViewModel model)
        {
            Dictionary<string, List<string>> dictionary = _processReceiveManager.GetPrintReciveBalanceDictionary(model.ReceiveDetail.CuttingBatchId, model.ReceiveDetail.CuttingTagId, ProcessCode.PRINTING);
            model.Dictionary = dictionary;
            var sizeList = model.Dictionary.Where(x => x.Key == "SizeRefId").Select(x => x.Value).First().ToList();
            model.ReceiveDictionary = new Dictionary<string, PROD_ProcessReceiveDetail>();
            foreach (var size in sizeList.Select((x, i) => new { Value = x, Index = i }))
            {
                model.ReceiveDictionary.Add(size.Value, new PROD_ProcessReceiveDetail()
                {
                    SizeRefId = size.Value,
                    ProcessReceiveId = model.ReceiveDetail.ProcessReceiveId,
                    ColorRefId = model.ReceiveDetail.ColorRefId,
                    CuttingTagId = model.ReceiveDetail.CuttingTagId,
                    CuttingBatchId = model.ReceiveDetail.CuttingBatchId,
                    CompId = PortalContext.CurrentUser.CompId,
                });
            }
            return PartialView("~/Areas/Production/Views/PrintReceive/_PrintReciveBalance.cshtml", model);
        }
        [AjaxAuthorize(Roles = "printreceive-2,printreceive-3")]
        public ActionResult AddProcessReceiveDetail(ProcessReceiveViewModel model)
        {
            ModelState.Clear();
            List<string> sizeNames = model.Dictionary.Where(x => x.Key == "SIZE").Select(x => x.Value).First();
            model.Dictionary.Add(model.CuttingBatchRefId, sizeNames);
            model.DoDictionary.Add(model.CuttingBatchRefId + "-" + model.ReceiveDetail.CuttingTagId + "C" + model.ReceiveDetail.ColorRefId, model.ReceiveDictionary);
            return PartialView("~/Areas/Production/Views/PrintReceive/_PrintReceiveDetailLsit.cshtml", model);
        }
        public ActionResult PrintBalanceStatust(ProductionReportViewModel model)
        {
            const string factoryType = "F";
            model.Parties = _partyManager.GetParties(factoryType);
            model.ProcessRefId = ProcessCode.PRINTING;
            if (model.IsSearch)
            {
                model.DataTable = _productionReportManager.GetMinimumSendReceive(PortalContext.CurrentUser.CompId, model.PartyId, model.CuttingBatch.OrderStyleRefId, model.ProcessRefId); 
            }
            model.IsSearch = true;
            model.Buyers = _processDeliveryManager.GetBuyerByPartyId(model.PartyId);
            model.OrderList = _cuttingProcessStyleActive.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
            model.Styles = _cuttingProcessStyleActive.GetStyleByOrderNo(model.CuttingBatch.OrderNo);
            return View(model);
        }

        [AjaxAuthorize(Roles = "printreceive-3")]
        public ActionResult Delete(long processReceiveId)
        {
            int deleted = _processReceiveManager.DeleteProcessReceiveById(processReceiveId);
            return deleted>0 ? Reload() : ErrorResult("Delete Failed");
        }
    }
}