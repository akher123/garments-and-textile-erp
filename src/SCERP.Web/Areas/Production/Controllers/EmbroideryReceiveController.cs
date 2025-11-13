using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class EmbroideryReceiveController : BaseController
    {
        private readonly IProcessReceiveManager _processReceiveManager;
        private readonly IPartyManager _partyManager;
        private readonly OmBuyerManager _buyerManager;
        public EmbroideryReceiveController(OmBuyerManager buyerManager,IPartyManager partyManager, IProcessReceiveManager processReceiveManager)
        {
            this._processReceiveManager = processReceiveManager;
            _partyManager = partyManager;
            _buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "embroideryreceive-1,embroideryreceive-2,embroideryreceive-3")]
        public ActionResult Index(ProcessReceiveViewModel model)
        {
            ModelState.Clear();
            const string factoryType = "F";
            int totalRecords = 0;
            model.Receives = _processReceiveManager.GetProcessReceiveLsitByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, model.Receive.PartyId, ProcessCode.EMBROIDARY, out totalRecords);
            model.TotalRecords = totalRecords;
            model.Parties = _partyManager.GetParties(factoryType);
            return View(model);
        }
        [AjaxAuthorize(Roles = "embroideryreceive-2,embroideryreceive-3")]
        public ActionResult Save(ProcessReceiveViewModel model)
        {
            try
            {
                int saveIndex = 0;
             
                model.Receive.ProcessRefId = ProcessCode.EMBROIDARY;
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
                    saveIndex = _processReceiveManager.EditProcessReceive(model.Receive);
                }
                else
                {
                    const string printReceiveFor = "ER";
                    model.Receive.RefNo = _processReceiveManager.GetProcessReceiveRefNo(printReceiveFor, ProcessCode.EMBROIDARY);
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
        [AjaxAuthorize(Roles = "embroideryreceive-2,embroideryreceive-3")]
        public ActionResult Edit(ProcessReceiveViewModel model)
        {
            const string factoryType = "F";
            model.Receive.GateEntrydate = DateTime.Now;
            model.Receive.InvoiceDate = DateTime.Now;
            model.Parties = _partyManager.GetParties(factoryType);
            if (model.Receive.ProcessReceiveId > 0)
            {
                model.Dictionary = _processReceiveManager.GetReceiveDictionary(model.Receive.ProcessReceiveId);
                model.Receive = _processReceiveManager.GetProcessReceiveById(model.Receive.ProcessReceiveId);
                model.DoDictionary = _processReceiveManager.GetProcessReceiveDetailDictionary(model.Receive.ProcessReceiveId);
                model.ReceiveBalances = _processReceiveManager.GetProcessReceiveBalance(ProcessCode.PRINTING, model.Receive.PartyId, model.SearchString);
            }
            else
            {
                const string printReceiveFor = "ER";
                model.Receive.RefNo = _processReceiveManager.GetProcessReceiveRefNo(printReceiveFor, ProcessCode.EMBROIDARY);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "embroideryreceive-2,embroideryreceive-3")]
        public ActionResult GetPrintingDeliveryJobs(ProcessReceiveViewModel model)
        {

            List<SpPodProcessReceiveBalance> receiveBalanceList = _processReceiveManager.GetProcessReceiveBalance(ProcessCode.EMBROIDARY, model.Receive.PartyId, model.SearchString);
            model.ReceiveBalances = receiveBalanceList;
            return PartialView("~/Areas/Production/Views/EmbroideryReceive/_EmbroieryRecivProcessDelivery.cshtml", model);
        }
        [AjaxAuthorize(Roles = "embroideryreceive-2,embroideryreceive-3")]
        public ActionResult GetEmbroieryReciveBalance(ProcessReceiveViewModel model)
        {
            Dictionary<string, List<string>> dictionary = _processReceiveManager.GetPrintReciveBalanceDictionary(model.ReceiveDetail.CuttingBatchId, model.ReceiveDetail.CuttingTagId, ProcessCode.EMBROIDARY);
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
            return PartialView("~/Areas/Production/Views/EmbroideryReceive/_EmbroieryReciveBalance.cshtml", model);
        }
        [AjaxAuthorize(Roles = "embroideryreceive-2,embroideryreceive-3")]
        public ActionResult AddProcessReceiveDetail(ProcessReceiveViewModel model)
        {
            ModelState.Clear();
            List<string> sizeNames = model.Dictionary.Where(x => x.Key == "SIZE").Select(x => x.Value).First();
            model.Dictionary.Add(model.CuttingBatchRefId, sizeNames);
            model.DoDictionary.Add(model.CuttingBatchRefId + "-" + model.ReceiveDetail.CuttingTagId + "C" + model.ReceiveDetail.ColorRefId, model.ReceiveDictionary);
            return PartialView("~/Areas/Production/Views/EmbroideryReceive/_EmbroieryReciveDetailLsit.cshtml", model);
        }

        public ActionResult EmbroideryBalanceStatus(ProductionReportViewModel model)
        {
            model.ProcessRefId = ProcessCode.EMBROIDARY;
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }
        [AjaxAuthorize(Roles = "embroideryreceive-3")]
        public ActionResult Delete(long processReceiveId)
        {
            var deleteIndex = _processReceiveManager.DeleteProcessReceiveById(processReceiveId);
            return deleteIndex > 0 ? Reload() : ErrorResult("Failed To Delete Factory");
        }
    }
}