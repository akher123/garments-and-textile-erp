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
    public class ReDyeingFabricReceiveManager : IReDyeingFabricReceiveManager
    {
        private readonly IReDyeingFabricReceiveRepository _reDyeingFabricReceiveRepository;
        private readonly IRepository<Inventory_ReDyeingFabricReceiveDetail> _redyeingFabricReceiveDetailRepository;
        public ReDyeingFabricReceiveManager(IReDyeingFabricReceiveRepository reDyeingFabricReceiveRepository, IRepository<Inventory_ReDyeingFabricReceiveDetail> redyeingFabricReceiveDetail)
        {
            _reDyeingFabricReceiveRepository = reDyeingFabricReceiveRepository;
            _redyeingFabricReceiveDetailRepository = redyeingFabricReceiveDetail;
        }

        public List<Inventory_ReDyeingFabricReceive> GetReDyeingFabReceivesByPaging(string compId, string searchString, int pageIndex, out int totalRecords)
        {
            var reDyeingFabricReceives =
                _reDyeingFabricReceiveRepository.GetWithInclude(x => x.CompId == compId && (x.RefNo.Contains(searchString) || String.IsNullOrEmpty(searchString)&&(x.Party.Name.Contains(searchString) || String.IsNullOrEmpty(searchString))), "Party");
            var pageSize = AppConfig.PageSize;
            totalRecords = reDyeingFabricReceives.Count();
            reDyeingFabricReceives = reDyeingFabricReceives
                .OrderByDescending(r => r.RefNo)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            return reDyeingFabricReceives.ToList();
        }

        public string GetNewRefNo(string compId)
        {
            var maxRefId = _reDyeingFabricReceiveRepository.Filter(x => x.CompId == compId).Max(x => x.RefNo) ?? "0";
            return maxRefId.IncrementOne().PadZero(6);
        }

        public int EditRedyeingFabricReceive(Inventory_ReDyeingFabricReceive model)
        {

            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                Inventory_ReDyeingFabricReceive reDyeingFabricReceive = _reDyeingFabricReceiveRepository.FindOne(x => x.ReDyeingFabricReceiveId == model.ReDyeingFabricReceiveId);
                reDyeingFabricReceive.PartyId = model.PartyId;
                reDyeingFabricReceive.ChallanDate = model.ChallanDate;
                reDyeingFabricReceive.ChallanNo = model.ChallanNo;
                reDyeingFabricReceive.ReceiveDate = model.ReceiveDate;
                reDyeingFabricReceive.GatEntryNo = model.GatEntryNo;
                reDyeingFabricReceive.Remarks = model.Remarks;

                _reDyeingFabricReceiveRepository.Edit(reDyeingFabricReceive);
                _redyeingFabricReceiveDetailRepository.Delete(x => x.ReDyeingFabricReceiveId == model.ReDyeingFabricReceiveId);
                edited = _redyeingFabricReceiveDetailRepository.SaveList(model.Inventory_ReDyeingFabricReceiveDetail.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public int SaveFinishFabricIssue(Inventory_ReDyeingFabricReceive model)
        {
            return _reDyeingFabricReceiveRepository.Save(model);
        }

        public Inventory_ReDyeingFabricReceive GetReDyeingFabricReceiveById(long reDyeingFabricReceiveId)
        {
            return _reDyeingFabricReceiveRepository.FindOne(x => x.ReDyeingFabricReceiveId == reDyeingFabricReceiveId);
        }

        public List<VwReDyeingFabricReceiveDetail> GetVwReDyeingFabricReceiveDetailById(long reDyeingFabricReceiveId)
        {
            return _reDyeingFabricReceiveRepository.GetVwReDyeingFabricReceiveDetailById(reDyeingFabricReceiveId);
        }

        public int DeleteRedyeingFabricReceive(long reDyeingFabricReceiveId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                Inventory_ReDyeingFabricReceive reDyeingFabricReceive = _reDyeingFabricReceiveRepository.FindOne(x => x.ReDyeingFabricReceiveId == reDyeingFabricReceiveId);
                deleted += _redyeingFabricReceiveDetailRepository.Delete(x => x.ReDyeingFabricReceiveId == reDyeingFabricReceiveId);
                _reDyeingFabricReceiveRepository.Delete(reDyeingFabricReceive);
           
                transaction.Complete();
            }
            return deleted;
        }
    }
}
