using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using SCERP.Model.Custom;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class LeaveSettingRepository : Repository<LeaveSetting>, ILeaveSettingRepository
    {

        public LeaveSettingRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public LeaveSetting GetLeaveSettingById(int? id)
        {
            try
            {
                return
                    Context.LeaveSettings.Where(x => x.Id == id && x.IsActive == true)
                        .Include(x => x.BranchUnit.Branch)
                        .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<LeaveSetting> GetAllLeaveSettings(int startPage, int pageSize, LeaveSetting leaveSetting, SearchFieldModel searchFieldModel, out int totalRecords)
        {
            List<LeaveSetting> leaveSettings = null;

            try
            {
                var queryableLeaveSettings = Context.LeaveSettings.Include(x => x.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnit.Unit)
                    .Include(x => x.EmployeeType)
                    .Include(x => x.LeaveType)
                    .Where(x => x.IsActive&&((x.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                     searchFieldModel.SearchByBranchUnitId == 0) &&
                                                           (x.EmployeeTypeId == searchFieldModel.SearchByEmployeeTypeId ||
                                                            searchFieldModel.SearchByEmployeeTypeId == 0) &&
                                                           (x.LeaveTypeId == searchFieldModel.SearchByLeaveTypeId ||
                                                            searchFieldModel.SearchByLeaveTypeId == 0) &&
                                                           (x.BranchUnit.Branch.CompanyId ==
                                                            searchFieldModel.SearchByCompanyId ||
                                                            searchFieldModel.SearchByCompanyId == 0)));

                totalRecords = queryableLeaveSettings.Count();

                switch (leaveSetting.sort)
                {
                    case "BranchUnit.Branch.Company.Name":

                        switch (leaveSetting.sortdir)
                        {
                            case "DESC":
                                leaveSettings = queryableLeaveSettings
                                    .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;


                            default:
                                leaveSettings = queryableLeaveSettings
                                    .OrderBy(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                                break;

                        }

                        break;
                    case "BranchUnit.Unit.Name":

                        switch (leaveSetting.sortdir)
                        {
                            case "DESC":
                                leaveSettings = queryableLeaveSettings
                                    .OrderByDescending(r => r.BranchUnit.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;


                            default:
                                leaveSettings = queryableLeaveSettings
                                    .OrderBy(r => r.BranchUnit.Unit.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;
                        }
                        break;
                    case "EmployeeType.Title":

                        switch (leaveSetting.sortdir)
                        {
                            case "DESC":
                                leaveSettings = queryableLeaveSettings
                                    .OrderByDescending(r => r.EmployeeType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;


                            default:
                                leaveSettings = queryableLeaveSettings
                                    .OrderBy(r => r.EmployeeType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;
                        }
                        break;

                    case "LeaveType.Title":

                        switch (leaveSetting.sortdir)
                        {
                            case "DESC":
                                leaveSettings = queryableLeaveSettings
                                    .OrderByDescending(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;


                            default:
                                leaveSettings = queryableLeaveSettings
                                    .OrderBy(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;
                        }
                        break;

                    default:

                        switch (leaveSetting.sortdir)
                        {
                            case "DESC":
                                leaveSettings = queryableLeaveSettings
                                    .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;


                            default:
                                leaveSettings = queryableLeaveSettings
                                    .OrderBy(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                                break;

                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return leaveSettings;
        }



        public List<LeaveType> GetAllLeaveType()
        {
            return Context.LeaveTypes.Where(p => p.IsActive).OrderBy(x=>x.Title).ToList();
        }

        public List<EmployeeType> GetAllEmployeeType()
        {
            return Context.EmployeeTypes.Where(p => p.IsActive).OrderBy(x=>x.Title).ToList();
        }

        public List<LeaveSetting> GetAllLeaveSettingsBySearchKey(LeaveSetting leaveSetting, SearchFieldModel searchFieldModel)
        {
            List<LeaveSetting> leaveSettings;
            try
            {
                leaveSettings = Context.LeaveSettings.Where(x => x.IsActive == true &&
                                                                   ((x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0) &&
                                                                    (x.EmployeeTypeId == searchFieldModel.SearchByEmployeeTypeId || searchFieldModel.SearchByEmployeeTypeId == 0) &&
                                                                    (x.LeaveTypeId == searchFieldModel.SearchByLeaveTypeId || searchFieldModel.SearchByLeaveTypeId == 0) &&
                                                                    (x.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                                                                   )).Include(x => x.BranchUnit.Branch.Company)
                                                                     .Include(x => x.BranchUnit.Unit)
                                                                     .Include(x=>x.EmployeeType)
                                                                     .Include(x=>x.LeaveType)
                                                                     .OrderBy(r => r.LeaveType.Title).ToList();           
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return leaveSettings;
        }
    }
}
