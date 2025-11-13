using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.Manager.CommonManager;
using SCERP.Common;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Model.CommonModel;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class CommFileUploadController : BaseController
    {
 
    
        public ActionResult Index(DocumentViewModel model)
        {
            ModelState.Clear();
            model.Documents = DocumentManager.GetDocumnets(model.SrcType,model.RefId);
            return PartialView("~/Areas/Commercial/Views/CommFileUpload/Index.cshtml", model);
        }

        public ActionResult Edit(DocumentViewModel model)
        {
            if (model.DocumentId>0)
            {
                var document = DocumentManager.GetDocumentById(model.DocumentId);
                model.DocumentId = document.DocumentId;
                model.RefId = document.RefId;
                model.Path = document.Path;
                model.SrcType = document.SrcType;
                model.Description = document.Description;
                model.Name = document.Name;
            }
            return View(model);
        }
        public ActionResult Save(Document document)
        {
            try
            {
                var saveIndex = document.DocumentId > 0 ? DocumentManager.EditDocument(document) : DocumentManager.SaveDocument(document);
                if (saveIndex > 0)
                {
                    return RedirectToAction("Index", new { RefId = document.RefId, SrcType =document.SrcType});
                }
            }
            catch (Exception exception)
            {
                
               Errorlog.WriteLog(exception);
               return ErrorResult(exception.Message);
            }
           
            return ErrorResult("Save Failed !");
        }


        public ActionResult Delete(Document document)
        {
            try
            {
                int deleteIndex = DocumentManager.DeleteDocument(document.DocumentId);
                if (deleteIndex > 0)
                {
                    return RedirectToAction("Index", new { RefId = document.RefId, SrcType = document.SrcType });
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

            return ErrorResult("Save Failed !");
        
        }

      
    }
}