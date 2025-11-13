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
    public class SkillSetCategoryManager : BaseManager, ISkillSetCategoryManager
    {

        private readonly ISkillSetCategoryRepository _skillSetCategoryRepository = null;

        public SkillSetCategoryManager(SCERPDBContext context)
        {
            _skillSetCategoryRepository=new SkillSetCategoryRepository(context);
            
        }

        public List<SkillSetCategory> GetAllSkillSetCategoryByPaging(int startPage, int pageSize, out int totalRecords, SkillSetCategory skillSetCategory)
        {
            var skillsetCategories = new List<SkillSetCategory>();
            try
            {
                skillsetCategories = _skillSetCategoryRepository.GetAllSkillSetCategoryByPaging(startPage, pageSize, out totalRecords, skillSetCategory).ToList();

            }
            catch (Exception exception) 
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return skillsetCategories;
        }

        public bool CheckExistingSkillSetCategory(SkillSetCategory skillSetCategory)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _skillSetCategoryRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.CategoryId != skillSetCategory.CategoryId &&
                            x.CategoryName.Replace(" ", "").ToLower().Equals(skillSetCategory.CategoryName.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public SkillSetCategory GetSkillSetCategoryById(int? categoryId)
        {

            SkillSetCategory skillSetCategory = null;
            try 
            {
                skillSetCategory = _skillSetCategoryRepository.GetSkillSetCategoryById(categoryId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return skillSetCategory;
        }



        public int SaveSkillSetCategory(SkillSetCategory skillsetcategory)
        {
            var saveSkillSetCategory = 0;

            try
            {
                skillsetcategory.CreatedDate = DateTime.Now;
                skillsetcategory.CreatedBy = PortalContext.CurrentUser.UserId;
                skillsetcategory.IsActive = true;
                saveSkillSetCategory = _skillSetCategoryRepository.Save(skillsetcategory);

            }
            catch (Exception exception)
            { 
                
                Errorlog.WriteLog(exception);
            }
            return saveSkillSetCategory;

        }

        public int EditSkillSetCategory(SkillSetCategory skillsetcategory)
        {
            var editedSkillSetCategory = 0;

            try
            {
                skillsetcategory.EditedDate = DateTime.Now;
                skillsetcategory.EditedBy = PortalContext.CurrentUser.UserId;
                editedSkillSetCategory = _skillSetCategoryRepository.Edit(skillsetcategory);

            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
            }

            return editedSkillSetCategory;

        }
        public int DeleteSkillSetCategory(SkillSetCategory skillsetcategory)
        {
            var deletedSkillSetCategory = 0;

            try
            {
                skillsetcategory.EditedDate = DateTime.Now;
                skillsetcategory.EditedBy = PortalContext.CurrentUser.UserId;
                skillsetcategory.IsActive = false;
                deletedSkillSetCategory = _skillSetCategoryRepository.Edit(skillsetcategory);
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
            }

            return deletedSkillSetCategory;


        }

        public List<SkillSetCategory> GetAllSkillSetCategory()
        {
            List<SkillSetCategory> skillSetCategories = null;

            try
            {
                skillSetCategories = _skillSetCategoryRepository.Filter(x => x.IsActive).OrderBy(x => x.CategoryName).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return skillSetCategories;
        }
    }
}
