
using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class NonProductiveTimeManager : INonProductiveTimeManager
    {
        private readonly INonProductiveTimeRepository _nonProductiveTimeRepository;
        public NonProductiveTimeManager(INonProductiveTimeRepository nonProductiveTimeRepository)
        {
            _nonProductiveTimeRepository = nonProductiveTimeRepository;
        }

        public int EditNonProductiveTime(PROD_NonProductiveTime nonProductiveTime)
        {
            PROD_NonProductiveTime npt =_nonProductiveTimeRepository.FindOne(x => x.NonProductiveTimeId == nonProductiveTime.NonProductiveTimeId);
            npt.BuyerRefId = nonProductiveTime.BuyerRefId;
            npt.OrderStyleRefId = nonProductiveTime.OrderStyleRefId;
            npt.OrderNo = nonProductiveTime.OrderNo;
            npt.MachineId = nonProductiveTime.MachineId;
            npt.Solution = nonProductiveTime.Solution;
            npt.StartTime = nonProductiveTime.StartTime;
            npt.EndTime = nonProductiveTime.EndTime;
            npt.EntryDate = nonProductiveTime.EntryDate;
            npt.Manpower = nonProductiveTime.Manpower;
            npt.Solution = nonProductiveTime.Solution;
            npt.DownTimeCategoryId = nonProductiveTime.DownTimeCategoryId;
            npt.ResponsibleDepartment = nonProductiveTime.ResponsibleDepartment;
            npt.CompId = nonProductiveTime.CompId;
            npt.Remarks = nonProductiveTime.Remarks;
            return _nonProductiveTimeRepository.Edit(npt);
        }

        public int SaveNonProductiveTime(PROD_NonProductiveTime nonProductiveTime)
        {
          return  _nonProductiveTimeRepository.Save(nonProductiveTime);
        }

        public int DeleteNpt(int nonProductiveTimeId)
        {
            PROD_NonProductiveTime npt =
                 _nonProductiveTimeRepository.FindOne(x => x.NonProductiveTimeId == nonProductiveTimeId);
            return _nonProductiveTimeRepository.DeleteOne(npt);
        }

        public PROD_NonProductiveTime GetNptById(int nonProductiveTimeId)
        {
            PROD_NonProductiveTime npt =
                _nonProductiveTimeRepository.FindOne(x => x.NonProductiveTimeId == nonProductiveTimeId);
            return npt;
        }

        public string GetNptRefId(string compId)
        {
           string maxRefId= _nonProductiveTimeRepository.Filter(x => x.CompId == compId).ToList().Max(x => x.NptRefId);
          return maxRefId.IncrementOne().PadZero(6);
        }

        public List<VwNonProductiveTime> GetNpts(DateTime? fromDate, string compId)
        {
           return _nonProductiveTimeRepository.GetNpts(fromDate, compId);
         
        }

        public List<VwNonProductiveTime> GetDateWiseNpts(DateTime? fromDate, DateTime? toDate, string compId)
        {
            return _nonProductiveTimeRepository.GetDateWiseNpts(fromDate,toDate, compId);
        }
    }
}
