using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class BrandManager : IBrandManager
    {
        private readonly IBrandRepository _brandRepository;
        public ResponsModel ResponsModel { get; set; }
        public BrandManager(SCERPDBContext context)
        {
            _brandRepository=new BrandRepository(context);
         
        }
        public ResponsModel GetBrandsByPaging(Inventory_Brand model, out int totalRecords)
        {
            ResponsModel=new ResponsModel();
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<Inventory_Brand> brands;
            try
            {
                brands = _brandRepository.GetBrandsByPaging(model);
                totalRecords = brands.Count();
                if (totalRecords > 0)
                {
                    switch (model.sort)
                    {
                        case "Name":
                            switch (model.sortdir)
                            {
                                case "DESC":
                                    brands = brands
                                        .OrderByDescending(r => r.Name)
                                        .Skip(index*pageSize)
                                        .Take(pageSize);

                                    break;
                                default:
                                    brands = brands
                                        .OrderBy(r => r.Name)
                                        .Skip(index*pageSize)
                                        .Take(pageSize);
                                    break;
                            }
                            break;
                        default:
                            brands = brands
                                .OrderByDescending(r => r.Name)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    ResponsModel.Status = true;
                    ResponsModel.Data = brands.ToList();
                }
                else
                {
                    ResponsModel.Message = "Data  not found by "+model.Name;
                    ResponsModel.Status = false;
                }
              
            }
            catch (SqlException exception)
            {
                ResponsModel.Message = "Error number: " + exception.Number + " - " + exception.Message;
                ResponsModel.Status = false;
                throw;
            }

         
            return ResponsModel;
        }

        public Inventory_Brand GetBrandById(int brandId)
        {
            return _brandRepository.FindOne(x => x.IsActive && x.BrandId == brandId);
        }

        public ResponsModel EditBrand(Inventory_Brand model)
        {
     
            try
            {
                ResponsModel = new ResponsModel();
                var inventoryBrand = _brandRepository.FindOne(x => x.BrandId == model.BrandId);
                inventoryBrand.Name = model.Name;
                inventoryBrand.Description = model.Description;
                inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryBrand.EditedDate = DateTime.Now;
                inventoryBrand.IsActive = true;
                ResponsModel.Status = _brandRepository.Edit(inventoryBrand) > 0;
           
            }
            catch (SqlException exception)
            {
                ResponsModel.Message = "Brand Not Edit ! Error number: " + exception.Number + " - " + exception.Message;
                ResponsModel.Status = false;
                throw;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Contact With Vendor";
                ResponsModel.Status = false;
                throw ;
            }

            return ResponsModel;
        }

        public ResponsModel SaveBrand(Inventory_Brand model)
        {
            try
            {
                ResponsModel = new ResponsModel();
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                ResponsModel.Status=_brandRepository.Save(model) > 0;
                if (!ResponsModel.Status)
                {
                    ResponsModel.Message = "Brand Not Edit";
                }
            }
            catch (SqlException exception)
            {
                ResponsModel.Message = "Error number: " + exception.Number + " - " + exception.Message;
                throw;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Contact With Vendor";
                throw;
            }
            return ResponsModel;

        }

        public ResponsModel DeleteBrand(int brandId)
        {
           
            try
            {
                ResponsModel=new ResponsModel();
                var inventoryBrand = _brandRepository.FindOne(x => x.BrandId == brandId);
                inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryBrand.EditedDate = DateTime.Now;
                inventoryBrand.IsActive = false;
                ResponsModel.Status = _brandRepository.Edit(inventoryBrand)>0;
                if (!ResponsModel.Status)
                {
                    ResponsModel.Message = "Fail To delete brand";
                }
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Contact With Vendor";

                throw ;
            }
            return ResponsModel;
        }

        public bool IsExistBrandName(Inventory_Brand model)
        {
            return _brandRepository.Exists(x => x.Name.ToLower().Equals(model.Name.ToLower())&&x.BrandId!=model.BrandId);
        }

        public List<Inventory_Brand> GetBrands()
        {
            return _brandRepository.Filter(x => x.IsActive).OrderBy(x=>x.Name).ToList();
        }
    }
}
