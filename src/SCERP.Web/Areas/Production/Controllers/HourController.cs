using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class HourController : BaseController
    {
        private readonly IHourManager _hourManager;

        public HourController(IHourManager hourManager)
        {
            _hourManager = hourManager;
        }
        public ActionResult Index(HourViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Hours = _hourManager.GetAllHourByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords,model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(HourViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.Hour.HourId > 0)
                {
                    PROD_Hour hour = _hourManager.GethourById(model.Hour.HourId,PortalContext.CurrentUser.CompId);
                    model.Hour.HourRefId = hour.HourRefId;
                     model.Hour.HourName = hour.HourName;
                }
                else
                {
                    model.Hour.HourRefId = _hourManager.GetNewHourRefId();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(HourViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                if (_hourManager.IsHourExist(model.Hour))
                {
                    return ErrorResult("Hour :" + model.Hour.HourName + " " + "Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.Hour.HourId > 0 ? _hourManager.EditHour(model.Hour) : _hourManager.SaveHour(model.Hour);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Hour :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Hour !");
        }

        [HttpGet]
        public ActionResult Delete(long hourId)
        {
            var index = 0;
            try
            {
                index = _hourManager.DeleteHour(hourId);
                if (index == -1)// This hour Id used by another table
                {
                    return ErrorResult("Could not possible to delete hour because of it's already used.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Hour :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Hour !");
        }
	}
}