using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class RequisitiontypeManager : IRequisitiontypeManager
    {
        private readonly IRquisitiontypeRepository _rquisitiontypeRepository;
        public ResponsModel ResponsModel { get; set; }
        public RequisitiontypeManager(SCERPDBContext context)
        {
            ResponsModel = new ResponsModel();
            _rquisitiontypeRepository = new RquisitiontypeRepository(context);
        }

        public List<Inventory_RequsitionType> GetRquisitiontypesByPaging(Inventory_RequsitionType model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;
            IQueryable<Inventory_RequsitionType> requsitionTypes = _rquisitiontypeRepository.GetRquisitiontypesByPaging(model);
            totalRecords = requsitionTypes.Count();
            switch (model.sort)
            {
                case "Title":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            requsitionTypes = requsitionTypes
                                .OrderByDescending(r => r.Title)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            requsitionTypes = requsitionTypes
                                .OrderBy(r => r.Title)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    requsitionTypes = requsitionTypes
                               .OrderByDescending(r => r.Title)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                    break;
            }
            return requsitionTypes.ToList();
        }

        public Inventory_RequsitionType GetRquisitiontypeById(int requisitionTypeId)
        {
            return _rquisitiontypeRepository.FindOne(x => x.IsActive && x.RequisitionTypeId == requisitionTypeId);
        }

        public ResponsModel EditRquisitiontype(Inventory_RequsitionType model)
        {

            try
            {
                var requisitionTypes = _rquisitiontypeRepository.FindOne(x => x.RequisitionTypeId == model.RequisitionTypeId);
                requisitionTypes.Title = model.Title;
                requisitionTypes.Description = model.Description;
                requisitionTypes.EditedBy = PortalContext.CurrentUser.UserId;
                requisitionTypes.EditedDate = DateTime.Now;
                requisitionTypes.IsActive = true;
                ResponsModel.Status = _rquisitiontypeRepository.Edit(requisitionTypes) > 0;
            }
            catch (SqlException exception)
            {
                ResponsModel.Message = "RequsitionType Not Edit ! Error number: " + exception.Number + " - " + exception.Message;
               
                throw;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Contact With Vendor";
          
                throw;
            }

            return ResponsModel;

        }

        public ResponsModel SaveRquisitiontype(Inventory_RequsitionType model)
        {
     
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                ResponsModel.Status = _rquisitiontypeRepository.Save(model)>0;
            }
            catch (SqlException exception)
            {
                ResponsModel.Message = "RequsitionType Not Save ! Error number: " + exception.Number + " - " + exception.Message;
                throw;
            }
            catch (Exception exception)
            {
                ResponsModel.Message = "Internal Error ! Contact With Vendor";
                throw;
            }

            return ResponsModel;
        }

        public int DeleteRequsitionType(int requisitionTypeId)
        {
            var deleteIndex = 0;
            try
            {
                var inventoryBrand = _rquisitiontypeRepository.FindOne(x => x.RequisitionTypeId == requisitionTypeId);
                inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryBrand.EditedDate = DateTime.Now;
                inventoryBrand.IsActive = false;
                deleteIndex = _rquisitiontypeRepository.Edit(inventoryBrand);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return deleteIndex;
        }

        public List<Inventory_RequsitionType> GetRquisitiontypes()
        {
            return _rquisitiontypeRepository.Filter(x => x.IsActive).ToList();
        }

        public bool IsExistRquisitiontype(Inventory_RequsitionType model)
        {
            return _rquisitiontypeRepository.Exists(x => x.Title.ToLower().Equals(model.Title.ToLower()) && x.RequisitionTypeId != model.RequisitionTypeId);
        }


    }
}
