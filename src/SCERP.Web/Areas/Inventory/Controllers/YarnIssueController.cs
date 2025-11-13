using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using Spell;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class YarnIssueController : BaseController
    {
       
        private readonly IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IStockRegisterManager _stockRegisterManager;
        public YarnIssueController(IOmBuyerManager buyerManager,IAdvanceMaterialIssueManager advanceMaterialIssueManager, IPartyManager partyManager, IStockRegisterManager stockRegisterManager)
        {
            _advanceMaterialIssueManager = advanceMaterialIssueManager;
            _partyManager = partyManager;
            _stockRegisterManager = stockRegisterManager;
            _buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "yarnissue-1,yarnissue-2,yarnissue-3")]
        public ActionResult Index(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords= 0;
            model.MaterialIssue.IType = (int)ActionType.YarnIssue;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarnissue-2,yarnissue-3")]
        public ActionResult Edit(AdvanceMaterialIssueViewModel model)
        {
            const string partyType = "P";
            ModelState.Clear();
            if (model.MaterialIssue.AdvanceMaterialIssueId>0)
            {
                model.MaterialIssue =_advanceMaterialIssueManager.GetVwAdvanceMaterialIssueById( model.MaterialIssue.AdvanceMaterialIssueId);
                model.MaterialIssueDetails =_advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(model.MaterialIssue.AdvanceMaterialIssueId).ToDictionary(x=>Convert.ToString(x.AdvanceMaterialIssueDetailId),x=>x);
                model.SearchString = model.MaterialIssue.ProgramRefId;
                model.ProgramYarnWithdrows = _advanceMaterialIssueManager.GetProgramYarnWithdrow(model.MaterialIssue.ProgramRefId);
            }
            else
            {
                model.MaterialIssue.IRefId = _advanceMaterialIssueManager.GetNewRefId((int) StoreType.Yarn);
               
            }
            model.MaterialIssue.IRNoteDate = DateTime.Now;
          //  model.Parties = _partyManager.GetParties(partyType);
           // model.OmBuyers = _buyerManager.GetAllBuyers();
            model.StoreList = Enum.GetValues(typeof(StoreType)).Cast<StoreType>().Select(x => new { StoreId = Convert.ToInt16(x), Name = x });
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarnissue-2,yarnissue-3")]
        public ActionResult Save(AdvanceMaterialIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                Inventory_AdvanceMaterialIssue materialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssueById(model.MaterialIssue.AdvanceMaterialIssueId);
                materialIssue.AdvanceMaterialIssueId = model.MaterialIssue.AdvanceMaterialIssueId;
                materialIssue.OrderNo = model.MaterialIssue.OrderNo;
                materialIssue.StyleNo = model.MaterialIssue.StyleNo;
                materialIssue.RefPerson = model.MaterialIssue.RefPerson;
                materialIssue.SlipNo = model.MaterialIssue.IRefId;
                materialIssue.Remarks = model.MaterialIssue.Remarks;
                materialIssue.IRNoteNo = model.MaterialIssue.IRNoteNo;
                materialIssue.IRNoteDate = model.MaterialIssue.IRNoteDate;
                materialIssue.IRefId = model.MaterialIssue.IRefId;
                materialIssue.VehicleNo = model.MaterialIssue.VehicleNo;
                materialIssue.DriverName = model.MaterialIssue.DriverName;
                materialIssue.StoreId =(int) StoreType.Yarn;
                materialIssue.PartyId = model.MaterialIssue.PartyId;
                materialIssue.IType = (int)ActionType.YarnIssue; 
                materialIssue.ProcessRefId = ProcessType.KNITTING;
                materialIssue.IssuedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                materialIssue.CompId = PortalContext.CurrentUser.CompId;
                materialIssue.BuyerRefId = model.MaterialIssue.BuyerRefId;
                materialIssue.ProgramRefId = model.MaterialIssue.ProgramRefId;
                materialIssue.Inventory_AdvanceMaterialIssueDetail = model.MaterialIssueDetails.Select(x => x.Value).Select(x => new Inventory_AdvanceMaterialIssueDetail()
                {
                    AdvanceMaterialIssueDetailId = x.AdvanceMaterialIssueDetailId,
                    AdvanceMaterialIssueId = x.AdvanceMaterialIssueId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    ItemId = x.ItemId,
                    IssueQty = x.IssueQty,
                    IssueRate = x.IssueRate,
                    FColorRefId = x.FColorRefId,
                    GColorRefId = x.GColorRefId,
                    QtyInBag = x.QtyInBag,
                    CompId = PortalContext.CurrentUser.CompId
                }).ToList();
                materialIssue.Amount = materialIssue.Inventory_AdvanceMaterialIssueDetail.Sum(x => x.IssueQty * x.IssueRate);
                materialIssue.SlipNo = string.Join(",", model.MaterialIssueDetails.Select(y => y.Value.ColorName).ToList());

                if (materialIssue.ProgramRefId.Length != 10 && materialIssue.ProcessRefId == ProcessType.KNITTING)
                {
                    return ErrorResult("Invalid Knitting Program No");
                }
                if (materialIssue.Inventory_AdvanceMaterialIssueDetail.Any())
                {
                    saveIndex = _advanceMaterialIssueManager.SaveAdvanceMaterialIssue(materialIssue);
                }
                else
                {
                    return ErrorResult("Failed to Save Yarn Issue!Please Isseue One Item");
                }
               
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Save Failed!");
        }
        [AjaxAuthorize(Roles = "yarnissue-3")]
        public ActionResult Delete(long advanceMaterialIssueId)
        {
            int deleted = 0;
            try
            {
                const int iType = (int) ActionType.YarnIssue;
                deleted = _advanceMaterialIssueManager.DeleteAdvanceMaterialIssue(advanceMaterialIssueId, iType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
       
            return deleted > 0 ? Reload() : ErrorResult("Delete Failed!");
        }

        public ActionResult AddNewRow(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            if (model.SinH - model.MaterialIssueDetail.IssueQty >= 0)
            {

                bool isCountValied = _stockRegisterManager.IsYarnCountValid(model.MaterialIssueDetail.ItemId, model.MaterialIssueDetail.FColorRefId, model.MaterialIssueDetail.ColorRefId, model.MaterialIssueDetail.SizeRefId);
                if (isCountValied)
                {
                    model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    model.MaterialIssueDetails.Add(model.Key, model.MaterialIssueDetail);

                    return PartialView("~/Areas/Inventory/Views/YarnIssue/_AddNewRow.cshtml", model);
                }
                else
                {
                    return ErrorResult(model.MaterialIssueDetail.SizeName + " yarn count is invalied for lot :" + model.MaterialIssueDetail.ColorName + "and Color :" + model.MaterialIssueDetail.FColorName);
                }


           
            }
            else
            {
                return ErrorResult("Not Enough Quantity");
            }
        }

        public ActionResult YarnIssueChallanReport(long advanceMaterialIssueId, string key)
        {
        
            List<VwAdvanceMaterialIssueDetail> advanceMaterialIssueDetails =_advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(advanceMaterialIssueId);
            decimal totalQty = advanceMaterialIssueDetails.Sum(x => x.IssueQty);
            string inWord = SpellAmount.InWrods(totalQty).NumberToWord();
            const string reportName = "YarsnIssueChallan";
            string reportTitle = "";
        
            switch (key)
            {
                case "INV":
                    reportTitle = "YARN ISSUE REPORT";
                    break;
            
               
            }
            var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord), new ReportParameter("ReportTitle", reportTitle) };
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), reportName + ".rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DeliveryChallanDSet", advanceMaterialIssueDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = 1, MarginLeft = 0.3, MarginRight = 0.3, MarginBottom = 1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation,reportParameters);
        }

        public ActionResult YarnIssueSummaryReport(AdvanceMaterialIssueViewModel model)
        {
            const int iType = (int)ActionType.YarnIssue;
            List<VwAdvanceMaterialIssue> advanceMaterialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssues(model.FromDate, model.ToDate, model.SearchString, iType);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;

            ReportParameter reportTitle = new ReportParameter("ReportTitle", "YARN ISSUE SUMMARY REPORT");
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
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "IssueSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter, reportTitle };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("IssueSummaryDSet", advanceMaterialIssue) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }
	}
}