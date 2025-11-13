using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.TrackingModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class ConfirmationMediaController : BaseController
    {
        private IConfirmationMediaManager _confirmationMediaManager;
        public ConfirmationMediaController(IConfirmationMediaManager confirmationMediaManager)
        {
            _confirmationMediaManager = confirmationMediaManager;
        }
        public ActionResult Index(ConfirmationMediaViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.ConfirmationMediaList = _confirmationMediaManager.GetAllConfirmationMediaListByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(ConfirmationMediaViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ConfirmationMediaId > 0)
                {
                    TrackConfirmationMedia confirmationMedia = _confirmationMediaManager.GetConfirmationMediaById(model.ConfirmationMediaId);
                    model.ConfirmationMedia = confirmationMedia.ConfirmationMedia;
                    model.Remarks = confirmationMedia.Remarks;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Save(ConfirmationMediaViewModel model)
        {
            int index = 0;
            try
            {
                TrackConfirmationMedia confirmatonMedia = new TrackConfirmationMedia
                {   ConfirmationMediaId = model.ConfirmationMediaId,
                    ConfirmationMedia = model.ConfirmationMedia,
                    Remarks = model.Remarks
                };
                if (_confirmationMediaManager.IsConfirmationMediaExist(confirmatonMedia))
                {
                    return
                        ErrorResult("Confirmation Media :" + model.ConfirmationMedia + " " +
                                    "Already Exist ! Please Entry Another One");
                }
                else
                {
                    index = model.ConfirmationMediaId > 0 ? _confirmationMediaManager.EditConfirmationMedia(confirmatonMedia) : _confirmationMediaManager.SaveConfirmationMedia(confirmatonMedia);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Confirmation Media :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Confirmation Media !");
        }

        public ActionResult Delete(long confirmationMediaId)
        {
            int index = 0;
            try
            {
                index = _confirmationMediaManager.DeleteConfirmationMedia(confirmationMediaId);
                if (index == -1)// This vehicle Id used by another talbe
                {
                    return ErrorResult("Could not possible to delete confirmation Media because of it's already used in another table.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Confirmation Media :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delele Confirmation Media !");
        }
    }
}