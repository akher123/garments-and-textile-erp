
using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class OutStationDutyManager : IOutStationDutyManager
    {
        private readonly IOutStationDutyRepository _outStationDutyRepository = null;
        public OutStationDutyManager(SCERPDBContext context)
        {
            _outStationDutyRepository = new OutStationDutyRepository(context);
        }

        public int SaveOutStationDuty(OutStationDuty model)
        {
            int saveIndex = 0;
            try
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.IsActive = true;
                saveIndex = _outStationDutyRepository.Save(model);
            }
            catch (Exception exception)
            {
                
                throw new Exception(exception.Message,exception.InnerException);
            }
            return saveIndex;
        }

        public List<VOutStationDutyDetail> GetAllOutStationDutyDetail(int startPage, int pageSize, OutStationDuty model, SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            List<VOutStationDutyDetail> vOutStationDutyDetails = null;
            try
            {
                vOutStationDutyDetails = _outStationDutyRepository.GetAllOutStationDutyDetail(startPage, pageSize, model, searchFieldModel, out  totalRecords);
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.InnerException.Message);
            }

            return vOutStationDutyDetails;
        }

        public OutStationDuty GetOutStationDutyById(int outStationDutyId)
        {
           OutStationDuty outStationDuty;
            try
            {
                outStationDuty = _outStationDutyRepository.GetOutStationDutyById(outStationDutyId);
            }
            catch (Exception exception)
            {
              
                throw new Exception(exception.InnerException.Message);
            }

            return outStationDuty;
        }

        public int EditOutStationDuty(OutStationDuty model)
        {
            var editIndex = 0;
            try
            {
                var outStation = _outStationDutyRepository.FindOne(x=>x.OutStationDutyId==model.OutStationDutyId);
                outStation.EditedDate = DateTime.Now;
                outStation.EditedBy = PortalContext.CurrentUser.UserId;
                outStation.DutyDate = model.DutyDate;
                outStation.Location = model.Location;
                outStation.Purpose = model.Purpose;
                editIndex = _outStationDutyRepository.Edit(outStation);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return editIndex;
        }

        public int DeleteOutstationDutyById(int outStationDutyId)
        {
            var deleteIndex = 0;
       
            try
            {
                var outStationDuty = _outStationDutyRepository.FindOne(x => x.IsActive && x.OutStationDutyId == outStationDutyId);
                outStationDuty.EditedDate = DateTime.Now;
                outStationDuty.EditedBy = PortalContext.CurrentUser.UserId;
                outStationDuty.IsActive = false;
                deleteIndex = _outStationDutyRepository.Edit(outStationDuty);
            }
            catch (Exception exception)
            {
               throw new Exception(exception.Message);
           
            }
            return deleteIndex;
        }

        public List<VOutStationDutyDetail> GetOutStationDutyBySearchKey(SearchFieldModel searchField)
        {
            List<VOutStationDutyDetail> vOutStationDutyDetails = null;
            try
            {
                vOutStationDutyDetails = _outStationDutyRepository.GetOutStationDutyBySearchKey(searchField);
            }
            catch (Exception exception)
            {
               
                throw new Exception(exception.InnerException.Message);
            }

            return vOutStationDutyDetails;
        }
    }
}
