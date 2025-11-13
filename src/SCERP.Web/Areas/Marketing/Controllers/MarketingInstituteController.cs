using SCERP.BLL.IManager.IMarketingManager;
using SCERP.Common;
using SCERP.Web.Areas.Marketing.Models;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Marketing.Controllers
{
   
    public class MarketingInstituteController : BaseController
    {
        private readonly IMarketingInstituteManager _marketingInstituteManager;
        public MarketingInstituteController(IMarketingInstituteManager marketingInstituteManager)
        {
            _marketingInstituteManager = marketingInstituteManager;
        }
            [AjaxAuthorize(Roles = "marketinfo-1,marketinfo-2,marketinfo-3")]
        public ActionResult Index(MarketingInstituteViewModel model)
        {

            ModelState.Clear();
            int totalRecords = 0;
            model.MarketingInstitutes = _marketingInstituteManager.GetMarketingtInstitute(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "marketinfo-2,marketinfo-3")]
        public ActionResult Edit(MarketingInstituteViewModel model)
        {
            ModelState.Clear();
            if (model.MarketingInstitute.InstituteId > 0)
            {
                model.Status = _marketingInstituteManager.GetAllMarketingStatus();
                model.MarketingInstitute = _marketingInstituteManager.GetMarketingtInstituteById(model.MarketingInstitute.InstituteId);
            }
            else
            {
                //model.MarketingInstitute.InstituteId = _marketingInstituteManager.GetNewRefId();
                model.Status = _marketingInstituteManager.GetAllMarketingStatus();
                model.MarketingInstitute.IsAvailable = true;
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "marketinfo-2,marketinfo-3")]
        public ActionResult Save(MarketingInstituteViewModel model)
        {
            try
            {
                var saved = model.MarketingInstitute.InstituteId > 0 ? _marketingInstituteManager.EditMarketingtInstitute(model.MarketingInstitute) : _marketingInstituteManager.SaveMarketingtInstitute(model.MarketingInstitute);
                return saved > 0 ? Reload() : ErrorResult("House Keeping Good Information not save successfully!!");
            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }

        [AjaxAuthorize(Roles = "marketinfo-3")]
        public ActionResult Delete(int marketingInstituteId)
        {
            var deleted = 0;
            var mi = _marketingInstituteManager.GetMarketingtInstituteById(marketingInstituteId);
            deleted = _marketingInstituteManager.DeleteMarketingtInstitute(mi);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public JsonResult GetJsonOfInstitute(int InstituteId)
        {


            var institute = _marketingInstituteManager.GetMarketingtInstituteById(InstituteId);
            return Json(new { contactperson = institute.DecisionMaker, mobile = institute.Mobile, telephone = institute.Telephone, email = institute.Email,remarks=institute.Remarks }, JsonRequestBehavior.AllowGet);

        }
    }
}