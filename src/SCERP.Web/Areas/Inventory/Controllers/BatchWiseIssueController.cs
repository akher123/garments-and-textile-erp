using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{


    public class BatchWiseIssueController : BaseController
    {
        private readonly IBatchManager _batchManager;
        private readonly IMaterialIssueManager _materialIssueManager;
        private readonly IEmailSendManager _emailSendManager;
        public BatchWiseIssueController(IBatchManager batchManager, IMaterialIssueManager materialIssueManager, IEmailSendManager emailSendManager)
        {
            _batchManager = batchManager;
            _materialIssueManager = materialIssueManager;
            _emailSendManager = emailSendManager;
        }
        [AjaxAuthorize(Roles = "batchwiseissue-1,batchwiseissue-2,batchwiseissue-3")]
        public ActionResult Index(BachWiseIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.IType = (int)MaterialIssueType.BatchWiseIssue;

            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            else
            {
                model.VMaterialIssues = _materialIssueManager.GetMaterialIssues(model, out totalRecords);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "batchwiseissue-2,batchwiseissue-3")]
        public ActionResult Edit(BachWiseIssueViewModel model)
        {
            ModelState.Clear();
            model.IssueReceiveDate = DateTime.Now;
            model.IssueReceiveNoteDate = DateTime.Now;
           // model.Batches = MaterialIssueManager.GetAllBatch();
            model.ToppingList=Enum.GetValues(typeof(BatchToping)).Cast<BatchToping>().Select(x=>new {ToppingType=Convert.ToInt32(x), Value=x.ToString() });
            if (model.MaterialIssueId>0)
            {
                VMaterialIssue materialIssue = _materialIssueManager.GetVMaterialIssueById(model.MaterialIssueId);
                model.MaterialIssueRequisitionId = materialIssue.MaterialIssueRequisitionId;
                model.MaterialIssueId = materialIssue.MaterialIssueId;
                model.IssueReceiveNo = materialIssue.IssueReceiveNo;
                model.IssueReceiveDate = materialIssue.IssueReceiveDate;
                model.IssueReceiveNoteNo = materialIssue.IssueReceiveNoteNo;
                model.IssueReceiveNoteDate = materialIssue.IssueReceiveDate;
                model.BtRefNo = materialIssue.BtRefNo;
                model.BatchNo = materialIssue.BatchNo;
                model.Quantity = materialIssue.Quantity;
                model.Note = materialIssue.Note;
                model.ToppingType = materialIssue.ToppingType;
                model.MaterialIssueDetails = _materialIssueManager.GetMaterialIssueDetails(model.MaterialIssueId).ToDictionary(x => Convert.ToString(x.MaterialIssueDetailId), x => x);
            }
            else
            {
                model.IssueReceiveNo = _materialIssueManager.GetNewIssueReceiveNo();
            }
         
            return View(model);
        }
        public ActionResult AddNewRow(BachWiseIssueViewModel model)
        {
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.MaterialIssueDetails.Add(model.Key,model.IssueDetail);
            if (model.IssueDetail != null && model.IssueDetail.ItemId==-1)
            {
                return ErrorResult("Invalid Item Please select correct one");
            }
            if (model.IssueDetail != null && model.IssueDetail.RequiredQuantity==0)
            {
                return ErrorResult("RQty is Required !!! ");
            }
            if (model.IssueDetail != null && model.IssueDetail.RequiredQuantity <0)
            {
                return ErrorResult("RQty is not allow negative figure !!! ");
            }
            if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity == 0)
            {
                return ErrorResult("IQty is Required !!! ");
            }
            if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity < 0)
            {
                return ErrorResult("IQty is not allow negative figure !!! ");
            }
            if (model.IssueDetail != null && model.IssueDetail.IssuedQuantity > model.IssueDetail.RequiredQuantity)
            {
                return ErrorResult("IQty not greater than RQty!!! ");
            }
            if (model.IssueDetail != null && model.IssueDetail.StockInHand < model.IssueDetail.IssuedQuantity)
            {
                return ErrorResult("IQty not greater than SinH !!! ");
            }
            return PartialView("~/Areas/Inventory/Views/BatchWiseIssue/_AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "batchwiseissue-2,batchwiseissue-3")]
        public ActionResult Save(BachWiseIssueViewModel model)
        {
        
            var saveIndex = 0;
            try
            {
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
                       IssueReceiveNoteNo = model.IssueReceiveNoteNo, // bathc no. is IssueReceiveNo  . 
                       IssueReceiveNoteDate = model.IssueReceiveNoteDate,
                       CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(),
                       CreatedDate = DateTime.Now,
                       IsActive = true,
                   },
                   MaterialIssueRequisitionId = model.MaterialIssueRequisitionId,
                    MaterialIssueId = model.MaterialIssueId,
                    IssueReceiveDate = model.IssueReceiveDate,
                    IssueReceiveNo = model.IssueReceiveNo,
                    IType = (int)MaterialIssueType.BatchWiseIssue,
                    ToppingType=model.ToppingType,
                    Quantity = model.Quantity,
                    BtRefNo = model.BtRefNo,
                    Note = model.Note,
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
                         IsActive = true,
                         CreatedBy = PortalContext.CurrentUser.UserId,
                         CreatedDate = DateTime.Now,
                    });
                }
                if (model.MaterialIssueDetails.Any())
                {
                    if (model.MaterialIssueId > 0)
                    {
                        saveIndex = _materialIssueManager.EditMaterialIssue(materialIssue);

                    }
                    else
                    {
                        saveIndex=_materialIssueManager.SaveMaterialIssue(materialIssue);
                        ChemicalIssueReport(materialIssue.MaterialIssueId);
                    }
             
                }
                else
                {
                    return ErrorResult("Please add issue items  !!");
                }

              
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
              return  ErrorResult(String.Format(exception.Message));
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save data"));
          
        }
        [AjaxAuthorize(Roles = "batchwiseissue-3")]
        public ActionResult Delete(BachWiseIssueViewModel model)
        {
            var deleteIndex = 0;
            const int batchWiseIssue = 2;
            try
            {

                deleteIndex = _materialIssueManager.DeleteBatchWiseIssue(model.MaterialIssueId, batchWiseIssue);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }

        public JsonResult BatchAutoComplite(string searchString)
        {
            object batchList = _batchManager.BatchAutoComplite(PortalContext.CurrentUser.CompId, searchString);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }

        public void ChemicalIssueReport(int materialIssueId)
        {
            DataTable chemicalIssueChallan = _materialIssueManager.GetChemicalIssueChallan(materialIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "ChemicalIssueReport.rdlc");
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ChemicalIssueDSet", chemicalIssueChallan) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 1, MarginRight = 1, MarginBottom = .2 };
            ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);

            DbEmailModel dbEmail = _emailSendManager.GetDbEmailByTemplateId(EmailTemplateRefId.CHAMICALE_ISSUE, PortalContext.CurrentUser.CompId);
            dbEmail.Subject = "BATCH WISE CHAMICAL ISSUE NOTE";
            dbEmail.Body = "BATCH WISE CHAMICAL ISSUE IS CREATED BY :" + PortalContext.CurrentUser.Name;
            dbEmail.FileAttachments = HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + ReportType.PDF);
            bool send = _emailSendManager.SendDbEmail(dbEmail);


        }

    }
}