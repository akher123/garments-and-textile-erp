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

namespace SCERP.BLL.Manager.HRMManager
{
    public class SkillSetDifficultyManager : BaseManager, ISkillSetDifficultyManager
    {
        private readonly ISkillSetDifficultyRepository _skillSetDifficultyRepository = null;

        public SkillSetDifficultyManager(SCERPDBContext context)
        {
            _skillSetDifficultyRepository = new SkillSetDifficultyRepository(context);

        }

        public List<SkillSetDifficulty> GetAllSkillSetDifficultyByPaging(int startPage, int pageSize, out int totalRecords,SkillSetDifficulty skillSetDifficulty)
        {
            var skillSetDifficulties = new List<SkillSetDifficulty>();
            try
            {
                skillSetDifficulties = _skillSetDifficultyRepository.GetAllSkillSetDifficultyByPaging(startPage, pageSize, out totalRecords, skillSetDifficulty).ToList();

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return skillSetDifficulties;
        }

        public SkillSetDifficulty GetSkillSetDifficultyById(int? skillSetDifficultyId)
        {
            SkillSetDifficulty skillSetDifficulty = null;
            try
            {
                skillSetDifficulty = _skillSetDifficultyRepository.GetSkillSetDifficultyById(skillSetDifficultyId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return skillSetDifficulty;
        }

        public bool CheckExistingSkillSetDifficulty(SkillSetDifficulty skillSetDifficulty)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _skillSetDifficultyRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.SkillSetDifficultyId != skillSetDifficulty.SkillSetDifficultyId &&
                            x.DifficultyName.Replace(" ", "").ToLower().Equals(skillSetDifficulty.DifficultyName.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int EditSkillSetDifficulty(SkillSetDifficulty skillsetdifficulty)
        {
            var editedSkillSetDifficulty = 0;

            try
            {
                skillsetdifficulty.EditedDate = DateTime.Now;
                skillsetdifficulty.EditedBy = PortalContext.CurrentUser.UserId;
                editedSkillSetDifficulty = _skillSetDifficultyRepository.Edit(skillsetdifficulty);

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return editedSkillSetDifficulty;
        }


        public int SaveSkillSetDifficulty(SkillSetDifficulty skillsetdifficulty)
        {
            var saveSkillSetDifficulty = 0;
            try
            {
                skillsetdifficulty.CreatedDate = DateTime.Now;
                skillsetdifficulty.CreatedBy = PortalContext.CurrentUser.UserId;
                skillsetdifficulty.IsActive = true;
                saveSkillSetDifficulty = _skillSetDifficultyRepository.Save(skillsetdifficulty);
                 
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return saveSkillSetDifficulty;
        }

        public int DeleteSkillSetDifficulty(SkillSetDifficulty skillsetdifficulty)
        {
            var deletedSkillSetDifficulty = 0;
             
            try
            {
                skillsetdifficulty.EditedDate = DateTime.Now;
                skillsetdifficulty.EditedBy = PortalContext.CurrentUser.UserId;
                skillsetdifficulty.IsActive = false;
                deletedSkillSetDifficulty = _skillSetDifficultyRepository.Edit(skillsetdifficulty);
            }
            catch (Exception exception)
            { 
                Errorlog.WriteLog(exception);
            }
            return deletedSkillSetDifficulty;

        }



        public List<SkillSetDifficulty> GetAllSkillSetDifficulty()
        {
            List<SkillSetDifficulty> skillSetDifficulties = null;

            try
            {
                skillSetDifficulties = _skillSetDifficultyRepository.Filter(x => x.IsActive).OrderBy(x => x.DifficultyName).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return skillSetDifficulties;
        }

        public List<SkillSetDifficulty> GetAllEmployeeSkillBySearchKey(string searchKey)
        {
            var difficulties = new List<SkillSetDifficulty>();

            try
            {
                difficulties = _skillSetDifficultyRepository.GetAllEmployeeSkillBySearchKey(searchKey);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return difficulties;
        }
    }
}
