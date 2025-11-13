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
    public class EmployeeWorkShiftManager : IEmployeeWorkShiftManager
    {
        private readonly IEmployeeWorkShiftRepository _employeeWorkShiftRepository = null;
        public EmployeeWorkShiftManager(SCERPDBContext context)
        {
            _employeeWorkShiftRepository = new EmployeeWorkShiftRepository(context);
        }
        public int SaveEmployeeWorkShifts(List<EmployeeWorkShift> employeeWorkShifts)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = _employeeWorkShiftRepository.SaveEmployeeWorkShifts(employeeWorkShifts);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return saveIndex;
        }
        public EmployeeWorkShift GetEmployeeWorkshiftById(int employeeWorkShiftId)
        {
            EmployeeWorkShift employeeWorkShift;
            try
            {
                employeeWorkShift = _employeeWorkShiftRepository.GetEmployeeWorkshiftById(employeeWorkShiftId);

            }
            catch(Exception exception)
            {

                throw new Exception(exception.Message,exception.InnerException);
            }

            return employeeWorkShift;
        }

        public int ChangeEmployeeWorkShifts(EmployeeWorkShift model)
        {
            var saveIndex = 0;
            try
            {
                bool isExist =
                    _employeeWorkShiftRepository.Exists(
                        x =>
                            x.BranchUnitWorkShiftId == model.BranchUnitWorkShiftId 
                            && x.EmployeeWorkShiftId == model.EmployeeWorkShiftId 
                            && x.EmployeeId == model.EmployeeId                       
                            &&x.Remarks==model.Remarks);

                EmployeeWorkShift employeeWorkShiftObj = _employeeWorkShiftRepository.FindOne(x => x.Status && x.IsActive && x.EmployeeWorkShiftId == model.EmployeeWorkShiftId);
                employeeWorkShiftObj.EditedBy = PortalContext.CurrentUser.UserId;
                employeeWorkShiftObj.EditedDate = DateTime.Now;

                employeeWorkShiftObj.StartDate = employeeWorkShiftObj.ShiftDate; // Do not remove this
                employeeWorkShiftObj.EndDate = employeeWorkShiftObj.ShiftDate; // Do not remove this

                if (isExist)
                {
                    employeeWorkShiftObj.Status = true;
                    employeeWorkShiftObj.BranchUnitWorkShiftId = model.BranchUnitWorkShiftId;
                    saveIndex = _employeeWorkShiftRepository.Edit(employeeWorkShiftObj);
                }
                else
                {
                    employeeWorkShiftObj.Status = false;                                    
                    saveIndex = _employeeWorkShiftRepository.Edit(employeeWorkShiftObj);

                    if (saveIndex > 0)
                    {
                        model.StartDate = employeeWorkShiftObj.ShiftDate; // Do not remove this
                        model.EndDate = employeeWorkShiftObj.ShiftDate; // Do not remove this

                        model.Status = true;
                        model.ShiftDate = employeeWorkShiftObj.ShiftDate;
                        model.IsActive = true;
                        model.CreatedBy = PortalContext.CurrentUser.UserId;
                        model.CreatedDate = DateTime.Now;
                        saveIndex += _employeeWorkShiftRepository.Save(model);
                    }
                    
                }
              
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }   
            return saveIndex;
        }


        public bool CheckEmployeeExistingWorkShift(EmployeeWorkShift model)
        {
            var isExist = false;
            try
            {
                 isExist =
                    _employeeWorkShiftRepository.Exists(
                        x =>
                            x.BranchUnitWorkShiftId == model.BranchUnitWorkShiftId
                            && x.EmployeeId == model.EmployeeId
                            && x.ShiftDate == model.ShiftDate);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }
        public List<VEmployeeWorkShiftDetail> GetAllAssignedEmployeeWorkShift(int searchField, int pageSize, out int totalRecords, EmployeeWorkShift model,
            SearchFieldModel searchFieldModel)
        {
            List<VEmployeeWorkShiftDetail> employeeWorkShifs;
            try
            {
                employeeWorkShifs = _employeeWorkShiftRepository.GetAllAssignedEmployeeWorkShift(searchField,pageSize,out totalRecords,model,searchFieldModel);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeWorkShifs;
        }

        public int DeleteEmployeeWorkShift(int? id)
        {
         
            var deleteIndex = 0;
            try
            {
                var employeeWorkShiftObj = _employeeWorkShiftRepository.FindOne(x => x.IsActive &&x.Status&& x.EmployeeWorkShiftId == id);
                employeeWorkShiftObj.IsActive = false;
                deleteIndex = _employeeWorkShiftRepository.Edit(employeeWorkShiftObj);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return deleteIndex;
       
        }

        public VEmployeeWorkShiftDetail GetEmployeeWorkshiftDetailById(int employeeWorkShiftId)
        {
            VEmployeeWorkShiftDetail employeeWorkShift;
            try
            {


                employeeWorkShift = _employeeWorkShiftRepository.GetEmployeeWorkshiftDetailById(employeeWorkShiftId);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }

            return employeeWorkShift;
        }

        public List<VEmployeeWorkShiftDetail> GetEmployeeWorkShiftDetailBySearchKey(SearchFieldModel searchField)
        {
            List<VEmployeeWorkShiftDetail> employeeWorkShifs;
            try
            {
                employeeWorkShifs = _employeeWorkShiftRepository.GetEmployeeWorkShiftDetailBySearchKey(searchField);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeWorkShifs;
        }

        public List<EmployeesForWorkShiftCustomModel> GetEmployeesForWorkShift(EmployeeWorkShift model, SearchFieldModel searchFieldModel)
        {
            List<EmployeesForWorkShiftCustomModel> employeesForWorkShift;
            try
            {
                employeesForWorkShift = _employeeWorkShiftRepository.GetEmployeesForWorkShift(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return employeesForWorkShift;
        }

        public int SaveNewJoiningEmployeeWorkShift(Guid employeeId, DateTime? joiningDate, int branchUnitId)
        {            
            return _employeeWorkShiftRepository.SaveNewJoiningEmployeeWorkShift(employeeId, joiningDate, branchUnitId);
        }

        public int UpdateWorkShiftQuick(List<int> workShiftList, int searchByBranchUnitWorkShiftId)
        {

            return _employeeWorkShiftRepository.UpdateWorkShiftQuick(workShiftList, searchByBranchUnitWorkShiftId);

        }



    }
}
