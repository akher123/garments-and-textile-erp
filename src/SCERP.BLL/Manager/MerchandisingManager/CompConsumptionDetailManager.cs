using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class CompConsumptionDetailManager : ICompConsumptionDetailManager
    {

        private readonly ICompConsumptionDetailRepository _compConsumptionDetailRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly string _compId;
        public CompConsumptionDetailManager(ICompConsumptionDetailRepository compConsumptionDetailRepository, IConsumptionRepository consumptionRepository)
        {
            _consumptionRepository = consumptionRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _compConsumptionDetailRepository = compConsumptionDetailRepository;
        }

        public List<VCompConsumptionDetail> GetVCompConsumptionDetails(int? compomentSlNo, string consRefId, string orderStyleRefId)
        {
            var comConsDetail = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.CompId == _compId && x.CompomentSlNo == compomentSlNo && x.ConsRefId == consRefId && x.OrderStyleRefId == orderStyleRefId).OrderBy(x=>x.SizeRow);
            return comConsDetail.ToList();
        }

        public int UpdateCompConsDetail(VCompConsumptionDetail model, string updateKey)
        {
            var updateIndex = 0;
            switch (updateKey)
            {
                case "GSM":
                    updateIndex = UpdateGsm(model);
                    break;
                case "Length":
                    updateIndex = UpdateLength(model);
                    break;
                case "Width":
                    updateIndex = UpdateWidth(model);
                    break;
                case "PAllow":
                    updateIndex = UpdateAllow(model);
                    break;
                case "PDzCons":
                    updateIndex = UpdatePPQty(model);
                    break;
                case "LayQty":
                    updateIndex = UpdateLayQty(model);
                    break;
                case "ProcessLoss":
                    updateIndex = UpdateProcessLoss(model);
                    break;
                case "ApprovedLD":
                    updateIndex = UpdateApprovedLD(model);
                    break;
            }

            return updateIndex;
        }

        public List<VCompConsumptionDetail> GetComConsumptionsFabric(string orderStyleRefId)
        {
            var comConsDetails = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);//&&x.ItemCode.StartsWith("10")&&x.ConsGroup=="P"
            return comConsDetails.GroupBy(x => new { x.ConsRefId, x.OrderStyleRefId , x.GColorRefId, x.GColorName }).ToList()
                  .Select(g => new VCompConsumptionDetail()
                  {
                      GreyQty = g.Sum(x => x.CompType > 1 ? x.TQty : x.GreyQty),
                      TQty = g.Sum(x => x.TQty),
                      YConsQty = g.First().YConsQty,
                      ItemName = g.First().ItemName,
                      UnitName = g.First().UnitName,
                      GColorName = g.First().GColorName,
                      GColorRefId = g.First().GColorRefId,
                      ConsRefId = g.First().ConsRefId,
                  }).Where(h=>h.GreyQty>0).ToList();
        }

        public int UpdateFabricSize(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails =
                 _compConsumptionDetailRepository.Filter(
                     x =>
                         x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId &&
                         x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);

            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PSizeRefId = model.PSizeRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PSizeRefId = model.PSizeRefId;

                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PSizeRefId = model.PSizeRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PSizeRefId = model.PSizeRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;

            }

            return updateIndex;
        }

        public int UpdateGrWidh(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.TableWidthID = model.TableWidthID;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.TableWidthID = model.TableWidthID;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.TableWidthID = model.TableWidthID;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.TableWidthID = model.TableWidthID;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
            }
            return updateIndex;
        }

        public int UpdateGrColor(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails =
                _compConsumptionDetailRepository.Filter(
                    x =>
                        x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId &&
                        x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);

            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.BaseColorRefId = model.BaseColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(
                            x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.BaseColorRefId = model.BaseColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.BaseColorRefId = model.BaseColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.BaseColorRefId = model.BaseColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
            }
            return updateIndex;
        }


        private int UpdateCollarCuffGsm(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);

                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateGsm(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);

                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.GSM = model.GSM;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }

        private int UpdateDzConsCollarCuffDetail(OM_CompConsumptionDetail comDetail, int updateIndex)
        {
           // comDetail.RequiredQty = 
            var plPerc = comDetail.QuantityP*((comDetail.ProcessLoss + comDetail.PAllow)*0.01M);
            var ttl = comDetail.QuantityP + plPerc;
            comDetail.RequiredQty = Convert.ToInt32(ttl);
            comDetail.Weight = comDetail.RequiredQty * comDetail.PPQty;
            comDetail.TQty = comDetail.RequiredQty * comDetail.PPQty;
            updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
            return updateIndex;
        }
        private int UpdateDzConsDetail(OM_CompConsumptionDetail comDetail, int updateIndex)
        {
           
            comDetail.Weight = comDetail.QuantityP * comDetail.PPQty;
            comDetail.TQty = comDetail.Weight * (1 + (comDetail.PAllow * .01M));
            updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
            return updateIndex;
        }


        private int UpdateConsDetail(OM_CompConsumptionDetail comDetail, int updateIndex)
        {
            comDetail.PPQty = (comDetail.GSM*comDetail.Length*comDetail.Width*0.0000001M)/
                              (comDetail.LayQty == 0 ? 1 : comDetail.LayQty);
            comDetail.Weight = comDetail.QuantityP*comDetail.PPQty;
            comDetail.TQty = comDetail.Weight*(1 + (comDetail.PAllow*.01M));
            updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
            return updateIndex;
        }

        private int UpdateLength(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Length = model.Length;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Length = model.Length;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Length = model.Length;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Length = model.Length;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateWidth(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Width = model.Width;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Width = model.Width;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Width = model.Width;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.Width = model.Width;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateAllow(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateCollarCuffAllow(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PAllow = model.PAllow;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdatePPQty(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons/12.0M;

                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }

        private int UpdateCollarCuffPPQty(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;

                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PPQty = model.PDzCons / 12.0M;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateLayQty(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.LayQty = model.LayQty;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.LayQty = model.LayQty;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.LayQty = model.LayQty;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.LayQty = model.LayQty;
                        updateIndex = UpdateConsDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }

        public int UpdateFabricConsQty(string consRefId)
        {
            var effRows = 0;
            using (var transaction=new TransactionScope())
            {
                var consumtion = _consumptionRepository.FindOne(x => x.ConsRefId == consRefId && x.CompId == _compId);
                var totalQty = _compConsumptionDetailRepository.Filter(x => x.ConsRefId == consRefId && x.CompID == _compId)
                     .Sum(x => x.TQty);
                consumtion.Quantity = totalQty;
                effRows = _consumptionRepository.Edit(consumtion);
                effRows += _consumptionRepository.UpdateFabricConsuption(consRefId,_compId);
                transaction.Complete();
            }

            return effRows;

        }

     

        private int UpdateApprovedLD(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ApprovedLD = model.ApprovedLD;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ApprovedLD = model.ApprovedLD;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ApprovedLD = model.ApprovedLD;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ApprovedLD = model.ApprovedLD;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
            }
            return updateIndex;
        }
        private int UpdateProcessLoss(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex= _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
            }
            return updateIndex;
        }

        private int UpdateCollarCuffProcessLoss(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails = _compConsumptionDetailRepository.Filter(x => x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId && x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);
            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "2":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "3":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
                case "4":
                    compConsumptionDetails = compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.ProcessLoss = model.ProcessLoss;
                        updateIndex = UpdateDzConsCollarCuffDetail(comDetail, updateIndex);
                    }
                    break;
            }
            return updateIndex;
        }
        //Collar Cuff Consumtion 
        public int UpdateCollarCuffConsDetail(VCompConsumptionDetail model, string updateKey)
        
        {
            var updateIndex = 0;
            switch (updateKey)
            {
                case "GSM":
                    updateIndex = UpdateCollarCuffGsm(model);
                    break;
                case "PAllow":
                    updateIndex = UpdateCollarCuffAllow(model);
                    break;
                case "PDzCons":
                    updateIndex = UpdateCollarCuffPPQty(model);
                    break;
                case "LayQty":
                    updateIndex = UpdateLayQty(model);
                    break;
                case "ProcessLoss":

                    updateIndex = UpdateCollarCuffProcessLoss(model);
                    break;
                case "ApprovedLD":
                    updateIndex = UpdateApprovedLD(model);
                    break;
            }

            return updateIndex;
        }

        public int UpdateProductColor(VCompConsumptionDetail model)
        {
            var updateIndex = 0;
            var compConsumptionDetails =
                _compConsumptionDetailRepository.Filter(
                    x =>
                        x.CompID == _compId && x.OrderStyleRefId == model.OrderStyleRefId &&
                        x.ConsRefId == model.ConsRefId && x.CompomentSlNo == model.CompomentSlNo);

            switch (model.ConsTypeRefId)
            {
                case "1":
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PColorRefId = model.PColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "2":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(
                            x => x.GColorRefId == model.GColorRefId && x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PColorRefId = model.PColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "3":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GColorRefId == model.GColorRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PColorRefId = model.PColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
                case "4":
                    compConsumptionDetails =
                        compConsumptionDetails.Where(x => x.GSizeRefId == model.GSizeRefId);
                    foreach (var comDetail in compConsumptionDetails)
                    {
                        comDetail.PColorRefId = model.PColorRefId;
                        updateIndex += _compConsumptionDetailRepository.Edit(comDetail);
                    }
                    break;
            }
            return updateIndex;
        }
    }
}
