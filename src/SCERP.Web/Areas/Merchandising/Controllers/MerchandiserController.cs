using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class MerchandiserController : BaseController
    {
        private IMerchandiserManager _merchandiserManager;

        public MerchandiserController(IMerchandiserManager merchandiserManager )
        {
            this._merchandiserManager = merchandiserManager;
        }
        [AjaxAuthorize(Roles = "merchandiser-1,merchandiser-2,merchandiser-3")]
        public ActionResult Index(MerchandiserViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Merchandisers = _merchandiserManager.GetMerchandiserByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
 
        }
       [AjaxAuthorize(Roles = "merchandiser-2,merchandiser-3")]
        public ActionResult Edit(MerchandiserViewModel model)
        {
            ModelState.Clear();
       
            if (model.MerchandiserId > 0)
            {
                var merchandiser = _merchandiserManager.GetMerchandiserById(model.MerchandiserId);
                model.MerchandiserId = merchandiser.MerchandiserId;
                model.EmpName = merchandiser.EmpName;
                model.EmpId = merchandiser.EmpId;
                model.Address1 = model.Address1;
                model.Address2 = model.Address2;
                model.Address3 = model.Address3;
                model.Address3 = model.Address3;
                model.Phone = model.Phone;
                model.Email = merchandiser.Email;
    
            }
            else
            {
                model.EmpId = _merchandiserManager.GetMerchandiserRefId();
            }
          
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "merchandiser-2,merchandiser-3")]
        public ActionResult Save(OM_Merchandiser model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.MerchandiserId > 0 ? _merchandiserManager.EditMerchandiser(model) : _merchandiserManager.SaveMerchandiser(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Merchandiser information save fail " + errorMessage);
        }
        [AjaxAuthorize(Roles = "merchandiser-3")]
        public ActionResult Delete(OM_Merchandiser model)
        {

            var saveIndex = _merchandiserManager.DeleteMerchandiser(model.EmpId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Merchandiser because of it's all ready used in buyer Order");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
	}
}