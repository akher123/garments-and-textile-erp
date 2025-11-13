using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class MailSendController : BaseController
    {
        public readonly IMailSendManager _MailSendManager;
        public readonly IModuleManager _ModuleManager;

        public MailSendController(IMailSendManager mailSendManager, IModuleManager ModuleManager)
        {
            _MailSendManager = mailSendManager;
            _ModuleManager = ModuleManager;
        }
        public ActionResult Index(MailSendViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Modules = _ModuleManager.GetModules();
                model.MailSendList = _MailSendManager.GetAllMailSendByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString,out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(MailSendViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.MailSend.MailSendId > 0)
                {
                    MailSend mailSend = _MailSendManager.GetMailSendByMailSendId(model.MailSend.MailSendId, PortalContext.CurrentUser.CompId);
                    model.MailSend.ModuleId = mailSend.ModuleId;
                    model.MailSend.ReportName = mailSend.ReportName;
                    model.MailSend.MailSubject = mailSend.MailSubject;
                    model.MailSend.MailBody = mailSend.MailBody;
                    string[] mailAddress = mailSend.MailAddress.Split(',');
                    
                    int mailIndex = mailAddress.Count();
                    if (mailIndex == 3)
                    {
                        model.MailSend.MailAddress = mailAddress[0];
                        model.MailAddress2 = mailAddress[1];
                        model.MailAddress3 = mailAddress[2];
                    }
                    if (mailIndex == 2)
                    {
                        model.MailSend.MailAddress = mailAddress[0];
                        model.MailAddress2 = mailAddress[1];
                    }
                    model.MailSend.MailAddress = mailAddress[0];
                    
                    string[] person = mailSend.PersonName.Split(',');
                    int personIndex = person.Count();
                    if (personIndex == 3)
                    {
                        model.MailSend.PersonName = person[0];
                        model.PersonName2 = person[1];
                        model.PersonName3 = person[2];
                    }
                    if (personIndex == 2)
                    {
                        model.MailSend.PersonName = person[0];
                        model.PersonName2 = person[1];
                    }
                    model.MailSend.PersonName = person[0];
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            model.Modules = _ModuleManager.GetModules();
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(MailSendViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                model.MailSend.MailAddress = model.MailSend.MailAddress + "," + model.MailAddress2 + "," +
                             model.MailAddress3;
                model.MailSend.PersonName = model.MailSend.PersonName + "," + model.PersonName2 + "," + model.PersonName3;
                if (_MailSendManager.IsMailSendExist(model.MailSend))
                {
                    return ErrorResult( "This Information Already Exist ! Please Entry another one");
                }
                else
                {
                    model.MailSend.FileName = @"D:\\Report_TempFile\\" + model.MailSend.ReportName + ".pdf";
                    index = model.MailSend.MailSendId > 0 ? _MailSendManager.EditMailSend(model.MailSend) : _MailSendManager.SaveMailSend(model.MailSend);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Hour :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Hour !");
        }
        public ActionResult Delete(int MailSendId)
        {
            var index = 0;
            try
            {
                index = _MailSendManager.DeleteMailSend(MailSendId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Mail Send :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delele Mail Send !");
        }
	}
}