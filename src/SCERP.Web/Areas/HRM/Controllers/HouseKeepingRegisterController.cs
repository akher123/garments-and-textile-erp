using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class HouseKeepingRegisterController : BaseController
    {

        private readonly IHouseKeepingRegisterManager _houseKeepingRegisterManager;
        private readonly IHouseKeepingItemManager _houseKeepingItemManager;

        public HouseKeepingRegisterController(IHouseKeepingRegisterManager houseKeepingRegisterManager, IHouseKeepingItemManager houseKeepingItemManager)
        {
            _houseKeepingRegisterManager = houseKeepingRegisterManager;
            _houseKeepingItemManager = houseKeepingItemManager;
        }
        [AjaxAuthorize(Roles = "housekeepingregister-1,housekeepingregister-2,housekeepingregister-3")]
        public ActionResult Index(HouseKeepingRegisterViewModel model)
        {
             ModelState.Clear();
            int totalRecords = 0;
            model.HouseKeepingRegisters = _houseKeepingRegisterManager.GetHouseKeepingRegisters(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            return View(model);
          
        }
        [AjaxAuthorize(Roles = "housekeepingregister-2,housekeepingregister-3")]
        public ActionResult Edit(int houseKeepingRegisterId=0)
        {
            HouseKeepingRegisterViewModel model=new HouseKeepingRegisterViewModel();
            ModelState.Clear();
            model.HouseKeepingItems = _houseKeepingItemManager.GetHouseKeepingItems(PortalContext.CurrentUser.CompId);
            if (houseKeepingRegisterId > 0)
            {
                model.HouseKeepingRegister.ReturnDate=DateTime.Now;
                model.HouseKeepingRegister = _houseKeepingRegisterManager.GetHouseKeepingRegisterById(houseKeepingRegisterId);
                model.SearchString = model.HouseKeepingRegister.Employee.Name + "[" +
                                     model.HouseKeepingRegister.Employee.EmployeeCardId + "]";
               
            }
            else
            {
                model.HouseKeepingRegister.IusseDate = DateTime.Now;
            }
          
            return View(model);
        }
         [AjaxAuthorize(Roles = "housekeepingregister-2,housekeepingregister-3")]
        public ActionResult Save(HouseKeepingRegisterViewModel model)
        {
            try
            {
              
                model.HouseKeepingRegister.CompId = PortalContext.CurrentUser.CompId;

                var saved = model.HouseKeepingRegister.HouseKeepingRegisterId > 0 ? _houseKeepingRegisterManager.EditHouseKeepingRegister(model.HouseKeepingRegister) : _houseKeepingRegisterManager.SaveHouseKeepingResiter(model.HouseKeepingRegister);
                return saved > 0 ? Reload() : ErrorResult("House Keeping Good Information not save successfully!!");

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }
        }
        [AjaxAuthorize(Roles = "housekeepingregister-3")]
        public ActionResult Delete(int houseKeepingResigerId)
        {
            var deleted = 0;
            var hk = _houseKeepingRegisterManager.GetHouseKeepingRegisterById(houseKeepingResigerId);
            deleted = _houseKeepingRegisterManager.DeleteHouseKeepingRegister(hk);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult HouseKeepingIssueReport(string cardId)
        {
            DataTable hkDset = _houseKeepingRegisterManager.GetHouseKeepingIssueReport(cardId);
            string path = Path.Combine(Server.MapPath("~/Areas/HRM/Reports"), "HouseKeepingRegisterReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("HKDSET", hkDset) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

	}
}