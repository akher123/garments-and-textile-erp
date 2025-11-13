using System;
using System.Transactions;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeEntitlementController : BaseHrmController
    {
        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeeentitlementinfo-1,employeeentitlementinfo-2,employeeentitlementinfo-3")]
        public ActionResult Index(SCERP.Web.Areas.HRM.Models.ViewModels.EmployeeEntitlementInfoViewModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }

                var employeeEntitlementInfos = EmployeeEntitlementManager.GetEmployeeEntitlementInfoByEmployeeGuidId(_employeeGuidId);
                model.EmployeeEntitlements = employeeEntitlementInfos;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeentitlementinfo-2,employeeentitlementinfo-3")]
        public ActionResult Edit(Models.ViewModels.EmployeeEntitlementInfoViewModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                model.Entitlements = EntitlementManager.GetAllEntitlements();
                var employeeEntitlementInfo = EmployeeEntitlementManager.GetEmployeeEntitlementInfoById(_employeeGuidId, model.Id);

                if (employeeEntitlementInfo != null)
                {
                    model.EntitlementId = employeeEntitlementInfo.EntitlementId;
                    model.FromDate = employeeEntitlementInfo.FromDate;
                    model.ToDate = employeeEntitlementInfo.ToDate;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeentitlementinfo-2,employeeentitlementinfo-3")]
        public ActionResult Save(SCERP.Web.Areas.HRM.Models.ViewModels.EmployeeEntitlementInfoViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {
                var employeeEntitlementInfo = EmployeeEntitlementManager.GetEmployeeEntitlementInfoById(model.EmployeeId, model.Id) ?? new EmployeeEntitlement();

                employeeEntitlementInfo.EmployeeId = model.EmployeeId;
                employeeEntitlementInfo.EntitlementId = model.EntitlementId;
                employeeEntitlementInfo.FromDate = model.FromDate;
                employeeEntitlementInfo.ToDate = model.ToDate;

                if (model.Id > 0)
                {
                    saveIndex = EmployeeEntitlementManager.EditEmployeeEntitlementInfo(employeeEntitlementInfo);
                }
                else
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var latestEmployeeEntitlementInfo = EmployeeEntitlementManager.GetLatestEmployeeEntitlementInfoByEmployeeGuidId(model.EmployeeId, model.EntitlementId);
                        if (latestEmployeeEntitlementInfo != null)
                        {
                            if (latestEmployeeEntitlementInfo.FromDate > employeeEntitlementInfo.FromDate)
                                return ErrorResult("Invalid date!");


                            if (employeeEntitlementInfo.FromDate != null)
                                latestEmployeeEntitlementInfo.ToDate = employeeEntitlementInfo.FromDate.Value.AddDays(-1);

                            EmployeeEntitlementManager.UpdateEmployeeEntitlementInfoDate(latestEmployeeEntitlementInfo);
                        }

                        saveIndex = EmployeeEntitlementManager.SaveEmployeeEntitlementInfo(employeeEntitlementInfo);
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            return ErrorResult("Failed to save data!");

        }

        [AjaxAuthorize(Roles = "employeeentitlementinfo-3")]
        public ActionResult Delete(int Id)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            var deleteEmployeeEntitlementInfo = 0;

            try
            {
                var employeeEntitlementInfo = EmployeeEntitlementManager.GetEmployeeEntitlementInfoById(_employeeGuidId, Id);
                deleteEmployeeEntitlementInfo = EmployeeEntitlementManager.DeleteEmployeeEntitlementInfo(employeeEntitlementInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeeEntitlementInfo > 0 ? Reload() : ErrorResult();
        }

    }
}
