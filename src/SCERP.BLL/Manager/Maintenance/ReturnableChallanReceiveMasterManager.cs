using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.Maintenance
{
    public class ReturnableChallanReceiveMasterManager : IReturnableChallanReceiveMasterManager
    {
        private readonly IReturnableChallanReceiveMasterRepository _challanReceiveMasterRepository;
        private readonly IReturnableChallanReceiveRepository _returnableChallanReceiveRepository;

        public ReturnableChallanReceiveMasterManager(IReturnableChallanReceiveMasterRepository challanReceiveMasterRepository, IReturnableChallanReceiveRepository returnableChallanReceiveRepository)
        {
            _challanReceiveMasterRepository = challanReceiveMasterRepository;
            _returnableChallanReceiveRepository = returnableChallanReceiveRepository;
        }

        public List<Maintenance_ReturnableChallanReceiveMaster> GetChallanReceiveMasterByPaging(int pageIndex, string sort, string sortdir, string searchString,string compId,string challanType, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var challanReceiveMasterList = _challanReceiveMasterRepository.GetWithInclude(x => x.CompId == compId && x.Maintenance_ReturnableChallan.ChllanType==challanType && ((x.ChallanNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)) || (x.Maintenance_ReturnableChallan.ReturnableChallanRefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))), "Maintenance_ReturnableChallan");
            totalRecords = challanReceiveMasterList.Count();
            switch (sort)
            {
                case "Messrs":
                    switch (sortdir)
                    {
                        case "DESC":
                            challanReceiveMasterList = challanReceiveMasterList
                                .OrderByDescending(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            challanReceiveMasterList = challanReceiveMasterList
                                .OrderBy(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    challanReceiveMasterList = challanReceiveMasterList
                        .OrderByDescending(r => r.ReturnableChallanId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return challanReceiveMasterList.ToList();
        }
        public int EditChallanReceiveMaster(Maintenance_ReturnableChallanReceiveMaster model)
        {
            int index = 0;
            using (var transaction = new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                Maintenance_ReturnableChallanReceiveMaster returnableChallanReceiveMaster =_challanReceiveMasterRepository.FindOne( x =>x.CompId == compId && x.ReturnableChallanReceiveMasterId == model.ReturnableChallanReceiveMasterId);
                returnableChallanReceiveMaster.ChallanNo = model.ChallanNo;
                returnableChallanReceiveMaster.ReceiveDate = model.ReceiveDate;
                long totalAmount = 0;
                foreach (var returnableChallanReceive in model.Maintenance_ReturnableChallanReceive)
                {
                    totalAmount += Convert.ToInt64(returnableChallanReceive.Amount);
                }
                long totalChangedAmount = (long) (totalAmount - returnableChallanReceiveMaster.TotalAmount);
                returnableChallanReceiveMaster.TotalAmount = returnableChallanReceiveMaster.TotalAmount + totalChangedAmount;
                index += _challanReceiveMasterRepository.Edit(returnableChallanReceiveMaster);

                index +=_returnableChallanReceiveRepository.Delete(x => x.CompId == compId && x.ReturnableChallanReceiveMasterId == model.ReturnableChallanReceiveMasterId);

                foreach (var returnableChallanReceive in model.Maintenance_ReturnableChallanReceive)
                {
                    Maintenance_ReturnableChallanReceive challanReceive =new Maintenance_ReturnableChallanReceive();
                    challanReceive.CompId = PortalContext.CurrentUser.CompId;
                    challanReceive.ReturnableChallanReceiveMasterId = model.ReturnableChallanReceiveMasterId;
                    challanReceive.ReturnableChallanDetailId = returnableChallanReceive.ReturnableChallanDetailId;
                    challanReceive.ReceiveQty = returnableChallanReceive.ReceiveQty;
                    challanReceive.RejectQty = returnableChallanReceive.RejectQty;
                    challanReceive.Amount = returnableChallanReceive.Amount;
                    index += _returnableChallanReceiveRepository.Save(challanReceive);
                }
                transaction.Complete();
            }
            return index;
        }

        public int SaveChallanReceiveMaster(Maintenance_ReturnableChallanReceiveMaster model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.RetChallanMasterRefId = "R001";
            long totalAmount = 0;
            foreach (var returnableChallanReceive in model.Maintenance_ReturnableChallanReceive)
            {
                totalAmount += Convert.ToInt64(returnableChallanReceive.Amount);
            }
            model.TotalAmount = totalAmount;
            return _challanReceiveMasterRepository.Save(model);
        }

        public Maintenance_ReturnableChallanReceiveMaster GetChallanReceiveMasterByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId)
        {
            return
                _challanReceiveMasterRepository.FindOne(
                    x => x.CompId == compId && x.ReturnableChallanReceiveMasterId == returnableChallanReceiveMasterId);
        }
        public int DeleteReturnableChallanReceiveMaster(long returnableChallanReceiveMasterId, string compId) 
        {
            int index = 0; 
            using (var transaction = new TransactionScope())
            {
                index +=_returnableChallanReceiveRepository.Delete(x =>x.CompId == compId && x.ReturnableChallanReceiveMasterId == returnableChallanReceiveMasterId);
                index += _challanReceiveMasterRepository.Delete(x=>x.CompId==compId && x.ReturnableChallanReceiveMasterId==returnableChallanReceiveMasterId);
                transaction.Complete();
            }
            return index;
        }

        public DataTable GetReturnableChallanReceive(long returnableChallanReceiveMasterId, string compId)
        {
            return _challanReceiveMasterRepository.GetReturnableChallanReceive(returnableChallanReceiveMasterId, compId);
        }
    }
}
