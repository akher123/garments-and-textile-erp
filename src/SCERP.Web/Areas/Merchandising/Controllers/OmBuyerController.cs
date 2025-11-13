using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OmBuyerController : BaseController
    {

        private IOmBuyerManager _omBuyerManager;

        public OmBuyerController(IOmBuyerManager omBuyerManager )
        {
            this._omBuyerManager = omBuyerManager;
        }

        [AjaxAuthorize(Roles = "assignbuyer-1,assignbuyer-2,assignbuyer-3")]
        public ActionResult Index(OmBuyerViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Buyers = _omBuyerManager.GetBuyersByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "assignbuyer-2,assignbuyer-3")]
        [HttpGet]
        public ActionResult Edit(OmBuyerViewModel model)
        {
            ModelState.Clear();
            model.Countries = CountryManager.GetAllCountries();
            if (model.BuyerId > 0)
            {
                var buyer = _omBuyerManager.GetBuyerById(model.BuyerId);
                model.BuyerId = buyer.BuyerId;
                model.BuyerName = buyer.BuyerName;
                model.BuyerRefId = buyer.BuyerRefId;
                model.Address1 = buyer.Address1;
                model.Address2 = buyer.Address2;
                model.Address3 = buyer.Address3;
                model.Address3 = buyer.Address3;
                model.CountryId = buyer.CountryId;
                model.CityId = buyer.CityId;
                model.Phone = buyer.Phone;
                model.Fax = buyer.Fax;
                model.EMail = buyer.EMail;
                model.Remarks = buyer.Remarks;
                model.Cities = CityManager.GetCityByCountry(model.CountryId);
            }
            else
            {
                model.BuyerRefId = _omBuyerManager.GetNewBuyerRefId();
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "assignbuyer-2,assignbuyer-3")]
        [HttpPost]
        public ActionResult Save(OM_Buyer model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.BuyerId > 0 ? _omBuyerManager.EditBuyer(model) : _omBuyerManager.SaveBuyer(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload()  : ErrorResult( errorMessage);
            }
            else
            {
                 var buyerList= _omBuyerManager.GetAllBuyers();
                 return Json(buyerList, JsonRequestBehavior.AllowGet);
            }
           

        }
        [AjaxAuthorize(Roles = "assignbuyer-3")]
        public ActionResult Delete(OM_Buyer model)
        {

            var saveIndex = _omBuyerManager.DeleteDelete(model.BuyerRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Buyer because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingBuyer(OM_Buyer model)
        {
            var isExist = !_omBuyerManager.CheckExistingBuyer(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}