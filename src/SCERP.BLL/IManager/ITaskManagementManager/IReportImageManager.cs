using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
    public interface IReportImageManager
    {
        List<TmReportImageInfo> GetAllReportImageByPaging(TmReportImageInfo model, out int totalRecords);
        List<TmReportImageInfo> GetAllShowReportImageByPaging(TmReportImageInfo model, out int totalRecords);
        List<TmReportImageInfo> GetAllReportImage();
        TmReportImageInfo GetReportByReporImageId(int reportId);
        int SaveReportImage(TmReportImageInfo model);
        int EditReportImage(TmReportImageInfo model);
        int DeleteReportImage(int reportId);
        bool IsReportImagetExist(TmReportImageInfo model);
        string GetNewReportImageNumber();
    }
}
