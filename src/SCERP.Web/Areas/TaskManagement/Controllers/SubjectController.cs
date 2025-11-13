using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly ITmModuleManager _tmModuleManager;
        private readonly ISubjectManager _subjectManager;
        public SubjectController(ITmModuleManager tmModuleManager, ISubjectManager subjectManager)
        {
            _tmModuleManager = tmModuleManager;
            _subjectManager = subjectManager;
        }
        public ActionResult Index(SubjectViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Subjects = _subjectManager.GetALLSubjectByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
        public ActionResult Save(SubjectViewModel model)
        {
            var index = 0;
            try
            {
                    bool exist = _subjectManager.IsSubjectExist(model.Subject);
                    if (!exist)
                    {
                        if (model.Subject.SubjectId > 0)
                        {
                            index = _subjectManager.EditSubject(model.Subject);
                        }
                        else
                        {
                            index = _subjectManager.SaveSubject(model.Subject); 
                        }
                    }
                    else
                    {
                        return ErrorResult("Module Name and Subject Name Already Exist ! Please Entry Another One");
                    }
            }
            catch (Exception exception)
            {
               Errorlog.WriteLog(exception);
            }
      
            return index > 0 ? Reload() : ErrorResult("Fail to Save Subject");
        }
        public ActionResult Edit(SubjectViewModel model)
        {
             ModelState.Clear();
            try
            {
                if (model.SubjectId > 0)
                {
                    model.Subject = _subjectManager.GetSubjectBySubjectId(model.SubjectId);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            model.Modules = _tmModuleManager.GetAllModule();
            return View(model);
        }
        public ActionResult Delete(int subjectId)
        {
            int index = 0;
            try
            {
                 index = _subjectManager.DeleteSubject(subjectId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }
	}
}