using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class GreyIssueManager : IGreyIssueManager
    {
        private readonly IGreyIssueRepository _greyIssueRepository;
        private readonly IRepository<Inventory_GreyIssueDetail> _greyIssueDetailRepository;

        public GreyIssueManager(IGreyIssueRepository greyIssueRepository, IRepository<Inventory_GreyIssueDetail> greyIssueDetailRepository)
        {
            _greyIssueRepository = greyIssueRepository;
            _greyIssueDetailRepository = greyIssueDetailRepository;
        }

        public List<Inventory_GreyIssue> GetGreyReceiveByPaging(DateTime? fromDate, DateTime? toDate, string searchString, string compId, int pageIndex,
            out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            IQueryable<Inventory_GreyIssue> queryable = _greyIssueRepository.GetWithInclude(x => x.CompId == compId && ((x.ChallanNo.Contains(searchString) || String.IsNullOrEmpty(searchString)) || (x.RefId.Contains(searchString) || String.IsNullOrEmpty(searchString))), "Party");
            totalRecords= queryable.Count();
            queryable = queryable
            .OrderByDescending(r => r.GreyIssueId)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
            return queryable.ToList();
        }

        public List<KnittingOrderDelivery> GetKnittingOrderDelivery(int programId, long greyIssueId)
        {
            List<KnittingOrderDelivery> knittingOrderDeliveries = _greyIssueRepository.GetKnittingOrderDelivery(programId, greyIssueId);
            return knittingOrderDeliveries;
        }

        public int SaveGreyIssue(Inventory_GreyIssue greyIssue)
        {
           return   _greyIssueRepository.Save(greyIssue);
        }

        public string GetNewRow(string compId)
        {

            var refIdDesgit = _greyIssueRepository.Filter(x => x.CompId == compId).Max(x => x.RefId) ?? "0";
            string refId = refIdDesgit.IncrementOne().PadZero(8);
            return refId;
        }

        public Inventory_GreyIssue GetGreyissueById(long p)
        {
            return _greyIssueRepository.FindOne(x => x.GreyIssueId == p);
        }

        public int DeleteGreyIssue(long greyIssueId)
        {
            return _greyIssueRepository.DeleteGreyIssue(greyIssueId);
        }

        public int EditGreyIssue(Inventory_GreyIssue greyIssue)
        {
            Inventory_GreyIssue geGreyIssue = _greyIssueRepository.FindOne(x => x.GreyIssueId == greyIssue.GreyIssueId);
            geGreyIssue.ChallanDate = greyIssue.ChallanDate;
            geGreyIssue.ChallanNo = greyIssue.ChallanNo;
            geGreyIssue.Through = greyIssue.Through;
            geGreyIssue.VheicalNo = greyIssue.VheicalNo;
            geGreyIssue.Mobile = greyIssue.Mobile;
            geGreyIssue.Remarks = greyIssue.Remarks;
            geGreyIssue.EditedDate = greyIssue.EditedDate;
            geGreyIssue.EditedBy = PortalContext.CurrentUser.UserId;
            _greyIssueDetailRepository.Delete(x => x.GreyIssueId == greyIssue.GreyIssueId);
            _greyIssueDetailRepository.SaveList(greyIssue.Inventory_GreyIssueDetail.ToList());
            return _greyIssueRepository.Edit(geGreyIssue);
        }
        public DataTable GetGeryIssuePartyChallan(long greyIssueId)
        {
            return _greyIssueRepository.GetGeryIssuePartyChallan(greyIssueId);
        }
        public int GreyIssureApproval(long greyIssueId)
        {
            Inventory_GreyIssue greyIssue = _greyIssueRepository.FindOne(x => x.GreyIssueId == greyIssueId);
            greyIssue.IsApproved = greyIssue.IsApproved != true;
            greyIssue.ApprovedBy = greyIssue.IsApproved == true ? PortalContext.CurrentUser.UserId : null;
            return _greyIssueRepository.Edit(greyIssue);
        }
    }
}
