using System;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Common;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class WorkShiftManager : BaseManager, IWorkShiftManager
    {
        private readonly IWorkShiftRepository _workShiftRepository = null;

        public WorkShiftManager(SCERPDBContext context)
        {
            _workShiftRepository = new WorkShiftRepository(context);
        }

        public List<WorkShift> GetAllWorkShiftsByPaging(int startPage, int pageSize, WorkShift workShift, out int totalRecords)
        {
            List<WorkShift> workShifts;

            try
            {
                workShifts = _workShiftRepository.GetAllWorkShiftsByPaging(startPage, pageSize, out totalRecords, workShift).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return workShifts;
        }

        public List<WorkShift> GetAllWorkShiftsBySearchKey(string searchKey)
        {
            List<WorkShift> workShift;

            try
            {
                workShift = !String.IsNullOrEmpty(searchKey)
                    ? _workShiftRepository.Filter(
                        x =>
                            x.IsActive == true &&
                            x.Name.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower())).ToList()
                    : _workShiftRepository.Filter(x => x.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workShift;
        }

        //public bool IsWorkShiftExist(WorkShift workShift)
        //{
        //    bool isExist=false;
        //    try
        //    {
        //        isExist =
        //            _workShiftRepository.Exists(
        //                x =>
        //                    x.IsActive == true && x.WorkShiftId != workShift.WorkShiftId &&
        //                    x.Name.Replace(" ", "").ToLower().Equals(workShift.Name.Replace(" ", "").ToLower()));
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return isExist;
        //}

        public bool IsWorkShiftExist(WorkShift workShift)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _workShiftRepository.Exists(
                        x =>
                            x.IsActive == true && x.WorkShiftId != workShift.WorkShiftId &&
                            x.NameDetail.Replace(" ", "").ToLower().Equals(workShift.NameDetail.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int SaveWorkShift(WorkShift workShift)
        {
            var saveIndex = 0;
            try
            {
                workShift.CreatedDate = DateTime.Now;
                workShift.CreatedBy = PortalContext.CurrentUser.UserId;
                workShift.IsActive = true;
                saveIndex = _workShiftRepository.Save(workShift);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return saveIndex;

        }

        public IQueryable<Model.WorkShift> GetAllWorkShifts()
        {
            return _workShiftRepository.Filter(w => w.IsActive).OrderBy(x => x.Name);
        }

        public Model.WorkShift GetWorkShiftById(int? id)
        {
            return _workShiftRepository.GetWorkShiftById(id);
        }

        public int EditWorkShift(Model.WorkShift workShift)
        {

            var editIndex = 0;
            try
            {
                workShift.EditedDate = DateTime.Now;
                workShift.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _workShiftRepository.Edit(workShift);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return editIndex;
        }

        public int DeleteWorkShift(int id)
        {

            var deleteIndex = 0;
            try
            {
                var workShift = _workShiftRepository.FindOne(x => x.WorkShiftId == id && x.IsActive);
                workShift.IsActive = false;
                workShift.EditedDate = DateTime.Now;
                workShift.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _workShiftRepository.Edit(workShift);
            }
            catch (Exception)
            {
                deleteIndex = 0;
            }
            return deleteIndex;
        }

        public List<WorkShiftRosterDetail> GetWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            return _workShiftRepository.GetWorkShiftRoster(shiftRoster);
        }
        public int SaveWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            return _workShiftRepository.SaveWorkShiftRoster(shiftRoster);
        }

        public int ChangeWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            return _workShiftRepository.ChangeWorkShiftRoster(shiftRoster);
        }
    }
}