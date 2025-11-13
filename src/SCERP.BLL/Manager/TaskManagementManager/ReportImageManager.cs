using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;
using SCERP.Common;


namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class ReportImageManager : IReportImageManager
    {
        private readonly IReportImageRepository _reportImageRepository;
        public ReportImageManager(IReportImageRepository reportImageRepository)
        {
            _reportImageRepository = reportImageRepository;
        }
        public int DeleteReportImage(int reportImageId)
        {
            return _reportImageRepository.Delete(x => x.ReportImageId == reportImageId);
        }

        public int EditReportImage(TmReportImageInfo model)
        {
            TmReportImageInfo reportImageInfo = _reportImageRepository.FindOne(x => x.ReportImageId== model.ReportImageId);
            
            reportImageInfo.SubjectId = model.SubjectId;
            reportImageInfo.ReportName = model.ReportName;
            reportImageInfo.ReportNo = model.ReportNo;
            reportImageInfo.ReportImageUrl = model.ReportImageUrl;
            reportImageInfo.Remarks = model.Remarks;
            reportImageInfo.ProjectReportUrl = model.ProjectReportUrl;

            return _reportImageRepository.Edit(reportImageInfo);
        }

        public List<TmReportImageInfo> GetAllReportImageByPaging(TmReportImageInfo model, out int totalRecords)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            var reportImageList = _reportImageRepository.Filter(x => x.ReportName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString));
            
            totalRecords = reportImageList.Count();
            switch (model.sort)
            {
                case "ReportName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.ReportName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.ReportName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "ReportNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.ReportNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.ReportNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


                case "SubjectId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.SubjectId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.SubjectId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    reportImageList = reportImageList
                        .OrderByDescending(r => r.ReportImageId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            
            return reportImageList.ToList();
        }

        public List<TmReportImageInfo> GetAllShowReportImageByPaging(TmReportImageInfo model, out int totalRecords)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var index = model.PageIndex;
            var pageSize = 1; //AppConfig.PageSize;

            var reportImageList = _reportImageRepository.Filter(x => x.ReportName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString));

            totalRecords = reportImageList.Count();
            switch (model.sort)
            {
                case "ReportName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.ReportName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.ReportName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "ReportNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.ReportNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.ReportNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


                case "SubjectId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            reportImageList = reportImageList
                                 .OrderByDescending(r => r.SubjectId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            reportImageList = reportImageList
                                 .OrderBy(r => r.SubjectId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    reportImageList = reportImageList
                        .OrderByDescending(r => r.ReportImageId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return reportImageList.ToList();
        }

        public string GetNewReportImageNumber()
        {
            var reportNumber = _reportImageRepository.All().Max(x => x.ReportNo);
            return reportNumber.IncrementOne().PadZero(5);
        }

        public List<TmReportImageInfo> GetAllReportImage()
        {
            var reportNumber = _reportImageRepository.All();
            return reportNumber.ToList();
        }

        public TmReportImageInfo GetReportByReporImageId(int reportId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var itemList = _reportImageRepository.Filter(x => x.ReportImageId == reportId).FirstOrDefault(x => x.ReportImageId == reportId);
            return itemList;
        }

        public bool IsReportImagetExist(TmReportImageInfo model)
        {
            return _reportImageRepository.Exists(x => x.ReportImageId == model.ReportImageId && x.SubjectId == model.SubjectId && x.ReportNo==model.ReportNo && x.ReportImageUrl == model.ReportImageUrl&& x.Remarks == model.Remarks && x.ReportName ==model.ReportName && x.ProjectReportUrl==model.ProjectReportUrl);
        }

        public int SaveReportImage(TmReportImageInfo model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;

            return _reportImageRepository.Save(model);
        }
    }
}
