using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class MerchandiserManager : IMerchandiserManager
    {
        private readonly IMerchandiserRepository _merchandiserRepository;
        private readonly string _compId;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        public MerchandiserManager(IUserMerchandiserRepository userMerchandiserRepository,IMerchandiserRepository merchandiserRepository, IBuyerOrderRepository buyerOrderRepository)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _merchandiserRepository = merchandiserRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public IEnumerable<OM_Merchandiser> GetMerchandisers()
        {
            
            return _merchandiserRepository.Filter(x => x.CompId == _compId).OrderBy(x => x.EmpName).ToList();
        }

        public List<OM_Merchandiser> GetMerchandiserByPaging(OM_Merchandiser model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var merchandiserList = _merchandiserRepository.Filter(x => x.CompId == _compId
                && ((x.EmpId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.Phone.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.EmpName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = merchandiserList.Count();
            switch (model.sort)
            {
                case "EmpName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            merchandiserList = merchandiserList
                                .OrderByDescending(r => r.EmpName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            merchandiserList = merchandiserList
                                .OrderBy(r => r.EmpName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "EmpId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            merchandiserList = merchandiserList
                                .OrderByDescending(r => r.EmpId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            merchandiserList = merchandiserList
                                .OrderBy(r => r.EmpId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    merchandiserList = merchandiserList
                        .OrderBy(r => r.EmpId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return merchandiserList.ToList();
        }

        public OM_Merchandiser GetMerchandiserById(int merchandiserId)
        {
            return _merchandiserRepository.FindOne(x => x.MerchandiserId == merchandiserId && x.CompId == _compId);
        }

        public string GetMerchandiserRefId()
        {
           return _merchandiserRepository.GetMerchandiserRefId(_compId);
        }

        public int EditMerchandiser(OM_Merchandiser model)
        {
            var merchandiser = _merchandiserRepository.FindOne(x => x.MerchandiserId == model.MerchandiserId && x.CompId == _compId);
            merchandiser.EmpName = model.EmpName;
            merchandiser.Address1 = model.Address1;
            merchandiser.Address2 = model.Address2;
            merchandiser.Address3 = model.Address3;
            merchandiser.Address3 = model.Address3;
            merchandiser.Phone = model.Phone;
            merchandiser.Email = model.Email;

            return _merchandiserRepository.Edit(merchandiser);
        }

        public int SaveMerchandiser(OM_Merchandiser model)
        {
            model.CompId = _compId;
            model.TeamId = 0;
            model.TeamLdrId ="00000";
            return _merchandiserRepository.Save(model);
        }

        public int DeleteMerchandiser(string empId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.MerchandiserId == empId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _merchandiserRepository.Delete(x => x.EmpId == empId && x.CompId == _compId);
            }
            return deleted;
        }

        public IEnumerable<OM_Merchandiser> GetPermitedMerchandisers()
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =
                _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId)
                    .Select(x => x.MerchandiserRefId)
                    .ToArray();
            return _merchandiserRepository.Filter(x => x.CompId == _compId && permitedMerchandiserLsit.Contains(x.EmpId)).OrderBy(x => x.EmpName).ToList();
        }
    }
}
