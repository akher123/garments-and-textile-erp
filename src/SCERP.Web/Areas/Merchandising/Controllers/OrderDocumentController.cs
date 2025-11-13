using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OrderDocumentController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public OrderDocumentController(IOmBuyerManager buyerManager, IOmBuyOrdStyleManager buyOrdStyle)
        {
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
        }
        public ActionResult Index(DocumentViewModel model)
        {
            ModelState.Clear();
            model.SrcType = 1; // Order Document
            model.Documents = DocumentManager.GetDocumnets(model.SrcType,model.RefId);
            return PartialView("~/Areas/Merchandising/Views/OrderDocument/Index.cshtml", model);
        }

        public ActionResult Edit(DocumentViewModel model)
        {
            if (model.DocumentId>0)
            {
                var document = DocumentManager.GetDocumentById(model.DocumentId);
                model.DocumentId = document.DocumentId;
                model.RefId = document.RefId;
                model.Path = document.Path;
                model.Description = document.Description;
                model.Name = document.Name;
            }
            return View(model);
        }
        public ActionResult Save(Document document)
        {
            try
            {
                document.SrcType = 1;// Order Document
                var saveIndex = document.DocumentId > 0 ? DocumentManager.EditDocument(document) : DocumentManager.SaveDocument(document);
                if (saveIndex > 0)
                {
                    return RedirectToAction("Index", new { RefId = document.RefId });
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
                    return RedirectToAction("Index", new { RefId = document.RefId });
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

            return ErrorResult("Save Failed !");
        
        }

        public ActionResult StyleDocument(DocumentViewModel model)
        {
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.BuyerList = _buyerManager.GetAllBuyers();
                return View(model);
            }
            else
            {
                ModelState.Clear();
                model.SrcType = 1; // Order Document
                model.BuyerList = _buyerManager.GetAllBuyers();
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.BuyerRefId);
                model.Documents = DocumentManager.GetDocumnets(model.SrcType, model.RefId);
                return View(model);
            }
        }
    }
}