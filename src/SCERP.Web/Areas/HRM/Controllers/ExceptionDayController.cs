using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class ExceptionDayController : BaseController
    {
        private readonly IExceptionDayManager _exceptionDayManager;
        private readonly ICompanyManager _companyManager;
        private readonly IBranchUnitManager _branchUnitManager;
        private readonly IBranchManager _branchManager;
        public ExceptionDayController(IExceptionDayManager exceptionDayManager, ICompanyManager companyManager, IBranchManager branchManager, IBranchUnitManager branchUnitManager)
        {
            _exceptionDayManager = exceptionDayManager;
            _companyManager = companyManager;
            _branchManager = branchManager;
            _branchUnitManager = branchUnitManager;
        }
        public ActionResult Index(ExceptionDayViewModel model)
        {
            try
            {
                int totalRecords = 0;
                model.ExceptionDays = _exceptionDayManager.GetAllExceptionDayByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
                return View(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
        }
        public ActionResult Save(ExceptionDayViewModel model)
        {
            var index = 0;
            try
            {
                    bool isExist = _exceptionDayManager.IsExceptionDayExist(model.ExceptionDay);
                    if (!isExist)
                    {
                        if (model.ExceptionDay.ExceptionDayId > 0)
                        {
                            index = _exceptionDayManager.EditExceptionDay(model.ExceptionDay);
                        }
                        else
                        {
                            index = _exceptionDayManager.SaveExceptionDay(model.ExceptionDay);
                        }  
                    }
                    else
                    {
                        return ErrorResult("This Information Already Exist ! Please Entry another one");
                    }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Save Exception Day");
        }
        public ActionResult Edit(ExceptionDayViewModel model)
        {
            ModelState.Clear();
            if (model.ExceptionDayId > 0)
            {
                ExceptionDay exceptionDay = _exceptionDayManager.GetExceptionDayByExceptionDayId(model.ExceptionDayId);
                model.CompanyId = exceptionDay.BranchUnit.Branch.CompanyId;
                model.ExceptionDay.BranchUnitId = exceptionDay.BranchUnit.BranchUnitId;
                model.BranchId = exceptionDay.BranchUnit.BranchId;
                model.ExceptionDay = exceptionDay;
                model.BranchUnits = _branchUnitManager.GetBranchUnitByBranchId(model.BranchId);
                model.Branches = _branchManager.GetAllBranchesByCompanyId(model.CompanyId);
            }
            else
            {
                model.ExceptionDay.ExceptionDate = DateTime.Now;
            }
             
            model.Companies = _companyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult Delete(int exceptionDayId)
        {
            var index =_exceptionDayManager.DeleteExceptionDay(exceptionDayId);
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Exception Day");
        }
        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = _branchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllBranchUnitsByBranchId(int branchId)
        {
            var brancheUnits = _branchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }
	}
}