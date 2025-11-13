using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class ExceptionDayManager : IExceptionDayManager
    {
        private readonly IExceptionDayRepository _exceptionDayRepository;
        public ExceptionDayManager(IExceptionDayRepository exceptionDayRepository)
        {
            _exceptionDayRepository = exceptionDayRepository;
        }
        public List<ExceptionDay> GetAllExceptionDayByPaging(ExceptionDay model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var exceptionDayList =
                _exceptionDayRepository.GetWithInclude(
                    x => ((x.BranchUnit.Branch.Company.Name.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                          || (x.BranchUnit.Branch.Name.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                          || (x.BranchUnit.Unit.Name.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                        ), "BranchUnit.Branch.Company", "BranchUnit.Unit");
            totalRecords = exceptionDayList.Count();
            switch (model.sort)
            {
                case "BranchUnit.Branch.Company.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exceptionDayList = exceptionDayList
                                 .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            exceptionDayList = exceptionDayList
                                 .OrderBy(r => r.BranchUnit.Branch.Company.Name).ThenBy(r => r.BranchUnit.Branch.Company.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BranchUnit.Branch.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exceptionDayList = exceptionDayList
                                 .OrderByDescending(r => r.BranchUnit.Branch.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            exceptionDayList = exceptionDayList
                                 .OrderBy(r => r.BranchUnit.Branch.Name).ThenBy(r => r.BranchUnit.Branch.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BranchUnit.Unit.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exceptionDayList = exceptionDayList
                                 .OrderByDescending(r => r.BranchUnit.Unit.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            exceptionDayList = exceptionDayList
                                 .OrderBy(r => r.BranchUnit.Unit.Name).ThenBy(r => r.BranchUnit.Unit.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    exceptionDayList = exceptionDayList
                        .OrderByDescending(r => r.ExceptionDayId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return exceptionDayList.ToList();
        }

        public ExceptionDay GetExceptionDayByExceptionDayId(int exceptionDayId)
        {

            var exceptionDay = _exceptionDayRepository.GetWithInclude((x => x.IsActive), "BranchUnit.Branch").FirstOrDefault(x => x.ExceptionDayId == exceptionDayId);
            if (exceptionDay!=null)
            {
                return exceptionDay;
            }
            else
            {
                throw new Exception("ExceptionDay Not found");
            }
        }

        public int SaveExceptionDay(ExceptionDay model)
        {
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _exceptionDayRepository.Save(model);
        }

        public int EditExceptionDay(ExceptionDay model)
        {
            var exceptionDay = _exceptionDayRepository.FindOne(x=>x.ExceptionDayId==model.ExceptionDayId);
            exceptionDay.EditedDate = model.EditedDate;
            exceptionDay.BranchUnitId = model.BranchUnitId;
            exceptionDay.IsExceptionForWeekend = model.IsExceptionForWeekend;
            exceptionDay.IsExceptionForHoliday = model.IsExceptionForHoliday;
            exceptionDay.IsDeclaredAsGeneralDay = model.IsExceptionForGeneralDay;
            exceptionDay.IsDeclaredAsWeekend = model.IsDeclaredAsWeekend;
            exceptionDay.IsDeclaredAsHoliday = model.IsDeclaredAsHoliday;
            exceptionDay.IsDeclaredAsGeneralDay = model.IsDeclaredAsGeneralDay;
            return _exceptionDayRepository.Edit(exceptionDay);
        }

        public int DeleteExceptionDay(int exceptionDayId)
        {
            return _exceptionDayRepository.Delete(x => x.ExceptionDayId == exceptionDayId);
        }

        public bool IsExceptionDayExist(ExceptionDay model)
        {
            return _exceptionDayRepository.Exists(x => x.BranchUnitId ==model.BranchUnitId && x.ExceptionDate==model.ExceptionDate); 
        }
    }
}
