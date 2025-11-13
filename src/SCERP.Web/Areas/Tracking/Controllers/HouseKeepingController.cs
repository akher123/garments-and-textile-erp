using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class HouseKeepingController : BaseController
    {
        // GET: Tracking/HouseKeeping
        //private readonly ICashLcManager _cashLcManager;
        private readonly IHouseKeepingItemManager _houseKeepingItemManager;
        public HouseKeepingController(IHouseKeepingItemManager houseKeepingItemManager)
        {
            
            _houseKeepingItemManager = houseKeepingItemManager;
        }

       // [AjaxAuthorize(Roles = "cashlcmachinary-1,cashlmachinary-2,cashlcmachinary-3")]
        public ActionResult Index(HouseKeepingItemViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            
            model.HouseKeepingItems = _houseKeepingItemManager.GetAllHouseKeepingItemPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        //[AjaxAuthorize(Roles = "cashlcmachinary-2,cashlcmachinary-3")]
        public ActionResult Edit(HouseKeepingItemViewModel model)
        {
            ModelState.Clear();
            try
            {
               

                if (model.HouseKeepingItemId > 0)
                {
                    HouseKeepingItem houseKeepingItem = _houseKeepingItemManager.GetHouseKeepingItemById(model.HouseKeepingItemId);
                    model.HouseKeepingItem = houseKeepingItem;
                }
                else
                {

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "cashlcmachinary-2,cashlcmachinary-3")]
        public ActionResult Save(HouseKeepingItemViewModel model)
        {
            var index = 0;

            try
            {
                bool exist = _houseKeepingItemManager.IsHouseKeepingItemExist(model.HouseKeepingItem);
                if (!exist)
                {
                    if (model.HouseKeepingItem.HouseKeepingItemId > 0)
                    {
                        
                        index = _houseKeepingItemManager.EditHouseKeepingItem(model.HouseKeepingItem);
                    }
                    else
                    {
                        
                        index = _houseKeepingItemManager.SaveHouseKeepingItem(model.HouseKeepingItem);
                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }

        [AjaxAuthorize(Roles = "cashlcmachinary-3")]
        public ActionResult Delete(int houseKeepingItemId)
        {
            var index = 0;
            try
            {
                index = _houseKeepingItemManager.DeleteHouseKeepingItem(houseKeepingItemId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }

    }
}
}