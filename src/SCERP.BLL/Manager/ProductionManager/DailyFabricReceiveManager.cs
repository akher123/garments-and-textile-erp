using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class DailyFabricReceiveManager : IDailyFabricReceiveManager
    {
        private readonly IDailyFabricReceiveRepository _dailyFabricReceiveRepository;
        public DailyFabricReceiveManager(IDailyFabricReceiveRepository dailyFabricReceiveRepository)
        {
            _dailyFabricReceiveRepository = dailyFabricReceiveRepository;
        }
        public List<SpProdDailyFabricReceive> GetVwReceivedFabricProductionSummary( string searchString, string buyerRefId, DateTime receivedDate )
        {
            var compId = PortalContext.CurrentUser.CompId;
            var fabricReceivedList = _dailyFabricReceiveRepository.GetDailyFabricReceived(compId, receivedDate, buyerRefId, searchString);
            return fabricReceivedList;

        }
        public VwReceivedFabricProductionSummary GetDailyFabricReceive(string styleName, string orderNo, string orderStyleRefId, string consRefId, string componentRefId, string colorRefId)
        {
            return _dailyFabricReceiveRepository.GetDailyFabricReceive(styleName, orderNo, orderStyleRefId, consRefId, componentRefId, colorRefId);
        }

        public int SaveDailyFabricReceive(PROD_DailyFabricReceive dailyFabricReceive)
        {
            dailyFabricReceive.CreatedBy = PortalContext.CurrentUser.UserId;
            dailyFabricReceive.CreatedDate = DateTime.Now;
            dailyFabricReceive.CompId = PortalContext.CurrentUser.CompId;
            return _dailyFabricReceiveRepository.Save(dailyFabricReceive);
        }

        public int EditDailyFabricReceive(PROD_DailyFabricReceive dailyFabricReceive)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var dailyFabric =
                _dailyFabricReceiveRepository.FindOne(
                    x => x.FabricReceiveId == dailyFabricReceive.FabricReceiveId && x.CompId == compId && x.ColorRefId == dailyFabricReceive.ColorRefId && x.ComponentRefId == dailyFabricReceive.ComponentRefId&&x.OrderStyleRefId==dailyFabricReceive.OrderStyleRefId&&x.ConsRefId==dailyFabricReceive.ConsRefId);
            dailyFabric.EditedDate = DateTime.Now;
            dailyFabric.EditedBy = PortalContext.CurrentUser.UserId;
            dailyFabric.ReceivedDate = dailyFabricReceive.ReceivedDate;
            dailyFabric.FabricQty = dailyFabricReceive.FabricQty;
            return _dailyFabricReceiveRepository.Edit(dailyFabric);
        }

        public List<PROD_DailyFabricReceive> GetDailyFabricReceiveList(string orderStyleRefId, string consRefId, string componentRefId, string colorRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _dailyFabricReceiveRepository.Filter(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == compId && x.ConsRefId == consRefId && x.ComponentRefId == componentRefId && x.ColorRefId == colorRefId).ToList();
        }

        public PROD_DailyFabricReceive GetDailyFabricReceiveById(long fabricReceiveId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _dailyFabricReceiveRepository.FindOne(x => x.FabricReceiveId == fabricReceiveId && x.CompId == compId);
        }

 
      

        public PROD_DailyFabricReceive GetDailyFabricReceiveByTodayDate(DateTime receivedDate, string orderStyleRefId, string componentRefId,
            string consRefId, string colorRefId)
        {
                string compId = PortalContext.CurrentUser.CompId;
            return _dailyFabricReceiveRepository.FindOne(x=>x.CompId==compId&&x.OrderStyleRefId==orderStyleRefId&&x.ColorRefId==colorRefId&&x.ConsRefId==consRefId&&x.ComponentRefId==componentRefId&&x.ReceivedDate==receivedDate);
        }
    }
}
