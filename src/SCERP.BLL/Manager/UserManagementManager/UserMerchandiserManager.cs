using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserMerchandiserManager : IUserMerchandiserManager
    {
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        private readonly IMerchandiserRepository _merchandiserRepository;
        public UserMerchandiserManager(IUserMerchandiserRepository userMerchandiserRepository, IMerchandiserRepository merchandiserRepository)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _merchandiserRepository = merchandiserRepository;
        }

        public List<UserMerchandiser> GetPermitedUserMerchandiser(Guid employeeId)
        {
            var compId = PortalContext.CurrentUser.CompId;
            return _userMerchandiserRepository.Filter(x => x.IsActive == true && x.EmployeeId == employeeId && x.CompId == compId).ToList();
        }

        public int SaveUserUserMerchandiser(List<string> merchandiserIdList, Guid employeeId)
        {
            var saveIndex = 0;
             var compId = PortalContext.CurrentUser.CompId;
            var userMerchandiserList = merchandiserIdList.Select(x => new UserMerchandiser()
            {
                EmployeeId = employeeId,
                MerchandiserRefId = x,
                CompId = PortalContext.CurrentUser.CompId,
                CreateDate = DateTime.Now,
                IsActive = true,
                CreatedBy = PortalContext.CurrentUser.UserId,
            }).ToList();
            var existingUserPermisson = _userMerchandiserRepository.Filter(x => x.EmployeeId == employeeId && x.IsActive && x.CompId == compId).ToList();
        
            var finalUserMerchandiser = userMerchandiserList;
            if (!existingUserPermisson.Any())
            {
                saveIndex = _userMerchandiserRepository.SaveList(userMerchandiserList);
            }
            else
            {
                foreach (var user in existingUserPermisson)
                {
                    var isExist =
                        userMerchandiserList.Exists(
                            x => x.EmployeeId == user.EmployeeId && x.MerchandiserRefId == user.MerchandiserRefId);
                    if (isExist)
                    {
                        finalUserMerchandiser.RemoveAll(x => x.EmployeeId == user.EmployeeId && x.MerchandiserRefId == user.MerchandiserRefId);
                    }
                    else
                    {
                        saveIndex += _userMerchandiserRepository.Delete(x => x.EmployeeId == user.EmployeeId && x.MerchandiserRefId == user.MerchandiserRefId);
                    }

                }
                saveIndex = finalUserMerchandiser.Any() ? _userMerchandiserRepository.SaveList(finalUserMerchandiser) : 1;
            }

            return saveIndex;
        }

        public bool IsUserMerchandiser(Guid? userId)
        {
          
            bool userMarchandiser = _merchandiserRepository.IsMerchandiser(userId);
            return userMarchandiser;
        }
    }
}
