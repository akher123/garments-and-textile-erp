using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class GenderManager : BaseManager, IGenderManager
    {
        private readonly IGenderRepository _genderRepository= null;

        public GenderManager(SCERPDBContext context)
        {
            _genderRepository = new GenderRepository(context);
        }

        

        public List<Gender> GetAllGenders()
        {
            List<Gender> genders = null;

            try
            {
                genders = _genderRepository.Filter(x => x.IsActive).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return genders;
        }

        public List<Gender> GetGenders(int startPage, int pageSize, Gender gender, out int totalRecords)
        {
            List<Gender> genders;
            try
            {
                genders = _genderRepository.GetGenders(startPage, pageSize, gender, out totalRecords);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return genders;
        }

        public Gender GetGenderById(byte genderId)
        {
            Gender gender;
            try
            {
                gender =
                    _genderRepository.FindOne(x => x.IsActive && x.GenderId == genderId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return gender;
        }

        public bool IsExistGender(Gender model)
        {
            bool isExist;
            try
            {
                isExist = _genderRepository.Exists(x => x.IsActive == true
                    && x.GenderId != model.GenderId
                     && (x.Title.Replace("", " ").ToLower() == model.Title.Replace("", " ").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditGender(Gender model)
        {
            var editIndex = 0;
            try
            {
                var genderObj = _genderRepository.FindOne(x => x.IsActive && x.GenderId == model.GenderId);
                genderObj.Title = model.Title;
                genderObj.TitleInBengali = model.TitleInBengali;
                genderObj.EditedDate = DateTime.Now;
                genderObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _genderRepository.Edit(genderObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveGender(Gender model)
        {

            var saveIndex = 0;
            try
            {
                
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _genderRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
     
        }

        public int DeleteGender(int genderId)
        {

            var deleteIndex = 0;
            try
            {
                var genderObj = _genderRepository.FindOne(x => x.IsActive && x.GenderId == genderId);
        
                genderObj.EditedDate = DateTime.Now;
                genderObj.EditedBy = PortalContext.CurrentUser.UserId;
                genderObj.IsActive = false;
                deleteIndex = _genderRepository.Edit(genderObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

    }
}
