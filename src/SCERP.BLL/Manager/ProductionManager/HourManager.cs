using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class HourManager : IHourManager
    {
        private readonly IHourRepository _hourRepository;
        private readonly ISewingOutPutProcessRepository _sewingOutPutProcessRepository;

        public HourManager(IHourRepository hourRepository, ISewingOutPutProcessRepository sewingOutPutProcessRepository)
        {
            _hourRepository = hourRepository;
            _sewingOutPutProcessRepository = sewingOutPutProcessRepository;
        }
        public List<PROD_Hour> GetAllHour()
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _hourRepository.Filter(x => x.CompId == compId).ToList();
        }

        public List<PROD_Hour> GetAllHourByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var hourList =
                _hourRepository.Filter(x =>x.HourName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                             || x.HourRefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = hourList.Count();
            switch (sort)
            {
                case "HourName":
                    switch (sortdir)
                    {
                        case "DESC":
                            hourList = hourList
                                 .OrderByDescending(r => r.HourName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            hourList = hourList
                                 .OrderBy(r => r.HourId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    hourList = hourList
                        .OrderByDescending(r => r.HourId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return hourList.ToList();
        }

        public PROD_Hour GethourById(int hourId, string compId)
        {
            return _hourRepository.FindOne(x => x.CompId == compId && x.HourId == hourId);
        }

        public bool IsHourExist(PROD_Hour model)
        {
            return _hourRepository.Exists(
            x => x.CompId == PortalContext.CurrentUser.CompId && x.HourId != model.HourId && x.HourName == model.HourName);
        }

        public int EditHour(PROD_Hour model)
        {
            var hour = _hourRepository.FindOne(x =>x.CompId==PortalContext.CurrentUser.CompId && x.HourId == model.HourId);
            hour.HourName = model.HourName;
            return _hourRepository.Edit(hour);
        }

        public int SaveHour(PROD_Hour model)
        {
            model.HourRefId = GetNewHourRefId();
            model.CompId = PortalContext.CurrentUser.CompId;
            model.Status = "O";
            return _hourRepository.Save(model);
        }

        public int DeleteHour(long hourId)
        {
            var deleted = 0;
            if (_sewingOutPutProcessRepository.Exists(x =>x.CompId==PortalContext.CurrentUser.CompId && x.HourId == hourId))
            {
                deleted = -1;// This hourId Id used by another table
            }
            else
            {
                string compId = PortalContext.CurrentUser.CompId;
                return _hourRepository.Delete(x => x.CompId == compId && x.HourId == hourId);
            }
            return deleted;
        }

        public string GetNewHourRefId()
        {
            var maxRefId = _hourRepository.All().Max(x => x.HourRefId);
            return maxRefId.IncrementOne().PadZero(2);
        }
    }
}
