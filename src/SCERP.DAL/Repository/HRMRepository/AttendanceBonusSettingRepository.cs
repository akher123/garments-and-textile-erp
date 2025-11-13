using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class AttendanceBonusSettingRepository : Repository<AttendanceBonusSetting>, IAttendanceBonusSettingRepository
    {
        public AttendanceBonusSettingRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public AttendanceBonusSetting GetAttendanceBonusSettingById(int? id)
        {
            return Context.AttendanceBonusSetting.FirstOrDefault(x => x.AttendanceBonusSettingId == id);
        }

        public List<AttendanceBonusSetting> GetAllAttendanceBonusSettingsByPaging(int startPage, int pageSize, out int totalRecords, AttendanceBonusSetting attendanceBonusSetting)
        {
            IQueryable<AttendanceBonusSetting> attendanceBonusSettings;

            try
            {
                var searchByFromDate = attendanceBonusSetting.FromDate;
                var searchByToDate = attendanceBonusSetting.ToDate;

                attendanceBonusSettings = Context.AttendanceBonusSetting.Where(x => x.IsActive == true && x.FromDate <= searchByFromDate && x.ToDate >= searchByToDate);
                totalRecords = attendanceBonusSettings.Count();

                switch (attendanceBonusSetting.sort)
                {
                    case "AbsentDays":

                        switch (attendanceBonusSetting.sortdir)
                        {
                            case "DESC":
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderByDescending(r => r.AbsentDays)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderBy(r => r.AbsentDays)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    case "LateDays":

                        switch (attendanceBonusSetting.sortdir)
                        {
                            case "DESC":
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderByDescending(r => r.LateDays)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderBy(r => r.LateDays)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    case "LeavewithoutApplication":

                        switch (attendanceBonusSetting.sortdir)
                        {
                            case "DESC":
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderByDescending(r => r.LeavewithoutApplication)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderBy(r => r.LeavewithoutApplication)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
            
                                break;
                        }

                        break;


                    case "LeavewithApplication":

                        switch (attendanceBonusSetting.sortdir)
                        {
                            case "DESC":
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderByDescending(r => r.LeavewithApplication)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                attendanceBonusSettings = attendanceBonusSettings
                                    .OrderBy(r => r.LeavewithApplication)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    default:
                        attendanceBonusSettings = attendanceBonusSettings.OrderBy(p => p.FromDate)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);

                        break;

                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return attendanceBonusSettings.ToList();
        }

        public List<AttendanceBonusSetting> GetAllAttendanceBonusSettings()
        {
            return Context.AttendanceBonusSetting.Where(x => x.IsActive == true).OrderBy(y => y.FromDate).ToList();
        }
    }
}
