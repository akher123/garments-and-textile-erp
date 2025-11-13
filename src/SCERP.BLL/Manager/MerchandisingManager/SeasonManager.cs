using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class SeasonManager : ISeasonManager
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly string _compId;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        public SeasonManager(ISeasonRepository seasonRepository, BuyerOrderRepository buyerOrderRepository)
        {
            _seasonRepository = seasonRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<OM_Season> GetSeasons()
        {
            return _seasonRepository.Filter(x=>x.CompId==_compId).OrderBy(x => x.SeasonName).ToList();
        }

        public List<OM_Season> GetSeasonsByPaging(OM_Season model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var seasons = _seasonRepository.Filter(x => x.CompId == _compId 
                && ((x.SeasonName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower())||String.IsNullOrEmpty(model.SearchString))
                || (x.SeasonRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = seasons.Count();
            switch (model.sort)
            {
                case "SeasonName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            seasons = seasons
                                .OrderByDescending(r => r.SeasonName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            seasons = seasons
                                .OrderBy(r => r.SeasonName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "SeasonRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            seasons = seasons
                                .OrderByDescending(r => r.SeasonRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            seasons = seasons
                                .OrderBy(r => r.SeasonRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    seasons = seasons
                        .OrderByDescending(r => r.SeasonRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return seasons.ToList();
        }

        public string GetNewSeasonRefId()
        {

            return _seasonRepository.GetNewSeasonRefId(_compId);
        }

        public OM_Season GetSeasonById(int seasonId)
        {
            return _seasonRepository.FindOne(x => x.SeasonId == seasonId);
        }

        public int EditSeason(OM_Season model)
        {
           var season= _seasonRepository.FindOne(x => x.SeasonId == model.SeasonId);
           season.SeasonName = model.SeasonName;
           return _seasonRepository.Edit(season);
        }

        public int SaveSEason(OM_Season model)
        {
            model.CompId = _compId;
            model.SeasonRefId = _seasonRepository.GetNewSeasonRefId(_compId);
            return _seasonRepository.Save(model);
        }

        public int DeleteSeason(string seasonRefId)
        {
            var isUsesd=_buyerOrderRepository.Exists(x => x.SeasonRefId == seasonRefId&&x.CompId==_compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _seasonRepository.Delete(x => x.SeasonRefId == seasonRefId);
            }
            return deleted;
        }

        public OM_Season GetSeasonsBySesonRefId(string seasonRefId)
        {
            return _seasonRepository.FindOne(x => x.SeasonRefId == seasonRefId&&x.CompId==_compId);
        }

        public bool CheckExistingSeason(OM_Season model)
        {
           return _seasonRepository.Exists( x => x.CompId == _compId && x.SeasonId != model.SeasonId &&x.SeasonName.Trim().ToLower()==model.SeasonName.Trim().ToLower());
        }
    }
}
