using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class ItemTypeManager: IItemTypeManager
   {
       private readonly IItemTypeRepository _itemTypeRepository;

       public ItemTypeManager(IItemTypeRepository itemTypeRepository)
       {
           _itemTypeRepository = itemTypeRepository;
       }

       public List<OM_ItemType> GetAllItemByPagingType(string title, string compId)
       {
            return _itemTypeRepository.Filter(x => x.Title.Trim().Contains(title) || String.IsNullOrEmpty(title)).ToList();
        }

       public bool IsItemTypeExist(OM_ItemType model)
       {
            return _itemTypeRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.ItemTypeId != model.ItemTypeId && x.Title == model.Title);
        }

       public int EditItemType(OM_ItemType model)
       {
           OM_ItemType itemType = _itemTypeRepository.FindOne(x=>x.CompId==PortalContext.CurrentUser.CompId && x.ItemTypeId==model.ItemTypeId);
           itemType.Title = model.Title;
           itemType.Description = model.Description;
           return _itemTypeRepository.Edit(itemType);
       }

       public int SaveItemType(OM_ItemType model)
       {
           model.CompId = PortalContext.CurrentUser.CompId;
           return _itemTypeRepository.Save(model);
       }

       public int DeleteItemType(int itemTypeId)
       {
           return
               _itemTypeRepository.Delete(
                   x => x.CompId == PortalContext.CurrentUser.CompId && x.ItemTypeId == itemTypeId);
       }

       public List<OM_ItemType> GetAllItemType(string compId)
       {
           return _itemTypeRepository.Filter(x => x.CompId == compId).ToList();
       }


       public OM_ItemType GetItemTypeById(int itemTypeId, string compId)
       {
           return _itemTypeRepository.FindOne(x => x.CompId == compId && x.ItemTypeId == itemTypeId);
       }
   }
}
