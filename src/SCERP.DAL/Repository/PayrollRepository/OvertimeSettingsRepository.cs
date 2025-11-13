using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.PayrollRepository
{
    public class OvertimeSettingsRepository : Repository<OvertimeSettings>, IOvertimeSettingsRepository
    {
        private readonly IEmployeeDesignationRepository _employeeDesignationRepository = null;
        public OvertimeSettingsRepository(SCERPDBContext context) : base(context)
        {
            _employeeDesignationRepository=new EmployeeDesignationRepository(context);
        }

        public List<OvertimeSettings> GetAllOvertimeSettings(int startPage, int pageSize, out int totalRecords,OvertimeSettings model)
        {
            IQueryable<OvertimeSettings> overtimeSettings;
            try
            {
                overtimeSettings = Context.OvertimeSettingss
                    .Where(x => x.IsActive);
                totalRecords = overtimeSettings.Count();
                switch (model.sortdir)
                {
                    case "DESC":
                        overtimeSettings = overtimeSettings.OrderByDescending(x=>x.OvertimeRate)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                              
                        break;
                    default:
                        overtimeSettings = overtimeSettings.OrderByDescending(x => x.OvertimeRate)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return overtimeSettings.ToList();
        }

        public OvertimeSettings GetLatestOvertimeSettingInfo()
        {
            OvertimeSettings overtimeSettings = null;
            try
            {
                overtimeSettings = Context.OvertimeSettingss.FirstOrDefault(x => x.IsActive == true && x.FromDate <= DateTime.Now);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return overtimeSettings;
        }

        public int UpdateLatestOvertimeSettingInfoDate(OvertimeSettings overtimeSettings)
        {
            var updated = 0;
            try
            {
                updated = Edit(overtimeSettings);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }
    }
}
