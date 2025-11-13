using System.Data;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IReportManager
    {
        List<COMMLcInfo> GetLcIndividual(int lcId);
        List<COMMLcInfo> GetLcInfos(COMMLcInfo lcInfo);
        DataTable GetSalesContactByLcId(int LcId);
        DataTable GetSalesContactExpByLcId(int LcId);
        List<CommBbLcReport> GetBbLcInfos(CommBbLcInfo bbLcInfo);
        List<BbLcIndividualReport> GetBbLcIndividual(int bbLcId);
        Acc_CompanySector GetActiveCompanySectory(Guid? employeeId);
        List<CommImportExportPerformanceReport> GetCommImportExportPerformance(long? buyerId);
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
        DataTable GetBblcByItemTypeAndBank(CommBbLcInfo commBbLcInfo);
        List<CommCashLc> GetCashLcInfo(CommCashLc cashLc);
        DataTable GetLcInfoDetail(int lcId);
    }
}
