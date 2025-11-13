using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OmStyleController : BaseMerchandisingController
    {
        private readonly IOmStyleManager _omStyleManager;

        public OmStyleController(IOmStyleManager omStyleManager )
        {
            this._omStyleManager = omStyleManager;
        }
        [AjaxAuthorize(Roles = "style-1,style-2,style-3")]
        public ActionResult Index(OmStyleViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.VStyles = _omStyleManager.GetStylePaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "style-2,style-3")]
        [HttpGet]
        public ActionResult Edit(OmStyleViewModel model)
        {
            ModelState.Clear();
            if (model.StyleId > 0)
            {
                var style = _omStyleManager.GetVStyleById(model.StyleId);
                model.StyleId = style.StyleId;
                model.StylerefId = style.StylerefId;
                model.StyleName = style.StyleName;
                model.ItemId = style.ItemId;
                model.ItemName = style.ItemName;
            }
            else
            {
                model.StylerefId = _omStyleManager.GetNewStyleRefId();
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "style-2,style-3")]
        [HttpPost]
        public ActionResult Save(OM_Style model)
        {

            var index = 0;
          
            var errorMessage = "";
            try
            {
              
                if (model.ItemId>0)
                {
                    var isExist = !_omStyleManager.CheckExistingStyle(model);
                    if (isExist)
                    {
                        index = model.StyleId > 0 ? _omStyleManager.EditStyle(model) : _omStyleManager.SaveStyle(model);
                    }
                    else
                    {
                        return ErrorResult(model.StyleName+ " Style Name Already Exist !");
                    }    
                }
                else
                {
                    return ErrorResult("Invalid Item name !!!");
                }
               
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload() : ErrorResult("Style save fail " + errorMessage);
            }
            else
            {
                var styleLsit = _omStyleManager.GetAllStyles();
                return Json(styleLsit, JsonRequestBehavior.AllowGet);
            }
           
     
        }
        [AjaxAuthorize(Roles = "style-3")]
        public ActionResult Delete(OM_Style model)
        {

            var saveIndex = _omStyleManager.DeleteStyle(model.StylerefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Style because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }

        public JsonResult AutocompliteItemForStyle(string searchKey)
        {
            var items = _omStyleManager.GetItemForStyle(searchKey);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

   
    }
}
