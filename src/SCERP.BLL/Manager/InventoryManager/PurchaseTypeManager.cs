using System;
using System.Collections.Generic;
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
   public class PurchaseTypeManager : IPurchaseTypeManager
   {
       private IPurchaseTypeRepository _purchaseTypeRepository;
       public PurchaseTypeManager(SCERPDBContext context)
       {
           _purchaseTypeRepository = new PurchaseTypeRepository(context);
       }

       public List<Inventory_PurchaseType> GetPurchaseTypesByPaging(Inventory_PurchaseType model, out int totalRecords)
       {
           int index = model.PageIndex;
           int pageSize = AppConfig.PageSize;
           IQueryable<Inventory_PurchaseType> purchaseTypes = _purchaseTypeRepository.GetPurchaseTypesByPaging(model);
           totalRecords = purchaseTypes.Count();
           switch (model.sort)
           {
               case "Title":
                   switch (model.sortdir)
                   {
                       case "DESC":
                           purchaseTypes = purchaseTypes
                               .OrderByDescending(r => r.Title)
                               .Skip(index * pageSize)
                               .Take(pageSize);

                           break;
                       default:
                           purchaseTypes = purchaseTypes
                               .OrderBy(r => r.Title)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                           break;
                   }
                   break;
               default:
                   purchaseTypes = purchaseTypes
                              .OrderByDescending(r => r.Title)
                              .Skip(index * pageSize)
                              .Take(pageSize);
                   break;
           }
           return purchaseTypes.ToList();
       }

       public Inventory_PurchaseType GetPurchaseTypeById(int purchaseTypeId)
       {
           return _purchaseTypeRepository.FindOne(x => x.IsActive && x.PurchaseTypeId == purchaseTypeId);
       }

       public int EditPurchaseType(Inventory_PurchaseType model)
       {
           var editIndex = 0;
           try
           {
               var requisitionTypes = _purchaseTypeRepository.FindOne(x => x.PurchaseTypeId == model.PurchaseTypeId);
               requisitionTypes.Title = model.Title;
               requisitionTypes.Description = model.Description;
               requisitionTypes.EditedBy = PortalContext.CurrentUser.UserId;
               requisitionTypes.EditedDate = DateTime.Now;
               requisitionTypes.IsActive = true;
               editIndex = _purchaseTypeRepository.Edit(requisitionTypes);
           }
           catch (Exception exception)
           {
               throw exception;
           }
           return editIndex;
       }

       public int SavePurchaseType(Inventory_PurchaseType model)
       {

           var saveIndex = 0;
           try
           {
               model.CreatedBy = PortalContext.CurrentUser.UserId;
               model.CreatedDate = DateTime.Now;
               model.IsActive = true;
               saveIndex = _purchaseTypeRepository.Save(model);
           }
           catch (Exception exception)
           {

               throw exception;
           }
           return saveIndex;
       }

       public int DeletePurchaseType(int purchaseTypeId)
       {
           var deleteIndex = 0;
           try
           {
               var inventoryBrand = _purchaseTypeRepository.FindOne(x => x.PurchaseTypeId == purchaseTypeId);
               inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
               inventoryBrand.EditedDate = DateTime.Now;
               inventoryBrand.IsActive = false;
               deleteIndex = _purchaseTypeRepository.Edit(inventoryBrand);
           }
           catch (Exception exception)
           {

               throw exception;
           }
           return deleteIndex;
       }

       public bool IsExistPurchaseType(Inventory_PurchaseType model)
       {
           return _purchaseTypeRepository.Exists(x => x.Title.ToLower().Equals(model.Title.ToLower()) && x.PurchaseTypeId != model.PurchaseTypeId);
       }

       public List<Inventory_PurchaseType> GetPurchaseTypes()
       {
           return _purchaseTypeRepository.Filter(x => x.IsActive).ToList();
       }
   }
}
