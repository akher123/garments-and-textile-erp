using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class KnittingRollIssueManager : IKnittingRollIssueManager
    {
        private readonly IKnittingRollIssueRepository _knittingRollIssueRepository;
        private IRepository<PROD_KnittingRollIssueDetail> _knittingRollDetails; 
        public KnittingRollIssueManager(IRepository<PROD_KnittingRollIssueDetail> knittingRollDetails,IKnittingRollIssueRepository knittingRollIssueRepository)
        {
            _knittingRollIssueRepository = knittingRollIssueRepository;
            _knittingRollDetails = knittingRollDetails;
        }

        public List<PROD_KnittingRollIssue> GetKnittingRollIssueByPaging(int pageIndex, string orderStyleRefId, string sortdir, string searchKey, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var knittingRollIssueList = _knittingRollIssueRepository.Filter(x => x.CompId == compId && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId)) 
                &&((x.IssueRefNo.Contains(searchKey.ToLower().Replace(" ","")) || String.IsNullOrEmpty(searchKey))||(x.ProgramRefId.Contains(searchKey.ToLower().Replace(" ","")) || String.IsNullOrEmpty(searchKey))));
            totalRecords = knittingRollIssueList.Count();
            var knittingRollIssues = knittingRollIssueList.OrderByDescending(r => r.KnittingRollIssueId).Skip(index * pageSize).Take(pageSize);
            return knittingRollIssues.ToList();
        }

        public string GetNewRefNo(string compId,int? challanType)
        {
            string prifix = "IR";
            var refId = "";
            if (challanType==3)
            {
                prifix = "RC";
                refId = _knittingRollIssueRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && (x.ChallanType == 3)).Max(x => x.IssueRefNo.Substring(2)) ?? "0";
            }
            else
            {
                 refId = _knittingRollIssueRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && (x.ChallanType != 3)).Max(x => x.IssueRefNo.Substring(2)) ?? "0";
                
            }
            return prifix + refId.IncrementOne().PadZero(6);
        }

        public PROD_KnittingRollIssue GetKnittingRollIssueById(int knittingRollIssueId)
        {
            return _knittingRollIssueRepository.FindOne(x => x.KnittingRollIssueId == knittingRollIssueId);
        }

        public int SaveKnittingRollIssue(PROD_KnittingRollIssue knittingRollIssue)
        {
            knittingRollIssue.Qty = knittingRollIssue.PROD_KnittingRollIssueDetail.Sum(x => x.RollQty);
          return  _knittingRollIssueRepository.Save(knittingRollIssue);
        }

        public int EditKnittingRollIssue(PROD_KnittingRollIssue model)
        {
            int saved = 0;
            using (var transaction=new TransactionScope())
            {
    
            _knittingRollDetails.Delete(x => x.KnittingRollIssueId == model.KnittingRollIssueId);
           var knittingRollIssue= _knittingRollIssueRepository.FindOne(x => x.KnittingRollIssueId == model.KnittingRollIssueId);
           knittingRollIssue.BatchNo = model.BatchNo;
           knittingRollIssue.ProgramRefId = model.ProgramRefId;
           knittingRollIssue.BuyerRefId = model.BuyerRefId;
           knittingRollIssue.OrderNo = model.OrderNo;
           knittingRollIssue.OrderStyleRefId = model.OrderStyleRefId;
           knittingRollIssue.Qty = model.PROD_KnittingRollIssueDetail.Sum(x => x.RollQty);
            knittingRollIssue.IssueDate = model.IssueDate;
            knittingRollIssue.Remarks = model.Remarks;
            knittingRollIssue.Editedby = model.Editedby;
            knittingRollIssue.EditedDate = model.EditedDate;
            knittingRollIssue.ChallanType = model.ChallanType;
            knittingRollIssue.Posted = model.Posted;
                _knittingRollIssueRepository.Edit(knittingRollIssue);
            var knittingRollsIssueDetails = model.PROD_KnittingRollIssueDetail.Select(x =>
            {
                x.KnittingRollIssueId = model.KnittingRollIssueId;
                return x;
            });
            saved+= _knittingRollDetails.SaveList(knittingRollsIssueDetails.ToList());
            transaction.Complete();
            }
            return saved;

        }

        public List<VwKnittingRollIssueDetail> GetKnittingRollsByOrderStyleRefId(string programRefId,int challanType, string compId)
        {
            return _knittingRollIssueRepository.GetKnittingRollsByOrderStyleRefId(programRefId, challanType, compId);
        }

        public List<VwKnittingRollIssueDetail> GetRollIssueDetailsByKnittingRollIssueId(int knittingRollIssueId)
        {
            return _knittingRollIssueRepository.GetRollIssueDetailsByKnittingRollIssueId(knittingRollIssueId);
        }

        public int DeleteKnittingRollIssueById(long knittingRollIssueId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {

                _knittingRollDetails.Delete(x => x.KnittingRollIssueId == knittingRollIssueId);

               deleted+= _knittingRollIssueRepository.Delete(x => x.KnittingRollIssueId == knittingRollIssueId);
              
                transaction.Complete();
            }
            return deleted;

        }

        public List<PROD_KnittingRollIssue> GetKnittingRollIssueByOrderStyleRefId(string orderStyleRefId)
        {
            return
                _knittingRollIssueRepository.Filter(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.OrderStyleRefId == orderStyleRefId).ToList();
        }

        public int IsReceivedRollChallan(int knittingRollIssueId, string currentUserCompId)
        {
            PROD_KnittingRollIssue rollIssue = _knittingRollIssueRepository.FindOne(x => x.CompId == currentUserCompId && x.KnittingRollIssueId == knittingRollIssueId);
            rollIssue.IsRecived = rollIssue.IsRecived != true;
            rollIssue.ReceivedBy = rollIssue.IsRecived == true ? PortalContext.CurrentUser.UserId : null;
            return _knittingRollIssueRepository.Edit(rollIssue);
        }

        public object GetRollBySearchKey(string searchKey)
        {
           return _knittingRollDetails
               .GetWithInclude( x => x.PROD_KnittingRoll.RollRefNo
               .Contains(searchKey) || string.IsNullOrEmpty(searchKey), "PROD_KnittingRoll").Take(10)
               .ToList().Select(x => new { x.PROD_KnittingRoll.RollRefNo, x.KnittingRollIssueId, x.KnittingRollId, x.PROD_KnittingRoll.Quantity}).ToList();
        }

        public List<PROD_KnittingRollIssue> GetPartyKnittingChallanList(int pageIndex, string searchString, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var knittingRollIssueList = _knittingRollIssueRepository.Filter(x => x.CompId == compId&x.ProgramRefId.Substring(0,2)=="OR"
                && ((x.IssueRefNo.Contains(searchString.ToLower().Replace(" ", "")) || String.IsNullOrEmpty(searchString)) || (x.ProgramRefId.Contains(searchString.ToLower().Replace(" ", "")) || String.IsNullOrEmpty(searchString))));
            totalRecords = knittingRollIssueList.Count();
            var knittingRollIssues = knittingRollIssueList.OrderByDescending(r => r.KnittingRollIssueId).Skip(index * pageSize).Take(pageSize);
            return knittingRollIssues.ToList();
        }

    
    }
}
