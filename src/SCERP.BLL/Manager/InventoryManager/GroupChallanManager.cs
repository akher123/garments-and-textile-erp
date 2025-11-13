using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class GroupChallanManager : IGroupChallanManager
    {
        private IRepository<Inventory_GroupChallan> _groupChallanRepository;
        private IAdvanceMaterialIssueRepository _advanceMaterialIssueRepository;
        public GroupChallanManager(IRepository<Inventory_GroupChallan> groupChallanRepository, IAdvanceMaterialIssueRepository advanceMaterialIssueRepository)
        {
            _groupChallanRepository = groupChallanRepository;
            _advanceMaterialIssueRepository = advanceMaterialIssueRepository;
        }

        public List<Inventory_GroupChallan> GetAllGroupChallanByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var groupChallans =
                _groupChallanRepository.Filter(x => x.Name.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                             || x.RefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = groupChallans.Count();
            switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            groupChallans = groupChallans
                                 .OrderByDescending(r => r.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            groupChallans = groupChallans
                                 .OrderBy(r => r.GroupChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    groupChallans = groupChallans
                        .OrderByDescending(r => r.GroupChallanId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return groupChallans.ToList();
        }

        public Inventory_GroupChallan GetGroupChallanById(int groupChallanId, string compId)
        {
            Inventory_GroupChallan groupChallan = _groupChallanRepository.FindOne(x => x.CompId == compId && x.GroupChallanId == groupChallanId);
            groupChallan.MaterialIssues = _advanceMaterialIssueRepository.Filter(x => x.GroupChallanId == groupChallanId).ToList();
            return groupChallan;
        }

        public string GetNewRefId()
        {
            var maxRefId = _groupChallanRepository.All().Max(x => x.RefId);
            return maxRefId.IncrementOne().PadZero(6);
        }

        public int EditGroupChallan(Inventory_GroupChallan model)
        {
            using (var transaction = new TransactionScope())
            {

                var gchallan = _groupChallanRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.GroupChallanId == model.GroupChallanId);
                gchallan.Name = model.Name;
                gchallan.GDate = model.GDate;
                gchallan.Remarks = model.Remarks;
                var edited = _groupChallanRepository.Edit(gchallan);

                foreach (var materialIssue in _advanceMaterialIssueRepository.Filter(x => x.GroupChallanId == model.GroupChallanId))
                {
                    var mi = _advanceMaterialIssueRepository.FindOne(x => x.AdvanceMaterialIssueId == materialIssue.AdvanceMaterialIssueId);
                    mi.GroupChallanId = 0;
                    _advanceMaterialIssueRepository.Edit(mi);
                }
                foreach (var materialIssue in model.MaterialIssues)
                {
                    var mi = _advanceMaterialIssueRepository.FindOne(x => x.AdvanceMaterialIssueId == materialIssue.AdvanceMaterialIssueId);
                    mi.GroupChallanId = model.GroupChallanId;
                    _advanceMaterialIssueRepository.Edit(mi);
                }

                transaction.Complete();
                return edited;
            }
        }

        public int DeleteGroupChallan(int groupChallanId)
        {
            using (var transaction = new TransactionScope())
            {

                foreach (var materialIssue in _advanceMaterialIssueRepository.Filter(x => x.GroupChallanId == groupChallanId))
                {
                    var mi =
                        _advanceMaterialIssueRepository.FindOne(x => x.AdvanceMaterialIssueId == materialIssue.AdvanceMaterialIssueId);
                       mi.GroupChallanId = 0;
                    _advanceMaterialIssueRepository.Edit(mi);
                }
                var deleted = _groupChallanRepository.Delete(x => x.GroupChallanId == groupChallanId);
                transaction.Complete();
                return deleted;
            }
        }

        public int SaveGroupChallan(Inventory_GroupChallan model)
        {
            using (var transaction = new TransactionScope())
            {

                model.RefId = GetNewRefId();
                model.CompId = PortalContext.CurrentUser.CompId;


                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                var saved = _groupChallanRepository.Save(model);
                foreach (var materialIssue in model.MaterialIssues)
                {
                    var mi = _advanceMaterialIssueRepository.FindOne(x => x.AdvanceMaterialIssueId == materialIssue.AdvanceMaterialIssueId);
                    mi.GroupChallanId = model.GroupChallanId;
                    _advanceMaterialIssueRepository.Edit(mi);
                }

                transaction.Complete();
                return saved;
            }
        }
    }
}
