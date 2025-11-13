using SCERP.Common;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class JobCardEditController : BaseHrmController
    {
        public ActionResult Index(JobCardEditView model)
        {
            ModelState.Clear();

            ViewBag.JobCardName = new SelectList(new[]
                                                    {
                                                      new {Id = "EmployeeJobCardModel", Value = "EmployeeJobCardModel"}
                                                     ,new {Id = "EmployeeJobCard_10PM", Value = "EmployeeJobCard_10PM"}
                                                     ,new {Id = "EmployeeJobCard_10PM_NoWeekend", Value = "EmployeeJobCard_10PM_NoWeekend"}
                                                     ,new {Id = "EmployeeJobCard_Original_NoPenalty", Value = "EmployeeJobCard_Original_NoPenalty"}
                                                     ,new {Id = "EmployeeJobCard_Original_NoWeekend", Value = "EmployeeJobCard_Original_NoWeekend"}
            }
                                                     , "Id", "Value", model.JobCardName);

            JobCardEditView edit = HrmReportManager.GetJobCardEditInfo(model.JobCardName, model.EmployeeCardId, model.FromDate, model.ToDate);

            if (edit != null)
            {
                model = edit;
            }
            return View("~/Areas/HRM/Views/JobCardEdit/Index.cshtml", model);
        }

        public ActionResult Save(JobCardEditView model)
        {
            int result = 0;

            try
            {
                result = HrmReportManager.EditJobCard(model);

                foreach (var t in model.Inout)
                {
                    t.InOutName = model.JobCardName;
                    t.EmployeeCardId = model.EmployeeCardId;
                    HRMReportManager.EditEmployeeInOut(t);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (result > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}