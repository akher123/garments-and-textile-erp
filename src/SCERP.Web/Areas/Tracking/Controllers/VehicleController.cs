using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.TrackingModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class VehicleController : BaseController
    {
        private readonly IVehicleManager _vehicleManager;

        public VehicleController(IVehicleManager vehicleManager)
        {
            _vehicleManager = vehicleManager;
        }
        public ActionResult Index(VehicleViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Vehicles = _vehicleManager.GetAllVehicleByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(VehicleViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.VehicleId > 0)
                {
                    TrackVehicle vehicle = _vehicleManager.GetVehicleById(model.VehicleId);
                    model.VehicheType = vehicle.VehicheType;
                    model.Remarks = vehicle.Remarks;
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
        public ActionResult Save(VehicleViewModel model)
        {
            var index = 0;
            try
            {
                  var vehicle = new TrackVehicle {VehicleId = model.VehicleId,VehicheType = model.VehicheType, Remarks = model.Remarks };
                   if (_vehicleManager.IsVehicleTypeExist(vehicle))
                    {
                        return ErrorResult("Vehiche Type :" + model.VehicheType + " " + "Already Exist ! Please Entry another one");
                    }
                    else
                    {
                        index = model.VehicleId > 0 ? _vehicleManager.EditVehicle(vehicle) : _vehicleManager.SaveVehicle(vehicle);
                    }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Vehicle :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Vehicle !");
        }
        
        [HttpGet]
        public ActionResult Delete(long vehicleId)
        {
            var index = 0;
            try
            {
                index = _vehicleManager.DeleteVehicle(vehicleId);
                if (index == -1)// This vehicle Id used by another table
                {
                    return ErrorResult("Could not possible to delete vehicle because of it's already used in Vehicle Gate Entry.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Vehicle :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delele Vehicle !");
        }
    }
}