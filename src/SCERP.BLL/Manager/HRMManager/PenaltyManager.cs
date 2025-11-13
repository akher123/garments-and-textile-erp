using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class PenaltyManager : IPenaltyManager
    {

        private readonly IPenaltyRepository _penaltyRepository;

        public PenaltyManager(IPenaltyRepository penaltyRepository)
        {
            _penaltyRepository = penaltyRepository;
        }

        public List<VwPenaltyEmployee> GetAllPenaltyByPaging(HrmPenalty model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            IQueryable<VwPenaltyEmployee> penaltyList =
                _penaltyRepository.GetVwPenaltyEmployee(
                    x => x.IsActive == true && ((x.EmployeeName.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                                                || (x.ClaimerName.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                                                || (x.EmployeeCardId == model.SearchString || String.IsNullOrEmpty(model.SearchString))));

            totalRecords = penaltyList.Count();

            switch (model.sort)
            {
                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyList = penaltyList
                                .OrderByDescending(r => r.EmployeeCardId)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyList = penaltyList
                                .OrderBy(r => r.EmployeeCardId)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "EmployeeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyList = penaltyList
                                .OrderByDescending(r => r.EmployeeName)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyList = penaltyList
                                .OrderBy(r => r.EmployeeName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "ClaimerName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyList = penaltyList
                                .OrderByDescending(r => r.ClaimerName)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyList = penaltyList
                                .OrderBy(r => r.ClaimerName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "PenaltyType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyList = penaltyList
                                .OrderByDescending(r => r.PenaltyType)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyList = penaltyList
                                .OrderBy(r => r.PenaltyType)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "PenaltyDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            penaltyList = penaltyList
                                .OrderByDescending(r => r.PenaltyDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            penaltyList = penaltyList
                                .OrderBy(r => r.PenaltyDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


                default:
                    penaltyList = penaltyList
                        .OrderByDescending(r => r.PenaltyDate)
                        .Skip(index*pageSize)
                        .Take(pageSize);
                    break;
            }
            return penaltyList.ToList();
        }

        public bool IsPenaltyExist(HrmPenalty model)
        {
            return _penaltyRepository.Exists(x => x.PenaltyId != model.PenaltyId &&
                                                  x.EmployeeId == model.EmployeeId &&
                                                  x.PenaltyTypeId == model.PenaltyTypeId &&
                                                  x.PenaltyDate == model.PenaltyDate &&
                                                  x.IsActive);
        }

        public int SavePenalty(HrmPenalty model)
        {
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _penaltyRepository.Save(model);
        }

        public int EditePenalty(HrmPenalty model)
        {
            var penalty = _penaltyRepository.FindOne(x => x.IsActive == true && x.PenaltyId == model.PenaltyId);

            penalty.PenaltyTypeId = model.PenaltyTypeId;
            penalty.Penalty = model.Penalty;
            penalty.PenaltyDate = model.PenaltyDate;
            penalty.Reason = model.Reason;
            penalty.ClaimerId = model.ClaimerId;
            penalty.EditedBy = PortalContext.CurrentUser.UserId;
            penalty.EditedDate = DateTime.Now;
            return _penaltyRepository.Edit(penalty);
        }

        public HrmPenalty GetPenaltyByPenaltyId(int penaltyId)
        {
            return _penaltyRepository.FindOne(x => x.IsActive == true && x.PenaltyId == penaltyId);
        }

        public int DeletePenalty(int penaltyId)
        {
            HrmPenalty penalty = _penaltyRepository.FindOne(x => x.IsActive && x.PenaltyId == penaltyId);
            penalty.EditedDate = DateTime.Now;
            penalty.EditedBy = PortalContext.CurrentUser.UserId;
            penalty.IsActive = false;
            return _penaltyRepository.Edit(penalty);
        }

        public List<SPGetAbsentOtPenaltyEmployee> GetAbsentOtPenaltyEmployee(SearchFieldModel searchFieldModel)
        {
            return _penaltyRepository.GetAbsentOtPenaltyEmployee(searchFieldModel);
        }

        public int SavePenaltyEmployee(List<HrmAbsentOTPenalty> penaltyEmployees, DateTime fromDate)
        {
            return _penaltyRepository.SavePenaltyEmployee(penaltyEmployees, fromDate);
        }
    }
}
