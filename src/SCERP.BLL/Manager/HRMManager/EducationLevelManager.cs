using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EducationLevelManager : BaseManager, IEducationLevelManager
    {
        private readonly IEducationLevelRepository _educationLevelRepository = null;

        public EducationLevelManager(SCERPDBContext context)
        {
            this._educationLevelRepository = new EducationLevelRepository(context);
        }
        public List<EducationLevel> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, EducationLevel educationLevel)
        {
            List<EducationLevel> educationLevels = null;
            try
            {
                educationLevels = _educationLevelRepository.GetAllEducationLevelsByPaging(startPage, pageSize, out totalRecords, educationLevel).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }
            return educationLevels;
        }

        public EducationLevel GetEducationLevelById(int? id)
        {
            EducationLevel educationLevel = null;
            try
            {
                educationLevel = _educationLevelRepository.GetEducationLevelById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return educationLevel;
        }



        public int SaveEducationLevel(EducationLevel educationLevel)
        {
            var savedEducationLevel = 0;
            try
            {
                educationLevel.CreatedDate = DateTime.Now;
                educationLevel.CreatedBy = PortalContext.CurrentUser.UserId;
                educationLevel.IsActive = true;
                savedEducationLevel = _educationLevelRepository.Save(educationLevel);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedEducationLevel;
        }

        public int EditEducationLevel(EducationLevel educationLevel)
        {

            var editedEducationLevel = 0;
            try
            {
                educationLevel.EditedDate = DateTime.Now;
                educationLevel.EditedBy = PortalContext.CurrentUser.UserId;
                editedEducationLevel = _educationLevelRepository.Edit(educationLevel);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedEducationLevel;
        }


        public int DeleteEducationLevel(EducationLevel educationLevel)
        {
            var deletedEducationLevel = 0;
            try
            {
                educationLevel.EditedDate = DateTime.Now;
                educationLevel.EditedBy = PortalContext.CurrentUser.UserId;
                educationLevel.IsActive = false;
                deletedEducationLevel = _educationLevelRepository.Edit(educationLevel);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deletedEducationLevel;
        }

        public bool CheckExistingEducationLevel(EducationLevel educationLevel)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _educationLevelRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != educationLevel.Id &&
                            x.Title.Replace(" ", "").ToLower().Equals(educationLevel.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<EducationLevel> GetEducationLevelBySearchKey(string searchKey)
        {
            var educationLevels = new List<EducationLevel>();
            try
            {
                educationLevels =
                    _educationLevelRepository.Filter(
                        x => x.Title.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()) || String.IsNullOrEmpty(searchKey)).ToList();


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return educationLevels;
        }

        public List<EducationLevel> GetAllEducationLevels()
        {
            List<EducationLevel> educationLevels;
            try
            {
                educationLevels = _educationLevelRepository.Filter(x => x.IsActive).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return educationLevels;
        }

    }
}
