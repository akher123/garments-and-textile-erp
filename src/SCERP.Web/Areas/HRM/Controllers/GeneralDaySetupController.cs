using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class GeneralDaySetupController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "generaldaysetup-1,generaldaysetup-2,generaldaysetup-3")]
        public ActionResult Index(GeneralDaySetupViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;

                model.GeneralDaySetups = GeneralDaySetupManager.GetGeneralDaySetup(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "generaldaysetup-2,generaldaysetup-3")]
        public ActionResult Edit(GeneralDaySetupViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
               
                if (model.GeneralDaySetupId > 0)
                {
                    GeneralDaySetup generalDaySetup = GeneralDaySetupManager.GetGeneralDaySetupById(model.GeneralDaySetupId);
                    model.SearchFieldModel.SearchByCompanyId = generalDaySetup.BranchUnitDepartment.BranchUnit.Branch.CompanyId;
                    model.SearchFieldModel.SearchByBranchId = generalDaySetup.BranchUnitDepartment.BranchUnit.BranchId;
                    model.SearchFieldModel.SearchByBranchUnitId = generalDaySetup.BranchUnitDepartment.BranchUnitId;

                    model.BranchUnitDepartmentId = generalDaySetup.BranchUnitDepartmentId;
                    model.DeclaredDate = generalDaySetup.DeclaredDate;
                    model.Description = generalDaySetup.Description;
                    model.IsAllowedExternal = generalDaySetup.IsAllowedExternal;
                    model.IsAllowedInternal = generalDaySetup.IsAllowedInternal;

                    model.CreatedBy = generalDaySetup.CreatedBy;
                    model.EditedBy = generalDaySetup.EditedBy;
                    model.CreatedDate = generalDaySetup.CreatedDate;
                    model.EditedDate = generalDaySetup.EditedDate;
                    model.IsActive = generalDaySetup.IsActive;

                    model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "generaldaysetup-2,generaldaysetup-3")]
        public ActionResult Save(GeneralDaySetupViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var generalDaySetup = new GeneralDaySetup()
                {
                    GeneralDaySetupId = model.GeneralDaySetupId,
                    BranchUnitDepartmentId = model.BranchUnitDepartmentId,
                    DeclaredDate = model.DeclaredDate,
                    Description = model.Description,
                    IsAllowedExternal = model.IsAllowedExternal,
                    IsAllowedInternal = model.IsAllowedInternal,
                    IsActive = true
                };

  
                if (model.GeneralDaySetupId > 0)
                    saveIndex = GeneralDaySetupManager.EditGeneralDaySetup(generalDaySetup);
                else
                {
                    var checkExistingGeneralSetupDay = GeneralDaySetupManager.CheckExistingGeneralDaySetup(model.DeclaredDate);
                    if (checkExistingGeneralSetupDay)
                    {
                        return ErrorResult("Date already exist");
                    }

                    saveIndex = GeneralDaySetupManager.SaveGeneralDaySetup(generalDaySetup);
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        [AjaxAuthorize(Roles = "generaldaysetup-3")]
        public ActionResult Delete(GeneralDaySetupViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = GeneralDaySetupManager.DeleteGeneralDaySetup(model.GeneralDaySetupId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

    }
}