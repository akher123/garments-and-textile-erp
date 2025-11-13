using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SkillMatrixManager : ISkillMatrixManager
    {
        private readonly ISkillMatrixRepository _skillMatrixRepository;
        private readonly ISkillMatrixDetailRepository _skillMatrixDetailRepository;

        public SkillMatrixManager(ISkillMatrixRepository skillMatrixRepository, ISkillMatrixDetailRepository skillMatrixDetailRepository)
        {
            _skillMatrixRepository = skillMatrixRepository;
            _skillMatrixDetailRepository = skillMatrixDetailRepository;
        }
        public List<VwSkillMatrixEmployee> GetAllSkillMatrixByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
             var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwSkillMatrixEmployee> skillMatrixList = _skillMatrixRepository.GetAllSkillMatrixByPaging(searchString,PortalContext.CurrentUser.CompId);
            totalRecords = skillMatrixList.Count();
             switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            skillMatrixList = skillMatrixList
                                 .OrderByDescending(r => r.SkillMatrixId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            skillMatrixList = skillMatrixList
                                 .OrderBy(r => r.SkillMatrixId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    skillMatrixList = skillMatrixList
                        .OrderByDescending(r => r.SkillMatrixId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
         return skillMatrixList.ToList();
        }

        public int SaveSkillMatrix(HrmSkillMatrix model)
        {
            int saveIndex = 0;
            model.CompId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            saveIndex = _skillMatrixRepository.Save(model);
            return saveIndex;
        }

        public int EditSkillMatrix(HrmSkillMatrix model)
        {
            int edited = 0;
            int deleteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                deleteIndex = DeleteSkillMatrix(model.SkillMatrixId,PortalContext.CurrentUser.CompId);
                model.SkillMatrixId = 0;
                edited = SaveSkillMatrix(model);
                transaction.Complete();
            }
            return edited;
        }

        public bool IsSkilMatrixExist(Guid employeeId, int skillMatrixId, string compId)
        {
            return _skillMatrixRepository.Exists(x => x.IsActive == true && x.CompId == compId 
                && x.EmployeeId == employeeId && x.SkillMatrixId != skillMatrixId);
        }

        public HrmSkillMatrix GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId)
        {
            return _skillMatrixRepository.FindOne(x => x.IsActive == true && x.CompId == compId && x.SkillMatrixId == skillMatrixId);
        }

        public int DeleteSkillMatrix(int skillMatrixId, string compId)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                List<HrmSkillMatrixDetail> deTails =
                    _skillMatrixDetailRepository.Filter(
                        x => x.IsActive == true && x.CompId == compId && x.SkillMatrixId == skillMatrixId).ToList();
                foreach (var detail in deTails)
                {
                   HrmSkillMatrixDetail deTail =
                    _skillMatrixDetailRepository.FindOne(
                        x => x.IsActive == true && x.CompId == compId && x.SkillMatrixDetailId == detail.SkillMatrixDetailId);
                   deTail.IsActive = false;
                   deTail.EditedBy = PortalContext.CurrentUser.UserId;
                   deTail.EditedDate = DateTime.Now;
                   edited += _skillMatrixDetailRepository.Edit(deTail);
                }

                HrmSkillMatrix skillMatrix =
                _skillMatrixRepository.FindOne(
                    x => x.IsActive == true && x.CompId == compId && x.SkillMatrixId == skillMatrixId);
                skillMatrix.IsActive = false;
                skillMatrix.EditedBy = PortalContext.CurrentUser.UserId;
                skillMatrix.EditedDate = DateTime.Now;
                edited += _skillMatrixRepository.Edit(skillMatrix);

                transaction.Complete();
            }
            return edited;
  
        }
     }
    }

