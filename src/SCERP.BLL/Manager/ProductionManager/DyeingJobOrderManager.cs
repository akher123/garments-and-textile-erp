using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class DyeingJobOrderManager : IDyeingJobOrderManager
    {
        private readonly IDyeingJobOrderRepository _dyeingJobOrderRepository;
        private readonly IRepository<PROD_DyeingJobOrderDetail> _dyeingJobOrderDetailRepository; 
        public DyeingJobOrderManager(IDyeingJobOrderRepository dyeingJobOrderRepository, IRepository<PROD_DyeingJobOrderDetail> dyeingJobOrderDetailRepository)
        {
            _dyeingJobOrderRepository = dyeingJobOrderRepository;
            _dyeingJobOrderDetailRepository = dyeingJobOrderDetailRepository;
        }

        public List<PROD_DyeingJobOrder> GetDyeingJobOrderByPaging(string searchString, int pageIndex, string sort, string sortdir, DateTime? fromDate,
            DateTime? toDate, long partyId, out int totalRecord)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<PROD_DyeingJobOrder> dyeingJobOrders =
                _dyeingJobOrderRepository.GetWithInclude(x => x.CompId == PortalContext.CurrentUser.CompId, "Party");

            totalRecord = dyeingJobOrders.Count();
            dyeingJobOrders = dyeingJobOrders.OrderByDescending(
                              x => x.JobRefId)
                              .Skip(index * pageSize)
                              .Take(pageSize);
            return dyeingJobOrders.ToList();

        }

        public string GetJobRefId(string compId)
        {
            var refString = _dyeingJobOrderRepository.Filter(x => x.CompId == compId )
                    .Max(x => x.JobRefId) ?? "0";
            var jobRefId=refString.IncrementOne().PadZero(6);
            return jobRefId;
        }

        public PROD_DyeingJobOrder GetDyeingJobOrderById(long dyeingJobOrderId)
        {
            return _dyeingJobOrderRepository.FindOne(x => x.DyeingJobOrderId == dyeingJobOrderId);
        }

        public int SaveDyeingJobOrder(PROD_DyeingJobOrder jobOrder)
        {
          return  _dyeingJobOrderRepository.Save(jobOrder);
        }

        public List<VwDyeingJobOrderDetail> GetDyeingJobOrderDetails(long dyeingJobOrderId)
        {
            return _dyeingJobOrderRepository.GetDyeingJobOrderDetails(dyeingJobOrderId);
        }

        public int EditDyeingJobOrder(PROD_DyeingJobOrder model)
        {
            int edited = 0;
            using (var transaction=new TransactionScope())
            {
                _dyeingJobOrderDetailRepository.Delete(x => x.DyeingJobOrderId == model.DyeingJobOrderId);
                var jobOrder = _dyeingJobOrderRepository.FindOne(x => x.DyeingJobOrderId == model.DyeingJobOrderId);
                jobOrder.JobDate = model.JobDate;
                jobOrder.DeliveryDate = model.DeliveryDate;
                jobOrder.JobType = model.JobType;
                jobOrder.WorkOrderNo = model.WorkOrderNo;
                jobOrder.BuyerRefId = model.BuyerRefId;
                jobOrder.BuyerName = model.BuyerName;
                jobOrder.OrderNo = model.OrderNo;
                jobOrder.OrderName = model.OrderName;
                jobOrder.OrderStyleRefId = model.OrderStyleRefId;
                jobOrder.StyleName = model.StyleName;
                jobOrder.ProcessRefId = model.ProcessRefId;
                jobOrder.Remarks = model.Remarks;
                edited=  _dyeingJobOrderRepository.Edit(jobOrder);
                foreach (PROD_DyeingJobOrderDetail jobOrderDetail in model.PROD_DyeingJobOrderDetail)
                {
                    jobOrderDetail.DyeingJobOrderId = jobOrder.DyeingJobOrderId;
                    _dyeingJobOrderDetailRepository.Save(jobOrderDetail);
                }
               transaction.Complete();
            }
            return edited;

        }

        public int DeleteDyeingJobOrderDetail(long dyeingJobOrderId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                _dyeingJobOrderDetailRepository.Delete(x => x.DyeingJobOrderId == dyeingJobOrderId);
                _dyeingJobOrderRepository.Delete(x => x.DyeingJobOrderId == dyeingJobOrderId);
              
                transaction.Complete();
            }
            return deleted;
        }

        public DataTable DyeingJobOrderReportDataTable(long dyeingJobOrderId)
        {
            string cmdText = String.Format("exec SpProdDyeingJobOrderReort {0}", dyeingJobOrderId);
            return _dyeingJobOrderRepository.ExecuteQuery(cmdText);
        }

        public List<Dropdown> GetDyeingJobOrderByPartyId(long partyId)
        {
            return _dyeingJobOrderRepository.GetDyeingJobOrderByPartyId(partyId);
        }

        public List<VwDyeingJobOrderDetail> LoadKnittingRollIssueChallan(string challanRefId)
        {
            string sp = string.Format("exec knittingRollissueChallan @RefId='{0}'", challanRefId);
            DataTable dt=  _dyeingJobOrderRepository.ExecuteQuery(sp);
            List<VwDyeingJobOrderDetail> jobOrderDetails = new List<VwDyeingJobOrderDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VwDyeingJobOrderDetail jobOrd = new VwDyeingJobOrderDetail();
                jobOrd.ItemId = Convert.ToInt32(dt.Rows[i]["ItemId"]);
                jobOrd.ItemName = dt.Rows[i]["ItemName"].ToString();
                jobOrd.Gsm = Convert.ToDouble(dt.Rows[i]["GSM"]);
                jobOrd.ComponentRefId = dt.Rows[i]["ComponentRefId"].ToString();
                jobOrd.ComponentName = dt.Rows[i]["ComponentName"].ToString();
                jobOrd.FdSizeRefId = dt.Rows[i]["FinishSizeRefId"].ToString();
                jobOrd.FdName = dt.Rows[i]["FinishSizeName"].ToString();
                jobOrd.ColorRefId = dt.Rows[i]["ColorRefId"].ToString();
                jobOrd.ColorName = dt.Rows[i]["ColorName"].ToString();
                jobOrd.ColorRefId = dt.Rows[i]["ColorRefId"].ToString();
                jobOrd.MdSizeRefId = dt.Rows[i]["SizeRefId"].ToString();
                jobOrd.MdName = dt.Rows[i]["SizeName"].ToString();
                jobOrd.Remarks = dt.Rows[i]["Remarks"].ToString();
                jobOrd.Quantity = Convert.ToDouble(dt.Rows[i]["Qty"]);
                jobOrd.GreyWit = Convert.ToDouble(dt.Rows[i]["Qty"]);
                jobOrd.ComponentRefId = dt.Rows[i]["ComponentRefId"].ToString()??"000";
                jobOrd.ComponentName = dt.Rows[i]["ComponentName"].ToString()??"--";
                jobOrderDetails.Add(jobOrd);
            }
            return jobOrderDetails;
        }

        public List<Dropdown> GetKnittingRollIssueChallan(string orderStyleRefId)
        {
            return _dyeingJobOrderRepository.GetKnittingRollIssueChallan(orderStyleRefId);
        }
    }
}
