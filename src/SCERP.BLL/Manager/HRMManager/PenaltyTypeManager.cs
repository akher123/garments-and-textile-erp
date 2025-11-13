using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class PenaltyTypeManager : IPenaltyTypeManager
    {
        private readonly IPenaltyTypeRepository _penaltyTypeRepository;
        public PenaltyTypeManager(IPenaltyTypeRepository penaltyTypeRepository)
        {
            _penaltyTypeRepository = penaltyTypeRepository;
        }
        public List<HrmPenaltyType> GetAllPenaltyTypeByPaging(HrmPenaltyType model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var penaltyTypeList =
                _penaltyTypeRepository.Filter(
                    x => x.IsActive && (x.Type.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = penaltyTypeList.Count();
            switch (model.sort)
            {
                case "Type":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyTypeList = penaltyTypeList
                                 .OrderByDescending(r => r.Type)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyTypeList = penaltyTypeList
                                 .OrderBy(r => r.Type).ThenBy(r => r.Type)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    penaltyTypeList = penaltyTypeList
                        .OrderByDescending(r => r.Type)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return penaltyTypeList.ToList();
        }

        public int SavePenaltyType(HrmPenaltyType model)
        {
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _penaltyTypeRepository.Save(model);
        }

        public int EditPenaltyType(HrmPenaltyType model)
        {
            var penaltyType = _penaltyTypeRepository.FindOne(x => x.PenaltyTypeId == model.PenaltyTypeId);
            penaltyType.Type = model.Type;
            penaltyType.Description = model.Description;
            penaltyType.EditedBy = PortalContext.CurrentUser.UserId;
            penaltyType.EditedDate = DateTime.Now;
            return _penaltyTypeRepository.Edit(penaltyType);
        }

        public HrmPenaltyType GetPenaltyTypeByPenaltyTypeId(int penaltyTypeId)
        {
            return _penaltyTypeRepository.FindOne(x => x.PenaltyTypeId == penaltyTypeId);
        }

        public int DeletePenaltyType(int penaltiTypeId)
        {
            HrmPenaltyType penaltyType = _penaltyTypeRepository.FindOne(x => x.IsActive == true && x.PenaltyTypeId == penaltiTypeId);
            penaltyType.EditedDate = DateTime.Now;
            penaltyType.EditedBy = PortalContext.CurrentUser.UserId;
            penaltyType.IsActive = false;
            return _penaltyTypeRepository.Edit(penaltyType);
        }

        public List<HrmPenaltyType> GetAllPenaltyTypes()
        {
            return _penaltyTypeRepository.GetAllPenaltyTypes();
        }

        public bool IsPenaltyTypeExist(HrmPenaltyType model)
        {
            return _penaltyTypeRepository.Exists(x=>x.Type==model.Type);
        }
    }
}
