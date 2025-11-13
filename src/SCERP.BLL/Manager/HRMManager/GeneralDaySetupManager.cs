using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.DAL.Repository.PlanningRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class GeneralDaySetupManager : IGeneralDaySetupManager
    {
        private readonly IGeneralDaySetupRepository _generalDaySetupRepository = null;

        public GeneralDaySetupManager(SCERPDBContext context)
        {
            _generalDaySetupRepository = new GeneralDaySetupRepository(context);
        }

        public List<GeneralDaySetup> GetGeneralDaySetup(int startPage, int pageSize, out int totalRecords, GeneralDaySetup model, SearchFieldModel searchFieldModel)
        {
            List<GeneralDaySetup> generalDaySetups;
            try
            {
                generalDaySetups = _generalDaySetupRepository.GetGeneralDaySetup(startPage, pageSize, out totalRecords, model, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return generalDaySetups;
        }


        public int DeleteGeneralDaySetup(int generalDaySetupId)
        {
            var deleteIndex = 0;
            try
            {
                var generalDaySetupObj = _generalDaySetupRepository.FindOne(x => x.IsActive && x.GeneralDaySetupId == generalDaySetupId);
                generalDaySetupObj.IsActive = false;
                generalDaySetupObj.EditedDate = DateTime.Now;
                generalDaySetupObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _generalDaySetupRepository.Edit(generalDaySetupObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }


        public GeneralDaySetup GetGeneralDaySetupById(int generalDaySetupId)
        {
            GeneralDaySetup generalDaySetup;
            try
            {
                generalDaySetup = _generalDaySetupRepository.GetGeneralDaySetupById(generalDaySetupId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return generalDaySetup;
        }


        public int EditGeneralDaySetup(GeneralDaySetup model)
        {
            var editIndex = 0;
            try
            {
                var generalDaySetupObj = _generalDaySetupRepository.FindOne(x => x.IsActive && x.GeneralDaySetupId == model.GeneralDaySetupId);
                generalDaySetupObj.BranchUnitDepartmentId = model.BranchUnitDepartmentId;
                generalDaySetupObj.DeclaredDate = model.DeclaredDate;
                generalDaySetupObj.Description = model.Description;
                generalDaySetupObj.IsAllowedExternal = model.IsAllowedExternal;
                generalDaySetupObj.IsAllowedInternal = model.IsAllowedInternal;
                generalDaySetupObj.CreatedDate = model.CreatedDate;
                generalDaySetupObj.CreatedBy = model.CreatedBy;
                generalDaySetupObj.EditedDate = DateTime.Now;
                generalDaySetupObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _generalDaySetupRepository.Edit(generalDaySetupObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }


        public int SaveGeneralDaySetup(GeneralDaySetup model)
        {
            var saveIndex = 0;
            var generalDaySetup = new GeneralDaySetup()
            {
                BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                DeclaredDate = model.DeclaredDate,
                Description = model.Description,
                IsAllowedExternal = model.IsAllowedExternal,
                IsAllowedInternal = model.IsAllowedInternal,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };
            try
            {
                saveIndex = _generalDaySetupRepository.Save(generalDaySetup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public List<GeneralDaySetup> GetGeneralDaySetupByBranchUnitDepartmentId(int generalDaySetupId)
        {

            List<GeneralDaySetup> generalDaySetups;
            try
            {
                generalDaySetups = _generalDaySetupRepository.GetGeneralDaySetupByBranchUnitDepartmentId(generalDaySetupId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return generalDaySetups;
        }



        public bool CheckExistingGeneralDaySetup(DateTime? declaredDate)
        {
            return _generalDaySetupRepository.CheckExistingGeneralDaySetup(declaredDate);
        }
    }
}
