using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;
using SCERP.Model.Production;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IExportManager
    {
        List<CommExport> GetExportByPaging(ProSearchModel<CommExport> model, out int  totalRecords);
        CommExport GetExportById(long exportId);
        List<CommExport> GetExportByLcId(int LcId);
        string GetNewExportRefId();
        int EditExport(CommExport commExport);
        int SaveExport(CommExport commExport);
        int DeleteExport(long exportId);
        List<CommExport> GetExportLsit(DateTime? fromDate, DateTime? toDate, string searchString);
        List<OM_BuyerOrder> GetBuyerOrderbyExportId(long exportId);
        List<OM_BuyOrdStyle> GetStyleNoByOrderNo(string orderNo);
        int SaveExportDetail(CommExportDetail exportDetail);
        int EditExportDetail(CommExportDetail exportDetail);
        bool IsExistExportDetail(CommExportDetail expDetail);
        List<CommExportDetail> GetExportDetailByExportId(long exportId);
        int SavePackDetail(CommPackingListDetail packingListDetail);
        int EditPackDetail(CommPackingListDetail packDetail);
        List<CommPackingListDetail> GetPackingDetailByExportId(long exportId);
    }
}
