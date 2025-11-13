using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Xml.Serialization;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using SCERP.Model;
using System.Data;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class ReportRepository : Repository<object>, IReportRepository
    {
  
        private readonly string _companyId;

        public ReportRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public List<COMMLcInfo> GetLcInfos(COMMLcInfo lcInfo)
        {
            long? buyerId = lcInfo.BuyerId ?? 0;
            int? lcType = lcInfo.LcType ?? 0;
            DateTime? fromDate = lcInfo.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = lcInfo.ToDate ?? new DateTime(2000, 01, 01);
            string lcBank = lcInfo.LcIssuingBank ?? "";
            string receiveBank = lcInfo.ReceivingBank ?? "";
            string lcNo = lcInfo.LcNo ?? "";

            List<COMMLcInfo> lcInfos = Context.Database.SqlQuery<COMMLcInfo>("SPCommGetLcInfo @CompanyId, @BuyerId, @LcType, @FromDate, @ToDate, @LcBank, @ReceiveBank, @LCNO", new SqlParameter("CompanyId", _companyId), new SqlParameter("BuyerId", buyerId), new SqlParameter("LcType", lcType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("LcBank", lcBank), new SqlParameter("ReceiveBank", receiveBank), new SqlParameter("LCNO", lcNo)).ToList();
            return lcInfos.ToList();
        }

        public List<COMMLcInfo> GetLcIndividual(int lcId)
        {
            List<COMMLcInfo> individual = Context.Database.SqlQuery<COMMLcInfo>("SPCommLcInfoIndividualReport @CompanyId, @LcId", new SqlParameter("CompanyId", _companyId), new SqlParameter("LcId", lcId)).ToList();
            return individual.ToList();
        }

        public List<BbLcIndividualReport> GetBbLcIndividual(int bbLcId)
        {
            List<BbLcIndividualReport> individual = Context.Database.SqlQuery<BbLcIndividualReport>("SPCommBbLcInfoIndividualReport @CompanyId, @BbLcId", new SqlParameter("CompanyId", _companyId), new SqlParameter("BbLcId", bbLcId)).ToList();
            return individual.ToList();
        }

        public List<CommBbLcInfo> GetBbLcInfoByLcId(int lcId)
        {
            List<CommBbLcInfo> bblcList = new List<CommBbLcInfo>();
            bblcList = Context.CommBbLcInfos.Where(p => p.LcRefId == lcId).ToList();
            return bblcList;
        }

        public List<CommExport> GetExportByLcId(int lcId)
        {
            List<CommExport> exportList = new List<CommExport>();
            exportList = Context.CommExports.Where(p => p.LcId == lcId).ToList();
            return exportList;
        }

        public List<CommImportExportPerformanceReport> GetCommImportExportPerformance()
        {
            List<CommImportExportPerformanceReport> importExport = Context.Database.SqlQuery<CommImportExportPerformanceReport>("SPCommImportExportPerformanceReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return importExport.ToList();
        }

        public List<CommBbLcReport> GetBbLcInfos(CommBbLcInfo bbLcInfo)
        {
            long? supplierId = bbLcInfo.SupplierCompanyRefId ?? 0;
            int? bbLcType = bbLcInfo.BbLcType ?? 0;
            int? lcRefId = bbLcInfo.LcRefId;
            DateTime? fromDate = bbLcInfo.FromDate ?? new DateTime(1900, 01, 01);
            DateTime? toDate = bbLcInfo.ToDate ?? new DateTime(2050, 01, 01);

            DateTime? maturityFrom = bbLcInfo.MaturityDateFrom ?? new DateTime(2000, 01, 01);
            DateTime? maturityTo = bbLcInfo.MaturityDateTo ?? new DateTime(2000, 01, 01);

            DateTime? expiryFrom = bbLcInfo.ExpiryDateFrom ?? new DateTime(2000, 01, 01);
            DateTime? expiryTo = bbLcInfo.ExpiryDateTo ?? new DateTime(2000, 01, 01);

            string bblcNo = bbLcInfo.BbLcNo;
            int? IssuingBankId = bbLcInfo.IssuingBankId ?? 0;

            List<CommBbLcReport> lcInfos = Context.Database.SqlQuery<CommBbLcReport>("SPCommGetBbLcReport @CompanyId, @SupplierId, @LcType,@LcRefId, @FromDate, @ToDate, @MaturityDateFrom, @MaturityDateTo, @ExpiryDateFrom, @ExpiryDateTo, @BbLcNo, @IssuingBankId ", new SqlParameter("CompanyId", _companyId), new SqlParameter("SupplierId", supplierId), new SqlParameter("LcType", bbLcType), new SqlParameter("LcRefId", lcRefId), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("MaturityDateFrom", maturityFrom), new SqlParameter("MaturityDateTo", maturityTo), new SqlParameter("ExpiryDateFrom", expiryFrom), new SqlParameter("ExpiryDateTo", expiryTo), new SqlParameter("BbLcNo", bblcNo), new SqlParameter("IssuingBankId", IssuingBankId)).ToList();
            return lcInfos.ToList();
        }

        public Acc_CompanySector GetActiveCompanySectory(Guid? employeeId)
        {
            int? companyId = 0;
            var activecompanySector = Context.Acc_ActiveCompanySector.FirstOrDefault(p => p.EmployeeId == employeeId && p.IsActive == true);
            if (activecompanySector != null)
                companyId = activecompanySector.CompanyId;
            var companySectory = Context.Acc_CompanySector.FirstOrDefault(p => p.IsActive == true && p.Id == companyId);
            return companySectory;
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            var company = Context.Companies.FirstOrDefault(p => p.CompanyRefId == companyId && p.IsActive == true);

            if (company != null)
                return company.Name;
            else
                return "Not Found !";
        }

        public string GetCompanyAddressByCompanyId(string companyId)
        {
            var company = Context.Companies.FirstOrDefault(p => p.CompanyRefId == companyId && p.IsActive == true);

            if (company != null)
                return company.FullAddress;
            else
                return "Not Found !";
        }

        public List<VwCommLcInfo> GetLcStatus(string rStatus, int bangkId)
        {
            return Context.VwCommLcInfos.Where(x => x.IsActive && x.RStatus == rStatus && (x.ReceivingBankId == bangkId || bangkId == 0))
                .ToList();
        }

        public DataTable GetSalesContactByLcId(int LcId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPSalesContact"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LcId", SqlDbType.VarChar).Value = LcId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public DataTable GetBblcByItemTypeAndBank(CommBbLcInfo commBbLcInfo)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPCommGetBbLcItemTypeReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.NVarChar).Value = "001";
                cmd.Parameters.Add("@ItemType", SqlDbType.Int).Value = commBbLcInfo.ItemType;
                cmd.Parameters.Add("@IssuingBankId", SqlDbType.Int).Value = commBbLcInfo.IssuingBankId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = commBbLcInfo.FromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = commBbLcInfo.ToDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public DataTable GetSalesContactExpByLcId(int LcId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPSalesContactExp"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LcId", SqlDbType.VarChar).Value = LcId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public bool SaveImportExport(CommImportExportPerformance importExport)
        {
            Context.CommImportExportPerformances.Add(importExport);
            Context.SaveChanges();
            return true;
        }

        public bool DeleteAllImportExport()
        {
            Context.Database.ExecuteSqlCommand("TRUNCATE TABLE CommImportExportPerformance");
            return true;
        }

        public List<CommExportListReport> GetExportListReport(CommExportListReport export)
        {
            DateTime? fromDate = export.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = export.ToDate ?? new DateTime(2000, 01, 01);
            string searchKey = export.SearchString;

            List<CommExportListReport> exportList = Context.Database.SqlQuery<CommExportListReport>("SPCommExportListReport @FromDate, @ToDate, @SearchKey", new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("SearchKey", searchKey)).ToList();
            return exportList.ToList();
        }

        public List<CommLcToOrderReport> GetLcOrderReport(CommLcToOrderReport export)
        {
            DateTime? fromDate = export.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = export.ToDate ?? new DateTime(2000, 01, 01);
            string searchKey = export.SearchString;

            List<CommLcToOrderReport> exportList = Context.Database.SqlQuery<CommLcToOrderReport>("SPCommLcToOrderReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return exportList.ToList();
        }

        public List<CommBankAdviceReport> GetBankAdviceReport(CommBankAdviceReport bankAdvice)
        {
            Int64? exportId = bankAdvice.ExportId;

            List<CommBankAdviceReport> bankAdvices = Context.Database.SqlQuery<CommBankAdviceReport>("SPCommBankAdviceReport @ExportId", new SqlParameter("ExportId", exportId)).ToList();
            return bankAdvices.ToList();
        }

        public List<CommLcWithOrderSummaryReport> GetCommLcWithOrderSummaryReport()
        {
            List<CommLcWithOrderSummaryReport> lcOrderSummary = Context.Database.SqlQuery<CommLcWithOrderSummaryReport>("SPCommLcWithOrderSummaryReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return lcOrderSummary.ToList();
        }

        public List<CommLcWithOrderDetailReport> GetCommLcWithOrderDetailReport()
        {
            List<CommLcWithOrderDetailReport> lcOrderDetail = Context.Database.SqlQuery<CommLcWithOrderDetailReport>("SPCommLcWithOrderDetailReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return lcOrderDetail.ToList();
        }

        public List<CommGetLcWithoutOrderReport> CommGetLcWithoutOrderReport()
        {
            List<CommGetLcWithoutOrderReport> lcWithoutOrder = Context.Database.SqlQuery<CommGetLcWithoutOrderReport>("SPCommGetLcWithoutOrderReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return lcWithoutOrder.ToList();
        }

        public List<CommGetOrderWithoutLcReport> CommGetOrderWithoutLcReport()
        {
            List<CommGetOrderWithoutLcReport> orderWithoutLc = Context.Database.SqlQuery<CommGetOrderWithoutLcReport>("SPCommGetOrderWithoutLcReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return orderWithoutLc.ToList();
        }

        public List<CommLcWithOrderDetailReport> GetCommLcDetailReport()
        {
            List<CommLcWithOrderDetailReport> lcDetail = Context.Database.SqlQuery<CommLcWithOrderDetailReport>("SPCommLcDetailReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return lcDetail.OrderBy(p => p.BuyerName).ToList();
        }

        public List<CommCommercialInvoiceReport> GetCommercialInvoiceReport()
        {
            List<CommCommercialInvoiceReport> commInvoice = Context.Database.SqlQuery<CommCommercialInvoiceReport>("SPCommCommercialInvoiceReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return commInvoice.OrderBy(p => p.ExportNo).ToList();
        }

        public List<CommPackingListReport> GetPackingListReport(long exportId)
        {
            List<CommPackingListReport> packing = Context.Database.SqlQuery<CommPackingListReport>("SPCommPackingListReport @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return packing.OrderBy(p => p.PackingListId).ToList();
        }

        public List<CommLcOrderSummary> GetLcOrderSummary()
        {
            List<CommLcOrderSummary> lcOrderSummaries = Context.Database.SqlQuery<CommLcOrderSummary>("SPCommLcOrderSummary").ToList();
            return lcOrderSummaries.ToList();
        }

        public List<CommCashLc> GetCashLcInfo(CommCashLc cashLc)
        {
            string supplierName = cashLc.SupplierName;
            string cashLcNo = cashLc.CashLcNo;
            DateTime? fromDate = cashLc.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = cashLc.ToDate ?? new DateTime(2000, 01, 01);
            string shipmentMode = cashLc.WayOfEntry;
            string portOfDelivery = cashLc.PortOfDelivery;
            string itemName = cashLc.Item;

            List<CommCashLc> lcInfos = Context.Database.SqlQuery<CommCashLc>("SPCommGetCashLcReport @SupplierName, @CashLcNo, @LcFromDate, @LcToDate, @ShipmentMode, @PortOfDelivery, @ItemName ", new SqlParameter("SupplierName", supplierName), new SqlParameter("CashLcNo", cashLcNo), new SqlParameter("LcFromDate", fromDate), new SqlParameter("LcToDate", toDate), new SqlParameter("ShipmentMode", shipmentMode), new SqlParameter("PortOfDelivery", portOfDelivery), new SqlParameter("ItemName", itemName)).ToList();
            return lcInfos.ToList();
        }


    }
}
