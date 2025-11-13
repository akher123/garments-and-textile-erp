using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class UnitDepartmentManager : IUnitDepartmentManager
    {
        private readonly IUnitDepartmentRepository _unitDepartmentRepository = null;
        public UnitDepartmentManager(SCERPDBContext context)
        {
            _unitDepartmentRepository = new UnitDepartmentRepository(context);
        }

        public List<UnitDepartment> GetAllUnitDepartmentsByPaging(int startPage, int pageSize, UnitDepartment unitDepartment, out int totalRecords)
        {
            List<UnitDepartment> unitDepartments;
            try
            {
                unitDepartments = _unitDepartmentRepository.GetAllUnitDepartmentsByPaging(startPage, pageSize, unitDepartment, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
            return unitDepartments;
        }

        public UnitDepartment GetUnitDepartmentById(int unitDepartmentById)
        {
            UnitDepartment unitDepartment;
            try
            {
                unitDepartment =
                    _unitDepartmentRepository.FindOne(x => x.IsActive && x.UnitDepartmentId == unitDepartmentById);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return unitDepartment;
        }

        public bool IsExistUnitDepartment(UnitDepartment unitDepartment)
        {
            bool isExist;
            try
            {
                isExist = _unitDepartmentRepository.Exists(x => x.IsActive == true
                    && x.UnitDepartmentId != unitDepartment.UnitDepartmentId
                     && x.UnitId == unitDepartment.UnitId
                      && x.DepartmentId == unitDepartment.DepartmentId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditUnitDepartment(UnitDepartment unitDepartment)
        {
            var editIndex = 0;
            try
            {
                var unitDepartmentObj = _unitDepartmentRepository.FindOne(x => x.IsActive && x.UnitDepartmentId == unitDepartment.UnitDepartmentId);
                unitDepartmentObj.UnitId = unitDepartment.UnitId;
                unitDepartmentObj.DepartmentId = unitDepartment.DepartmentId;
                unitDepartmentObj.EditedDate = DateTime.Now;
                unitDepartmentObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _unitDepartmentRepository.Edit(unitDepartmentObj);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveUnitDepartment(UnitDepartment unitDepartment)
        {
            var saveIndex = 0;
            try
            {
                unitDepartment.CreatedBy = PortalContext.CurrentUser.UserId;
                unitDepartment.CreatedDate = DateTime.Now;
                unitDepartment.IsActive = true;
                saveIndex = _unitDepartmentRepository.Save(unitDepartment);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteUnitDepartment(int unitDepartmentId)
        {
            var deleteIndex = 0;
            try
            {
                var unitDepartmentObj = _unitDepartmentRepository.FindOne(x => x.IsActive == true && x.UnitDepartmentId == unitDepartmentId);
                unitDepartmentObj.IsActive = false;
                unitDepartmentObj.EditedDate = DateTime.Now;
                unitDepartmentObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _unitDepartmentRepository.Edit(unitDepartmentObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<UnitDepartment> GetAllUnitDepartmentsBySearchKey(UnitDepartment unitDepartment)
        {

            List<UnitDepartment> unitDepartments = null;
            try
            {
                unitDepartments = _unitDepartmentRepository.All()
                                  .Where(x => x.IsActive == true && 
                                      (x.UnitId == unitDepartment.UnitId || unitDepartment.UnitId == 0) &&
                                      (x.DepartmentId == unitDepartment.DepartmentId || unitDepartment.DepartmentId == 0))
                                      .Include(x => x.Unit)
                                      .Include(x => x.Department)
                                      .OrderBy(r => r.Unit.Name)
                                      .ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return unitDepartments;
        }

        public IEnumerable GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            IEnumerable unitDepartments;
            try
            {
                
                unitDepartments =_unitDepartmentRepository.GetAllUnitDepatmeByBranchUnitId(branchUnitId);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return unitDepartments;
        }

        public List<UnitDepartment> GetUnitDepartmentByUnitId(int unitId)
        {
            List<UnitDepartment> unitDepartments;
            try
            {
                unitDepartments =
                    _unitDepartmentRepository.Filter(x => x.IsActive && x.UnitId == unitId)
                        .Include(x => x.Department)
                        .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
            return unitDepartments;
        }
    }
}
