using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class GoodsReceivingNoteController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "goodsreceivingnotes-1,goodsreceivingnotes-2,goodsreceivingnotes-3")]
        public ActionResult Index(GoodsReceivingNoteViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                return View(model);
            }
            model.VGoodsReceivingNotes = GoodsReceivingNoteManager.GetGoodsReceivingNoteByPaging(out totalRecords, model);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "goodsreceivingnotes-2,goodsreceivingnotes-3")]
        public ActionResult Edit(GoodsReceivingNoteViewModel model)
        {
            ModelState.Clear();
            var isExist = GoodsReceivingNoteManager.CheckGoodsReceivingNote(model.QualityCertificateId);
            if (isExist)
            {
                return ErrorResult("GoodsReceivingNote all ready converted");
            }
            else
            {
                model.GRNDate = DateTime.Now;
                var grnNumber = GoodsReceivingNoteManager.GetNewGoodsReceivingNoteNumber();
                model.GRNNumber = grnNumber;
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "goodsreceivingnotes-2,goodsreceivingnotes-3")]
        public JsonResult Save(Inventory_GoodsReceivingNote grn)
        {
            var effectedRows = 0;
            try
            {
                effectedRows = GoodsReceivingNoteManager.GoodsReceivingNote(grn);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return effectedRows == 0 ? ErrorResult("Fail to save data") : Reload();
        }

        public ActionResult SendToStoreLager(GoodsReceivingNoteViewModel model)
        {
            try
            {
                var goodsReceivingNote = new Inventory_GoodsReceivingNote
                {
                    GoodsReceivingNotesId = model.GoodsReceivingNotesId,
                    Remarks = model.Remarks,
                    DeductionAmt = model.DeductionAmt,
                    Inventory_StoreLedger = model.VQualityCertificateDetails.Select(x => new Inventory_StoreLedger()
                    {
                        ItemId = x.ItemId,
                        BrandId = x.BrandId,
                        SizeId = x.SizeId,
                        OriginId = x.OriginId,
                        GoodsReceivingNoteId = model.GoodsReceivingNotesId,
                        CurrencyId = x.CurrencyId,
                        TransactionType = Convert.ToString((int)StoreLedgerTransactionType.Receive),
                        Quantity = x.CorrectQuantity,
                        UnitPrice = x.UnitPrice,
                        Amount = x.CorrectQuantity * x.UnitPrice,
                        TransactionDate = model.TransactionDate.GetValueOrDefault(),

                        IsActive = true,
                        CreatedBy = PortalContext.CurrentUser.UserId,
                        CreatedDate = DateTime.Now,
                    }).ToList()
                };
                ResponsModel = GoodsReceivingNoteManager.SendToStoreLager(goodsReceivingNote);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return ResponsModel.Status ? Reload() : ErrorResult(ResponsModel.Message);

        }

        [AjaxAuthorize(Roles = "goodsreceivingnotes-1,goodsreceivingnotes-2,goodsreceivingnotes-3")]
        public ActionResult Report(int goodsReceivingNotesId)
        {
            string reportName = "GoodsReceivingNote";
            var reportParams = new List<ReportParameter> { new ReportParameter("GoodsReceivingNotesId", goodsReceivingNotesId.ToString()), new ReportParameter("CompId", PortalContext.CurrentUser.CompId) };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }

        [AjaxAuthorize(Roles = "goodsreceivingnotes-1,goodsreceivingnotes-2,goodsreceivingnotes-3")]
        public ActionResult UpdateStoreLedger(GoodsReceivingNoteViewModel model)
        {
            ModelState.Clear();
            var transactionDate = StoreLedgerManager.GetTransactionDateByGrnId(model.GoodsReceivingNotesId);
            var grn = GoodsReceivingNoteManager.GetGoodsReceivingNoteById(model.GoodsReceivingNotesId);
            model.GoodsReceivingNotesId = grn.GoodsReceivingNotesId;
            model.GRNNumber = grn.GRNNumber;
            model.GRNDate = grn.GRNDate;
            model.Description = grn.Description;
            model.DeductionAmt = grn.DeductionAmt;
            model.Remarks = grn.Remarks;
            model.IsActive =false;
            model.TransactionDate = transactionDate ?? grn.GRNDate;
            model.VQualityCertificateDetails = QualityCertificateManager.GetQualityCertificateDetailIds(model.ItemStoreId, model.QualityCertificateId);
            model.IsSearch = true;
            return View(model);
        }

        public ActionResult Delete(GoodsReceivingNoteViewModel model)
        {
            int deleted = 0;
            try
            {
                deleted = GoodsReceivingNoteManager.DeleteGoodsReceivingNote(model.GoodsReceivingNotesId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

            return deleted > 0 ? Reload() : ErrorResult("Delete Failed !");
        }

        public ActionResult ApprovedGrn(GoodsReceivingNoteViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.IsGrnConverted = true;
            model.VGoodsReceivingNotes = GoodsReceivingNoteManager.GetGoodsReceivingNoteByPaging(out totalRecords, model);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult ApprovedGrnSave(int goodsReceivingNotesId)
        {

            int saved = GoodsReceivingNoteManager.ApprovedGrnSave(goodsReceivingNotesId);
            return ErrorResult(saved > 0 ? "Approved GRN Successfully" : "Failed To Approve GRN");
        }
    }


}