using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using Spell;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class YarnDeliveryController : BaseController
    {
        private readonly IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
        private readonly IPartyManager _partyManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IProgramManager _programManager;
        private readonly IStockRegisterManager _stockRegisterManager;
       
        public YarnDeliveryController(IProgramManager programManager,IStockRegisterManager stockRegisterManager, IOmBuyerManager buyerManager, IAdvanceMaterialIssueManager advanceMaterialIssueManager, IPartyManager partyManager)
        {
            _programManager = programManager;
            _buyerManager = buyerManager;
            _advanceMaterialIssueManager = advanceMaterialIssueManager;
            _partyManager = partyManager;
            _stockRegisterManager = stockRegisterManager;
         
        }
        [AjaxAuthorize(Roles = "yarndelivery-1,yarndelivery-2,yarndelivery-3")]
        public ActionResult Index(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords= 0;
            model.MaterialIssue.IType =(int) ActionType.YarnDelivery;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarndelivery-2,yarndelivery-3")]
        public ActionResult Edit(AdvanceMaterialIssueViewModel model)
        {
            const string partyType = "P";
            ModelState.Clear();
            if (model.MaterialIssue.AdvanceMaterialIssueId>0)
            {
                model.MaterialIssue =_advanceMaterialIssueManager.GetVwAdvanceMaterialIssueById( model.MaterialIssue.AdvanceMaterialIssueId);
                if (model.MaterialIssue.LockStatus.GetValueOrDefault() && model.MaterialIssue.AdvanceMaterialIssueId > 0)
                {
                    return ErrorResult("Edit is not Allowed");
                }
                model.MaterialIssueDetails =_advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(model.MaterialIssue.AdvanceMaterialIssueId).ToDictionary(x=>Convert.ToString(x.AdvanceMaterialIssueDetailId),x=>x);
                model.SearchString = model.MaterialIssue.ProgramRefId;
                model.ProgramYarnWithdrows= _advanceMaterialIssueManager.GetProgramYarnWithdrow(model.MaterialIssue.ProgramRefId);
            }
            else
            {
                int storeId = (int)StoreType.Yarn;
                model.MaterialIssue.IRefId = _advanceMaterialIssueManager.GetNewRefId(storeId);
                model.MaterialIssue.IRNoteNo ="DRY/"+model.MaterialIssue.IRefId;
                model.MaterialIssue.IRNoteDate = DateTime.Now;
            }
         
           
            model.Parties = _partyManager.GetParties(partyType);
            model.OmBuyers = _buyerManager.GetAllBuyers();
            model.StoreList = Enum.GetValues(typeof(StoreType)).Cast<StoreType>().Select(x => new { StoreId = Convert.ToInt16(x), Name = x });
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarndelivery-2,yarndelivery-3")]
        public ActionResult Save(AdvanceMaterialIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                Inventory_AdvanceMaterialIssue materialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssueById(model.MaterialIssue.AdvanceMaterialIssueId);
                if (materialIssue.LockStatus.GetValueOrDefault()&&materialIssue.AdvanceMaterialIssueId>0)
                {
                    return ErrorResult("Update is not Allowed");
                }
                else
                {

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
                materialIssue.OrderStyleRefId = model.MaterialIssue.OrderStyleRefId;
                materialIssue.StoreId =(int) StoreType.Yarn;
                materialIssue.PartyId = model.MaterialIssue.PartyId;
                materialIssue.IType = (int)ActionType.YarnDelivery;
                materialIssue.ProcessRefId = model.MaterialIssue.ProcessRefId;
                materialIssue.IssuedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                materialIssue.CompId = PortalContext.CurrentUser.CompId;
                materialIssue.BuyerRefId = model.MaterialIssue.BuyerRefId;
                materialIssue.LockStatus = false;
                materialIssue.ProgramRefId = model.MaterialIssue.ProgramRefId ?? "DP";
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
                    Wrapper = x.Wrapper,
                    CompId = PortalContext.CurrentUser.CompId
                }).ToList();
                materialIssue.SlipNo = string.Join(",",
                model.MaterialIssueDetails.Select(y => y.Value.ColorName).ToList());
                materialIssue.Amount = materialIssue.Inventory_AdvanceMaterialIssueDetail.Sum(x => x.IssueQty * x.IssueRate);

                }
              
                    if (materialIssue.ProgramRefId!=null&& materialIssue.ProgramRefId.Length != 10 && materialIssue.ProcessRefId == ProcessType.KNITTING)
                    {
                        return ErrorResult("Invalid Knitting Program No");
                    }
                    else if (materialIssue.ProgramRefId != null && materialIssue.ProgramRefId.Length != 10 && materialIssue.ProcessRefId == ProcessType.YARNDYEING)
                    {
                        return ErrorResult("Invalid Dyed Program No");
                    }
                    else if  (materialIssue.Inventory_AdvanceMaterialIssueDetail.Any())
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
      [AjaxAuthorize(Roles = "yarndelivery-3")]
        public ActionResult Delete(long advanceMaterialIssueId)
        {
            int deleted = 0;
            try
            {
                const int iType = (int) ActionType.YarnDelivery;
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
            if (model.SinH-model.MaterialIssueDetail.IssueQty>=0)
            {

                bool isCountValied = _stockRegisterManager.IsYarnCountValid(model.MaterialIssueDetail.ItemId, model.MaterialIssueDetail.FColorRefId, model.MaterialIssueDetail.ColorRefId, model.MaterialIssueDetail.SizeRefId);
                if (isCountValied)
                {
                    model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    model.MaterialIssueDetails.Add(model.Key, model.MaterialIssueDetail);
                    return PartialView("~/Areas/Inventory/Views/YarnDelivery/_AddNewRow.cshtml", model); 
                }
                else
                {
                    return ErrorResult(model.MaterialIssueDetail.SizeName + " yarn count is invalied for lot :" + model.MaterialIssueDetail.ColorName + "and Color :" + model.MaterialIssueDetail.FColorName);
                }

               
            }
            else
            {
                return ErrorResult("Issued Quantity is greater than SInH ");
            }
     
        }
        
        public ActionResult YarnDeliveryChallanReport(long advanceMaterialIssueId, string key)
        {

           var yarnIssue=  _advanceMaterialIssueManager.GetAdvanceMaterialIssueById(advanceMaterialIssueId);
            string processName = "";
            processName = yarnIssue.ProcessRefId == ProcessType.YARNDYEING ? "YARNDYEING" : "KNITTING";
            List<VwAdvanceMaterialIssueDetail> advanceMaterialIssueDetails =
                _advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(advanceMaterialIssueId);
            decimal totalQty = advanceMaterialIssueDetails.Sum(x => x.IssueQty);
            string inWord = SpellAmount.InWrods(totalQty).NumberToWord();
            const string reportName = "DeliveryChallan";
            string reportTitle = "";
        
            //switch (key)
            //{
            //    case "INV":

            //        reportTitle = "YARN DELIVERY CHALLAN & GATE PASS FOR " + processName; 
                    
                  
            //        break;
            //    default:
            //        reportTitle = "YARN GATE PASS FOR "+processName;
            //        break;
            //}
            reportTitle = "YARN DELIVERY CHALLAN & GATE PASS FOR " + processName; 
            var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord), new ReportParameter("ReportTitle", reportTitle) };
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), reportName + ".rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DeliveryChallanDSet", advanceMaterialIssueDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =11.69, PageHeight = 8.27, MarginTop = 1, MarginLeft = 0.2, MarginRight = 0.1, MarginBottom =.3 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation,reportParameters);
        }

        public ActionResult YarnDeliverySummaryReport(AdvanceMaterialIssueViewModel model)
        {
            const int iType = (int)ActionType.YarnDelivery;
            var advanceMaterialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssues(model.FromDate, model.ToDate, model.SearchString, iType);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
            var reportTitle = new ReportParameter("ReportTitle", "YARN DELIVERY SUMMARY REPORT");
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
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }

        public JsonResult YarnStockStatus(int itemId,string colorRefId)
        {
            var stockStatus = _stockRegisterManager.GetYarnStockStatus(itemId, colorRefId);
            return Json(stockStatus,JsonRequestBehavior.AllowGet);
        }

        public JsonResult YarnStockStatusByLot(string colorRefId)
        {
            var stockStatus = _stockRegisterManager.GetYarnStockStatusByLot( colorRefId);
            return Json(stockStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LockYarnDelivery(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.MaterialIssue.IType = (int)ActionType.YarnDelivery;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult DeliveryLock(long advanceMaterialIssueId)
        {
            int locked=  _advanceMaterialIssueManager.LoackYarnIssue(advanceMaterialIssueId);
            return ErrorResult(locked>0 ? "Yarn Delivery Locked Successfully !" : "Yarn Delivery Locked Failed !");
        }

        public JsonResult GetProgramAutocomplite(string searchString,long partyId,string processRefId)
        {
            var programList = _programManager.GetProgramAutocomplite(searchString, partyId, PortalContext.CurrentUser.CompId, processRefId);
            return Json(programList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YarnDeliveryList(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.FromDate = model.FromDate;
            model.ToDate = model.ToDate;
            model.MaterialIssue.IType = (int)ActionType.YarnDelivery;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult ProgramYarnWithdrow(string programRefId)
        {
            List<ProgramYarnWithdrow> programYarnWithdrows = _advanceMaterialIssueManager.GetProgramYarnWithdrow(programRefId);
            return PartialView("~/Areas/Inventory/Views/YarnDelivery/_ProgramYarnWithdrow.cshtml", programYarnWithdrows);
        }

    }
}