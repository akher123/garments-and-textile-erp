using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IBatchManager
    {
        List<VProBatch> GetBachListByPaging(VProBatch model, out int totalRecords);
        int EditBatch(Pro_Batch model);
        int SaveBatch(Pro_Batch model);
        VProBatch GetBachById(long batchId);
        VProBatch GetBachByRefNo(string btRefNo);
        bool IsBatchExist(Pro_Batch model);
        int SaveInActiveBatch(int batchId);
        string GetBachNewRefNo( string prefix);
        int DeleteBatch(long batchId, string compId);
        int UpdateBatchStatus(Pro_Batch model);
        List<VProBatch> GetBachStatus(DateTime? fromDate, DateTime? toDate, long partyId, int machineId);
        object BatchAutoComplite(string compId, string searchString);
        List<Pro_Batch> GetAllBatch(string compId);
        IEnumerable GetBachByBatchId(long batchId, string compId);

        double GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId);
        IEnumerable BatchAutoCompliteByParty(string searchString, long partyId);
        List<Pro_Batch> GetBachList(string searchString,int? btType,int btStatus, int pageIndex, out int totalRecords);
        DataTable GetBachQtyByStyle(string orderStyleRefId);
    }
}
