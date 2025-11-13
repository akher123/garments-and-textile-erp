using System;
using System.Collections;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttingTagController : BaseController
    {
        public readonly ICuttingTagManager _cuttingTagManager;
        private readonly IComponentManager _componentManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IPartyManager _partyManager;
        private readonly ICuttingTagSupplierManager _cuttingTagSupplierManager;
    
        public CuttingTagController( ICuttingTagManager cuttingTagManager, IComponentManager componentManager, ICuttingSequenceManager cuttingSequenceManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager buyOrdStyleManager, IPartyManager partyManager, ICuttingTagSupplierManager cuttingTagSupplierManager)

        {
          
            _cuttingTagManager = cuttingTagManager;
            _componentManager = componentManager;
            _cuttingSequenceManager = cuttingSequenceManager;
            _buyerManager = buyerManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _partyManager = partyManager;
            _cuttingTagSupplierManager = cuttingTagSupplierManager;
            _partyManager = partyManager;
        }
        [AjaxAuthorize(Roles = "cuttingtag-1,cuttingtag-2,cuttingtag-3")]
        public ActionResult Index(CuttingTagViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as  IEnumerable;
            if (!model.IsSearch)
            {
                model.IsSearch = true;

                return View(model);
            }
            int totalRecords = 0;
            model.CuttingSequences = _cuttingSequenceManager.GetCuttingSequenceByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.CuttingSequence.ColorRefId, model.CuttingSequence.OrderNo, model.CuttingSequence.BuyerRefId, model.CuttingSequence.OrderStyleRefId);
            model.TotalRecords = totalRecords;
            model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingSequence.BuyerRefId);
            model.StyleList = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingSequence.OrderNo);
            model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingSequence.OrderStyleRefId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult TagList(CuttingTagViewModel model)
        {
            ModelState.Clear();
            model.CuttingTags = _cuttingTagManager.GetAllCuttingTatByCuttingSequenceId(model.CuttingTag.CuttingSequenceId);
            model.Components = _componentManager.GetComponents();
            return View("~/Areas/Production/Views/CuttingTag/TagList.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult Save(CuttingTagViewModel model)
        {
            int index = 0;
            try
            {
                if (_cuttingTagManager.IsCuttingTagExist(model.CuttingTag))
                {
                    return ErrorResult("This Information Already Exist ! Please Entry Another One");
                }
                else
                {
                    index = model.CuttingTag.CuttingTagId > 0 ? _cuttingTagManager.EditCuttingTag(model.CuttingTag) : _cuttingTagManager.SaveCuttingTag(model.CuttingTag);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Save Cutting Tag" + exception);
            }
            if (index > 0)
            {
                model.CuttingTags = _cuttingTagManager.GetAllCuttingTatByCuttingSequenceId(model.CuttingTag.CuttingSequenceId);
                model.Components = _componentManager.GetComponents();
                return View("~/Areas/Production/Views/CuttingTag/TagList.cshtml", model);

            }
            else
            {
                return ErrorResult("Fail To Save Cutting Tag");
            }
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult Edit(CuttingTagViewModel model)
        {
            try
            {
                if (model.CuttingTag.CuttingTagId > 0)
                {
                    model.CuttingTag = _cuttingTagManager.GetCuttingTagByCuttingTagId(model.CuttingTag.CuttingTagId);
                }
                model.Components = _componentManager.GetComponents();
                model.CuttingTag.CuttingSequenceId = model.CuttingTag.CuttingSequenceId;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data");
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "cuttingtag-3")]
        public ActionResult Delete(CuttingTagViewModel model)
        {
            var index = 0;
            try
            {
                index = _cuttingTagManager.DeleteCuttingTag(model.CuttingTag.CuttingTagId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Cutting Tag :" + exception);
            }
            if (index>0)
            {
                model.CuttingTags = _cuttingTagManager.GetAllCuttingTatByCuttingSequenceId(model.CuttingTag.CuttingSequenceId);
                model.Components = _componentManager.GetComponents();
                return View("~/Areas/Production/Views/CuttingTag/TagList.cshtml", model);
            }
            else
            {
                return ErrorResult("Fail To Delele Cutting Tag ");
            }
           
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult GetCuttingTagSupplier(CuttingTagViewModel model)
        {
            ModelState.Clear();
            model.CuttingTags = _cuttingTagManager.GetAllCuttingTagSupplierByCuttingTagId(model.CuttingTagSupplier.CuttingTagId);
            return View("~/Areas/Production/Views/CuttingTag/TagSupplier.cshtml", model);
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult EditTagSupplier(CuttingTagViewModel model)
        {
            const  string factoryType = "F";
            try
            {
                model.CuttingTag = _cuttingTagManager.GetCuttingTagByCuttingTagId(model.CuttingTagSupplier.CuttingTagId);
                if (model.CuttingTagSupplier.CuttingTagSupplierId > 0)
                {
                    model.CuttingTagSupplier = _cuttingTagSupplierManager.GetCuttingTagByCuttingTagId(model.CuttingTagSupplier.CuttingTagSupplierId);
                }
                model.Parties = _partyManager.GetParties(factoryType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data");
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "cuttingtag-2,cuttingtag-3")]
        public ActionResult SaveCuttingTagSupplier(CuttingTagViewModel model)
        {
            int index = 0;
            try
            {
                if (_cuttingTagSupplierManager.IsCuttingTagSupplierExist(model.CuttingTagSupplier))
                {
                    return ErrorResult("This Information Already Exist");
                }
                else
                {
                    index = model.CuttingTagSupplier.CuttingTagSupplierId > 0 ? _cuttingTagSupplierManager.EditCuttingTagSupplier(model.CuttingTagSupplier) : _cuttingTagSupplierManager.SaveCuttingTagSupplier(model.CuttingTagSupplier);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Save Cutting Tag Supplier" + exception);
            }
            if (index > 0)
            {
                model.CuttingTags = _cuttingTagManager.GetAllCuttingTagSupplierByCuttingTagId(model.CuttingTagSupplier.CuttingTagId);
                return View("~/Areas/Production/Views/CuttingTag/TagSupplier.cshtml", model);
            }
            else
            {
                return ErrorResult("Fail To Save Cutting Tag Supplier");
            }
        }
        [AjaxAuthorize(Roles = "cuttingtag-3")]
        public ActionResult DeleteTagSupplier(CuttingTagViewModel model)
        {
            var index = 0;
            try
            {
                index = _cuttingTagSupplierManager.DeleteCuttingTagSupplier(model.CuttingTagSupplier.CuttingTagSupplierId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Cutting Tag Supplier:" + exception);
            }
            if (index > 0)
            {
                model.CuttingTags = _cuttingTagManager.GetAllCuttingTagSupplierByCuttingTagId(model.CuttingTagSupplier.CuttingTagId);
                return View("~/Areas/Production/Views/CuttingTag/TagSupplier.cshtml", model);
            }
            else
            {
                return ErrorResult("Fail To Delele Cutting Tag Supplier");
            }
        }
        public ActionResult PrintEmbroideryBalance(PrintEmbroideryBalanceViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.PrintEmbroideryBalanceList = _cuttingTagManager.GetPrintEmbroideryBalance(model.CuttingBatch.CuttingBatchRefId,model.CuttingBatch.BuyerRefId,model.CuttingBatch.OrderNo,model.CuttingBatch.OrderStyleRefId,model.CuttingBatch.ColorRefId);
            model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingBatch.BuyerRefId);
            model.StyleList = _buyOrdStyleManager.GetStyleByOrderNo(model.CuttingBatch.OrderNo);
            model.Colors = _buyOrdStyleManager.GetColorsByOrderStyleRefId(model.CuttingBatch.OrderStyleRefId);
            return View(model);
           
        }

        public JsonResult GetGetCuttingTagBySequence(string colorRefId, string componentRefId, string orderStyleRefId)
        {
            object tags = _cuttingTagManager.GetGetCuttingTagBySequence(colorRefId,componentRefId,orderStyleRefId);
            return Json(tags,JsonRequestBehavior.AllowGet);
        }
    }
}