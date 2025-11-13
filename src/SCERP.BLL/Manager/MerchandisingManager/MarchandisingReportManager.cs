using System;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class MarchandisingReportManager : IMarchandisingReportManager
    {
        private readonly IMarchandisingReportRepository _reportRepository;
        private readonly string _companyId;
        public MarchandisingReportManager(IMarchandisingReportRepository marchandisingReport)
        {
            _companyId = PortalContext.CurrentUser.CompId;
            _reportRepository = marchandisingReport;
        }

        public DataTable GetBuyerWiseOrderSummaryDataTable(DateTime? fromDate, DateTime? toDate, string buyerRefId)
        {
            return _reportRepository.GetBuyerWiseOrderSummaryDataTable(fromDate, toDate, buyerRefId, _companyId);
        }

        public DataTable GetConfirmedOrderStatus()
        {
            return _reportRepository.GetConfirmedOrderStatus(_companyId);
        }

        public DataTable GetDetailOrderStatus()
        {
            return _reportRepository.GetDetailOrderStatus(_companyId);
        }

        public DataTable GetProductionStatus(DateTime? fromDate, DateTime? toDate)
        {
            return _reportRepository.GetProductionStatus(_companyId, fromDate,toDate);
        }

        public DataTable GetShipmentStatus(string seasonRefId, string merchandiserId, string buyerRefId)
        {
            return _reportRepository.GetShipmentStatus(seasonRefId, merchandiserId, buyerRefId, _companyId);
        }

        public DataTable GetPandingConsumptionDataTable(string merchandiserId)
        {
            return _reportRepository.GetPandingConsumptionDataTable(merchandiserId, _companyId);
        }

        public DataTable GetPoSheet(long purchaseOrderId)
        {
            DataTable dt= _reportRepository.GetPoSheet(purchaseOrderId, _companyId);
            decimal sumTotal = dt.AsEnumerable()
               .Sum(r => r.Field<decimal>("Quantity") * r.Field<decimal>("xRate"));
            string inWord= _reportRepository.GetInWord(sumTotal);
            System.Data.DataColumn newColumn = new System.Data.DataColumn("InWord", typeof(System.String));
            newColumn.DefaultValue = inWord;
            dt.Columns.Add(newColumn);
            return dt;
        }

        public DataTable GetFabricWorkOrderSheet(string orderStyleRefId)
        {
            
            return _reportRepository.GetFabricWorkOrderSheet(orderStyleRefId, _companyId);
        }

        public DataTable GetFabricWorkOrderDetailSheet(string orderStyleRefId)
        {
            return _reportRepository.GetFabricWorkOrderDetailSheet(orderStyleRefId, _companyId);
        }

        public DataTable GetBulkFabricOrderSheet(int fabricOrderId)
        {
            return _reportRepository.GetBulkFabricOrderSheet(fabricOrderId, _companyId);
        }

        public DataTable GetRunningOrderStatus()
        {
            return _reportRepository.GetRunningOrderStatus( _companyId);
        }

        public DataTable GetSeasonWiseOrderSummary(DateTime? fromDate, DateTime? todate)
        {
            return _reportRepository.GetSeasonWiseOrderSummary(fromDate,todate, _companyId);
        }

        public int SendMailExecut()
        {
            return _reportRepository.SendMailExecut();
        }

        public DataTable GetYarnWorkOrderSheet(long purchaseOrderId)
        {
            return _reportRepository.GetYarnWorkOrderSheet(purchaseOrderId, _companyId);
        }

        public DataTable GetBulkYarnBooking(long purchaseOrderId)
        {
            return _reportRepository.GetBulkYarnBooking(purchaseOrderId, _companyId);
        }

        public DataTable GetOrderShipmentSummary(DateTime? fromDate, DateTime? toDate, Guid? userId)
        {
            return _reportRepository.GetOrderShipmentSummary(fromDate, toDate, userId, _companyId);
        }

        public DataTable GetRunningOrderOrderStatus(string compId)
        {
            return _reportRepository.GetRunningOrderOrderStatus(compId);
        }

        public DataTable GetPreCostSheet(string orderStyleRefId,string compId)
        {
            return _reportRepository.GetPreCostSheet(orderStyleRefId,compId);
        }

        public DataTable GetStyleWiseProduction(string orderStyleRefId, string compId)
        {
            return _reportRepository.GetStyleWiseProduction(orderStyleRefId, compId);
        }

        public DataTable GetCollarCuffBulkFabricOrderSheet(int fabricOrderId)
        {
            return _reportRepository.GetCollarCuffBulkFabricOrderSheet(fabricOrderId, _companyId);
        }

        public DataTable GetShipmentAlert(string buyerRefId, string orderNo, string orderStyleRefId, string compId)
        {
            return _reportRepository.GetShipmentAlert(buyerRefId, orderNo, orderStyleRefId, compId);
        }
    }
}
