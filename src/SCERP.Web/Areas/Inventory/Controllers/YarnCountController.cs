using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class YarnCountController : BaseController
    {
        private readonly IOmSizeManager _omSizeManager;

        public YarnCountController(IOmSizeManager omSizeManager)
        {
            this._omSizeManager = omSizeManager;
        }
        [AjaxAuthorize(Roles = "yarncount-1,yarncount-2,yarncount-3")]
        public ActionResult Index(OmSizeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.TypeId = "05";
            model.Sizes = _omSizeManager.GetOmSizesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
       [AjaxAuthorize(Roles = "yarncount-2,yarncount-3")]
        [HttpGet]
        public ActionResult Edit(OmSizeViewModel model)
        {
            ModelState.Clear();
            model.TypeId = "05"; //Yarn Count TypeId
            if (model.SizeId > 0)
            {
                var omSize = _omSizeManager.GetOmSizeById(model.SizeId);
                model.SizeName = omSize.SizeName;
                model.SizeRefId = omSize.SizeRefId;
                model.SizeId = omSize.SizeId;
                model.TypeId = model.TypeId;
                model.ItemType = omSize.ItemType.ToLower();
            }
            else
            {
                model.SizeRefId = _omSizeManager.GetNewOmSizeRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "yarncount-2,yarncount-3")]
        public ActionResult Save(OmSizeViewModel model)
        {
         
            var index = 0;
            var errorMessage = "";
            try
            {
                
                var size = new OM_Size
                {
                    SizeId = model.SizeId,
                    SizeRefId = model.SizeRefId,
                    SizeName = model.SizeName,
                    TypeId = model.TypeId, 
                    ItemType =  model.TypeName
                };
                bool isExist = _omSizeManager.CheckSizeExist(size);
                if (!isExist)
                {
                    index = model.SizeId > 0 ? _omSizeManager.EditOmSize(size) : _omSizeManager.SaveOmSize(size);
                }
                else
                {
                    var firstOrDefault = model.ItemTypes.FirstOrDefault(x => x.Id == size.TypeId);
                    if (firstOrDefault != null)
                        errorMessage = size.SizeName + " With Type " +
                                       firstOrDefault.Value + " Already Exist";
                }
            }
            catch (Exception exception)
            {
                errorMessage = "System Error : " + exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult( errorMessage);
        }



        [AjaxAuthorize(Roles = "yarncount-3")]
        public ActionResult Delete(OM_Size model)
        {

            var saveIndex = _omSizeManager.DeleteOmSize(model.SizeRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Siz because of it's already used in buyer Order style");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }

        public JsonResult SizeAutoComplite(string searchString,string typeId)
        {
            var sizeList = _omSizeManager.SizeAutoComplite(searchString, typeId);
            return Json(sizeList, JsonRequestBehavior.AllowGet);
        }
     
	}
}