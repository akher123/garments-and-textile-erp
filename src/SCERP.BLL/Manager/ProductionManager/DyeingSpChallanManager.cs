using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
   public class DyeingSpChallanManager: IDyeingSpChallanManager
   {
       private readonly IDyeingSpChallanRepository _dyeingSpChallanRepository;
       private readonly IDyeingSpChallanDetailRepository _dyeingSpChallanDetailRepository;

       public DyeingSpChallanManager(IDyeingSpChallanRepository dyeingSpChallanRepository, IDyeingSpChallanDetailRepository dyeingSpChallanDetailRepository)
       {
           _dyeingSpChallanRepository = dyeingSpChallanRepository;
           _dyeingSpChallanDetailRepository = dyeingSpChallanDetailRepository;
       }

       public List<PROD_DyeingSpChallan> GetAllDyeingSpChallanByPaging(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string searchString, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var dyeingSpChallanList = _dyeingSpChallanRepository.GetWithInclude(x => x.CompId == compId && (x.DyeingSpChallanRefId.Contains(searchString) || String.IsNullOrEmpty(searchString) && ((x.ChallanDate >= fromDate || fromDate == null) && (x.ChallanDate <= toDate.Value || toDate==null))), "Party");
            totalRecords = dyeingSpChallanList.Count();
            switch (sort)
            {
                case "challanNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            dyeingSpChallanList = dyeingSpChallanList
                                .OrderByDescending(r => r.DyeingSpChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            dyeingSpChallanList = dyeingSpChallanList
                                .OrderBy(r => r.DyeingSpChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    dyeingSpChallanList = dyeingSpChallanList
                        .OrderByDescending(r => r.DyeingSpChallanId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return dyeingSpChallanList.ToList();
        }

       public PROD_DyeingSpChallan GetDyeingSpChallanByDyeingSpChallanId(long dyeingSpChallanId, string compId)
       {
           return _dyeingSpChallanRepository.FindOne(x => x.CompId == compId && x.DyeingSpChallanId == dyeingSpChallanId);
       }

       public string GetDyeingSpChallanRefId()
       {
            var dyeingSpChallanRefId = _dyeingSpChallanRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).Max(x => x.DyeingSpChallanRefId.Substring(2)) ?? "0";
            return "SP" + dyeingSpChallanRefId.IncrementOne().PadZero(6);
        }

       public int SaveDyeingSpChallan(PROD_DyeingSpChallan model)
       {
            model.IsApproved = false;
            int save= _dyeingSpChallanRepository.Save(model);
           model = new PROD_DyeingSpChallan() {PROD_DyeingSpChallanDetail = new List<PROD_DyeingSpChallanDetail>()};
           return save;
       }
       public int EditDyeingSpChallan(PROD_DyeingSpChallan model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                _dyeingSpChallanDetailRepository.Delete(x => x.DyeingSpChallanId == model.DyeingSpChallanId);
                var dyeingSpChallan = _dyeingSpChallanRepository.FindOne(x => x.DyeingSpChallanId == model.DyeingSpChallanId);
                dyeingSpChallan.ChallanNo = model.ChallanNo;
                dyeingSpChallan.ChallanDate = model.ChallanDate;
                dyeingSpChallan.ExpDate = model.ExpDate;
                dyeingSpChallan.ParyId = model.ParyId;
                dyeingSpChallan.Remarks = model.Remarks;
                edited = _dyeingSpChallanRepository.Edit(dyeingSpChallan);
                var detail = model.PROD_DyeingSpChallanDetail.Select(x =>
                {
                    x.DyeingSpChallanId = dyeingSpChallan.DyeingSpChallanId;
                    return x;
                });
                edited += _dyeingSpChallanDetailRepository.SaveList(detail.ToList());
                transaction.Complete();
            }
            return edited;
        }
        public int DeleteDyeingSpChallan(long dyeingSpChallanId, string compId)
        {
            int deletedRow = 0;
            using (var transaction = new TransactionScope())
            {
                deletedRow += _dyeingSpChallanDetailRepository.Delete(x => x.CompId == compId && x.DyeingSpChallanId == dyeingSpChallanId);
                deletedRow += _dyeingSpChallanRepository.Delete(x => x.CompId == compId && x.DyeingSpChallanId == dyeingSpChallanId);
                transaction.Complete();
            }
            return deletedRow;
        }

       public int UpdateApprovedSp(long dyeingSpChallanId)
       {
          var dayeingSp= _dyeingSpChallanRepository.FindOne(x => x.DyeingSpChallanId == dyeingSpChallanId);
           dayeingSp.IsApproved = !dayeingSp.IsApproved;
           dayeingSp.ApprovedBy = dayeingSp.IsApproved ? PortalContext.CurrentUser.UserId : null;
           return _dyeingSpChallanRepository.Edit(dayeingSp);
       }

       public List<PROD_DyeingSpChallan> GetAllDyeingSpChallanList(string compId, bool isApproved)
       {
           var dyeingSpChallanList = _dyeingSpChallanRepository.GetWithInclude(x => x.CompId == compId && x.IsApproved == isApproved, "Party").OrderByDescending(x=>x.DyeingSpChallanRefId);
           return dyeingSpChallanList.ToList();

       }

       public IEnumerable DyeingSpChallanAutocomplite(string searchString, string compId)
       {
           return
               _dyeingSpChallanRepository.Filter(
                   x => x.CompId == compId && x.DyeingSpChallanRefId.Contains(searchString)).Take(20);
       }

       public IEnumerable<dynamic> GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId)
       {
           return _dyeingSpChallanDetailRepository.GetBatchItemQtyByBatchDetailId(batchDetailId, compId);
       }

        public DataTable GetDyeingSpChallanByStyle(string orderStyleRefId)
        {
            string sql = @"select 
                            ISNULL((select top(1) ItemName from Inventory_Item where ItemId=BD.ItemId),'Total :') AS Fabric,
                            (select ColorName from OM_Color where ColorRefId=BT.GrColorRefId and CompId=BT.CompId) AS Color,
                            (select SizeName from OM_Size where SizeRefId=BD.MdiaSizeRefId and CompId=BT.CompId) AS McDia,
                            (select SizeName from OM_Size where SizeRefId=BD.FdiaSizeRefId and CompId=BT.CompId) AS FDia,
                            BD.GSM
                            ,SUM(SPD.GreyWeight) AS GQuantity 
                            ,SUM(SPD.FinishWeight) AS FQuantity 
                            ,CAST(ROUND((SUM(SPD.GreyWeight)-SUM(SPD.FinishWeight))*100/SUM(SPD.GreyWeight),2) AS varchar(20))+' %' AS PLQuantity 
                            from PROD_DyeingSpChallan AS SP
                            INNER JOIN PROD_DyeingSpChallanDetail AS SPD ON SP.DyeingSpChallanId=SPD.DyeingSpChallanId
                            INNER JOIN PROD_BatchDetail AS BD ON SPD.BatchDetailId=BD.BatchDetailId 
                            INNER JOIN Pro_Batch AS BT ON BD.BatchId=BT.BatchId
                            where BT.OrderStyleRefId='{0}'
                            GROUP BY GROUPING SETS ((BD.ItemId,BD.GSM,BT.CompId,BT.GrColorRefId,BD.MdiaSizeRefId,BD.FdiaSizeRefId),()) ";
            sql = string.Format(sql, orderStyleRefId);
            return _dyeingSpChallanDetailRepository.ExecuteQuery(sql);
        }
    }
}
