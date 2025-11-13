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
    public class DepartmentLineManager : IDepartmentLineManager
    {
        private readonly IDepartmentLineRepository _departmentLineRepository = null;
        public DepartmentLineManager(SCERPDBContext context)
        {
            _departmentLineRepository = new DepartmentLineRepository(context);
        }

        public List<DepartmentLine> GetDepartmentLine(int startPage, int pageSize, out int totalRecords, DepartmentLine model,
            SearchFieldModel searchFieldModel)
        {
            List<DepartmentLine> departmentLins;
            try
            {
                departmentLins = _departmentLineRepository.GetDepartmentLine(startPage, pageSize, out totalRecords,
                    model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentLins;
        }



        public List<DepartmentLine> GetDepartmentLineBySearchKey(SearchFieldModel searchFieldModel)
        {
            List<DepartmentLine> departmentLins;
            try
            {
                departmentLins = _departmentLineRepository.GetDepartmentLineBySearchKey(searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentLins;
        }

        public int DeleteDepartmentLine(int departmentLineId)
        {
            var deleteIndex = 0;
            try
            {
                var departmentLineObj = _departmentLineRepository.FindOne(x => x.IsActive&& x.DepartmentLineId == departmentLineId);
                departmentLineObj.IsActive = false;
                departmentLineObj.EditedDate = DateTime.Now;
                departmentLineObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _departmentLineRepository.Edit(departmentLineObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public DepartmentLine GetDepartmentLineById(int departmentLineId)
        {
            DepartmentLine departmentLin;
            try
            {
                departmentLin = _departmentLineRepository.GetDepartmentLineById(departmentLineId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentLin;
        }

        public bool IsExistDepartmentLine(DepartmentLine model)
        {
            bool isExist;
            try
            {
                isExist =
                     _departmentLineRepository.Exists(
                         x =>
                             x.IsActive && x.DepartmentLineId != model.DepartmentLineId && x.BranchUnitDepartmentId == model.BranchUnitDepartmentId && x.LineId == model.LineId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return isExist;
        }

        public int EditranchDepartmentLine(DepartmentLine model)
        {
            var editIndex = 0;
            try
            {
                var departmentLineObj = _departmentLineRepository.FindOne(x => x.IsActive && x.DepartmentLineId == model.DepartmentLineId);
                departmentLineObj.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
                departmentLineObj.LineId = model.LineId;
                departmentLineObj.EditedDate = DateTime.Now;
                departmentLineObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _departmentLineRepository.Edit(departmentLineObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveDepartmentLine(DepartmentLine model)
        {
            var saveIndex = 0;
            var departmentLine = new DepartmentLine()
            {
                BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                LineId = model.LineId,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };
            try
            {
                saveIndex = _departmentLineRepository.Save(departmentLine);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public List<DepartmentLine> GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {

            List<DepartmentLine> departmentLins;
            try
            {
                departmentLins = _departmentLineRepository.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentLins;
        }
   
    }
}
