using System;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class PenaltyController : BaseController
    {
        private readonly IPenaltyTypeManager _penaltyTypeManager;
        private readonly IPenaltyManager _penaltyManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IEmployeeCompanyInfoManager _employeeCompanyInfoManager;

        public PenaltyController(IPenaltyTypeManager penaltyTypeManager, 
                                IPenaltyManager penaltyManager, 
                                IEmployeeManager employeeManager,
                                IEmployeeCompanyInfoManager employeeCompanyInfoManager)
        {
            _penaltyTypeManager = penaltyTypeManager;
            _penaltyManager = penaltyManager;
            _employeeManager = employeeManager;
            _employeeCompanyInfoManager = employeeCompanyInfoManager;
        }

        public ActionResult Index(PenaltyViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Penalties = _penaltyManager.GetAllPenaltyByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
                return View(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }

        }

        public ActionResult Save(PenaltyViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var checkEmployeeCardId = _employeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });

                if (!checkEmployeeCardId)
                {
                    return ErrorResult("Invalid Employee Card Id!");
                }
                
                if (model.ClaimerId == Guid.Empty)
                {
                    return ErrorResult("Invalid Claimer Id!");
                }

                //if(model.PenaltyTypeId == 1 && model.Penalty > 25)
                //{
                //    return ErrorResult("Penalty Hour goes out of limit! ");
                //}

                //if(model.PenaltyTypeId == 2 && model.Penalty > 6)
                //{
                //    return ErrorResult("Penalty Day goes out of limit! ");
                //}

                //if(model.PenaltyTypeId == 4 && model.Penalty > 3000)
                //{
                //    return ErrorResult("Penalty Taka goes out of limit! ");
                //}

                model.EmployeeId = _employeeManager.GetEmployeeIdByEmployeeCardId(model.EmployeeCardId);

                bool isExist = _penaltyManager.IsPenaltyExist(model);

                if (!isExist)
                {
                    if (model.PenaltyId > 0)
                    {
                        saveIndex = _penaltyManager.EditePenalty(model);
                    }
                    else
                    {
                        var penalty = new HrmPenalty()
                        {
                            EmployeeId =  model.EmployeeId,
                            EmployeeCardId = model.EmployeeCardId,
                            PenaltyTypeId = model.PenaltyTypeId,
                            Penalty = model.Penalty,
                            PenaltyDate = model.PenaltyDate,
                            Reason = model.Reason,
                            ClaimerId = model.ClaimerId,
                            IsActive = true
                        };
                        saveIndex = _penaltyManager.SavePenalty(penalty);
                    }
                }
                else
                {
                    return ErrorResult("Penalty already exists!");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save!");
        }

        public ActionResult Edit(PenaltyViewModel model)
        {
            ModelState.Clear();
            try
            {

                if (model.PenaltyId > 0)
                {
                    HrmPenalty penalty = _penaltyManager.GetPenaltyByPenaltyId(model.PenaltyId);
                    model.EmployeeCardId = penalty.EmployeeCardId;
                    model.PenaltyTypeId = penalty.PenaltyTypeId;
                    model.Penalty = penalty.Penalty;
                    model.PenaltyDate = penalty.PenaltyDate;
                    model.Reason = penalty.Reason;
                    model.ClaimerId = penalty.ClaimerId;
                    model.ClaimerName = _employeeManager.GetEmployeeNameByEmployeeId(penalty.ClaimerId);
                    model.EmployeeCompanyInfo = _employeeCompanyInfoManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);
                    if (model.EmployeeCompanyInfo != null)
                    {
                        model.EmployeeId = model.EmployeeCompanyInfo.EmployeeId;
                    }
                }
                else
                {
                    model.EmployeeCompanyInfo = _employeeCompanyInfoManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);
                    if (model.EmployeeCompanyInfo != null)
                    {
                        model.EmployeeCardId = model.EmployeeCardId;
                        model.EmployeeId = model.EmployeeCompanyInfo.EmployeeId;
                    }
                    model.PenaltyDate = DateTime.Now;
                }

                var penaltyTypes = _penaltyTypeManager.GetAllPenaltyTypes();
                model.PenaltyTypes = penaltyTypes;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult Delete(int penaltyId)
        {
            int index = 0;
            try
            {
                index = _penaltyManager.DeletePenalty(penaltyId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }

            return index > 0 ? Reload() : ErrorResult("Failed to delete penalty!");
        }

        public ActionResult GetEmployeeDetailByEmployeeCardID(PenaltyViewModel model)
        {
            var checkEmployeeCardId = _employeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }

            var employeeDetails = _employeeCompanyInfoManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            if (employeeDetails == null)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }
            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesByCardIdAndName(string serachString)
        {
            var employees = _employeeManager.GetEmployeesByCardIdAndName(serachString);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
    }
}