using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class FinishFabricIssueManager : IFinishFabricIssueManager
    {
        private readonly IFinishFabricIssueRepository _finishFabricIssueRepository;
        private readonly IFinishFabricIssueDetailRepository _finishFabricIssueDetailRepository;
        public FinishFabricIssueManager(IFinishFabricIssueRepository finishFabricIssueRepository, IFinishFabricIssueDetailRepository finishFabricIssueDetailRepository)
        {
            _finishFabricIssueRepository = finishFabricIssueRepository;
            _finishFabricIssueDetailRepository = finishFabricIssueDetailRepository;
        }

        public string GetFinishFabIssureRefId(string compId)
        {
            var dyeingSpChallanRefId = _finishFabricIssueRepository.Filter(x => x.CompId == compId).Max(x => x.FinishFabIssureRefId.Substring(2)) ?? "0";
            return "FD" + dyeingSpChallanRefId.IncrementOne().PadZero(6);
        }

        public Inventory_FinishFabricIssue GetFinishFabIssureById(long finishFabIssueId)
        {
            return _finishFabricIssueRepository.FindOne(x => x.FinishFabIssueId == finishFabIssueId);
        }

        public List<VwFinishFabricIssueDetail> GetFinishFabIssureDetails(long finishFabIssueId)
        {
            return _finishFabricIssueDetailRepository.GetFinishFabIssureDetails(finishFabIssueId);
        }

        public int SaveFinishFabricIssue(Inventory_FinishFabricIssue finishFabricIssue)
        {
            return _finishFabricIssueRepository.Save(finishFabricIssue);
        }

        public int EditFinishFabricIssue(Inventory_FinishFabricIssue model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                Inventory_FinishFabricIssue finishFabricIssue = _finishFabricIssueRepository.FindOne(x => x.FinishFabIssueId == model.FinishFabIssueId);
                finishFabricIssue.PartyId = model.PartyId;
                finishFabricIssue.ChallanDate = model.ChallanDate;
                finishFabricIssue.ChallanNo = model.ChallanNo;
                finishFabricIssue.VehicleType = model.VehicleType;
                finishFabricIssue.DriverName = model.DriverName;
                finishFabricIssue.DriverPhone = model.DriverPhone;
                finishFabricIssue.Remarks = model.Remarks;
                finishFabricIssue.EditedBy = model.EditedBy;
                _finishFabricIssueRepository.Edit(finishFabricIssue);
                _finishFabricIssueDetailRepository.Delete(x => x.FinishFabricIssueId == model.FinishFabIssueId);
                edited = _finishFabricIssueDetailRepository.SaveList(model.Inventory_FinishFabricIssueDetail.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public List<Inventory_FinishFabricIssue> GetFinishFabIssuresByPaging(int pageIndex, string sort, string sortdir, bool? isApproved,bool?isReceived, DateTime? toDate, DateTime? fromDate,string searchString, string compId,int? partyId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var finishFabricIssueList = _finishFabricIssueRepository.GetWithInclude(x => x.CompId == compId&&(x.PartyId== partyId || partyId==null) && (x.IsReceived == isReceived || isReceived == null) && (x.IsApproved == isApproved || isApproved == null) && (x.FinishFabIssureRefId.Contains(searchString) || String.IsNullOrEmpty(searchString)), "Party");
            totalRecords = finishFabricIssueList.Count();
            switch (sort)
            {
                case "challanNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            finishFabricIssueList = finishFabricIssueList
                                .OrderByDescending(r => r.ChallanNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            finishFabricIssueList = finishFabricIssueList
                                .OrderBy(r => r.ChallanNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    finishFabricIssueList = finishFabricIssueList
                        .OrderByDescending(r => r.FinishFabIssueId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return finishFabricIssueList.ToList();
        }

        public int ApprovedFabricDeliveryChallan(long finishFabIssueId)
        {
            Inventory_FinishFabricIssue finishFabricIssue = _finishFabricIssueRepository.FindOne(x => x.FinishFabIssueId == finishFabIssueId);
            finishFabricIssue.IsApproved = !finishFabricIssue.IsApproved;
            finishFabricIssue.ApprovedBy = finishFabricIssue.IsApproved ==true? PortalContext.CurrentUser.UserId : null;
            return _finishFabricIssueRepository.Edit(finishFabricIssue);
        }

        public int DeleteFinishFabricIssue(long finishFabIssueId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                 _finishFabricIssueDetailRepository.Delete(x => x.FinishFabricIssueId == finishFabIssueId);
                 deleted += _finishFabricIssueRepository.Delete(x => x.FinishFabIssueId == finishFabIssueId);
                transaction.Complete();
            }
            return deleted;
        }

        public object GetReceivedBatchAutocomplite(string compId, string searchString,long partyId)
        {
            return _finishFabricIssueDetailRepository.GetReceivedBatchAutocomplite(compId, searchString,partyId);
        }

        public IEnumerable GetReceivedBatchDetailByBatchId(long batchId)
        {
            return _finishFabricIssueDetailRepository.GetReceivedBatchDetailByBatchId(batchId);
        }

        public int UpdateFabricIssue(Inventory_FinishFabricIssue model)
        {
            int edited = 0;
            Inventory_FinishFabricIssue finishFabricIssue = _finishFabricIssueRepository.FindOne(x => x.FinishFabIssueId == model.FinishFabIssueId);
            finishFabricIssue.IsReceived = model.IsReceived;
            finishFabricIssue.Commerns = model.Commerns;
            finishFabricIssue.ReceivedBy = PortalContext.CurrentUser.UserId;
            finishFabricIssue.ReceivedDate = DateTime.Now;
            edited= _finishFabricIssueRepository.Edit(finishFabricIssue);
            return edited;
        }

        public object ReceivedBatchAutoComplite(string searchString)
        {
          return _finishFabricIssueDetailRepository.GetReceivedBatchAutoComplite(searchString);
        }

        public bool IsExistReceivedBatch(string batchNo)
        {
            return _finishFabricIssueDetailRepository.IsExistReceivedBatch(batchNo);
        }

        public DataTable GetDyingBillInvoices(DateTime? invoiceDate)
        {
             string spName = "exec spDyeingFinishFabricDeliveryInvoiceBills @InvoiceDate='{0}'";
            return _finishFabricIssueDetailRepository.ExecuteQuery(String.Format(spName, invoiceDate));
        }
    }
}
