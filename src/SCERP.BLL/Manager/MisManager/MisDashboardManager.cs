using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IMisManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IMisRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.MisManager
{
    public class MisDashboardManager : IMisDashboardManager
    {
        private IMisDashboardRepository _misDashboardRepository;
        private IBuyerOrderRepository _buyerOrderRepository;
        private IKnittingRollRepository _knittingRollRepository;
        public MisDashboardManager(IKnittingRollRepository knittingRollRepository,IMisDashboardRepository misDashboardRepository, IBuyerOrderRepository buyerOrderRepository)
        {
            _misDashboardRepository = misDashboardRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _knittingRollRepository = knittingRollRepository;
        }

        public DataTable GetMerchadiserWiseOrderStyleDtable()
        {
            DataTable dataTable= _misDashboardRepository.GetMerchadiserWiseOrderStyleDtable();
            var toInsert = dataTable.NewRow();
            toInsert[0] = "Total:";
            for (var i = 1; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("sum([" + dataTable.Columns[i].ColumnName + "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);
            return dataTable;
        }

        public DataTable GetBuyerWiseOrderStyleDtable()
        {
             DataTable dataTable= _misDashboardRepository.GetBuyerWiseOrderStyleDtable();
            var toInsert = dataTable.NewRow();
            toInsert[0] = "Total:";
            for (var i = 1; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("sum([" + dataTable.Columns[i].ColumnName + "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);

          

            return dataTable;
        }

        public DataTable GetOrderStatusSummaryDtable()
        {
            DataTable dataTable = _misDashboardRepository.GetOrderStatusSummaryDtable();
            return dataTable;
        }

        public DataTable GetSpMISReprotDashBoard()
        {
            DataTable dataTable = _misDashboardRepository.GetSpMISReprotDashBoard(PortalContext.CurrentUser.CompId);
            return dataTable;
        }

        public DataTable GetOrderStatusWithValue()
        {
            DataTable dataTable = _misDashboardRepository.GetOrderStatusWithValue(PortalContext.CurrentUser.CompId);
            return dataTable;
        }

        public DataTable RunningOrderSummary(string month, string buyer, string compId)
        {
            string sqlQuery = String.Format(@"exec SpRunningOrderSummary '{0}','{1}','{2}'",month.Split('-')[0],month.Split('-')[1], buyer);
            return _buyerOrderRepository.ExecuteQuery(sqlQuery);
        }

        public DataTable MachineWiseKnitting(DateTime? rolldate, string kType, string compId)
        {
           return _knittingRollRepository.MachineWiseKnitting(rolldate, kType, compId);
        }

        public DataTable GetMonthlyOrderStatus(string compId)
        {
            string sqlQuery = String.Format(@"exec SpMisQtyValue '{0}'", compId);
            DataTable dataTable= _buyerOrderRepository.ExecuteQuery(sqlQuery);
            var toInsert = dataTable.NewRow();
            toInsert[0] = "Total:";
            for (var i = 1; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("SUM([" +dataTable.Columns[i].ColumnName+ "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);
            return dataTable;
        }

        public DataTable GetSpBuyerOrderMasterDashBoard(string buyerRefId, string orderNo, string orderStyleRefId)
        {
            DataTable dataTable = _misDashboardRepository.GetBuyerOrderMaster(buyerRefId, orderNo, orderStyleRefId);
            return dataTable;
        }

        public DataTable GetLineWiseSewingProduction(int currentYear, string compId)
        {
            DataTable dataTable= _misDashboardRepository.GetLineWiseSewingProduction(currentYear, compId);
            var toInsert = dataTable.NewRow();
            toInsert[1] = "Total:";
            for (var i = 2; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("SUM([" +dataTable.Columns[i].ColumnName+ "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);
         
            return dataTable;
        }

        public DataTable StyleSewingProduction(int currentYear, string currentMonth, int lineId, string compId)
        {
            DataTable dataTable = _misDashboardRepository.StyleSewingProduction(currentYear,currentMonth,lineId, compId);
          
            return dataTable;
        }

        public DataTable StyleSewingDetailProduction(int currentYear, string currentMonth, int lineId, string compId)
        {
            DataTable dataTable = _misDashboardRepository.StyleSewingDetailProduction(currentYear, currentMonth, lineId, compId);

            return dataTable;
        }

        public DataTable GetLineWiseDailySewingProduction(int currentYear, string currentMonth, string compId)
        {
            DataTable dataTable = _misDashboardRepository.GetLineWiseDailySewingProduction(currentYear, currentMonth,  compId);
            var toInsert = dataTable.NewRow();
            toInsert[0] = "Total:";
            for (var i = 1; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("SUM([" + dataTable.Columns[i].ColumnName + "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);
            return dataTable;
        }

        public DataTable CommarcialExportImport()
        {
            DataTable dataTable = _misDashboardRepository.CommarcialExportImport(PortalContext.CurrentUser.CompId);
            return dataTable;
        }

        public SpMisSewingProductionBoard GetSewingProductionBoard(DateTime currentDate, string compId)
        {
            SpMisSewingProductionBoard sewingProductionBoard = _misDashboardRepository.GetSewingProductionBoard(currentDate, compId);
            if (sewingProductionBoard!=null)
            {
                sewingProductionBoard.Variance = sewingProductionBoard.TargetQty - sewingProductionBoard.ActualQty;
                //sewingProductionBoard.Variance = sewingProductionBoard.TargetQty - sewingProductionBoard.ActualQty;
                if (sewingProductionBoard.TargetQty > 0)
                {
                    sewingProductionBoard.Efficiency =
                  (sewingProductionBoard.ActualQty * 100) / sewingProductionBoard.TargetQty;
                }
            }
            else
            {
                sewingProductionBoard=new SpMisSewingProductionBoard();
            }
         
          
            return sewingProductionBoard;
        }

        public DataTable GetMontlyBuyerWiseProductionPlan()
        {
            DataTable dataTable = _misDashboardRepository.GetMontlyBuyerWiseProductionPlan();
            var toInsert = dataTable.NewRow();
            toInsert[0] = "Total:";
            for (var i = 1; i < dataTable.Columns.Count; i++)
            {
                toInsert[i] = dataTable.Compute("sum([" + dataTable.Columns[i].ColumnName + "])", "").ToString();
            }
            dataTable.Rows.InsertAt(toInsert, dataTable.Rows.Count + 1);



            return dataTable;
        }

        public DataTable MontlySewingPlanDetail(string month, string buyer, string compId)
        {
            string sqlQuery = String.Format(@"select BuyerName as BUYER,OrderName as [ORDER],StyleName as STYLE,Sum(TotalTargetQty) as QUANTITY from VwTargetProduction 
                              where BuyerName='{0}' and RIGHT(YEAR(StartDate), 2) ='{1}' and FORMAT(StartDate,'MMM')='{2}'
                              group by BuyerName,OrderName,StyleName",buyer,  month.Split('-')[1],month.Split('-')[0]);
            return _buyerOrderRepository.ExecuteQuery(sqlQuery);
        }

        public List<dynamic> GetDailyTargetAchivemnet(DateTime currentDate, string compId)
        {
          DataTable tvA= _buyerOrderRepository.ExecuteQuery(String.Format("EXEC spMisDailyTagetVsAchivemnet '{0}','{1}'", compId,  currentDate));
          return tvA.Todynamic().ToList(); ;
        }

        public List<dynamic> GetHourlyTargetAchivemnet(DateTime currentDate, string compId)
        {
            DataTable htvA = _buyerOrderRepository.ExecuteQuery(String.Format("select HourName,ISNULL(QAQty,0) as QAQty,ISNULL(QTQty,0) as QTQty from MIS_HourlyAchievement where CompId='{0}' and HourId <= 15", compId));
            return htvA.Todynamic().ToList();
        }

        public DataTable GetTnaSweingPandingStatus()
        {
            DataTable dataTable = _buyerOrderRepository.ExecuteQuery(String.Format("spTnaSweingPandingStatus"));
            return dataTable;
        }

        public DataTable GetTnaSweingUpcomingStatus()
        {
            DataTable dataTable = _buyerOrderRepository.ExecuteQuery(String.Format("spTnaUpcomingSweingStatus"));
            return dataTable;
        }

        public DataTable GetShipmentRatio(string orderStyleRefId)
        {
            DataTable dataTable = _buyerOrderRepository.ExecuteQuery(String.Format("Exec spShipRatio @OrderStyleRefId='{0}'", orderStyleRefId));
            return dataTable;
        }
    }
}
