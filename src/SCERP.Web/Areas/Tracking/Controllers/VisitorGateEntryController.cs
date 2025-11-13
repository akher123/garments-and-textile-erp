using System;
using System.IO;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.TrackingModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class VisitorGateEntryController : BaseController
    {
        private readonly IVisitorGateEntryManager _visitorGateEntryManager;
        private readonly IConfirmationMediaManager _confirmationMediaManager;
        private readonly IEmployeeManager _employeeManager;
        public VisitorGateEntryController(IVisitorGateEntryManager visitorGateEntryManager, IConfirmationMediaManager confirmationMediaManager, IEmployeeManager employeeManager)
        {
            _visitorGateEntryManager = visitorGateEntryManager;
            _confirmationMediaManager = confirmationMediaManager;
            _employeeManager = employeeManager;
        }
        public ActionResult Index(VisitorGateEntryViewModel model)
        {
            try
            {
                var totalRecords = 0;
                ModelState.Clear();
                model.DataList = _visitorGateEntryManager.GetVisitorGateEntryByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Save(VisitorGateEntryViewModel model)
        {
            var index = 0;
            try
            {
                if (_visitorGateEntryManager.IsVitorGateEntryExist(model.VisitorGateEntry))
                {
                    return ErrorResult("This Informaton Already Exist ! Please Entry Another One.");
                }
                else
                {
                    index = model.VisitorGateEntry.VisitorGateEntryId > 0 ? _visitorGateEntryManager.EditVisitorGateEntry(model.VisitorGateEntry) : _visitorGateEntryManager.SaveVisitorGateEntry(model.VisitorGateEntry);
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Visitor Gate Entry :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Save/Edit Visitor Gate Entry !");
        }
        public ActionResult Edit(VisitorGateEntryViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.VisitorGateEntryId > 0)
                {
                    TrackVisitorGateEntry visitorGateEntry = _visitorGateEntryManager.GetVisitorGateEntryById(model.VisitorGateEntryId);
                    model.VisitorGateEntry = visitorGateEntry;
                    model.VisitorGateEntry.CheckInTime = visitorGateEntry.EntryDate.ToShortTimeString();
                    model.VisitorGateEntry.CheckOutTime = visitorGateEntry.ExitDate.GetValueOrDefault().ToShortTimeString();
                    model.EmployeeName = _employeeManager.GetEmployeeNameByEmployeeId(visitorGateEntry.EmployeeId.GetValueOrDefault());
                 
                }
                else
                {
                    model.VisitorGateEntry.EntryDate = DateTime.Now;
                    string time = DateTime.Now.ToString("hh:mm tt");
                    model.VisitorGateEntry.CheckInTime = time;
                }
                model.ConfirmationMediaList = _confirmationMediaManager.GetAllConfirmationMediaList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ShowForCheckOut(VisitorGateEntryViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.VisitorGateEntryId > 0)
                {
                    TrackVisitorGateEntry visitorGateEntry = _visitorGateEntryManager.GetVisitorGateEntryById(model.VisitorGateEntryId);

                    model.VisitorGateEntry = visitorGateEntry;
                    model.VisitorGateEntry.CheckOutStatus = Convert.ToInt32(CheckOutStatus.CheckOut);
                    if (visitorGateEntry.ExitDate == null)
                    {
                        model.VisitorGateEntry.ExitDate = DateTime.Now;
                        string defaultCheckoutTime = DateTime.Now.ToString("hh:mm tt");
                        model.VisitorGateEntry.CheckOutTime = defaultCheckoutTime;
                    }
                    else
                    {
                        model.VisitorGateEntry.ExitDate = visitorGateEntry.ExitDate;
                        string checkoutTime = Convert.ToDateTime(visitorGateEntry.ExitDate).ToShortTimeString();
                        model.VisitorGateEntry.CheckOutTime = checkoutTime;
                    }
                    model.EmployeeName = _employeeManager.GetEmployeeNameByEmployeeId(visitorGateEntry.EmployeeId.GetValueOrDefault());
                }
                model.ConfirmationMediaList = _confirmationMediaManager.GetAllConfirmationMediaList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Delete(long visitorGateEntryId)
        {
            var index = 0;
            try
            {
                index = _visitorGateEntryManager.DeleteVisitorGateEntry(visitorGateEntryId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Visitor Gate Entry :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Visitor Gate Entry !");
        }
        public JsonResult GetVisitorGateEntryByPhone(string phone)
        {
            var visitorGateEntry = _visitorGateEntryManager.GetVisitorGateEntryByPhone(phone);
            return Json(visitorGateEntry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeesBySearchCharacter(string searchCharacter)
        {
            var employees = _employeeManager.GetEmployeesBySearchCharacter(searchCharacter);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Upload(string image)
        {
            string imageName =string.Format("{0}.png", Guid.NewGuid().ToString());
            string imagePath =  Path.Combine(Server.MapPath(@"~/VisitorImages"), imageName);
            image = image.Substring("data:image/png;base64,".Length);
            var buffer = Convert.FromBase64String(image);
            System.IO.File.WriteAllBytes(imagePath, buffer);
            return Json(new { Success = true, ImagePath = Url.Content(@"~/VisitorImages/" + imageName) }, JsonRequestBehavior.AllowGet);
        }
    }
}