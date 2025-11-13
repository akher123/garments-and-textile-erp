using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class AttendanceBonusRepository : Repository<AttendanceBonus>, IAttendanceBonusRepository
    {
        public AttendanceBonusRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<AttendanceBonus> GetAttendanceBonusByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, AttendanceBonus model)
        {
            IQueryable<AttendanceBonus> attendanceBonuses;

            try
            {
                attendanceBonuses = Context.AttendanceBonus
                    .Include(p => p.EmployeeDesignation)
                    .Include(p => p.EmployeeDesignation.EmployeeType)
                    .Where(p => p.IsActive &&
                          (p.EmployeeDesignation.EmployeeTypeId == searchFieldModel.SearchByEmployeeTypeId || searchFieldModel.SearchByEmployeeTypeId == 0) && 
                          (p.DesignationId == searchFieldModel.SearchByEmployeeDesignationId || searchFieldModel.SearchByEmployeeDesignationId == 0));

                totalRecords = attendanceBonuses.Count();

                switch (model.sort)
                {
                    case "EmployeeDesignation.Title":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                attendanceBonuses = attendanceBonuses
                                    .OrderByDescending(r => r.EmployeeDesignation.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                attendanceBonuses = attendanceBonuses
                                    .OrderBy(r => r.EmployeeDesignation.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "Amount":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                attendanceBonuses = attendanceBonuses
                                    .OrderByDescending(r => r.Amount)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                attendanceBonuses = attendanceBonuses
                                    .OrderBy(r => r.Amount)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "FromDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                attendanceBonuses = attendanceBonuses
                                    .OrderByDescending(r => r.FromDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                attendanceBonuses = attendanceBonuses
                                    .OrderBy(r => r.FromDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "ToDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                attendanceBonuses = attendanceBonuses
                                    .OrderByDescending(r => r.ToDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                attendanceBonuses = attendanceBonuses
                                    .OrderBy(r => r.ToDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;



                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                attendanceBonuses = attendanceBonuses
                                    .OrderByDescending(r => r.DesignationId)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                attendanceBonuses = attendanceBonuses
                                    .OrderBy(r => r.DesignationId)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }
            return attendanceBonuses.ToList();
        }

        public List<AttendanceBonus> GetAttendanceBonusesBySearchKey(int searchByEmployeeTypeId, int? searchByEmployeeDesignationId)
        {
            IQueryable<AttendanceBonus> attendanceBonus;

            try
            {
                attendanceBonus = Context.AttendanceBonus.Include(x => x.EmployeeDesignation).Include(x => x.EmployeeDesignation.EmployeeType)
                    .Where(x => x.IsActive && (x.EmployeeDesignation.EmployeeTypeId == searchByEmployeeTypeId)
                                && (x.DesignationId == searchByEmployeeTypeId || searchByEmployeeDesignationId == 0));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return attendanceBonus.ToList();
        }

        public AttendanceBonus GetAttendanceBonusById(int attendanceBonusId)
        {
            AttendanceBonus attendanceBonus;

            try
            {
                attendanceBonus = Context.AttendanceBonus.Include(x => x.EmployeeDesignation.EmployeeType.EmployeeGrade).Include(x => x.EmployeeDesignation.EmployeeType).SingleOrDefault(x => x.AttendanceBonusId == attendanceBonusId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return attendanceBonus;
        }
    }
}
