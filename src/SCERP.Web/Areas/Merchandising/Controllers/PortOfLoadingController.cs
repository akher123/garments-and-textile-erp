using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class PortOfLoadingController : BaseController
    {

        private readonly IPortOfLoadingManager _portOfLoadingManager;

        public PortOfLoadingController(IPortOfLoadingManager portOfLoadingManager)
        {
            this._portOfLoadingManager = portOfLoadingManager;
        }
        [AjaxAuthorize(Roles = "portofloading-1,portofloading-2,portofloading-3")]
        public ActionResult Index(PortOfLoadingViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.PortOfLoadings = _portOfLoadingManager.GetPortOfLoadingsByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "portofloading-2,portofloading-3")]
        public ActionResult Edit(PortOfLoadingViewModel model)
        {
            ModelState.Clear();
            model.Countries = CountryManager.GetAllCountries();
            if (model.PortOfLoadingId > 0)
            {
                var portOfLoading = _portOfLoadingManager.GetPortOfLoadingById(model.PortOfLoadingId);
                model.PortOfLoadingId = portOfLoading.PortOfLoadingId;
                model.PortOfLoadingRefId = portOfLoading.PortOfLoadingRefId;
                model.CompId = portOfLoading.CompId;
                model.CountryId = portOfLoading.CountryId;
                model.PortOfLoadingName = portOfLoading.PortOfLoadingName;
                model.PortType = portOfLoading.PortType;

            }
            else
            {
                model.PortOfLoadingRefId = _portOfLoadingManager.GetNewPortOfLoadingfId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "portofloading-2,portofloading-3")]
        public ActionResult Save(OM_PortOfLoading model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.PortOfLoadingId > 0 ? _portOfLoadingManager.EditPortOfLoading(model) : _portOfLoadingManager.SavePortOfLoading(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Port Name save fail " + errorMessage);
        }
        [AjaxAuthorize(Roles = "portofloading-3")]
        public ActionResult Delete(OM_PortOfLoading model)
        {

            var saveIndex = _portOfLoadingManager.DeletePortOfLoading(model.PortOfLoadingRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete PortOfLoading because of it's all ready used in buyer shipment ");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingPortOfLoading(OM_PortOfLoading model)
        {
            var isExist = !_portOfLoadingManager.CheckExistingPortOfLoading(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}