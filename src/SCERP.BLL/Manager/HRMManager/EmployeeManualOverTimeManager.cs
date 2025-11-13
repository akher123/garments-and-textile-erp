using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeManualOverTimeManager : IEmployeeManualOverTimeManager
    {
        private readonly IEmployeeManualOverTimeRepository _employeeManualOverTimeRepository;
        private readonly string _compId;

        public EmployeeManualOverTimeManager(IEmployeeManualOverTimeRepository employeeManualOverTimeRepository)
        {
            _employeeManualOverTimeRepository = employeeManualOverTimeRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }
        public int DeleteEmployeeManualOverTime(int id)
        {
            return _employeeManualOverTimeRepository.Delete(x => x.EmployeeManualOverTimeId == id);
        }

        public int EditEmployeeManualOverTime(EmployeeManualOverTime model)
        {
            EmployeeManualOverTime employeeManualOverTime = _employeeManualOverTimeRepository.FindOne(x => x.EmployeeManualOverTimeId == model.EmployeeManualOverTimeId);
            employeeManualOverTime.EmployeeManualOverTimeId = model.EmployeeManualOverTimeId;
            employeeManualOverTime.OverTimeHours = model.OverTimeHours;
            employeeManualOverTime.Date = model.Date;
            employeeManualOverTime.CreatedBy = model.CreatedBy;
            employeeManualOverTime.CreatedDate = model.CreatedDate;
            employeeManualOverTime.EditedDate = model.EditedDate;
            employeeManualOverTime.EditedBy = model.EditedBy;
            employeeManualOverTime.IsActive = model.IsActive;

            return _employeeManualOverTimeRepository.Edit(employeeManualOverTime);
        }

        public List<EmployeeManualOverTime> GetAllEmployeeManualOverTimeByPaging(EmployeeManualOverTime model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var mOverTimeList = _employeeManualOverTimeRepository.Filter(x => (x.EmployeeCardId.Trim().Contains(model.EmployeeCardId.Trim()) || String.IsNullOrEmpty(model.EmployeeCardId)));
            totalRecords = mOverTimeList.Count();
            switch (model.sort)
            {
                case "EmployeeManualOverTimeId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            mOverTimeList = mOverTimeList
                                 .OrderByDescending(r => r.EmployeeManualOverTimeId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            mOverTimeList = mOverTimeList
                                 .OrderBy(r => r.EmployeeManualOverTimeId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            mOverTimeList = mOverTimeList
                                 .OrderByDescending(r => r.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            mOverTimeList = mOverTimeList
                                 .OrderBy(r => r.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;




                default:
                    mOverTimeList = mOverTimeList
                        .OrderByDescending(r => r.EmployeeManualOverTimeId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            return mOverTimeList.ToList();
        }

        public List<EmployeeManualOverTime> GetAllEmployeeManualOverTimes()
        {
            var employeeManualOTList = _employeeManualOverTimeRepository.All();
            return employeeManualOTList.ToList();
        }

        public EmployeeManualOverTime GetEmployeeManualOverTimeById(int id)
        {
            var itemList =
                _employeeManualOverTimeRepository.Filter(x => x.EmployeeManualOverTimeId == id)
                .FirstOrDefault(x => x.EmployeeManualOverTimeId == id);

            return itemList;
        }

        public string GetNewEmployeeManualOverTimeRefId(string prifix)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeManualOverTimeExist(EmployeeManualOverTime model)
        {
            return
                _employeeManualOverTimeRepository.Exists
                    (x =>
                     x.EmployeeManualOverTimeId == model.EmployeeManualOverTimeId &&
                     x.OverTimeHours == model.OverTimeHours && x.Date == model.Date && x.CreatedBy == model.CreatedBy &&
                     x.CreatedDate == model.CreatedDate && x.EditedBy == model.EditedBy &&
                     x.EditedDate == model.EditedDate && x.IsActive == model.IsActive);
        }

        public int SaveEmployeeManualOverTime(EmployeeManualOverTime model)
        {
            return _employeeManualOverTimeRepository.Save(model);
        }
    }
}
