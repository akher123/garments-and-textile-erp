using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class GoodsReceivingNoteManager : IGoodsReceivingNoteManager
    {
        private readonly IGoodsReceivingNoteRepository _goodsReceivingNoteRepository;

        private readonly IStoreLedgerRepository _storeLedgerRepository;
        private readonly IQualityCertificateRepository _qualityCertificateRepository;
        public ResponsModel ResponsModel { get; set; }
        public GoodsReceivingNoteManager(SCERPDBContext context)
        {
            _storeLedgerRepository = new StoreLedgerRepository(context);
            _goodsReceivingNoteRepository = new GoodsReceivingNoteRepository(context);
            _qualityCertificateRepository = new QualityCertificateRepository(context);
        }

        public string GetNewGoodsReceivingNoteNumber()
        {
            return _goodsReceivingNoteRepository.GetNewGoodsReceivingNoteNumber();
        }

        public bool CheckGoodsReceivingNote(int qualityCertificateId)
        {
            return _goodsReceivingNoteRepository.Exists(x => x.QualityCertificateId == qualityCertificateId);
        }

        public int GoodsReceivingNote(Inventory_GoodsReceivingNote grn)
        {
            int effectedRows = 0;

            using (var transation = new TransactionScope())
            {
                var qualityCertificate = _qualityCertificateRepository.FindOne(x => x.QualityCertificateId == grn.QualityCertificateId);
                qualityCertificate.IsGrnConverted = true;
                effectedRows += _qualityCertificateRepository.Edit(qualityCertificate);
                grn.CreatedBy = PortalContext.CurrentUser.UserId;
                grn.CreatedDate = DateTime.Now;
                grn.IsActive = true;
                grn.IsApproved = false;
                effectedRows += _goodsReceivingNoteRepository.Save(grn);
                transation.Complete();
            }
            return effectedRows;
        }

        public List<VGoodsReceivingNote> GetGoodsReceivingNoteByPaging(out int totalRecords, VGoodsReceivingNote model)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VGoodsReceivingNote, bool>> predicate = x => (x.IsSendToStoreLedger == model.IsSendToStoreLedger || model.IsSendToStoreLedger == null) && (x.IsApproved == model.IsApproved || model.IsApproved == null)
                && ((x.ReceivedRegisterNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.RequisitionNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.GRNNumber.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.InvoiceNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.QCReferenceNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.SupplierCompanyName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.ReceivedRegisterNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                      && ((x.GRNDate >= model.FromDate || model.FromDate == null) && (x.GRNDate <= model.ToDate || model.ToDate == null));
            var vGoodsReceivingNotes = _goodsReceivingNoteRepository.GetGoodsReceivingNotes(predicate);
            totalRecords = vGoodsReceivingNotes.Count();
            switch (model.sort)
            {
                case "GRNNumber":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vGoodsReceivingNotes = vGoodsReceivingNotes
                                .OrderByDescending(r => r.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vGoodsReceivingNotes = vGoodsReceivingNotes
                                .OrderBy(x => x.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vGoodsReceivingNotes = vGoodsReceivingNotes
                               .OrderByDescending(r => r.ReceivedDate)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                    break;

            }
            return vGoodsReceivingNotes.ToList();
        }

        public Inventory_GoodsReceivingNote GetGoodsReceivingNoteById(int goodsReceivingNotesId)
        {
            return
                _goodsReceivingNoteRepository.FindOne(
                    x => x.IsActive && x.GoodsReceivingNotesId == goodsReceivingNotesId);
        }

        public ResponsModel SendToStoreLager(Inventory_GoodsReceivingNote goodsReceivingNote)
        {
            ResponsModel = new ResponsModel();
            var goodsReceivingNoteId = goodsReceivingNote.GoodsReceivingNotesId;
            var isExist = _goodsReceivingNoteRepository.Exists(x => x.IsActive && x.IsSendToStoreLedger && x.GoodsReceivingNotesId == goodsReceivingNoteId);

            var effectedRows = 0;
            using (var transaction = new TransactionScope())
            {

                if (isExist)
                {
                    var grns = _storeLedgerRepository.Filter(
                            x => x.GoodsReceivingNoteId == goodsReceivingNote.GoodsReceivingNotesId);
                    effectedRows = _storeLedgerRepository.DeleteRange(grns);
                }

                var goodsReceivingNoteObj = _goodsReceivingNoteRepository.FindOne(x => x.GoodsReceivingNotesId == goodsReceivingNoteId);
                goodsReceivingNoteObj.Remarks = goodsReceivingNote.Remarks;
                goodsReceivingNoteObj.DeductionAmt = goodsReceivingNote.DeductionAmt;
                goodsReceivingNoteObj.IsSendToStoreLedger = true;
                goodsReceivingNoteObj.EditedBy = PortalContext.CurrentUser.UserId;
                goodsReceivingNoteObj.EditedDate = DateTime.Now;
                effectedRows += _goodsReceivingNoteRepository.Edit(goodsReceivingNoteObj);
                effectedRows += goodsReceivingNote.Inventory_StoreLedger.Sum(inventoryStoreLedger => _storeLedgerRepository.Save(inventoryStoreLedger));
                if (effectedRows > 0)
                {
                    ResponsModel.Message = "Data Save Successfully";
                    ResponsModel.Status = true;
                }
                else
                {
                    ResponsModel.Message = "Data Not  Saved ";
                }
                transaction.Complete();
            }


            return ResponsModel;
        }

        public int DeleteGoodsReceivingNote(int goodsReceivingNotesId)
        {
            string transactionType = Convert.ToInt32(StoreLedgerTransactionType.Receive).ToString();
            int deleteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                deleteIndex =
                _storeLedgerRepository.Delete(x => x.GoodsReceivingNoteId == goodsReceivingNotesId && x.TransactionType == transactionType);

                deleteIndex += _goodsReceivingNoteRepository.Delete(x => x.GoodsReceivingNotesId == goodsReceivingNotesId && x.IsActive);

                transaction.Complete();
            }
            return deleteIndex;

        }

        public int ApprovedGrnSave(int goodsReceivingNotesId)
        {
            Inventory_GoodsReceivingNote grn =
                _goodsReceivingNoteRepository.FindOne(x => x.GoodsReceivingNotesId == goodsReceivingNotesId);
            grn.AppBy = PortalContext.CurrentUser.UserId;
            grn.IsApproved = !grn.IsApproved.GetValueOrDefault();
            return _goodsReceivingNoteRepository.Edit(grn);
        }
    }
}
