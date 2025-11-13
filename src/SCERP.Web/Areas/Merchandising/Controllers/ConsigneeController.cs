using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ConsigneeController : BaseController
    {
        private readonly IConsigneeManager _consigneeManager;
        private readonly IOmBuyerManager _omBuyerManager;

        public ConsigneeController(IConsigneeManager consigneeManager, IOmBuyerManager omBuyerManager)
        {
            this._consigneeManager = consigneeManager;
            this._omBuyerManager = omBuyerManager;
        }
       [AjaxAuthorize(Roles = "consignee-1,consignee-2,consignee-3")]
        public ActionResult Index(ConsigneeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Consignees = _consigneeManager.GetConsigneesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
       [AjaxAuthorize(Roles = "consignee-2,consignee-3")]
        public ActionResult Edit(ConsigneeViewModel model)
        {
            ModelState.Clear();
            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            model.Countries = CountryManager.GetAllCountries();
            if (model.ConsigneeId > 0)
            {
                var consignee = _consigneeManager.GetConsigneeById(model.ConsigneeId);
                model.ConsigneeId = consignee.ConsigneeId;
                model.ConsigneeName = consignee.ConsigneeName;
                model.ConsigneeRefId = consignee.ConsigneeRefId;
                model.Address1 = consignee.Address1;
                model.Address2 = consignee.Address2;
                model.Address3 = consignee.Address3;
                model.Address3 = consignee.Address3;
                model.CountryId = consignee.CountryId;
                model.CItyId = consignee.CItyId;
                model.Phone = consignee.Phone;
                model.Fax = consignee.Fax;
                model.EMail = consignee.EMail;
                model.ConsigneeLookup = consignee.ConsigneeLookup;
                model.PackList = consignee.PackList;
                model.SourceID = consignee.SourceID;
                model.BuyerRefId = consignee.BuyerRefId;
                model.Remarks = consignee.Remarks;
                model.Cities = CityManager.GetCityByCountry(model.CountryId);
            }
            else
            {
                model.ConsigneeRefId = _consigneeManager.GetNewConsigneeRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "consignee-2,consignee-3")]
        public ActionResult Save(OM_Consignee model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.ConsigneeId > 0 ? _consigneeManager.EditConsignee(model) : _consigneeManager.SaveConsignee(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload() : ErrorResult("Consignee information save fail " + errorMessage);
            }
            else
            {
                var consineeList=  _consigneeManager.GetConsignees();
                return Json(consineeList, JsonRequestBehavior.AllowGet);
            }
        
        }
        [AjaxAuthorize(Roles = "consignee-3")]
        public ActionResult Delete(OM_Consignee model)
        {

            var saveIndex = _consigneeManager.DeleteConsignee(model.ConsigneeRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Consignee because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingConsignee(OM_Consignee model)
        {
            var isExist = !_consigneeManager.CheckExistingConsignee(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}