using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class InventoryAuthorizedPersonController : BaseInventoryController
    {

        [AjaxAuthorize(Roles = "authorizedperson-1,authorizedperson-2,authorizedperson-3")]
        public ActionResult Index(InventoryAuthorizedPersonViewModel model)
        {
           
            try
            {
                ModelState.Clear();
                model.InventoryProcessTypeList = InventoryProcessUtility.GetProcessTypes();
                var totalRecords = 0;
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                }
                else
                {
                   model.InventoryAuthorizedPersons= InventoryAuthorizedPersonManager.GetInventoryAuthorizedPersonsByPaging(out totalRecords, model);
                   model.InventoryProcessLsit = InventoryProcessUtility.GetPocessByProcessType(model.ProcessTypeId);
                }

                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
   
        }
           [AjaxAuthorize(Roles = "authorizedperson-2,authorizedperson-3")]
        public JsonResult Save(InventoryAuthorizedPersonViewModel model)
        {
            var saveIndex = 0;
            try
            {
                model.ProcessName = InventoryProcessUtility.GetProcessName(model.ProcessId);
                model.ProcessTypeName = InventoryProcessUtility.GetProcessTypeName(model.ProcessTypeId);
                var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
                if (!checkEmployeeCardId)
                {
                    return ErrorResult("Invalid Id or Access denied!");
                }
                var isExist = InventoryAuthorizedPersonManager.IsExistAuthorizedPerson(model);
                if (isExist)
                {
                    return ErrorResult("AuthorizedPerson already Exist!");
                }
             
                saveIndex = model.AuthorizedPersonId > 0 ? InventoryAuthorizedPersonManager.EditInventoryAuthorizedPerson(model) : InventoryAuthorizedPersonManager.SaveInventoryAuthorizedPerson(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
   
        }
        [AjaxAuthorize(Roles = "authorizedperson-2,authorizedperson-3")]
        public ActionResult Edit(InventoryAuthorizedPersonViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.InventoryProcessTypeList = InventoryProcessUtility.GetProcessTypes();
                if (model.AuthorizedPersonId > 0)
                {
                    var authorizedPerson = InventoryAuthorizedPersonManager.GetAuthorizedPersonById(model.AuthorizedPersonId);
                    model.ProcessId = authorizedPerson.ProcessId;
                    model.EmployeeId = authorizedPerson.EmployeeId;
                    model.EmployeeCardId = authorizedPerson.EmployeeCardId;
                    model.ProcessTypeId = authorizedPerson.ProcessTypeId;
                    model.EmployeeCompanyInfo = InventoryAuthorizedPersonManager.GetEmployeeByEmployeeCardId(authorizedPerson.Employee.EmployeeCardId)??new VEmployeeCompanyInfoDetail();
                    model.EmployeeCardId = authorizedPerson.Employee.EmployeeCardId;
                    model.InventoryProcessLsit = InventoryProcessUtility.GetPocessByProcessType(authorizedPerson.ProcessTypeId); 
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "authorizedperson-3")]
        public ActionResult Delete(InventoryAuthorizedPersonViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = InventoryAuthorizedPersonManager.DeleteInventoryAuthorizedPerson(model.AuthorizedPersonId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(InventoryAuthorizedPersonViewModel model)
        {
            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }
            var employeeDetails = InventoryAuthorizedPersonManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            if (employeeDetails == null)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }

            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckAuthorizedPersonExist(Inventory_AuthorizedPerson model)
        {
            bool isExist = !InventoryAuthorizedPersonManager.IsExistAuthorizedPerson(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInventoryProcessByProcessTypeId(int processTypeId)
        {
            var pocessByProcessType = InventoryProcessUtility.GetPocessByProcessType(processTypeId);
            return Json(new {ProcessTypes = pocessByProcessType, Success = true}, JsonRequestBehavior.AllowGet);
        }
    }
}