using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using System.Data;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _commReportRepository = null;

        public ReportManager(IReportRepository commReportRepository)
        {
            _commReportRepository = commReportRepository;
        }

        public List<COMMLcInfo> GetLcIndividual(int lcId)
        {
            return _commReportRepository.GetLcIndividual(lcId);
        }

        public List<BbLcIndividualReport> GetBbLcIndividual(int bbLcId)
        {
            return _commReportRepository.GetBbLcIndividual(bbLcId);
        }

        public List<COMMLcInfo> GetLcInfos(COMMLcInfo lcInfo)
        {
            return _commReportRepository.GetLcInfos(lcInfo);
        }

        public List<CommImportExportPerformanceReport> GetCommImportExportPerformance(long? buyerId)
        {
            int countMax = 0;
            int serialId = 1;
            COMMLcInfo lcInfo = new COMMLcInfo();
            lcInfo.BuyerId = buyerId;
            var lclist = _commReportRepository.GetLcInfos(lcInfo);
            CommImportExportPerformance import = new CommImportExportPerformance();

            _commReportRepository.DeleteAllImportExport();

            foreach (var t in lclist)
            {
                List<CommBbLcInfo> bblclist = _commReportRepository.GetBbLcInfoByLcId(t.LcId);
                List<CommExport> exportlist = _commReportRepository.GetExportByLcId(t.LcId);

                var countlc = 1;
                var countbblc = bblclist.Count;
                var countexport = exportlist.Count;

                countMax = countbblc > countexport ? countbblc : countexport;

                for (int i = 0; i < countMax; i++)
                {
                    import = new CommImportExportPerformance();
                    import.SerialId = serialId;
                    import.LcId = t.LcId;
                    import.LcNo = t.LcNo;
                    import.LcDate = t.LcDate;
                    import.BuyerId = t.BuyerId;

                    if (countlc > i)
                    {
                        import.LcAmount = t.LcAmount;
                        import.LcQuantity = t.LcQuantity;
                        import.ExpiryDate = t.ExpiryDate;
                    }

                    if (countexport > i)
                    {
                        import.InvoiceNo = exportlist[i].InvoiceNo;
                        import.InvoiceDate = exportlist[i].InvoiceDate;
                        import.InvoiceValue = exportlist[i].InvoiceValue;
                        import.BankRefNo = exportlist[i].BankRefNo;
                        import.BankRefDate = exportlist[i].BankRefDate;
                        import.RealizedValue = exportlist[i].RealizedValue;
                        import.RealizedDate = exportlist[i].RealizedDate;
                        import.BillOfLadingNo = exportlist[i].BillOfLadingNo;
                        import.BillOfLadingDate = exportlist[i].BillOfLadingDate;
                        import.ExportNo = exportlist[i].ExportNo;
                        import.ExportDate = exportlist[i].ExportDate;
                        import.SBNo = exportlist[i].SBNo;
                        import.SBNoDate = exportlist[i].SBNoDate;
                    }

                    if (countbblc > i)
                    {
                        import.BbLcNo = bblclist[i].BbLcNo;
                        import.BbLcDate = bblclist[i].BbLcDate;
                        import.BbLcAmount = bblclist[i].BbLcAmount;
                        import.IfdbcNo = bblclist[i].IfdbcNo;
                        import.IfdbcDate = bblclist[i].IfdbcDate;
                        import.IfdbcValue = bblclist[i].IfdbcValue;
                        import.BbLcMatureDate = bblclist[i].MatureDate;
                        import.PcsSanctionAmount = bblclist[i].PcsSanctionAmount;
                        import.SupplierCompanyRefId = bblclist[i].SupplierCompanyRefId;
                    }

                    _commReportRepository.SaveImportExport(import);
                }
                serialId++;
            }
            List<CommImportExportPerformanceReport> result = _commReportRepository.GetCommImportExportPerformance();
            return result;
        }

        public List<CommBbLcReport> GetBbLcInfos(CommBbLcInfo bbLcInfo)
        {
            return _commReportRepository.GetBbLcInfos(bbLcInfo);
        }

        public Acc_CompanySector GetActiveCompanySectory(Guid? employeeId)
        {
            return _commReportRepository.GetActiveCompanySectory(employeeId);
        }

        public string GetCompanyNameByCompanyId(string companyId)
        {
            return _commReportRepository.GetCompanyNameByCompanyId(companyId);
        }
        public string GetCompanyAddressByCompanyId(string companyId)
        {
            return _commReportRepository.GetCompanyAddressByCompanyId(companyId);
        }

        public List<VwCommLcInfo> GetLcStatus(string rStatus, int bangkId)
        {
            return _commReportRepository.GetLcStatus(rStatus,bangkId);
        }

        public DataTable GetSalesContactByLcId(int LcId)
        {
            return _commReportRepository.GetSalesContactByLcId(LcId);
        }

        public DataTable GetBblcByItemTypeAndBank(CommBbLcInfo commBbLcInfo)
        {
            return _commReportRepository.GetBblcByItemTypeAndBank(commBbLcInfo);
        }
        public DataTable GetSalesContactExpByLcId(int LcId)
        {
            return _commReportRepository.GetSalesContactExpByLcId(LcId);
        }
        public List<CommExportListReport> GetExportListReport(CommExportListReport export)
        {
            return _commReportRepository.GetExportListReport(export);
        }

        public List<CommLcToOrderReport> GetLcOrderReport(CommLcToOrderReport export)
        {

            List<CommLcToOrderReport> lcOrder = new List<CommLcToOrderReport>();

            if (export.SearchString == "1")
                lcOrder = _commReportRepository.GetLcOrderReport(export).Where(p => p.ShipmentDate >= export.FromDate && p.ShipmentDate <= export.ToDate).ToList();

            else if (export.SearchString == "2")
                lcOrder = _commReportRepository.GetLcOrderReport(export).Where(p => p.LcDate >= export.FromDate && p.LcDate <= export.ToDate).ToList();

            else if (export.SearchString == "3")
                lcOrder = _commReportRepository.GetLcOrderReport(export).Where(p => p.ExpiryDate >= export.FromDate && p.ExpiryDate <= export.ToDate).ToList();

            else
                lcOrder = _commReportRepository.GetLcOrderReport(export);

            return lcOrder;
        }

        public List<CommBankAdviceReport> GetBankAdviceReport(CommBankAdviceReport bankAdvice)
        {
            return _commReportRepository.GetBankAdviceReport(bankAdvice);
        }

        public List<CommLcWithOrderSummaryReport> GetCommLcWithOrderSummaryReport()
        {
            return _commReportRepository.GetCommLcWithOrderSummaryReport();
        }

        public List<CommLcWithOrderDetailReport> GetCommLcWithOrderDetailReport()
        {
            return _commReportRepository.GetCommLcWithOrderDetailReport();
        }

        public List<CommGetLcWithoutOrderReport> CommGetLcWithoutOrderReport()
        {
            return _commReportRepository.CommGetLcWithoutOrderReport();
        }

        public List<CommGetOrderWithoutLcReport> CommGetOrderWithoutLcReport()
        {
            return _commReportRepository.CommGetOrderWithoutLcReport();
        }

        public List<CommLcWithOrderDetailReport> GetCommLcDetailReport()
        {
            return _commReportRepository.GetCommLcDetailReport();
        }

        public List<CommCommercialInvoiceReport> GetCommercialInvoiceReport()
        {        
            return _commReportRepository.GetCommercialInvoiceReport();
        }

        public List<CommPackingListReport> GetPackingListReport(long exportId)
        {
            return _commReportRepository.GetPackingListReport(exportId);
        }

        public List<CommLcOrderSummary> GetLcOrderSummary()
        {
            return _commReportRepository.GetLcOrderSummary();
        }
        public List<CommCashLc> GetCashLcInfo(CommCashLc cashLc)
        {
            return _commReportRepository.GetCashLcInfo(cashLc);
        }

        public DataTable GetLcInfoDetail(int lcId)
        {
            return _commReportRepository.ExecuteQuery(String.Format("execute spGetLcInfoReport {0}", lcId));
        }
    }
}
