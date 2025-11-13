using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class RedyeingFabricIssueManager : IRedyeingFabricIssueManager
    {
        private readonly IRedyeingFabricIssueRepository _redyeingFabricIssueRepository;
        private readonly IRepository<Inventory_RedyeingFabricIssueDetail> _redyeingDetailRepository;

        public RedyeingFabricIssueManager(IRedyeingFabricIssueRepository redyeingFabricIssueRepository, IRepository<Inventory_RedyeingFabricIssueDetail> redyeingDetailRepository)
        {
            _redyeingFabricIssueRepository = redyeingFabricIssueRepository;
            _redyeingDetailRepository = redyeingDetailRepository;
        }

        public List<Inventory_RedyeingFabricIssue> GetRedyeingFabIssueByPaging(string compId, string searchString, int pageIndex,
            out int totalRecords)
        {
            var redyeingFabricIssues = _redyeingFabricIssueRepository.GetWithInclude(x => x.CompId == compId && (x.RefNo.Contains(searchString) || String.IsNullOrEmpty(searchString)), "Party");
            var pageSize = AppConfig.PageSize;
            totalRecords = redyeingFabricIssues.Count();
            redyeingFabricIssues = redyeingFabricIssues
                .OrderByDescending(r => r.RefNo)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            return redyeingFabricIssues.ToList();
        }

        public string GetNewRefNo(string compId)
        {
            var maxRefId = _redyeingFabricIssueRepository.Filter(x => x.CompId == compId).Max(x => x.RefNo) ?? "0";
            return maxRefId.IncrementOne().PadZero(6);
        }

        public Inventory_RedyeingFabricIssue GetRedyeingFabricIssueById(long redyeingFabricIssueId)
        {
            return _redyeingFabricIssueRepository.FindOne(x => x.RedyeingFabricIssueId == redyeingFabricIssueId);
        }

        public List<VwRedyeingFabricIssueDetail> GetVwRedyeingFabricIssueDetailById(long redyeingFabricIssueId)
        {
            return _redyeingFabricIssueRepository.GetVwRedyeingFabricIssueDetailById(redyeingFabricIssueId);
        }

        public int EditRedyeingFabricIssue(Inventory_RedyeingFabricIssue model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                Inventory_RedyeingFabricIssue redyeingFabricIssue = _redyeingFabricIssueRepository.FindOne(x => x.RedyeingFabricIssueId == model.RedyeingFabricIssueId);
                redyeingFabricIssue.PartyId = model.PartyId;
                redyeingFabricIssue.ChallanDate = model.ChallanDate;
                redyeingFabricIssue.ChallanNo = model.ChallanNo;
                redyeingFabricIssue.VehicleType = model.VehicleType;
                redyeingFabricIssue.DriverName = model.DriverName;
                redyeingFabricIssue.DriverPhone = model.DriverPhone;
                redyeingFabricIssue.Remarks = model.Remarks;
                redyeingFabricIssue.EditedBy = model.EditedBy;
                _redyeingFabricIssueRepository.Edit(redyeingFabricIssue);
                _redyeingDetailRepository.Delete(x => x.RedyeingFabricIssueId == model.RedyeingFabricIssueId);
                edited = _redyeingDetailRepository.SaveList(model.Inventory_RedyeingFabricIssueDetail.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public int SaveRedyeingFabricIssue(Inventory_RedyeingFabricIssue model)
        {
            return _redyeingFabricIssueRepository.Save(model);
        }

        public object GetRedyeingReceivedBatchAutocomplite(string compId, string searchString, long partyId)
        {
            return _redyeingFabricIssueRepository.GetRedyeingReceivedBatchAutocomplite(compId, searchString, partyId);
        }

        public IEnumerable<dynamic> GetRedyeingReceivedBatchDetailByBatchId(long batchId)
        {
            return _redyeingFabricIssueRepository.GetRedyeingReceivedBatchDetailByBatchId(batchId);
        }

        public int DeleteRedyeingFabricIssue(long redyeingFabricIssueId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                _redyeingDetailRepository.Delete(x => x.RedyeingFabricIssueId == redyeingFabricIssueId);
                deleted += _redyeingFabricIssueRepository.Delete(x => x.RedyeingFabricIssueId == redyeingFabricIssueId);
                transaction.Complete();
            }
            return deleted;
        }

        public int ApproveRedyeingIssueById(long redyeingFabricIssueId)
        {

            Inventory_RedyeingFabricIssue consumption =
                _redyeingFabricIssueRepository.FindOne(x => x.RedyeingFabricIssueId == redyeingFabricIssueId);
            if (consumption.IsApproved)
            {
                consumption.ApprovedBy = null;
                consumption.IsApproved = false;
            }
            else
            {
                consumption.ApprovedBy = PortalContext.CurrentUser.UserId;
                consumption.IsApproved = true;
            }
            return _redyeingFabricIssueRepository.Edit(consumption);
        }

        public DataTable GetRedyeingFabricIssue(long redyeingFabricIssueId)
        {
            string sqlQuery = String.Format("exec spInvRedyeingFabricIssue '{0}'", redyeingFabricIssueId);
            return _redyeingFabricIssueRepository.ExecuteQuery(sqlQuery);
        }
    }
}
