using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class VisitorGateEntryManager : IVisitorGateEntryManager
    {
        private IVisitorGateEntryRepository _visitorGateEntryRepository;
        public VisitorGateEntryManager(IVisitorGateEntryRepository visitorGateEntryRepository)
        {
            _visitorGateEntryRepository = visitorGateEntryRepository;
        }

        public List<TrackVisitorGateEntry> GetVisitorGateEntryByPaging(TrackVisitorGateEntry model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            DateTime? toDate = null;
            if (model.ToDate != null)
            {
                toDate = model.ToDate.Value.AddDays(1);
            }
            var visitorList =
                _visitorGateEntryRepository.Filter(
                    x =>x.IsActive==true && (x.CheckOutStatus == model.CheckOutStatus || model.CheckOutStatus == 0)
                          && ((x.VisitorName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                          || (x.Phone == model.SearchString || String.IsNullOrEmpty(model.SearchString))
                          || (x.VisitorCardId == model.SearchString || String.IsNullOrEmpty(model.SearchString)))
                          && ((x.EntryDate >= model.FromDate || model.FromDate == null) && (x.EntryDate <= toDate || model.ToDate == null)));
            totalRecords = visitorList.Count();
            switch (model.sort)
            {
                case "VisitorName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            visitorList = visitorList
                                 .OrderByDescending(r => r.VisitorName).ThenBy(r => r.VisitorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            visitorList = visitorList
                                 .OrderBy(r => r.VisitorName).ThenBy(r => r.VisitorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "Phone":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            visitorList = visitorList
                                 .OrderByDescending(r => r.Phone)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            visitorList = visitorList
                                 .OrderBy(r => r.Phone)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "VisitorCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            visitorList = visitorList
                                 .OrderByDescending(r => r.VisitorCardId).ThenBy(r=>r.VisitorCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            visitorList = visitorList
                                 .OrderBy(r => r.VisitorCardId).ThenBy(r=>r.VisitorCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    visitorList = visitorList
                        .OrderByDescending(r => r.VisitorGateEntryId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return visitorList.ToList();
        }
        public bool IsVitorGateEntryExist(TrackVisitorGateEntry model)
        {
            int checkIn = Convert.ToInt16(CheckOutStatus.CheckIn);
            return _visitorGateEntryRepository.Exists(
                x => x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.VisitorGateEntryId != model.VisitorGateEntryId 
                    && ((x.VisitorCardId == model.VisitorCardId && x.CheckOutStatus == checkIn)
                    ||(x.Phone == model.Phone && x.CheckOutStatus == checkIn)));
        }
        public int SaveVisitorGateEntry(TrackVisitorGateEntry model)
        {
            model.EntryDate=model.EntryDate.ToMargeDateAndTime(model.CheckInTime);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.IsActive = true;
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CheckOutStatus = Convert.ToInt16(CheckOutStatus.CheckIn);
            return _visitorGateEntryRepository.Save(model);
        }

        public int EditVisitorGateEntry(TrackVisitorGateEntry model)
        {
            var visitorGateEntry = _visitorGateEntryRepository.FindOne(x =>x.IsActive==true && x.VisitorGateEntryId == model.VisitorGateEntryId);
            
            visitorGateEntry.VisitorName = model.VisitorName;
            visitorGateEntry.Address = model.Address;
            visitorGateEntry.Phone = model.Phone;
            visitorGateEntry.ImagePath = model.ImagePath;
            visitorGateEntry.TrackConfirmationMedia = model.TrackConfirmationMedia;
            visitorGateEntry.CheckOutStatus = model.CheckOutStatus;
            if (model.CheckOutStatus == Convert.ToInt32(CheckOutStatus.CheckIn))
            {
                visitorGateEntry.ExitDate = null;
            }
            else
            {
                visitorGateEntry.ExitDate = model.ExitDate.GetValueOrDefault().ToMargeDateAndTime(model.CheckOutTime);
            }
            visitorGateEntry.Remarks = model.Remarks;
            visitorGateEntry.EditedBy = PortalContext.CurrentUser.UserId;
            visitorGateEntry.EditedDate = DateTime.Now;
            return _visitorGateEntryRepository.Edit(visitorGateEntry);
        }
        public int DeleteVisitorGateEntry(long visitorGateEntryId)
        {
            TrackVisitorGateEntry visitorGateEntry =
                _visitorGateEntryRepository.FindOne(
                    x => x.IsActive == true && x.VisitorGateEntryId == visitorGateEntryId);
            visitorGateEntry.IsActive = false;
            visitorGateEntry.EditedBy = PortalContext.CurrentUser.UserId;
            visitorGateEntry.EditedDate = DateTime.Now;
            return _visitorGateEntryRepository.Edit(visitorGateEntry);
        }
        public TrackVisitorGateEntry GetVisitorGateEntryByPhone(string phone)
        {
            return _visitorGateEntryRepository.FindOne(x =>x.IsActive==true && x.Phone == phone);
        }
        public TrackVisitorGateEntry GetVisitorGateEntryById(long visitorGateEntryId)
        {
            return _visitorGateEntryRepository.FindOne(x => x.IsActive == true && x.VisitorGateEntryId == visitorGateEntryId);
        }  
    }
}
