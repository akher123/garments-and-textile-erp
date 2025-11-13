using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Custom;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IReportRepository:IRepository<object>
    {
        List<COMMLcInfo> GetLcIndividual(int lcId);
        List<COMMLcInfo> GetLcInfos(COMMLcInfo lcInfo);
        DataTable GetSalesContactByLcId(int LcId);
        DataTable GetSalesContactExpByLcId(int LcId);
        List<CommBbLcReport> GetBbLcInfos(CommBbLcInfo bbLcInfo);
        List<BbLcIndividualReport> GetBbLcIndividual(int bbLcId);
        Acc_CompanySector GetActiveCompanySectory(Guid? employeeId);
        List<CommImportExportPerformanceReport> GetCommImportExportPerformance();
        List<CommBbLcInfo> GetBbLcInfoByLcId(int lcId);
        DataTable GetBblcByItemTypeAndBank(CommBbLcInfo commBbLcInfo);
        List<CommExport> GetExportByLcId(int lcId);
        bool SaveImportExport(CommImportExportPerformance importExport);
        bool DeleteAllImportExport();
        List<CommExportListReport> GetExportListReport(CommExportListReport export);
        List<CommLcToOrderReport> GetLcOrderReport(CommLcToOrderReport export);
        List<CommBankAdviceReport> GetBankAdviceReport(CommBankAdviceReport bankAdvice);
        List<CommLcWithOrderSummaryReport> GetCommLcWithOrderSummaryReport();
        List<CommLcWithOrderDetailReport> GetCommLcWithOrderDetailReport();
        List<CommGetLcWithoutOrderReport> CommGetLcWithoutOrderReport();
        List<CommGetOrderWithoutLcReport> CommGetOrderWithoutLcReport();
        List<CommLcWithOrderDetailReport> GetCommLcDetailReport();
        string GetCompanyNameByCompanyId(string companyId);
        List<CommCommercialInvoiceReport> GetCommercialInvoiceReport();
        List<CommPackingListReport> GetPackingListReport(long exportId);
        List<CommLcOrderSummary> GetLcOrderSummary();
        string GetCompanyAddressByCompanyId(string companyId);
        List<VwCommLcInfo> GetLcStatus(string rStatus, int bangkId);
        List<CommCashLc> GetCashLcInfo(CommCashLc cashLc);
    }
}
