using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class KnittingRollManager : IKnittingRollManager
    {
        private readonly IKnittingRollRepository _knittingRollRepository;
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IProgramRepository _programRepository;
        public KnittingRollManager(IProgramRepository programManager, ITimeAndActionManager timeAndActionManager, IKnittingRollRepository knittingRollRepository)
        {
            _knittingRollRepository = knittingRollRepository;
            _timeAndActionManager = timeAndActionManager;
            _programRepository = programManager;
        }

        public List<VwKnittingRoll> GetKnittingRollsByPaging(string searchString, int pageIndex, string sort, string sortdir, DateTime? fromDate,
            DateTime? toDate, long partyId, out int totalRecord)
        {
            IQueryable<VwKnittingRoll> knittingRolls = _knittingRollRepository.GetKnittingRolls(x => x.CompId == PortalContext.CurrentUser.CompId &&

                    (x.ProgramRefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString.Trim().ToLower())
                    && (x.CharllRollNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString.Trim().ToLower()))

                    && ((x.RollDate >= fromDate || fromDate == null) && (x.RollDate <= toDate || toDate == null))));

            var pageSize = AppConfig.PageSize;

            totalRecord = knittingRolls.Count();
            knittingRolls = knittingRolls.OrderByDescending(
                    x => x.RollRefNo)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return knittingRolls.ToList();
        }

        public int SaveKnittingRoll(PROD_KnittingRoll knittingRoll)
        {
            int saved = _knittingRollRepository.Save(knittingRoll);
            if (saved > 0)
            {
                PLAN_Program program = _programRepository.FindOne(x => x.ProgramId == knittingRoll.ProgramId);
                if (program != null)
                {
                    _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.Bulk_Knitting_Solid_Fabric, knittingRoll.RollDate, program.OrderStyleRefId, knittingRoll.CompId);
                }
            }
            return saved;
        }

        public int EditKnittingRoll(PROD_KnittingRoll knittingRoll)
        {

            return _knittingRollRepository.Edit(knittingRoll);
        }

        public int DeleteKnittingRollById(long knittingRollId)
        {
            return _knittingRollRepository.Delete(x => x.KnittingRollId == knittingRollId);
        }

        public PROD_KnittingRoll GetKnittingRollById(long knittingRollId)
        {
            return _knittingRollRepository.FindOne(x => x.KnittingRollId == knittingRollId);
        }

        public string GetTodayRollNo(string compId, string prefix)
        {
            string rollNo = "";
            var knittingRoll = _knittingRollRepository.Filter(x => x.RollRefNo.Substring(0, 1) == prefix);

            rollNo = knittingRoll.Max(x => x.RollRefNo.Substring(1, 7));
            rollNo = prefix + rollNo.IncrementOne().PadZero(6);
            return rollNo;
        }

        public VwKnittingRoll GetVwKnittingRollById(long knittingRollId)
        {
            return _knittingRollRepository.GetKnittingRollById(knittingRollId);
        }

        public List<VwKnittingRoll> GetDailyKnittingRollByDate(DateTime fromDate, string compId)
        {
            DateTime toDate = fromDate.AddDays(1);
            IQueryable<VwKnittingRoll> knittingRolls = _knittingRollRepository.GetKnittingRolls(x => x.CompId == compId && x.RollDate == fromDate)
                   .OrderByDescending(x => x.CharllRollNo);

            return knittingRolls.ToList();
        }

        public DataTable MachineWiseKnitting(DateTime? fromDate, string kType, string compId)
        {
            return _knittingRollRepository.MachineWiseKnitting(fromDate, kType, compId);
        }

        public DataTable GetDailyKnittingRollSummaryByDate(DateTime dateTime, string compId)
        {
            return _knittingRollRepository.GetDailyKnittingRollSummaryByDate(dateTime, compId);
        }

        public DataTable GetKnittingReceiveBalance(string compId, string processRefId, string orderStyleRefId)
        {
            string sqlQuery = String.Format("exec SpProdKnittingReceiveBalance '{0}','{1}','{2}'", processRefId, orderStyleRefId, compId);
            return _knittingRollRepository.ExecuteQuery(sqlQuery);
        }

        public List<VwKnittingRoll> AutocompliteKnittingRoll(string orderStyleRefId, string compId)
        {
            return _knittingRollRepository.AutocompliteKnittingRoll(orderStyleRefId, compId);
        }

        public List<VwKnittingRoll> GetKnittingRollsByOrderStyleRefId(string orderStyleRefId)
        {
            IQueryable<VwKnittingRoll> knittingRolls = _knittingRollRepository.GetKnittingRolls(x => x.CompId == PortalContext.CurrentUser.CompId && x.OrderStyleRefId == orderStyleRefId);
            return knittingRolls.ToList();

        }

        public List<VwKnittingRoll> GetKnittingRollStatus(DateTime? fromDate, DateTime? toDate, string currentUserCompId)
        {
            IQueryable<VwKnittingRoll> knittingRolls = _knittingRollRepository.GetKnittingRolls(x => x.CompId == PortalContext.CurrentUser.CompId && ((x.RollDate >= fromDate || fromDate == null) && (x.RollDate <= toDate || toDate == null)));

            return knittingRolls.ToList();
        }

        public int SaveBullRolls(List<PROD_KnittingRoll> knittingRolls)
        {
            int saved = 0;
            const string prefix_knitting = "R";
            using (var transaction = new TransactionScope())
            {
                foreach (var knittingRoll in knittingRolls)
                {
                    knittingRoll.RollRefNo = GetTodayRollNo(knittingRoll.CompId, prefix_knitting);
                    saved = _knittingRollRepository.Save(knittingRoll);
                }
                transaction.Complete();
            }
            return saved;
        }

        public DataTable GetRollSticker(long knittingRollId, string compId)
        {
            string sqlQuery = String.Format("exec spGetRollSticker '{0}','{1}'", knittingRollId, compId);
            return _knittingRollRepository.ExecuteQuery(sqlQuery);
        }

        public DataTable GetKnittingRollsSummaryByOrderStyleRefId(string orderStyleRefId)
        {

            string sql = @"select 
                        ISNULL((select top(1) ItemName from Inventory_Item where ItemCode=KR.ItemCode),'Total :') AS Fabric,
                        (select ColorName from OM_Color where ColorRefId=KR.ColorRefId and CompId=KR.CompId) AS Color,
                        (select SizeName from OM_Size where SizeRefId=KR.SizeRefId and CompId=KR.CompId) AS McDia,
                        (select SizeName from OM_Size where SizeRefId=KR.FinishSizeRefId and CompId=KR.CompId) AS FDia,
                        KR.GSM
                        ,SUM(KR.Quantity) AS Quantity 
                        from PROD_KnittingRoll AS KR
                        INNER JOIN PLAN_Program AS PG ON KR.ProgramId=PG.ProgramId
                        where PG.OrderStyleRefId='{0}'
                        GROUP BY GROUPING SETS ((KR.ItemCode,KR.GSM,KR.CompId,KR.SizeRefId,KR.ColorRefId,KR.FinishSizeRefId),())";
            sql = string.Format(sql, orderStyleRefId);
            return _knittingRollRepository.ExecuteQuery(sql);
        }

        public List<VwKnittingRoll> GeKnittedRolls(string programRefId, string charllRollNo)
        {
            IQueryable<VwKnittingRoll> knittingRolls = _knittingRollRepository.GetKnittingRolls(x => x.CompId == PortalContext.CurrentUser.CompId &&
            (x.ProgramRefId.Trim().Contains(programRefId) && (x.CharllRollNo.Trim().Contains(charllRollNo) || String.IsNullOrEmpty(charllRollNo.Trim().ToLower()))));
            return knittingRolls.ToList();
        }

        public int CheckedRejectedRoll(int knittingRoleId)
        {
            PROD_KnittingRoll roll = _knittingRollRepository.FindOne(x => x.KnittingRollId == knittingRoleId);
            if (roll.IsRejected.GetValueOrDefault())
            {
                roll.RejQuantity = 0;
                roll.IsRejected = false;
            }
            else
            {
                roll.RejQuantity = roll.Quantity;
                roll.IsRejected = true;
            }
            return _knittingRollRepository.Edit(roll);
        }
    }
}
