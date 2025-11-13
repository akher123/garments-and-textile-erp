using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class ImportDetailsController : BaseController
    {
        private readonly IImportDetailsManager _importDetailsManager;
        public ImportDetailsController(IImportDetailsManager importDetailsManager)
        {
            _importDetailsManager = importDetailsManager;
        }

        // GET: Commercial/ImportDetails
        public ActionResult Index(int importId)
        {

            CommImportDetailViewModel model = new CommImportDetailViewModel();
            model.CommImportDetail.ImportId = importId;
            model.CommImportDetail.LCDate = DateTime.Now;
            model.commImportDetails = _importDetailsManager.GetImportDetailsByLcId(importId);
            return View(model);
        }

        public ActionResult Save(CommImportDetailViewModel model)
        {

            int saved = 0;
            try
            {
                if (model.CommImportDetail.ImportDetailId > 0)
                {

                    model.CommImportDetail.EditedBy = PortalContext.CurrentUser.UserId;
                    model.CommImportDetail.EditedDate = DateTime.Now;
                    saved = _importDetailsManager.EditImportDetails(model.CommImportDetail);
                }
                else
                {
                    
                    
                    model.CommImportDetail.CreatedDate = DateTime.Now;
                    model.CommImportDetail.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    saved = _importDetailsManager.SaveImportDetails(model.CommImportDetail);
                }
            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

            if (saved > 0)
            {

                model.commImportDetails = _importDetailsManager.GetImportDetailsByLcId(model.CommImportDetail.ImportId);
                return PartialView("~/Areas/Commercial/Views/ImportDetails/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed!Plse contact with vendor");
            }

        }

        public ActionResult Edit(int importDetailId)
        {
            CommImportDetailViewModel model = new CommImportDetailViewModel
            {
                
                CommImportDetail = _importDetailsManager.GetImportDetailsById(importDetailId) ?? new CommImportDetails()
                {
                    LCDate = DateTime.Now
                }
            };
            return PartialView("~/Areas/Commercial/Views/ImportDetails/_Edit.cshtml", model);
        }

        public ActionResult Refresh(int importId)
        {
            CommImportDetailViewModel model = new CommImportDetailViewModel
            {

                CommImportDetail =
                {
                    LCDate = DateTime.Now,
                    ImportId = importId
                }
            };
            return PartialView("~/Areas/Commercial/Views/ImportDetails/_Edit.cshtml", model);
        }

        public ActionResult Delete(int importDetailId, int importId)
        {
            CommImportDetailViewModel model = new CommImportDetailViewModel();
            var importDetail = _importDetailsManager.GetImportDetailsById(importDetailId);
            int delete = _importDetailsManager.DeleteImportDetails(importDetail);
            if (delete > 0)
            {

                model.commImportDetails = _importDetailsManager.GetImportDetailsByLcId(importId);
                return PartialView("~/Areas/Commercial/Views/ImportDetails/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Delete Failed!Please contact with vendor");
            }

        }
    }
}