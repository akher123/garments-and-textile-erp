using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SkillMatrixGradeManager : ISkillMatrixGradeManager
    {
        private readonly ISkillMatrixGradeRepository _skillMatrixGradeRepository;

        public SkillMatrixGradeManager(ISkillMatrixGradeRepository skillMatrixGradeRepository)
        {
            _skillMatrixGradeRepository = skillMatrixGradeRepository;
        }
        public HrmSkillMatrixGrade GetGradeNameByProcessPercentage(int processPercentage, string compId)
        {

            return _skillMatrixGradeRepository.FindOne(x => x.CompId == compId && x.GradeValueFrom <= processPercentage && x.GradeValueTo >= processPercentage);
        }

        public List<HrmSkillMatrixGrade> GetAllSkillMatrixGradeByPaging(int pageIndex, string sort, string sortdir, out int totalRecords,
            string searchString)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var skillMatrixGradeList =
                _skillMatrixGradeRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && (x.GradeName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)));
            totalRecords = skillMatrixGradeList.Count();
            switch (sort)
            {
                case "GradeName":
                    switch (sortdir)
                    {
                        case "DESC":
                            skillMatrixGradeList = skillMatrixGradeList
                                 .OrderByDescending(r => r.GradeName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            skillMatrixGradeList = skillMatrixGradeList
                                 .OrderBy(r => r.SkillMatrixGradeId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    skillMatrixGradeList = skillMatrixGradeList
                        .OrderByDescending(r => r.SkillMatrixGradeId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return skillMatrixGradeList.ToList();
        }

        public HrmSkillMatrixGrade GetSkillMatrixGradeBySkillMatrixGradeId(int skillMatrixGradeId, string compId)
        {
            return
                _skillMatrixGradeRepository.FindOne(
                    x => x.CompId == compId && x.IsActive == true && x.SkillMatrixGradeId == skillMatrixGradeId);
        }

        public bool IsSkillMatrixGradeExist(HrmSkillMatrixGrade model)
        {
            return _skillMatrixGradeRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.SkillMatrixGradeId != model.SkillMatrixGradeId && x.GradeName== model.GradeName);
        }

        public int EditSkillMatrixGrade(HrmSkillMatrixGrade model)
        {
            var skillMatrixGrade = _skillMatrixGradeRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.SkillMatrixGradeId == model.SkillMatrixGradeId);
            skillMatrixGrade.GradeName = model.GradeName;
            skillMatrixGrade.GradeValueFrom = model.GradeValueFrom;
            skillMatrixGrade.GradeValueTo = model.GradeValueTo;
            skillMatrixGrade.EditedBy = PortalContext.CurrentUser.UserId;
            skillMatrixGrade.EditedDate = DateTime.Now;
            return _skillMatrixGradeRepository.Edit(skillMatrixGrade);
        }

        public int SaveSkillMatrixGrade(HrmSkillMatrixGrade model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _skillMatrixGradeRepository.Save(model);
        }

        public int DeleteSkillMatrixGrade(int skillMatrixGradeId)
        {
            var skillMatrixGrade = _skillMatrixGradeRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.SkillMatrixGradeId == skillMatrixGradeId);
            skillMatrixGrade.EditedBy = PortalContext.CurrentUser.UserId;
            skillMatrixGrade.EditedDate = DateTime.Now;
            skillMatrixGrade.IsActive = false;
            return _skillMatrixGradeRepository.Edit(skillMatrixGrade);
        }
    }
}
