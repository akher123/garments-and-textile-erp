using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IKnittingRollManager
    {
       List<VwKnittingRoll> GetKnittingRollsByPaging
           (string searchString, int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, long partyId, out int outtotalRecord);

       int SaveKnittingRoll(PROD_KnittingRoll knittingRoll);
       int EditKnittingRoll(PROD_KnittingRoll knittingRoll);
       int DeleteKnittingRollById(long knittingProcessId);
       PROD_KnittingRoll GetKnittingRollById(long knittingRollId);
       string GetTodayRollNo(string compId,string prefix);
        VwKnittingRoll GetVwKnittingRollById(long knittingRollId);
       List<VwKnittingRoll> GetDailyKnittingRollByDate(DateTime fromDate, string compId);
       DataTable MachineWiseKnitting(DateTime? fromDate, string searchString, string compId);
       DataTable GetDailyKnittingRollSummaryByDate(DateTime dateTime, string compId);
       DataTable GetKnittingReceiveBalance(string compId,string processRefId, string orderStyleRefId);
       List<VwKnittingRoll> AutocompliteKnittingRoll( string orderStyleRefId, string compId);
       List<VwKnittingRoll> GetKnittingRollsByOrderStyleRefId(string orderStyleRefId);

        List<VwKnittingRoll> GetKnittingRollStatus(DateTime? modelFromDate, DateTime? modelToDate, string currentUserCompId);
        int SaveBullRolls(List<PROD_KnittingRoll> knittingRolls);
       DataTable GetRollSticker(long knittingRollId, string compId);
        DataTable GetKnittingRollsSummaryByOrderStyleRefId(string orderStyleRefId);
        List<VwKnittingRoll> GeKnittedRolls(string programRefId, string charllRollNo);
        int CheckedRejectedRoll(int knittingRoleId);
    }
}
