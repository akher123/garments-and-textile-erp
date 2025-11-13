using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class DistrictManager : BaseManager, IDistrictManager
    {
        private readonly IDistrictRepository _districtRepository = null;

        public DistrictManager(SCERPDBContext context)
        {
            _districtRepository = new DistrictRepository(context);
        }

        public List<District> GetAllDistrictsByPaging(int startPage, int pageSize, out int totalRecords, District district)
        {
            List<District> districts = null;
            try
            {
               districts = _districtRepository.GetAllDistrictsByPaging(startPage, pageSize, out totalRecords, district).ToList();
               
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return districts;
        }

        public List<District> GetAllDistricts()
        {
            List<District> district = null;

            try
            {
                district = _districtRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                district = null;
            }

            return district;
        }

        public District GetDistrictById(int? id)
        {
            District district = null;
            try
            {
                district = _districtRepository.GetDistrictById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                district = null;
            }

            return district;
        }

        public bool CheckExistingDistrict(District district)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _districtRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != district.Id &&
                            x.CountryId == district.CountryId &&
                            x.Name.Replace(" ", "").ToLower().Equals(district.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int SaveDistrict(District district)
        {
            var savedDistrict = 0;
            try
            {
                district.CreatedDate = DateTime.Now;
                district.CreatedBy = PortalContext.CurrentUser.UserId;
                district.IsActive = true;
                savedDistrict = _districtRepository.Save(district);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
              
            }

            return savedDistrict;
        }

        public int EditDistrict(District district)
        {
            var editedDistrict = 0;
            try
            {
                district.EditedDate = DateTime.Now;
                district.EditedBy = PortalContext.CurrentUser.UserId;
                editedDistrict = _districtRepository.Edit(district);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
             
            }

            return editedDistrict;
        }

        public int DeleteDistrict(District district)
        {
            var deletedDistrict = 0;
            try
            {
                district.EditedDate = DateTime.Now;
                district.EditedBy = PortalContext.CurrentUser.UserId;
                district.IsActive = false;
                deletedDistrict = _districtRepository.Edit(district);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
               
            }

            return deletedDistrict;
        }


        public List<District> GetDistrictBySearchKey(int searchByCountry, string searchByDistrict)
        {
            var districts = new List<District>();

            try
            {
                districts = _districtRepository.GetDistrictBySearchKey(searchByCountry, searchByDistrict);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return districts;
        }

        public List<District> GetDistrictsByCountry(int? countryId)
        {
            List<District> districts = null;
            try
            {
                districts = _districtRepository.GetDistrictsByCountry(countryId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
            return districts;
        }
    }
}
