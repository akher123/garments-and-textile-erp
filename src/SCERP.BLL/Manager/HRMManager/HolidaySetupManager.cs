using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class HolidaySetupManager:BaseManager, IHolidaySetupManager
    {

        private readonly IHolidaySetupRepository _holidaySetupRepository = null;
        private readonly IWeekendRepository _weekendRepository = null;
        public HolidaySetupManager(SCERPDBContext context)
        {
            _holidaySetupRepository = new HolidaySetupRepository(context);
            _weekendRepository = new WeekendRepository(context);

        }

        public int SaveHolidaye(HolidaysSetup holidaysSetup)
        {
            var save = 0;
            try
            {
                save = _holidaySetupRepository.Save(holidaysSetup);
            }
            catch (Exception)
            {
                save = 0;
            }
            return save;
        }

        public List<HolidaysSetup> GetAllHolidays()
        {
            var holidaysSetups = _holidaySetupRepository.All().ToList();
            return holidaysSetups;
        }

        public HolidaysSetup GetHolidayById(int id)
        {
            HolidaysSetup holidaysSetup;
            try
            {
                holidaysSetup = _holidaySetupRepository.FindOne(x => x.Id == id);
            }
            catch (Exception exception)
            {

             throw  new Exception(exception.Message);
            }
            return holidaysSetup;
        }

        public int EditHoliday(HolidaysSetup holidaysSetupObj)
        {
            var save = 0;
            try
            {
                save = _holidaySetupRepository.Edit(holidaysSetupObj);
            }
            catch (Exception)
            {
                save = 0;
            }
            return save;
        }

        public int DeleteHoliday(int id)
        {
            var delete=0;
            try
            {
                delete = _holidaySetupRepository.Delete(x => x.Id == id);
            }
            catch (Exception)
            {
                delete = 0;
            }
            return delete;
        }

        public List<Weekend> GetAllWeekends()
        {
          return  _weekendRepository.All().ToList();
        }

        public int UpdateWeekends(List<int> weekends)
        {
            var weekendStatus = 0;
            try
            {
                foreach (Weekend weekend in GetAllWeekends())
                {
                    weekend.IsActive = false;
                    _weekendRepository.Edit(weekend);
                }
                foreach (int weekendId in weekends)
                {
                    Weekend weekend = _weekendRepository.FindOne(x => x.Id == weekendId);
                    weekend.IsActive = true;
                    weekendStatus += _weekendRepository.Edit(weekend);
                }
            }
            catch (Exception)
            {

                weekendStatus = 0;
            }
            return weekendStatus;
        }

        public List<Weekend> GetAllActiveWeekends()
        {
           return _weekendRepository.Filter(x=>x.IsActive).OrderBy(x=>x.DayName).ToList();
        }

        public HolidaysSetup GetHolidayDetails(int id)
        {
          return  _holidaySetupRepository.FindOne(x => x.Id == id);
        }
    }
}
