using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.Manager.Maintenance
{
    public class ReturnableChallanReceiveManager : IReturnableChallanReceiveManager
   {
       private readonly IReturnableChallanReceiveRepository _returnableChallanReceiveRepository;
        private readonly IReturnableChallanDetailRepository _returnableChallanDetailRepository;
        public ReturnableChallanReceiveManager(IReturnableChallanReceiveRepository returnableChallanReceiveRepository, IReturnableChallanDetailRepository returnableChallanDetailRepository)
       {
           _returnableChallanReceiveRepository = returnableChallanReceiveRepository;
            _returnableChallanDetailRepository = returnableChallanDetailRepository;
       }
        public List<VwReturnableChallanReceive> GetAllReturnableChallanReceiveByReturnableChallanId(long returnableChallanId, string compId)
        {
            return
                _returnableChallanReceiveRepository.GetAllReturnableChallanReceiveByReturnableChallanId(
                    returnableChallanId, compId);
        }
        public List<VwReceiveDetail> GetChallanReceiveByDetailId(long returnableChallanDetailId, string compId)
        {
            return _returnableChallanReceiveRepository.GetChallanReceiveByDetailId(returnableChallanDetailId, compId);
        }

        public int EditReturnableChallanRecieve(Maintenance_ReturnableChallanReceive model)
        {
            int index = 0;

                string comId = PortalContext.CurrentUser.CompId;
                Maintenance_ReturnableChallanReceive challanReceive = _returnableChallanReceiveRepository.FindOne(x => x.CompId == comId && x.ReturnableChallanReceiveId == model.ReturnableChallanReceiveId);
                Maintenance_ReturnableChallanDetail detail = _returnableChallanDetailRepository.FindOne(x => x.CompId == comId && x.ReturnableChallanDetailId == challanReceive.ReturnableChallanDetailId);
                detail.ReceiveQty = Convert.ToInt32(detail.ReceiveQty) + (model.ReceiveQty - challanReceive.ReceiveQty);
                detail.RejectQty = detail.RejectQty + (model.RejectQty - challanReceive.RejectQty);
                index += _returnableChallanDetailRepository.Edit(detail);
                challanReceive.ReceiveDate = model.ReceiveDate;
                challanReceive.ReceiveQty = model.ReceiveQty;
                challanReceive.ChallanNo = model.ChallanNo;
                challanReceive.RejectQty = model.RejectQty;
                challanReceive.Amount = model.Amount;
                index += _returnableChallanReceiveRepository.Edit(challanReceive);

            return index;
        }

        public int SaveReturnableChallanReceive(Maintenance_ReturnableChallanReceive model)
        {
            int index = 0;
            model.CompId = PortalContext.CurrentUser.CompId;
            index+=_returnableChallanReceiveRepository.Save(model);
            Maintenance_ReturnableChallanDetail detail = _returnableChallanDetailRepository.FindOne(x => x.CompId == model.CompId && x.ReturnableChallanDetailId == model.ReturnableChallanDetailId);
            detail.ReceiveQty = Convert.ToInt32(detail.ReceiveQty) + model.ReceiveQty;
            detail.RejectQty = detail.RejectQty + model.RejectQty;
            index += _returnableChallanDetailRepository.Edit(detail);
            return index;
        }

        public Maintenance_ReturnableChallanReceive GetChallanReceiveByReturnableChallanReceiveId(long returnableChallanReceiveId)
        {
            return
                _returnableChallanReceiveRepository.FindOne(
                    x => x.ReturnableChallanReceiveId == returnableChallanReceiveId);
        }

        public int DeleteReturnableChallanReceive(Maintenance_ReturnableChallanReceive model)
        {
            int index = 0;
            string comId = PortalContext.CurrentUser.CompId;
             index += _returnableChallanReceiveRepository.Delete(x => x.ReturnableChallanReceiveId == model.ReturnableChallanReceiveId);
             Maintenance_ReturnableChallanDetail detail = _returnableChallanDetailRepository.FindOne(x => x.CompId == comId && x.ReturnableChallanDetailId == model.ReturnableChallanDetailId);
             detail.ReceiveQty = Convert.ToInt32(detail.ReceiveQty) - model.ReceiveQty;
             index += _returnableChallanDetailRepository.Edit(detail);
            return index;
        }

        public List<VwChallanReceiveMaster> GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId)
        {
            return _returnableChallanReceiveRepository.GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(returnableChallanReceiveMasterId, compId);
        }
   }
}
