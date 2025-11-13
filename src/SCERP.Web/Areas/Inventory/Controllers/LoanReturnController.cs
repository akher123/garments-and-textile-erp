using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.Manager.CommonManager;
using SCERP.BLL.Manager.InventoryManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class LoanReturnController : BaseController
    {
        private MaterialIssueManager _materialIssueManager;
        private SizeManager _sizeManager;
        private BrandManager _brandManager;
        private ItemStoreManager _itemStoreManager;
        private EmailSendManager _emailSendManager;
        public LoanReturnController(MaterialIssueManager materialIssueManager, SizeManager sizeManager, BrandManager brandManager, ItemStoreManager itemStoreManager, EmailSendManager emailSendManager)
        {
            this._materialIssueManager = materialIssueManager;
            _sizeManager = sizeManager;
            _brandManager = brandManager;
            _itemStoreManager = itemStoreManager;
            _emailSendManager = emailSendManager;
        }

        [AjaxAuthorize(Roles = "loanreturn-1,loanreturn-2,loanreturn-3")]
        public ActionResult Index(LoanReturnViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.IType =(int) MaterialIssueType.LoanReturn;
            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            else
            {
                model.VMaterialLoanReturns = _materialIssueManager.GetMaterialLoanReturns(model, out totalRecords);
            }
         
            model.TotalRecords=totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "loanreturn-2,loanreturn-3")]
        public ActionResult Edit(LoanReturnViewModel model)
        {
            
            ModelState.Clear();
            model.IssueReceiveDate = DateTime.Now;
            model.InventorySizes = _sizeManager.GetSizeList();
            model.InventoryBrands = _brandManager.GetBrands();
            model.Countries = CountryManager.GetAllCountries();
       
            if (model.MaterialIssueId > 0)
            {
                VMaterialLoanReturn materialIssue = _materialIssueManager.GetVMaterialLoanReturnById(model.MaterialIssueId);
                model.MaterialIssueRequisitionId = materialIssue.MaterialIssueRequisitionId;
                model.MaterialIssueId = materialIssue.MaterialIssueId;
                model.IssueReceiveNo = materialIssue.IssueReceiveNo;
                model.IssueReceiveDate = materialIssue.IssueReceiveDate;
                model.IssueReceiveNoteNo = materialIssue.LoanRefNo;
                model.Remarks = materialIssue.Remarks;
                model.SupplierId = materialIssue.SupplierId;
                model.MaterialIssueDetails = _materialIssueManager.GetMaterialIssueDetails(model.MaterialIssueId).ToDictionary(x => Convert.ToString(x.MaterialIssueDetailId), x => x);
            }
            else
            {
                model.IssueReceiveNo = _materialIssueManager.GetNewIssueReceiveNo();
            }
            model.Suppliers = SupplierCompanyManager.GetAllSupplierCompany();
            return View(model);
        }
         [AjaxAuthorize(Roles = "loanreturn-2,loanreturn-3")]
        public ActionResult Save(LoanReturnViewModel model)
        {
            int saveIndex = 0;
            var materialIssue = new Inventory_MaterialIssue
            {
                Inventory_MaterialIssueRequisition = new Inventory_MaterialIssueRequisition()
                {

                    BranchUnitDepartmentId = 11,
                    DepartmentSectionId = 17,
                    PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    SubmittedTo = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    IsModifiedByStore = true,
                    IsSentToStore = true,
                    SendingDate = DateTime.Now,
                    IssueReceiveNoteNo = model.IssueReceiveNoteNo,
                    IssueReceiveNoteDate =DateTime.Now,
                    CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                },
                MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                MaterialIssueId = model.MaterialIssueId,
                IssueReceiveDate = model.IssueReceiveDate,
                IssueReceiveNo = model.IssueReceiveNo,
                IType = (int) MaterialIssueType.LoanReturn, 
                BtRefNo = model.IssueReceiveNoteNo,// RInvocie
                SupplierId = model.SupplierId,
                Note = model.Remarks,
                PreparedByStore = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            foreach (var materialIssueDetail in model.MaterialIssueDetails.Values)
            {
                materialIssue.Inventory_MaterialIssueDetail.Add(new Inventory_MaterialIssueDetail()
                {
                    MaterialIssueDetailId = materialIssueDetail.MaterialIssueDetailId,
                    MaterialIssueId = model.MaterialIssueId,
                    ItemId = materialIssueDetail.ItemId,
                    StockInHand = materialIssueDetail.StockInHand.GetValueOrDefault(),
                    RequiredQuantity = materialIssueDetail.RequiredQuantity.GetValueOrDefault(),
                    MachineId = materialIssueDetail.MachineId,
                    CurrencyId = 1,
                    Remarks = materialIssueDetail.Remarks,
                    IssuedQuantity = materialIssueDetail.IssuedQuantity.GetValueOrDefault(),
                    IssuedItemRate = materialIssueDetail.IssuedItemRate.GetValueOrDefault(),
                    TransactionDate = model.IssueReceiveDate,
                    BrandId =materialIssueDetail.BrandId,
                    SizeId = materialIssueDetail.SizeId,
                    OriginId = materialIssueDetail.OriginId,
                    IsActive = true,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                });
            }
             try
             {
                 if (model.MaterialIssueDetails.Any())
                 {
                     saveIndex = model.MaterialIssueId > 0 ? _materialIssueManager.EditMaterialIssue(materialIssue) : _materialIssueManager.SaveMaterialIssue(materialIssue);
                 }
                 else
                 {
                     return ErrorResult("Please add issue items  !!");
                 }
             }
             catch (Exception exception)
             {
                 Errorlog.WriteLog(exception);
                 return ErrorResult(String.Format(exception.Message));
             }

            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save data"));
        }
        public ActionResult AutocompliteReceiveLoanInfo(LoanReturnViewModel model)
        {
            var recivedLoanItem = _itemStoreManager.AutocompliteReceiveLoanInfo(model.SearchString);
           return Json(recivedLoanItem,JsonRequestBehavior.AllowGet);
        }


        [AjaxAuthorize(Roles ="loanreturn-3")]
        public ActionResult Delete(BachWiseIssueViewModel model)
        {
            var deleteIndex = 0;
            const int loanIssue = (int)MaterialIssueType.LoanReturn;
            try
            {
                deleteIndex = _materialIssueManager.DeleteBatchWiseIssue(model.MaterialIssueId, loanIssue);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
        public ActionResult LoanReturnReport(int materialIssueId)
        {
            DataTable loandDataTable = _materialIssueManager.GetLoanReturnChallan(materialIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "LoanReturnReport.rdlc");
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LRTDSET", loandDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
           return  ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);


        }

	}
}