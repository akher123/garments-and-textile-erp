using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Common;
using SCERP.Model.HRMModel;
using System.Data.SqlClient;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class WorkShiftRepository : Repository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public WorkShift GetWorkShiftById(int? id)
        {
            return Context.WorkShifts.Find(id);
        }

        public override IQueryable<WorkShift> All()
        {
            return Context.WorkShifts.Where(x => x.IsActive == true);
        }

        public List<WorkShift> GetAllWorkShiftsByPaging(int startPage, int pageSize, out int totalRecords, WorkShift workShift)
        {
            IQueryable<WorkShift> workShiftList = Context.WorkShifts
                .Where(x => x.IsActive && (x.Name.Replace(" ", "")
                .ToLower().Contains(workShift.Name.Replace(" ", "").ToLower()) || string.IsNullOrEmpty(workShift.Name)));

            totalRecords = workShiftList.Count();

            try
            {
                switch (workShift.sort)
                {
                    case "Name":

                        switch (workShift.sortdir)
                        {
                            case "DESC":
                                workShiftList = workShiftList
                                    .OrderByDescending(r => r.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                workShiftList = workShiftList
                                    .OrderBy(r => r.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "NameDetail":

                        switch (workShift.sortdir)
                        {
                            case "DESC":
                                workShiftList = workShiftList
                                    .OrderByDescending(r => r.NameDetail)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                workShiftList = workShiftList
                                    .OrderBy(r => r.NameDetail)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "StartTime":

                        switch (workShift.sortdir)
                        {
                            case "DESC":
                                workShiftList = workShiftList
                                    .OrderByDescending(r => r.StartTime)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                workShiftList = workShiftList
                                    .OrderBy(r => r.StartTime)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                    case "EndTime":

                        switch (workShift.sortdir)
                        {
                            case "DESC":
                                workShiftList = workShiftList
                                    .OrderByDescending(r => r.EndTime)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;

                            default:
                                workShiftList = workShiftList
                                    .OrderBy(r => r.EndTime)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;
                    default:

                        workShiftList = workShiftList
                                .OrderBy(r => r.Name)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return workShiftList.ToList();
        }

        public List<WorkShift> GetAllWorkShifts()
        {
            return Context.WorkShifts.Where(x => x.IsActive == true).OrderBy(r => r.Name).ToList();
        }

        public List<WorkShiftRosterDetail> GetWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            if (String.IsNullOrEmpty(shiftRoster.UnitName))
                shiftRoster.UnitName = string.Empty;
            var unitNameParam = new SqlParameter { ParameterName = "UnitName", Value = shiftRoster.UnitName };

            if (String.IsNullOrEmpty(shiftRoster.GroupName))
                shiftRoster.GroupName = string.Empty;
            var groupNameParam = new SqlParameter { ParameterName = "GroupName", Value = shiftRoster.GroupName };

            if (String.IsNullOrEmpty(shiftRoster.EmployeeCardId))
                shiftRoster.EmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = shiftRoster.EmployeeCardId.Trim() };

            if (String.IsNullOrEmpty(shiftRoster.EmployeeName))
                shiftRoster.EmployeeName = string.Empty;
            var employeeNameParam = new SqlParameter { ParameterName = "EmployeeName", Value = shiftRoster.EmployeeName.Trim() };

            if (shiftRoster.FromDate == null)
                shiftRoster.FromDate = new DateTime(1900, 01, 01);
            var fromDateParam = new SqlParameter { ParameterName = "FromDate", Value = shiftRoster.FromDate };

            List<WorkShiftRosterDetail> Roster = Context.Database.SqlQuery<WorkShiftRosterDetail>("SPGetWorkShiftRoster @UnitName, @GroupName, @EmployeeCardId, @EmployeeName, @FromDate", unitNameParam, groupNameParam, employeeCardIdParam, employeeNameParam, fromDateParam).ToList();
            return Roster;
        }

        public int SaveWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            if (String.IsNullOrEmpty(shiftRoster.UnitName))
                shiftRoster.UnitName = string.Empty;
            var unitNameParam = new SqlParameter { ParameterName = "UnitName", Value = shiftRoster.UnitName };

            if (String.IsNullOrEmpty(shiftRoster.GroupName))
                shiftRoster.GroupName = string.Empty;
            var groupNameParam = new SqlParameter { ParameterName = "GroupName", Value = shiftRoster.GroupName };

            if (String.IsNullOrEmpty(shiftRoster.EmployeeCardId))
                shiftRoster.EmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = shiftRoster.EmployeeCardId.Trim() };

            List<WorkShiftRoster> result = Context.Database.SqlQuery<WorkShiftRoster>("SPSaveWorkShiftRoster @UnitName, @GroupName, @EmployeeCardId", unitNameParam, groupNameParam, employeeCardIdParam).ToList();
            return 1;
        }

        public int ChangeWorkShiftRoster(WorkShiftRoster shiftRoster)
        {
            if (String.IsNullOrEmpty(shiftRoster.EmployeeCardId))
                shiftRoster.EmployeeCardId = string.Empty;
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = shiftRoster.EmployeeCardId.Trim() };

            var dateParam = new SqlParameter { ParameterName = "Date", Value = shiftRoster.FromDate.Value };

            if (String.IsNullOrEmpty(shiftRoster.ShiftName))
                shiftRoster.ShiftName = string.Empty;
            var shiftNameParam = new SqlParameter { ParameterName = "ShiftName", Value = shiftRoster.ShiftName };

            List<WorkShiftRoster> result = Context.Database.SqlQuery<WorkShiftRoster>("SPAssignWorkShiftRoster @EmployeeCardId, @Date, @ShiftName", employeeCardIdParam, dateParam, shiftNameParam).ToList();
            return 1;
        }
    }
}