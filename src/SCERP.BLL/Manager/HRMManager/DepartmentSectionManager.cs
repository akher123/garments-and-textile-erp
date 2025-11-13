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
    public class DepartmentSectionManager : IDepartmentSectionManager
    {
        private readonly IDepartmentSectionRepository _departmentSectionRepository = null;
        public DepartmentSectionManager(SCERPDBContext context)
        {
            _departmentSectionRepository=new DepartmentSectionRepository(context);
        }

        public List<DepartmentSection> GetDepartmentSectionByPaging(int startPage, int pageSize, out int totalRecords, DepartmentSection model,
            SearchFieldModel searchFieldModel)
        {

            List<DepartmentSection> departmentSections;
            try
            {
                departmentSections = _departmentSectionRepository.GetDepartmentSectionByPaging(startPage, pageSize, out totalRecords,
                    model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentSections;
        }

        public DepartmentSection GetDepartmentSectionById(int departmentSectionId)
        {
            DepartmentSection departmentSection;
            try
            {
                departmentSection = _departmentSectionRepository.GetDepartmentSectionById(departmentSectionId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentSection;
        }

        public bool IsExistDepartmentSection(DepartmentSection model)
        {
            bool isExist;
            try
            {
                isExist =
                     _departmentSectionRepository.Exists(
                         x =>
                             x.IsActive && x.DepartmentSectionId != model.DepartmentSectionId && x.BranchUnitDepartmentId == model.BranchUnitDepartmentId && x.SectionId == model.SectionId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return isExist;
        }

        public int EditDepartmentSection(DepartmentSection model)
        {
            var editIndex = 0;
            try
            {
                var departmentSectionObj = _departmentSectionRepository.FindOne(x => x.IsActive && x.DepartmentSectionId == model.DepartmentSectionId);
                departmentSectionObj.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
                departmentSectionObj.SectionId = model.SectionId;
                departmentSectionObj.EditedDate = DateTime.Now;
                departmentSectionObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _departmentSectionRepository.Edit(departmentSectionObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveDepartmentSection(DepartmentSection model)
        {
            var saveIndex = 0;
            var departmentSection = new DepartmentSection()
            {
                BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                SectionId = model.SectionId,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };
            try
            {
                saveIndex = _departmentSectionRepository.Save(departmentSection);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public List<DepartmentSection> GetDepartmentSectionBySearchKey(SearchFieldModel searchFieldModel)
        {
            List<DepartmentSection> departmentSections;
            try
            {
                departmentSections = _departmentSectionRepository.GetDepartmentSectionBySearchKey(searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentSections;
        }

        public int DeleteDepartmentSection(int departmentSectionId)
        {
            var deleteIndex = 0;
            try
            {
                var departmentSectionObj = _departmentSectionRepository.FindOne(x => x.IsActive == true && x.DepartmentSectionId == departmentSectionId);
                departmentSectionObj.IsActive = false;
                departmentSectionObj.EditedDate = DateTime.Now;
                departmentSectionObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _departmentSectionRepository.Edit(departmentSectionObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }


        public List<DepartmentSection> GetDepartmentSectionByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            List<DepartmentSection> departmentSections;
            try
            {
                departmentSections = _departmentSectionRepository.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return departmentSections;
        }
    }
}
