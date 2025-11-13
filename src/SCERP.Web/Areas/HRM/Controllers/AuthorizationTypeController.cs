using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class AuthorizationTypeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "authorizationtype-1,authorizationtype-2,authorizationtype-3")]
        public ActionResult Index(AuthorizationTypeViewModel model)
        {
            try
            {
                ModelState.Clear();
                //if (!model.IsSearch)
                //{
                //    model.IsSearch = true;
                //    return View(model);
                //}
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.TypeName = model.SearchByTypeName;
                model.AuthorizationTypes = AuthorizationTypeManager.GetAuthorizationTypes(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "authorizationtype-2,authorizationtype-3")]
        public ActionResult Edit(AuthorizationTypeViewModel model)
        {
            ModelState.Clear();
            try
            {

                var processKeyList = from ProcessKeyEnum processKey in Enum.GetValues(typeof(ProcessKeyEnum))
                                     select new { ProcessKeyId = (int)processKey, Name = processKey.ToString() };

                var authorizationKeyList = from SCERP.Common.AuthorizationType authType in Enum.GetValues(typeof(SCERP.Common.AuthorizationType))
                                           select new { AuthorizationId = (int)authType, Name = authType.ToString() };

                if (model.Id > 0)
                {
                    var authorizationType = AuthorizationTypeManager.GetAuthorizationTypeById(model.Id);
                    model.Id = authorizationType.Id;
                    model.TypeName = authorizationType.TypeName;
                    model.ProcessKeyId = authorizationType.ProcessKeyId;
                    model.AuthorizationId = authorizationType.AuthorizationId;
                }

                ViewBag.ProcessKeyId = new SelectList(processKeyList, "ProcessKeyId", "Name", model.ProcessKeyId);
                ViewBag.AuthorizationId = new SelectList(authorizationKeyList, "AuthorizationId", "Name", model.AuthorizationId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "authorizationtype-2,authorizationtype-3")]
        public ActionResult Save(SCERP.Model.AuthorizationType authorization)
        {
            var saveIndex = 0;
            bool isExist = AuthorizationTypeManager.IsExistauthorization(authorization);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = authorization.Id > 0 ? AuthorizationTypeManager.EditAuthorizationType(authorization) : AuthorizationTypeManager.SaveAuthorizationType(authorization);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("{0} AuthorizationType already exist!", authorization.TypeName));
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "authorizationtype-3")]
        public ActionResult Delete(SCERP.Model.AuthorizationType authorizationType)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = AuthorizationTypeManager.DeleteAuthorizationType(authorizationType.Id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }
    }
}