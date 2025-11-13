using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttingBatchManager
    {
       string GetNewCuttingBatchRefId();
       PROD_CuttingBatch GetCuttingBatchByCuttingBatchId(string cuttingBatchRefId);
       bool IsCuttingBatchExist(PROD_CuttingBatch model);
       int EditCuttingBatch(PROD_CuttingBatch model);
       int SaveCuttingBatch(PROD_CuttingBatch model);
       Dictionary<string, List<string>> GetCuttingJobCards(string orederStyleRefId, string colorRefId, string componentRefId,string orderShipRefId);
       int GenerateBundleChat(long cuttingBatchId);
       int DeleteCuttingBatchByCuttingBatchId(long cuttingBatchId);


       List<VwCuttingBatch> GetAllCuttingBatchList(string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId, string componentRefId,string styleRefId);
       PROD_CuttingBatch GetCuttingBatchById(long cuttingBatchId);
       List<PROD_CuttingBatch> GetJobNoByComponentRefId(string colorRefId, string componentRefId, string orderStyleRefId,string orderShipRefId=null);

  

       int SaveApprovalStatus(long cuttingBatchId);

       bool IsOrderStyleRefIdExist(string orderStyleRefId);

       List<VwCuttingApproval> GetCuttingApproval
           (string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId, string componentRefId, string approvalStatus);

       List<VwCuttingBatch> GetAllCuttingBatchListByPaging(DateTime? cuttingDate,int?matchineId, string searchString, int pageIndex, out int totalRecords, out int totalBody);


       List<VwCuttingBatch> GetCuttingBatchList(string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId, string componentRefId, string cuttingBatchRefId,long cuttingBatchId);

       List<VwCuttingBatch> GetAllCuttingBatchForReport(DateTime? cuttingDate, string searchString,int ? matchineId);
       DataTable GetDailyMonthWiseCutting(int yearId, string compId);
    }
}
