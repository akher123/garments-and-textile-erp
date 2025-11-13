using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class SeasonController : BaseController
    {
        private ISeasonManager seasonManager;
        public SeasonController(ISeasonManager seasonManager)
        {
            this.seasonManager = seasonManager;
        }
        [AjaxAuthorize(Roles = "season-1,season-2,season-3")]
        public ActionResult Index(SeasonViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.OmSeasons = seasonManager.GetSeasonsByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "season-2,season-3")]
        public ActionResult Edit(SeasonViewModel model)
        {
            ModelState.Clear();
            if (model.SeasonId > 0)
            {
                var season = seasonManager.GetSeasonById(model.SeasonId);
                model.SeasonName = season.SeasonName;
                model.SeasonRefId = season.SeasonRefId;
                model.SeasonId = season.SeasonId;
            }
            else
            {
                model.SeasonRefId = seasonManager.GetNewSeasonRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "season-2,season-3")]
        public ActionResult Save(OM_Season model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                var isExist = !seasonManager.CheckExistingSeason(model);
                if (isExist)
                {
                    index = model.SeasonId > 0 ? seasonManager.EditSeason(model) : seasonManager.SaveSEason(model);
                }
                else
                {
                   return  ErrorResult( model.SeasonName+" Season Name Already Exist !");
                }
             
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload() : ErrorResult("Season save fail " + errorMessage);
            }
            else
            {
                var seasonList = seasonManager.GetSeasons();
                return Json(seasonList, JsonRequestBehavior.AllowGet);
            }
            
        }

        [AjaxAuthorize(Roles = "season-3")]
        public ActionResult Delete(OM_Season model)
        {

            var saveIndex = seasonManager.DeleteSeason(model.SeasonRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Session because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
   
    }
}