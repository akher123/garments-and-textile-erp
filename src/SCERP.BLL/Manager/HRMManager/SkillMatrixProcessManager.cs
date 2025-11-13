using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SkillMatrixProcessManager : ISkillMatrixProcessManager
    {
        private readonly ISkillMatrixProcessRepository _skillMatrixProcessRepository;
        private readonly ISkillMatrixDetailRepository _skillMatrixDetailRepository;

        public SkillMatrixProcessManager(ISkillMatrixProcessRepository skillMatrixProcessRepository, ISkillMatrixDetailRepository skillMatrixDetailRepository)
        {
            _skillMatrixProcessRepository = skillMatrixProcessRepository;
            _skillMatrixDetailRepository = skillMatrixDetailRepository;
        }
        public List<HrmSkillMatrixProcess> GetAllSkillMatrixProcessByPaging(int pageIndex, string sort, string sortdir, out int totalRecords,
    string searchString)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var skillMatrixProcessList =
                _skillMatrixProcessRepository.Filter(x =>x.CompId==PortalContext.CurrentUser.CompId && x.IsActive==true && (x.ProcessName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)));
            totalRecords = skillMatrixProcessList.Count();
            switch (sort)
            {
                case "ProcessName":
                    switch (sortdir)
                    {
                        case "DESC":
                            skillMatrixProcessList = skillMatrixProcessList
                                 .OrderByDescending(r => r.ProcessName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            skillMatrixProcessList = skillMatrixProcessList
                                 .OrderBy(r => r.SkillMatrixProcessId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    skillMatrixProcessList = skillMatrixProcessList
                        .OrderByDescending(r => r.SkillMatrixProcessId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return skillMatrixProcessList.ToList();
        }
        public List<HrmSkillMatrixProcess> GetAllSkillMatrixProcess()
        {
            return _skillMatrixProcessRepository.Filter(x=>x.CompId==PortalContext.CurrentUser.CompId && x.IsActive==true).ToList();
        }

        public string GetProcessNameBySkillMatrixProcessId(int skillMatrixProcessId, string compId)
        {
            HrmSkillMatrixProcess  skillMatrixProcess= _skillMatrixProcessRepository.FindOne(x => x.CompId == compId && x.IsActive==true && x.SkillMatrixProcessId == skillMatrixProcessId);
            return skillMatrixProcess.ProcessName;
        }

        public HrmSkillMatrixProcess GetSkillMatrixProcessBySkillMatrixProcessId(int skillMatrixProcessId, string compId)
        {
            return _skillMatrixProcessRepository.FindOne(x => x.CompId == compId && x.IsActive==true && x.SkillMatrixProcessId == skillMatrixProcessId);
        }

        public bool IsSkillMatrixProcessExist(HrmSkillMatrixProcess model)
        {
            return _skillMatrixProcessRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive==true && x.SkillMatrixProcessId != model.SkillMatrixProcessId && x.ProcessName == model.ProcessName || (x.ProcessName==model.ProcessName && x.DisplayOrder==model.DisplayOrder));
        }

        public int EditSkillMatrixProcess(HrmSkillMatrixProcess model)
        {
            var skillMatrixProcess = _skillMatrixProcessRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive==true && x.SkillMatrixProcessId == model.SkillMatrixProcessId);
            skillMatrixProcess.ProcessName = model.ProcessName;
            skillMatrixProcess.DisplayOrder = model.DisplayOrder;
            skillMatrixProcess.EditedBy = PortalContext.CurrentUser.UserId;
            skillMatrixProcess.EditedDate = DateTime.Now;
            return _skillMatrixProcessRepository.Edit(skillMatrixProcess);
        }

        public int SaveSkillMatrixProcess(HrmSkillMatrixProcess model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _skillMatrixProcessRepository.Save(model);
        }

        public int DeleteSkillMatrixProcess(int skillMatrixProcessId)
        {
            var deleted = 0;
            if (_skillMatrixDetailRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.SkillMatrixProcessId == skillMatrixProcessId))
            {
                deleted = -1;// This hourId Id used by another table
            }
            else
            {
                var skillMatrixProcess = _skillMatrixProcessRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.SkillMatrixProcessId == skillMatrixProcessId);
                skillMatrixProcess.EditedBy = PortalContext.CurrentUser.UserId;
                skillMatrixProcess.EditedDate = DateTime.Now;
                skillMatrixProcess.IsActive = false;
                deleted = _skillMatrixProcessRepository.Edit(skillMatrixProcess);
            }
            return deleted;
        }
    }
}
