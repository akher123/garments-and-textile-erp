using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.Model.PayrollModel;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class AdvancedIncomeTaxController : BaseController
    {
        private readonly IAdvanceIncomeTaxManager _advanceIncomeTaxManager;

        // GET: Payroll/AdvancedIncomeTax
        
        public AdvancedIncomeTaxController(IAdvanceIncomeTaxManager advanceIncomeTaxManager)
        {
            _advanceIncomeTaxManager = advanceIncomeTaxManager;
        }
        [AjaxAuthorize(Roles = "employeeait-1,employeeait-2,employeeait-3")]
        public ActionResult Index(AdvanceIncomeTaxViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.AdvanceIncomeTaxs = _advanceIncomeTaxManager.GetAllAdvanceIncomeTaxsByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "employeeait-2,employeeait-3")]
        public ActionResult Edit(AdvanceIncomeTaxViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.AdvanceTaxId > 0)
                {
                   

                    AdvanceIncomeTax advanceIncomeTax = _advanceIncomeTaxManager.GetAdvanceIncomeTaxId(model.AdvanceTaxId);
                    model.AdvanceIncomeTax=advanceIncomeTax;




                }
                else
                {


                    //var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
                    //model.EmployeeId = employee.EmployeeId;



                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeait-2,employeeait-3")]
        public ActionResult Save(AdvanceIncomeTaxViewModel model)
        {
            var index = 0;

            try
            {
                bool exist = _advanceIncomeTaxManager.IsStyleAdvanceIncomeTaxExist(model.AdvanceIncomeTax);
                if (!exist)
                {
                    if (model.AdvanceIncomeTax.AdvanceTaxId > 0)
                    {
                        var employee = EmployeeManager.GetEmployeeByCardId(model.AdvanceIncomeTax.EmployeeCardId);
                        model.AdvanceIncomeTax.EmployeeId = employee.EmployeeId;
                        model.AdvanceIncomeTax.EditedDate = DateTime.Now;
                        model.AdvanceIncomeTax.EditedBy = PortalContext.CurrentUser.UserId;
                        index = _advanceIncomeTaxManager.EditAdvanceIncomeTax(model.AdvanceIncomeTax);
                    }
                    else
                    {

                        var employee = EmployeeManager.GetEmployeeByCardId(model.AdvanceIncomeTax.EmployeeCardId);
                        model.AdvanceIncomeTax.EmployeeId = employee.EmployeeId;
                        model.AdvanceIncomeTax.CreatedDate=DateTime.Now;
                        model.AdvanceIncomeTax.CreatedBy= PortalContext.CurrentUser.UserId;
                        model.AdvanceIncomeTax.IsActive = true;
                        index = _advanceIncomeTaxManager.SaveAdvanceIncomeTax(model.AdvanceIncomeTax);

                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }
        [AjaxAuthorize(Roles = "employeeait-3")]
        public ActionResult Delete(int advanceTaxId)
        {
            var index = 0;
            try
            {

                index = _advanceIncomeTaxManager.DeleteAdvanceIncomeTax(advanceTaxId);


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }

    }
}