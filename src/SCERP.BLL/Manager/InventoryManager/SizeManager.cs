using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class SizeManager : ISizeManager
    {
        private ISizeRepository _sizeRepository;
        public ResponsModel ResponsModel { get; set; }
        public SizeManager(SCERPDBContext context)
        {
            ResponsModel = new ResponsModel();
            _sizeRepository=new SizeRepository(context);
        }

        public Inventory_Size GetSizeById(int sizeId)
        {
            return _sizeRepository.FindOne(x => x.IsActive && x.SizeId == sizeId);
        }
        public ResponsModel EditSize(Inventory_Size model)
        {
            try
            {
                var inventorySize = _sizeRepository.FindOne(x => x.SizeId == model.SizeId);
                inventorySize.Title = model.Title;
                inventorySize.Description = model.Description;
                inventorySize.EditedBy = PortalContext.CurrentUser.UserId;
                inventorySize.EditedDate = DateTime.Now;
                inventorySize.IsActive = true;
                ResponsModel.Status = _sizeRepository.Edit(inventorySize)>0;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Fail to edit ? Please contact with vendor";
                throw exception;
            }
            return ResponsModel;
        }

        public ResponsModel SaveSize(Inventory_Size model)
        {
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                ResponsModel.Status = _sizeRepository.Save(model)>0;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Fail to save data ! Please contact with vendor";
                throw exception;
            }
            return ResponsModel;
        }

        public int DeleteSize(int sizeId)
        {
            var deleteIndex = 0;
            try
            {
                var inventoryBrand = _sizeRepository.FindOne(x => x.SizeId ==sizeId);
                inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryBrand.EditedDate = DateTime.Now;
                inventoryBrand.IsActive = false;
                deleteIndex = _sizeRepository.Edit(inventoryBrand);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return deleteIndex;
        }

        public bool IsExistSizeTitle(Inventory_Size model)
        {
            return _sizeRepository.Exists(x => x.Title.ToLower().Equals(model.Title.ToLower()) && x.SizeId != model.SizeId);
        }

        public ResponsModel GetSizeListByPaging(Inventory_Size model, out int totalRecords)
        {
           
            try
            {
                var index = model.PageIndex;
                var pageSize = AppConfig.PageSize;
                var sizeList = _sizeRepository.GetSizeListByPaging(model);
                totalRecords = sizeList.Count();
                if (totalRecords > 0)
                {
                    switch (model.sort)
                    {
                        case "Title":
                            switch (model.sortdir)
                            {
                                case "DESC":
                                    sizeList = sizeList
                                        .OrderByDescending(r => r.Title)
                                        .Skip(index*pageSize)
                                        .Take(pageSize);

                                    break;
                                default:
                                    sizeList = sizeList
                                        .OrderBy(r => r.Title)
                                        .Skip(index*pageSize)
                                        .Take(pageSize);
                                    break;
                            }
                            break;
                        default:
                            sizeList = sizeList
                                .OrderByDescending(r => r.Title)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    ResponsModel.Data = sizeList.ToList();
                }
                else
                {
                    ResponsModel.Message = "Size Not found by " + model.Title;
                }
                ResponsModel.Status = totalRecords>0;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Please contact with vendor";
                throw;
            }

            return ResponsModel;
        }

        public List<Inventory_Size> GetSizeList()
        {
            IQueryable<Inventory_Size> sisList;
            try
            {
               sisList= _sizeRepository.Filter(x => x.IsActive);
            }
            catch (Exception exception)
            {
                
                throw;
            }
            return sisList.ToList();
        }
    }
}
