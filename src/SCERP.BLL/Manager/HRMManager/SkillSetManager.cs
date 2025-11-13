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
    public class SkillSetManager : BaseManager, ISkillSetManager
    {

        private readonly ISkillSetRepository _skillSetRepository = null;


        public SkillSetManager(SCERPDBContext context)
        {
            this._skillSetRepository = new SkillSetRepository(context);
        }

        public List<SkillSet> GetAllSkillSetsByPaging(int startPage, int pageSize, out int totalRecords, SkillSet skillSet)
        {
            List<SkillSet> skillSets = null;
            try
            {
                skillSets = _skillSetRepository.GetAllSkillSetsByPaging(startPage, pageSize,out totalRecords, skillSet);
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                Errorlog.WriteLog(exception);
            }

            return skillSets;
        }

        public SkillSet GetSkillSetById(int? id)
        {
            SkillSet skillSet = null;
            try
            {

                skillSet = _skillSetRepository.GetSkillSetById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                skillSet = null;
            }

            return skillSet;
        }

        public int SaveSkillSet(SkillSet skillSet)
        {
            var saveIndex = 0;
            try
            {
                skillSet.CreatedDate = DateTime.Now;
                skillSet.CreatedBy = PortalContext.CurrentUser.UserId;
                skillSet.IsActive = true;
                saveIndex = _skillSetRepository.Save(skillSet);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                saveIndex = 0;
            }
            return saveIndex;

        }

        public int EditSkillSet(SkillSet skillSet)
        {
            var editIndex = 0;
            try
            {
                skillSet.EditedDate = DateTime.Now;
                skillSet.EditedBy = PortalContext.CurrentUser.UserId;
           
                editIndex = _skillSetRepository.Edit(skillSet);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return editIndex;
        }

        public int DeleteSkillSet(SkillSet skillSet)
        {
            var deletedSkillSet = 0;
            try
            {

                skillSet.EditedDate = DateTime.Now;
                skillSet.EditedBy = PortalContext.CurrentUser.UserId;
                skillSet.IsActive = false;
                deletedSkillSet = _skillSetRepository.Edit(skillSet);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedSkillSet = 0;
            }

            return deletedSkillSet;
        }

        public bool IsExistSkillSets(SkillSet skillSet)
        {
            bool isExist=false;
            try
            {
               isExist= _skillSetRepository.SkillSetIsExist(skillSet);
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<SkillSet> GetSkillSetBySearchKey(string searchKey)
        {
           var skillSets=new List<SkillSet>();
            try
            {
                skillSets = !String.IsNullOrEmpty(searchKey)
                    ? _skillSetRepository.Filter(x => x.Title.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower())).ToList() 
                    : _skillSetRepository.Filter(x => x.IsActive == true).ToList();
              
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
         
            }
            return skillSets;
        }
    }
}
