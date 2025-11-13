using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class AccessoriesIssueController : BaseController
    {
        private readonly IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
        private readonly IPartyManager _partyManager;
        private IOmBuyerManager _buyerManager;
        private IOmBuyOrdStyleManager _buyOrdStyle;
        public AccessoriesIssueController(IOmBuyOrdStyleManager buyOrdStyle, IOmBuyerManager buyerManager, IAdvanceMaterialIssueManager advanceMaterialIssueManager, IPartyManager partyManager)
        {
            _buyOrdStyle = buyOrdStyle;
            _buyerManager = buyerManager;
            _advanceMaterialIssueManager = advanceMaterialIssueManager;
            _partyManager = partyManager;
        }
        [AjaxAuthorize(Roles = "accessoriesissue-1,accessoriesissue-2,accessoriesissue-3")]
        public ActionResult Index(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords= 0;
            model.MaterialIssue.IType =(int) ActionType.AccessoriesIssue;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
       [AjaxAuthorize(Roles = "accessoriesissue-2,accessoriesissue-3")]
        public ActionResult Edit(AdvanceMaterialIssueViewModel model)
        {
            const string partyType = "P";
            ModelState.Clear();
            if (model.MaterialIssue.AdvanceMaterialIssueId>0)
            {
           
                model.MaterialIssue =_advanceMaterialIssueManager.GetVwAdvanceMaterialIssueById( model.MaterialIssue.AdvanceMaterialIssueId);

                model.MaterialIssueDetails =
                    _advanceMaterialIssueManager.GetAccessorisEditRcvDetails(model.MaterialIssue
                        .AdvanceMaterialIssueId);
                var orderStyle = _buyOrdStyle.GetBuyOrdStyleByRefId(model.MaterialIssue.OrderStyleRefId);
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.MaterialIssue.BuyerRefId);
                model.StyleList = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(orderStyle.OrderNo);
            }
            else
            {
                model.MaterialIssue.PartyId = 1; 
                model.MaterialIssue.IRefId = _advanceMaterialIssueManager.GetAccNewRefId(); 
            }
       
            model.OmBuyers=_buyerManager.GetAllBuyers();  
            model.MaterialIssue.IRNoteDate = DateTime.Now;
            model.Parties = _partyManager.GetParties(partyType);
            return View(model);
        }

        [HttpPost]
        [AjaxAuthorize(Roles = "accessoriesissue-2,accessoriesissue-3")]
        public ActionResult Save(AdvanceMaterialIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                Inventory_AdvanceMaterialIssue materialIssue =
                    _advanceMaterialIssueManager.GetAdvanceMaterialIssueById(model.MaterialIssue
                        .AdvanceMaterialIssueId);
                materialIssue.AdvanceMaterialIssueId = model.MaterialIssue.AdvanceMaterialIssueId;
                materialIssue.OrderStyleRefId = model.MaterialIssue.OrderStyleRefId;
                materialIssue.BuyerRefId = model.MaterialIssue.BuyerRefId;
                var orderStyle = _buyOrdStyle.GetVBuyOrdStyleByRefId(model.MaterialIssue.OrderStyleRefId);
                materialIssue.OrderNo = orderStyle.RefNo;
                materialIssue.StyleNo = orderStyle.StyleName;
                materialIssue.RefPerson = model.MaterialIssue.RefPerson;
                materialIssue.SlipNo = model.MaterialIssue.IRNoteNo;
                materialIssue.Remarks = model.MaterialIssue.Remarks;
                materialIssue.IRNoteNo = model.MaterialIssue.IRNoteNo;
                materialIssue.IRNoteDate = model.MaterialIssue.IRNoteDate;
                materialIssue.IRefId = model.MaterialIssue.IRefId;
                materialIssue.StoreId = (int) StoreType.Acessories;
                materialIssue.PartyId = model.MaterialIssue.PartyId;
                materialIssue.IType = (int) ActionType.AccessoriesIssue;
                ; //AccessoriesIssue
                materialIssue.ProcessRefId = ProcessType.NA;
                materialIssue.IssuedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                materialIssue.CompId = PortalContext.CurrentUser.CompId;
                materialIssue.Inventory_AdvanceMaterialIssueDetail = model.MaterialIssueDetails.Select(x => x.Value).Where(x=>x.IssueQty>0)
                    .Select(x => new Inventory_AdvanceMaterialIssueDetail()
                    {
                        AdvanceMaterialIssueDetailId = x.AdvanceMaterialIssueDetailId,
                        AdvanceMaterialIssueId = x.AdvanceMaterialIssueId,
                        PurchaseOrderDetailId = x.PurchaseOrderDetailId,
                        ColorRefId = x.ColorRefId,
                        SizeRefId = x.SizeRefId,
                        GSizeRefId = x.GSizeRefId,
                        FColorRefId = x.FColorRefId,
                        ItemId = x.ItemId,
                        IssueQty = x.IssueQty,
                        IssueRate = x.IssueRate,
                        QtyInBag = x.QtyInBag,
                        CompId = PortalContext.CurrentUser.CompId
                    }).ToList();
                materialIssue.Amount =
                    materialIssue.Inventory_AdvanceMaterialIssueDetail.Sum(x => x.IssueQty * x.IssueRate);
                if (materialIssue.AdvanceMaterialIssueId == 0)
                {
                    model.MaterialIssue.IRefId = _advanceMaterialIssueManager.GetAccNewRefId();
                }
                saveIndex = _advanceMaterialIssueManager.SaveAdvanceMaterialIssue(materialIssue);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
               return ErrorResult("Save Failed!");
            }

        
    }

        [AjaxAuthorize(Roles = "accessoriesissue-3")]
        public ActionResult Delete(long advanceMaterialIssueId)
        {
            int deleted = 0;
            try
            {
                const int iType = (int) ActionType.AccessoriesIssue;
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
            model.MaterialIssueDetails =
                _advanceMaterialIssueManager.GetAccessoriesRcvSummary(model.MaterialIssue.OrderStyleRefId,
                    PortalContext.CurrentUser.CompId);
            return PartialView("~/Areas/Inventory/Views/AccessoriesIssue/_AddNewRow.cshtml", model);
        }


        public ActionResult AccessoriesIssueChallan(long advanceMaterialIssueId)
        {
            ModelState.Clear();

            DataTable accessoriesIssueDataTable = _advanceMaterialIssueManager.GetAccessoriesIssueChallanDataTable(advanceMaterialIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AccessoriesIssueChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AccessorisIssueDset", accessoriesIssueDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight =11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }
        public ActionResult GetAccessoriesIssueDetailStatus(AdvanceMaterialIssueViewModel model)
        {

            DataTable issueDetail = _advanceMaterialIssueManager.GetAccessoriesIssueDetailStatus(model.FromDate, model.ToDate, PortalContext.CurrentUser.CompId);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;

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
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AccessoriesIssueDetailReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AccIssueDSet", issueDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile((ReportType)Convert.ToInt16(model.ReportType), path, reportDataSources, deviceInformation, reportParameters);
        }
	}
}