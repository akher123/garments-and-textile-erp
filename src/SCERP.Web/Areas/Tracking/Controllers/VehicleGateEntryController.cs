using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.TrackingModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class VehicleGateEntryController : BaseController
    {
        private readonly IVehicleGateEntryManager _vehicleGateEntryManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IConfirmationMediaManager _confirmationMediaManager;
        private readonly IVehicleManager _vehicleManager;
        public VehicleGateEntryController(IVehicleGateEntryManager vehicleGateEntryManager, IEmployeeManager employeeManager, IConfirmationMediaManager confirmationMediaManager, IVehicleManager vehicleManager)
        {
            _vehicleGateEntryManager = vehicleGateEntryManager;
            _employeeManager = employeeManager;
            _confirmationMediaManager = confirmationMediaManager;
            _vehicleManager = vehicleManager;
        }
        public ActionResult Index(VehicleGateEntryViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.VehicleGateEntryList = _vehicleGateEntryManager.GetVehicleGateEntriListByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(VehicleGateEntryViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.VehicleGateEntryId > 0)
                {
                    model.VehicleGateEntry = _vehicleGateEntryManager.GetVehicleGateEntryById(model.VehicleGateEntryId);
                    model.VehicleGateEntry.CheckInTime = model.VehicleGateEntry.EntryDate.ToShortTimeString();
                    model.VehicleGateEntry.CheckOutTime = model.VehicleGateEntry.ExitDate.GetValueOrDefault().ToShortTimeString();
                    model.EmployeeName = _employeeManager.GetEmployeeNameByEmployeeId(model.VehicleGateEntry.EmployeeId.GetValueOrDefault());
                }
                else
                {
                    model.VehicleGateEntry.EntryDate = DateTime.Now;
                    model.VehicleGateEntry.CheckInTime = DateTime.Now.ToString("hh:mm tt");
                }
                model.ConfirmationMediaList = _confirmationMediaManager.GetAllConfirmationMediaList();
                model.Vehicles = _vehicleManager.GetAllVehicle();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(VehicleGateEntryViewModel model)
        {
            var index = 0;
            try
            {
                if (_vehicleGateEntryManager.IsVehicleGateEntryExist(model.VehicleGateEntry))
                {
                    return ErrorResult("This Informaton Already Exist ! Please Entry Another One");
                }
                else
                {
                    index = model.VehicleGateEntry.VehicleGateEntryId > 0 ? _vehicleGateEntryManager.EditVehicleGateEnty(model.VehicleGateEntry) : _vehicleGateEntryManager.SaveVehicleGateEntry(model.VehicleGateEntry);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Vehicle Gate Entry :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Save/Edit Vehicle Gate Entry !");
        }

        
        [HttpGet]
        public ActionResult ShowForCheckOut(VehicleGateEntryViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.VehicleGateEntryId > 0)
                {
                    TrackVehicleGateEntry vehicleGateEntry = _vehicleGateEntryManager.GetVehicleGateEntryById(model.VehicleGateEntryId);
                    model.VehicleGateEntry = vehicleGateEntry;
                    model.VehicleGateEntry.CheckOutStatus = Convert.ToInt32(CheckOutStatus.CheckOut);
                    if (vehicleGateEntry.ExitDate == null)
                    {
                        model.VehicleGateEntry.ExitDate = DateTime.Now;
                        model.VehicleGateEntry.CheckOutTime = DateTime.Now.ToString("hh:mm tt");
                    }
                    else
                    {
                        model.VehicleGateEntry.ExitDate = vehicleGateEntry.ExitDate;
                        model.VehicleGateEntry.CheckOutTime = Convert.ToDateTime(vehicleGateEntry.ExitDate).ToShortTimeString();
                    }
                    model.EmployeeName = _employeeManager.GetEmployeeNameByEmployeeId(vehicleGateEntry.EmployeeId.GetValueOrDefault());
                }
                model.ConfirmationMediaList = _confirmationMediaManager.GetAllConfirmationMediaList();
                model.Vehicles = _vehicleManager.GetAllVehicle();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }
        public ActionResult Delete(long vehicleGateEntryId)
        {
            var index = 0;
            try
            {
                index = _vehicleGateEntryManager.DeleteVehicleGateEntry(vehicleGateEntryId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Vehicle Gate Entry :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Vehicle Gate Entry !");
        }
        public JsonResult GetEmployeesBySearchCharacter(string searchCharacter)
        {
            var employees = _employeeManager.GetEmployeesBySearchCharacter(searchCharacter);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
    }
}