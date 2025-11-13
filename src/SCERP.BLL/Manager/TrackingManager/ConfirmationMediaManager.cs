using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class ConfirmationMediaManager : IConfirmationMediaManager
    {
        private IConfirmationMediaRepository _confirmationMediaRepository;
        private IVisitorGateEntryRepository _visitorGateEntryRepository;
        private IVehicleGateEntryRepository _vehicleGateEntryRepository;
        public ConfirmationMediaManager(IConfirmationMediaRepository confirmationMediaRepository, IVisitorGateEntryRepository visitorGateEntryRepository, IVehicleGateEntryRepository vehicleGateEntryRepository)
        {
            _confirmationMediaRepository = confirmationMediaRepository;
            _visitorGateEntryRepository = visitorGateEntryRepository;
            _vehicleGateEntryRepository = vehicleGateEntryRepository;

        }
        public List<TrackConfirmationMedia> GetAllConfirmationMediaList()
        {
            return _confirmationMediaRepository.All().OrderBy(y=>y.ConfirmationMedia).ToList();
        }

        public List<TrackConfirmationMedia> GetAllConfirmationMediaListByPaging(TrackConfirmationMedia model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;// How get value
           var confirmationMediaList =
               _confirmationMediaRepository.Filter(
                    x =>x.IsActive==true && (x.ConfirmationMedia.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = confirmationMediaList.Count();
            switch (model.sort)
            {
                case "ConfirmationMedia":
                    switch (model.sortdir)// Asc & Desc kivabe ashche?
                    {
                        case "DESC":
                            confirmationMediaList = confirmationMediaList
                                 .OrderByDescending(r => r.ConfirmationMedia)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            confirmationMediaList = confirmationMediaList
                                 .OrderBy(r => r.ConfirmationMedia)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                
                default:
                    confirmationMediaList = confirmationMediaList
                        .OrderByDescending(r => r.ConfirmationMediaId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return confirmationMediaList.ToList();
        }
        public bool IsConfirmationMediaExist(TrackConfirmationMedia model)
        {
            return _confirmationMediaRepository.Exists(
             x =>
                 x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.ConfirmationMediaId != model.ConfirmationMediaId && x.ConfirmationMedia == model.ConfirmationMedia);
        }
        public int SaveConfirmationMedia(TrackConfirmationMedia model)
        {
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _confirmationMediaRepository.Save(model);
        }

        public int EditConfirmationMedia(TrackConfirmationMedia model)
        {
            TrackConfirmationMedia confirmationMedia =
                _confirmationMediaRepository.FindOne(x => x.IsActive == true && x.ConfirmationMediaId == model.ConfirmationMediaId);

            confirmationMedia.ConfirmationMedia = model.ConfirmationMedia;
            confirmationMedia.Remarks = model.Remarks;
            confirmationMedia.EditedBy = PortalContext.CurrentUser.UserId;
            confirmationMedia.EditedDate = DateTime.Now;
            return _confirmationMediaRepository.Edit(confirmationMedia);
        }

        public int DeleteConfirmationMedia(long confirmationMediaId)
        {
            int deleted = 0;
            if ((_vehicleGateEntryRepository.Exists(x => x.IsActive == true && x.ConfirmationMediaId == confirmationMediaId)) || (_visitorGateEntryRepository.Exists(x=>x.IsActive==true && x.ConfirmationMediaId==confirmationMediaId)))
            {
                deleted = -1;// This vehicle Id used by another table
            }
            else
            {
                TrackConfirmationMedia confirmationMedia =
               _confirmationMediaRepository.FindOne(x => x.IsActive == true && x.ConfirmationMediaId == confirmationMediaId);
                confirmationMedia.IsActive = false;
                confirmationMedia.EditedBy = PortalContext.CurrentUser.UserId;
                confirmationMedia.EditedDate = DateTime.Now;
                return _confirmationMediaRepository.Edit(confirmationMedia);
            }
            return deleted;
        }
        public TrackConfirmationMedia GetConfirmationMediaById(int confirmationMediaId)
        {
            return _confirmationMediaRepository.FindOne(x => x.IsActive == true && x.ConfirmationMediaId == confirmationMediaId);
        }
       
    }
}
