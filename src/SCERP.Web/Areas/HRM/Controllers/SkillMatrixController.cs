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
    public class SkillMatrixController : BaseController
    {
        private readonly ISkillMatrixManager _skillMatrixManager;
        private readonly ISkillMatrixDetailManager _skillMatrixDetailManager;
        private readonly ISkillMatrixProcessManager _skillMatrixProcessManager;
        private readonly ISkillMatrixGradeManager _skillMatrixGradeManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IEmployeeCompanyInfoManager _employeeCompanyInfoManager;

        public SkillMatrixController(ISkillMatrixManager skillMatrixManager, ISkillMatrixDetailManager skillMatrixDetailManager, ISkillMatrixProcessManager skillMatrixProcessManager, ISkillMatrixGradeManager skillMatrixGradeManager, IEmployeeManager employeeManager, IEmployeeCompanyInfoManager employeeCompanyInfoManager)
        {
            _skillMatrixManager = skillMatrixManager;
            _skillMatrixDetailManager = skillMatrixDetailManager;
            _skillMatrixProcessManager = skillMatrixProcessManager;
            _skillMatrixGradeManager = skillMatrixGradeManager;
            _employeeManager = employeeManager;
            _employeeCompanyInfoManager = employeeCompanyInfoManager;
        }
        public ActionResult Index(SkillMatrixViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.VwSkillMatrixEmployeeList = _skillMatrixManager.GetAllSkillMatrixByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(SkillMatrixViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.SkillMatrix.SkillMatrixId > 0)
                {
                    HrmSkillMatrix skillMatrix = _skillMatrixManager.GetSkillMatrixBySkillMatrixId(model.SkillMatrix.SkillMatrixId,PortalContext.CurrentUser.CompId);
                    model.EmployeeCompanyInfo = _employeeCompanyInfoManager.GetEmployeeByEmployeeId(skillMatrix.EmployeeId);
                    model.EmployeeCardId = model.EmployeeCompanyInfo.EmployeeCardId;
                    List<VwSkillMatrix> vwSkillMatrix = _skillMatrixDetailManager.GetSkillMatrixBySkillMatrixId(model.SkillMatrix.SkillMatrixId, PortalContext.CurrentUser.CompId);
                    model.SkillMatrixDictionary=new Dictionary<string, VwSkillMatrix>();
                    foreach (var matrix in vwSkillMatrix)
                    {
                        VwSkillMatrix vwskillMatrix=new VwSkillMatrix();
                        vwskillMatrix.SkillMatrixGradeId = matrix.SkillMatrixGradeId;
                        vwskillMatrix.SkillMatrixProcessId = matrix.SkillMatrixProcessId;
                        vwskillMatrix.ProcessPercentage = matrix.ProcessPercentage;
                        vwskillMatrix.GradeName = matrix.GradeName;
                        vwskillMatrix.ProcessName = matrix.ProcessName;
                        model.Key =matrix.SkillMatrixDetailId.ToString();
                        model.SkillMatrixDictionary.Add(model.Key, vwskillMatrix);
                    }
                    
                }
                else
                {
                    model.EmployeeCompanyInfo = null;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            model.SkillMatrixProcesses = _skillMatrixProcessManager.GetAllSkillMatrixProcess();
            return View(model);
        }

        public ActionResult GetEmployeeDetailByEmployeeCardID(SkillMatrixViewModel model)
        {
            var checkEmployeeCardId = _employeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Employee Id !");
            }
            else
            {
                model.EmployeeCompanyInfo = _employeeCompanyInfoManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);
                int designationId = model.EmployeeCompanyInfo.DesignationId;

                if (designationId == 27 || designationId == 32 || designationId == 45 || designationId == 58)
                {
                    return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", model), Success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return ErrorResult("This Employee is out of Valid Designation !");
        }
        public ActionResult AddNewRow(SkillMatrixViewModel model)
        {
            HrmSkillMatrixGrade skillMatrixGrade = _skillMatrixGradeManager.GetGradeNameByProcessPercentage(model.VwSkillMatrix.ProcessPercentage, PortalContext.CurrentUser.CompId);
            model.VwSkillMatrix.SkillMatrixGradeId = skillMatrixGrade.SkillMatrixGradeId;
            model.VwSkillMatrix.GradeName = skillMatrixGrade.GradeName;
            string processName = _skillMatrixProcessManager.GetProcessNameBySkillMatrixProcessId(model.VwSkillMatrix.SkillMatrixProcessId, PortalContext.CurrentUser.CompId);
            model.VwSkillMatrix.ProcessName = processName;
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.SkillMatrixDictionary.Add(model.Key, model.VwSkillMatrix);
            return PartialView("~/Areas/HRM/Views/SkillMatrix/_AddNewRow.cshtml", model);
        }
        public ActionResult Save(SkillMatrixViewModel model)
        {
            int savedIndex = 0;
            try
            {
                model.SkillMatrix.EmployeeId = model.EmployeeCompanyInfo.EmployeeId;
                model.SkillMatrix.Name = model.EmployeeCompanyInfo.EmployeeName;
                model.SkillMatrix.Designation = model.EmployeeCompanyInfo.Designation;

                model.SkillMatrix.HrmSkillMatrixDetail = model.SkillMatrixDictionary.Select(x => x.Value).Select(x => new HrmSkillMatrixDetail()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    SkillMatrixProcessId = x.SkillMatrixProcessId,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    SkillMatrixGradeId = x.SkillMatrixGradeId,
                    ProcessPercentage = x.ProcessPercentage
                }).ToList();
                if(model.SkillMatrix.HrmSkillMatrixDetail.Count>0)
                { 
                bool isExist = _skillMatrixManager.IsSkilMatrixExist(model.EmployeeCompanyInfo.EmployeeId,model.SkillMatrix.SkillMatrixId,PortalContext.CurrentUser.CompId);
                if (!isExist)
                {
                    savedIndex = model.SkillMatrix.SkillMatrixId > 0 ? _skillMatrixManager.EditSkillMatrix(model.SkillMatrix) : _skillMatrixManager.SaveSkillMatrix(model.SkillMatrix);
                }
                else
                {
                    return ErrorResult("Already Exist.");
                }
                }
                else
                {
                    return ErrorResult("Select Process Name.");
                }
                
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Failed To Save/Edit");
        }
        public ActionResult Delete(int skillMatrixId)
        {
            int deleted = 0;
            deleted = _skillMatrixManager.DeleteSkillMatrix(skillMatrixId, PortalContext.CurrentUser.CompId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Skill Matrix.");
        }
    }
}