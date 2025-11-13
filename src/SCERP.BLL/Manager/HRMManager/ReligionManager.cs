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
    public class ReligionManager : BaseManager, IReligionManager
    {
        private readonly IReligionRepository _religionRepository = null;

        public ReligionManager(SCERPDBContext context)
        {
            this._religionRepository = new ReligionRepository(context);
        }


        public List<Religion> GetAllReligionsByPaging(int startPage, int pageSize, out int totalRecords, Religion religion)
        {
            List<Religion> religions;
            try
            {
                religions = _religionRepository.GetAllReligionsByPaging(startPage, pageSize, out totalRecords, religion).ToList();
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.InnerException.Message);
            }

            return religions;
        }

        public Religion GetReligionById(int religionId)
        {
            Religion religion = null;
            try
            {
                religion = _religionRepository.GetReligionById(religionId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return religion;
        }



        public int SaveReligion(Religion religion)
        {
            var savedReligion = 0;
            try
            {
                religion.CreatedDate = DateTime.Now;
                religion.CreatedBy = PortalContext.CurrentUser.UserId;
                religion.IsActive = true;
                savedReligion = _religionRepository.Save(religion);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return savedReligion;
        }

        public int EditReligion(Religion religion)
        {
            var editedReligion = 0;
            try
            {
                religion.EditedDate = DateTime.Now;
                religion.EditedBy = PortalContext.CurrentUser.UserId;
                editedReligion = _religionRepository.Edit(religion);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return editedReligion;
        }


        public int DeleteReligion(Religion religion)
        {
            var deletedReligion = 0;
            try
            {
                religion.EditedDate = DateTime.Now;
                religion.EditedBy = PortalContext.CurrentUser.UserId;
                religion.IsActive = false;
                deletedReligion = _religionRepository.Edit(religion);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }

            return deletedReligion;
        }

        public bool CheckExistingReligion(Religion religion)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _religionRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.ReligionId != religion.ReligionId &&
                            x.Name.Replace(" ", "").ToLower().Equals(religion.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return isExist;
        }

        public List<Religion> GetReligionBySearchKey(string searchKey)
        {
            List<Religion> religions;
            try
            {
                religions = _religionRepository.Filter(
                        x =>
                            x.Name.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()) ||
                            String.IsNullOrEmpty(searchKey)).ToList();

            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return religions;
        }

        public List<Religion> GetAllReligions()
        {
            List<Religion> religions = null;

            try
            {
                religions = _religionRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return religions;
        }
      

    }
}
