using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class PartyManager : IPartyManager
    {
        private readonly IPartyReposity _partyReposity;
        private readonly IRepository<VwParty> partyVwReposity;
        public PartyManager(IPartyReposity partyReposity, IRepository<VwParty> partyVwReposity)
        {
            _partyReposity= partyReposity;
            this.partyVwReposity = partyVwReposity;
        }

        public List<Party> GetPartiesByPaging(Party model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            string compId = PortalContext.CurrentUser.CompId;
            var partyList = _partyReposity.Filter( x =>x.CompId==compId&&x.PType==model.PType&&(x.Name.Trim().ToLower().Contains(model.Name.Trim().ToLower()) || String.IsNullOrEmpty(model.Name.Trim())));
            totalRecords = partyList.Count();

            if (totalRecords > 0)
            {
                switch (model.sort)
                {
                    case "Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                partyList = partyList
                                    .OrderByDescending(r => r.Name)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                partyList = partyList
                                    .OrderBy(r => r.Name)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "PartyRefNo":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                partyList = partyList
                                    .OrderByDescending(r => r.PartyId)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                partyList = partyList
                                    .OrderBy(r => r.PartyId)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        partyList = partyList
                            .OrderBy(r => r.PartyId)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            return partyList.ToList();
        }

        public Party GetPartyById(long partyId)
        {
            return _partyReposity.FindOne(x => x.IsActive && x.PartyId == partyId);
        }

        public string GetNewPartyRef()
        {
            var partyRefNo = _partyReposity.Filter(x=>x.IsActive).ToList().Max(x =>Convert.ToInt64( x.PartyRefNo));
            return Convert.ToString((Convert.ToInt32(partyRefNo) + 1));
        }

        public int SaveParty(Party model)
        {
           
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            model.CompId = PortalContext.CurrentUser.CompId;
            model.PartyRefNo = GetNewPartyRef();
            return _partyReposity.Save(model);
        }

        public int EditParty(Party model)
        {
            var party = _partyReposity.FindOne(x => x.PartyId == model.PartyId);
            party.Name = model.Name;
            party.Address = model.Address;
            party.Email = model.Email;
            party.Phone = model.Phone;
            party.ContactPersonName = model.ContactPersonName;
            party.ContactPhone = model.ContactPhone;
            party.CreatedBy = PortalContext.CurrentUser.UserId;
            party.EditedDate = DateTime.Now;
            party.CompId= PortalContext.CurrentUser.CompId;
           
            return _partyReposity.Edit(party);
        }

        public List<Party> GetParties(string pType)
        {
            return _partyReposity.Filter(x => x.IsActive && x.PType==pType&&x.CompId==PortalContext.CurrentUser.CompId).OrderBy(x=>x.Name).ToList();
        }

        public int DeleteParty(long partyId)
        {
            return _partyReposity.Delete(x => x.IsActive && x.PartyId == partyId);
        }

        public List<Party> GetAllParties(string compId)
        {
            return _partyReposity.Filter(x => x.IsActive && x.CompId == compId).ToList();
        }

        public int UpdateParty(int glId, long partyId, PartyType partyType)
        {

            var party = _partyReposity.FindOne(x => x.PartyId == partyId);
            switch (partyType)
            {
                case PartyType.K:
                    party.KglId = glId;
                    break;
                case PartyType.R:
                    party.KRglId = glId;
                    break;
                case PartyType.D:
                    party.DglId = glId;
                    break;
                case PartyType.P:
                    party.PGlId = glId;
                    break;
                case PartyType.E:
                    party.EmGlId = glId;
                    break;
            }

            party.EditedBy = PortalContext.CurrentUser.UserId;
            party.EditedDate = DateTime.Now;
            return  _partyReposity.Edit(party);

        }

        public VwParty GetPartyViewById(long partyId)
        {
           return partyVwReposity.FindOne(x => x.PartyId == partyId);
        }

        public List<VwParty> GetVwPartiesByPaging(int pageIndex, string pType, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            string compId = PortalContext.CurrentUser.CompId;
            var partyList = partyVwReposity.Filter(x => x.CompId == compId && x.PType == pType && (x.Name.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString.Trim())));
            totalRecords = partyList.Count();
            partyList = partyList
                                 .OrderByDescending(r => r.PartyId)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);

            return partyList.ToList();
        }
    }


}

