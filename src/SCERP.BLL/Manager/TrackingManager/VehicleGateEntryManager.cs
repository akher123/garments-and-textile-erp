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
    public class VehicleGateEntryManager : IVehicleGateEntryManager
    {
        private IVehicleGateEntryRepository _vehicleGateEntryRepository;
        public VehicleGateEntryManager(IVehicleGateEntryRepository vehicleGateEntryRepository)
        {
            _vehicleGateEntryRepository = vehicleGateEntryRepository;
        }
       
        public List<TrackVehicleGateEntry> GetVehicleGateEntriListByPaging(TrackVehicleGateEntry model, out int totalRecords)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            DateTime? toDate = null;
            if (model.ToDate != null)
            {
                toDate = model.ToDate.Value.AddDays(1);
            }
            var vehicleGateEntryList =
                _vehicleGateEntryRepository.Filter(
                    x => x.IsActive == true && ((x.CheckOutStatus == model.CheckOutStatus || model.CheckOutStatus == 0)
                          && ((x.VehicleNumber == model.SearchString || String.IsNullOrEmpty(model.SearchString))
                          || (x.InvoiceNumber == model.SearchString || String.IsNullOrEmpty(model.SearchString))
                           || (x.TrackVehicle.VehicheType.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)))
                          && ((x.EntryDate >= model.FromDate || model.FromDate == null) && (x.EntryDate <= toDate || model.ToDate == null)))).Include(x => x.TrackConfirmationMedia).Include(x => x.TrackVehicle);
            totalRecords = vehicleGateEntryList.Count();
            switch (model.sort)
            {
                case "VehicleNumber":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderByDescending(r => r.VehicleNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderBy(r => r.VehicleNumber).ThenBy(r => r.VehicleNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "GateEntryNumber":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderByDescending(r => r.GateEntryNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderBy(r => r.GateEntryNumber).ThenBy(r => r.GateEntryNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "TrackVehicle.VehicheType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderByDescending(r => r.TrackVehicle.VehicheType)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderBy(r => r.TrackVehicle.VehicheType).ThenBy(r => r.TrackVehicle.VehicheType)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


                case "InvoiceNumber":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderByDescending(r => r.InvoiceNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vehicleGateEntryList = vehicleGateEntryList
                                 .OrderBy(r => r.InvoiceNumber).ThenBy(r=>r.InvoiceNumber)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vehicleGateEntryList = vehicleGateEntryList
                        .OrderByDescending(r => r.VehicleGateEntryId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return vehicleGateEntryList.ToList();
        }
        public bool IsVehicleGateEntryExist(TrackVehicleGateEntry model)
        {
            int checkIn =Convert.ToInt16(CheckOutStatus.CheckIn);
            return _vehicleGateEntryRepository.Exists(
                x => x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.VehicleGateEntryId != model.VehicleGateEntryId && x.GateEntryNumber != model.GateEntryNumber
                    && ((x.VehicleNumber == model.VehicleNumber && x.CheckOutStatus == checkIn)));
        }
        public int SaveVehicleGateEntry(TrackVehicleGateEntry model)
        {
            model.EntryDate = model.EntryDate.ToMargeDateAndTime(model.CheckInTime);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.IsActive = true;
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CheckOutStatus = Convert.ToInt16(CheckOutStatus.CheckIn);
            return _vehicleGateEntryRepository.Save(model);
        }
        public int EditVehicleGateEnty(TrackVehicleGateEntry model)
        {
            var vehicleGateEntry = _vehicleGateEntryRepository.FindOne(x =>x.IsActive==true && x.VehicleGateEntryId == model.VehicleGateEntryId);
            vehicleGateEntry.VehicleNumber = model.VehicleNumber;
            vehicleGateEntry.InvoiceNumber = model.InvoiceNumber;
            vehicleGateEntry.GateEntryNumber = model.GateEntryNumber;
            vehicleGateEntry.TrackConfirmationMedia = model.TrackConfirmationMedia;
            vehicleGateEntry.ConfirmedBy = model.ConfirmedBy;
            vehicleGateEntry.CheckOutStatus = model.CheckOutStatus;
            vehicleGateEntry.Remarks = model.Remarks;
            if (model.CheckOutStatus == Convert.ToInt32(CheckOutStatus.CheckIn))
            {
                vehicleGateEntry.ExitDate = null;
            }
            else
            {
                vehicleGateEntry.ExitDate = model.ExitDate.GetValueOrDefault().ToMargeDateAndTime(model.CheckOutTime);
            }
            vehicleGateEntry.EditedBy = PortalContext.CurrentUser.UserId;
            vehicleGateEntry.EditedDate = DateTime.Now;
            return _vehicleGateEntryRepository.Edit(vehicleGateEntry); 
        }


        public TrackVehicleGateEntry GetVehicleGateEntryById(long vehicleGateEntryId)
        {
            return _vehicleGateEntryRepository.FindOne(x => x.VehicleGateEntryId == vehicleGateEntryId);
        }

        public int DeleteVehicleGateEntry(long vehicleGateEntryId)
        {
            return _vehicleGateEntryRepository.Delete(x => x.VehicleGateEntryId == vehicleGateEntryId);
        }

        
    }
}
