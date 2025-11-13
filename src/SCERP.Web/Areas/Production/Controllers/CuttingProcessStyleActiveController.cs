using System;
using System.Collections;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttingProcessStyleActiveController : BaseController
    {
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly ICuttingProcessStyleActiveManager _cuttingProcessStyleActiveManager;
        private readonly ICuttingBatchManager _cuttingBatchManager;

        public CuttingProcessStyleActiveController(IOmBuyOrdStyleManager buyOrdStyleManager, IOmBuyerManager buyerManager, ICuttingProcessStyleActiveManager cuttingProcessStyleActiveManager, ICuttingBatchManager cuttingBatchManager)
        {
            _buyOrdStyleManager = buyOrdStyleManager;
            _buyerManager = buyerManager;
            _cuttingProcessStyleActiveManager = cuttingProcessStyleActiveManager;
            _cuttingBatchManager = cuttingBatchManager;
        }
        [AjaxAuthorize(Roles = "styleincutting-1,styleincutting-2,styleincutting-3")]
        public ActionResult Index(CuttingProcessStyleActiveViewModel model)
        {
            ModelState.Clear();
            model.BuyerList = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            int totalRecords = 0;
            model.VwCuttingProcessStyleActiveList = _cuttingProcessStyleActiveManager.GetCuttingProcessStyleActiveByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.VwCuttingProcessStyleActive.BuyerRefId, model.VwCuttingProcessStyleActive.OrderNo, model.VwCuttingProcessStyleActive.OrderStyleRefId);
            model.OrderList = _buyOrdStyleManager.GetOrderAllByBuyer(model.VwCuttingProcessStyleActive.BuyerRefId);
            model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.VwCuttingProcessStyleActive.OrderNo);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "styleincutting-2,styleincutting-3")]
        public ActionResult Edit(CuttingProcessStyleActiveViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.CuttingProcessStyleActive.CuttingProcessStyleActiveId> 0)
                {
                    model.CuttingProcessStyleActive = _cuttingProcessStyleActiveManager.GetStyleInCuttingByCuttingProcessStyleActiveId(model.CuttingProcessStyleActive.CuttingProcessStyleActiveId);
                    model.OrderList = _buyOrdStyleManager.GetOrderByBuyer(model.CuttingProcessStyleActive.BuyerRefId);
                    model.Styles = _buyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.CuttingProcessStyleActive.OrderNo);
                }
                else
                {
                    model.CuttingProcessStyleActive.StartDate = DateTime.Now;
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
             model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }
        [AjaxAuthorize(Roles = "styleincutting-2,styleincutting-3")]
        public ActionResult Save(CuttingProcessStyleActiveViewModel model)
        {
            var index = 0;
            try
            {
                model.CuttingProcessStyleActive.CompId = PortalContext.CurrentUser.CompId;
                model.CuttingProcessStyleActive.ProcessRefId = ProcessCode.CUTTING;
                if (_cuttingProcessStyleActiveManager.IsCuttingProcessStyleActiveExist(model.CuttingProcessStyleActive))
                {
                    return ErrorResult("This Information Already Exist ! Please Entry another one");
                }
                else
                {
                    if (model.CuttingProcessStyleActive.EndDate< model.CuttingProcessStyleActive.StartDate && model.CuttingProcessStyleActive.StartDate != null)
                    {
                        return ErrorResult("End Date Must be greater than Start Date");
                    }
                    else
                    {
                        index = model.CuttingProcessStyleActive.CuttingProcessStyleActiveId > 0 ? _cuttingProcessStyleActiveManager.EditCuttingProcessStyleActive(model.CuttingProcessStyleActive) : _cuttingProcessStyleActiveManager.SaveCuttingProcessStyleActive(model.CuttingProcessStyleActive);
                    }
                     model.CuttingProcessStyleActive.ProcessRefId = ProcessType.CUTTING;
                   
                  }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save Data:" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save Data!");
        }
        [AjaxAuthorize(Roles = "styleincutting-3")]
        public ActionResult Delete(string orderStyleRefId)
        {
            int index = 0;
            try
            {
                if (_cuttingBatchManager.IsOrderStyleRefIdExist(orderStyleRefId))
                {
                    return ErrorResult("This Information Is Used In Another Table");
                }
                else
                {
                    index = _cuttingProcessStyleActiveManager.DeleteCuttingProcessStyleActive(orderStyleRefId); 
                }
                    
            }
            catch (Exception exception)
            {
               Errorlog.WriteLog(exception);
                ErrorResult("Failed to Delete");
            }
            return index > 0 ? Reload() : ErrorResult("Failed To Delete");
        }
        public ActionResult GetOrderByBuyer(string buyerRefId)
        {
            IEnumerable orderList = _buyOrdStyleManager.GetOrderByBuyer(buyerRefId);
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStyleByOrderNo(string orderNo)
        {
            IEnumerable styleList = _buyOrdStyleManager.GetStyleByOrderNo(orderNo);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CloseStyleActivity(CuttingProcessStyleActiveViewModel model)
        {
            model.CuttingProcessStyleActive = _cuttingProcessStyleActiveManager.GetStyleInCuttingByCuttingProcessStyleActiveId(model.VwCuttingProcessStyleActive.CuttingProcessStyleActiveId);
            model.CuttingProcessStyleActive.EndDate = model.VwCuttingProcessStyleActive.EndDate;
            _cuttingProcessStyleActiveManager.EditCuttingProcessStyleActive(model.CuttingProcessStyleActive);
            return Reload();
        }
        [HttpGet]
        public ActionResult CloseStyleActivity(long cuttingProcessStyleActiveId)
        {
            CuttingProcessStyleActiveViewModel model=new CuttingProcessStyleActiveViewModel();
            model.VwCuttingProcessStyleActive = _cuttingProcessStyleActiveManager.GetVwStyleInCuttingByCuttingProcessStyleActiveId(cuttingProcessStyleActiveId);
            return View(model);
        }
    }
}