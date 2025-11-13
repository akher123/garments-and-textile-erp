using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ItemTypeController : BaseController
    {
        private readonly IItemTypeManager _itemTypeManager;

        public ItemTypeController(IItemTypeManager itemTypeManager)
        {
            _itemTypeManager = itemTypeManager;
        }
        public ActionResult Index(ItemTypeViewModel model)
        {
            try
            {
                model.ItemTypes = _itemTypeManager.GetAllItemByPagingType(model.ItemType.Title, model.CompId);
                model.TotalRecords = model.ItemTypes.Count > 0 ? model.ItemTypes.Count() : 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :"+exception);
            }
            
            return View(model);
        }

        public ActionResult Edit(ItemTypeViewModel model)
        {
            try
            {
                if (model.ItemType.ItemTypeId>0)
                {
                    OM_ItemType itemType = _itemTypeManager.GetItemTypeById(model.ItemType.ItemTypeId,PortalContext.CurrentUser.CompId);
                    model.ItemType.ItemTypeId = itemType.ItemTypeId;
                    model.ItemType.Title = itemType.Title;
                    model.ItemType.Description = itemType.Description;
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Save(ItemTypeViewModel model)
        {
            int index = 0;
            try
            {
                if (_itemTypeManager.IsItemTypeExist(model.ItemType))
                {
                    return ErrorResult("ItemType :" + model.ItemType.Title + " " + "Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.ItemType.ItemTypeId > 0 ? _itemTypeManager.EditItemType(model.ItemType) : _itemTypeManager.SaveItemType(model.ItemType);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Save/Edit :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Item Type !");
        }

        public ActionResult Delete(int itemTypeId)
        {
            int index = 0;
            try
            {
                index = _itemTypeManager.DeleteItemType(itemTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Item Type :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Item Type !");
        }
    }
}