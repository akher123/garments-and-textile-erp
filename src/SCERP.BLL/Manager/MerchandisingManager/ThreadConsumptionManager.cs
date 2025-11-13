using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{   
    public class ThreadConsumptionManager : IThreadConsumptionManager
    {
        private readonly IThreadConsumptionRepository _consumptionRepository;
        private readonly IRepository<OM_ThreadConsumptionDetail> _consDetailRepository;
        public ThreadConsumptionManager(IThreadConsumptionRepository consumptionRepository, IRepository<OM_ThreadConsumptionDetail> consDetailRepository)
        {
            _consumptionRepository = consumptionRepository;
            _consDetailRepository = consDetailRepository;
        }

        public List<VwThreadConsumption> GetThreadConsumptionsByPaging(string compId,string searchString)
        {
            return _consumptionRepository.GetThreadConsumptionsByPaging(compId, searchString);
        }

        public int SaveThreadConsumption(OM_ThreadConsumption consumption)
        {
            return _consumptionRepository.Save(consumption);
        }

        public int EditThreadConsumption(OM_ThreadConsumption model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                OM_ThreadConsumption consumption = _consumptionRepository.FindOne(x => x.ThreadConsumptionId == model.ThreadConsumptionId);
                consumption.BuyerRefId = model.BuyerRefId;
                consumption.OrderNo = model.OrderNo;
                consumption.OrderStyleRefId = model.OrderStyleRefId;
                consumption.SizeRefId = model.SizeRefId;
                consumption.EntryDate = model.EntryDate;
                consumption.Remarks = model.Remarks;
                _consDetailRepository.Delete(x => x.ThreadConsumptionId == model.ThreadConsumptionId);
                edited += _consumptionRepository.Edit(consumption);
                foreach (var detail in model.OM_ThreadConsumptionDetail)
                {
                    detail.ThreadConsumptionId = consumption.ThreadConsumptionId;
                    edited += _consDetailRepository.Save(detail);
                }
                transaction.Complete();
            }
            return edited;
        }

        public int DeleteThreadConsumption(int threadConsumptionId)
        {
            int delted = 0;
            using (var transaction = new TransactionScope())
            {
                 OM_ThreadConsumption consumption = _consumptionRepository.FindOne(x => x.ThreadConsumptionId == threadConsumptionId);
                _consDetailRepository.Delete(x => x.ThreadConsumptionId == threadConsumptionId);
                delted = _consumptionRepository.DeleteOne(consumption);
                transaction.Complete();
            }
            return delted;
        }

        public OM_ThreadConsumption GetThreadConsumptionById(int threadConsumptionId)
        {
            OM_ThreadConsumption consumption = _consumptionRepository.GetWithInclude(x => x.ThreadConsumptionId == threadConsumptionId, "OM_ThreadConsumptionDetail").FirstOrDefault();
            return consumption;
        }

        public DataTable GetThreadConsumptionsReportDataTable(long threadConsumptionId)
        {
            return _consumptionRepository.GetThreadConsumptionsReportDataTable(threadConsumptionId);
        }

        public int ApproveThreadConsumption(int threadConsumptionId)
        {
            OM_ThreadConsumption consumption = _consumptionRepository.FindOne(x => x.ThreadConsumptionId == threadConsumptionId);
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
            return _consumptionRepository.Edit(consumption);
        }
    }
}
