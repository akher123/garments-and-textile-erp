using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class StanderdMinValueController : BaseController
    {
        private readonly IStanderdMinValueManager _standerdMinValueManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly ISubProcessManager _subProcessManager;
        public StanderdMinValueController(IStanderdMinValueManager standerdMinValue, IOmBuyOrdStyleManager buyOrdStyleManager, IBuyerOrderManager buyerOrderManager, ISubProcessManager subProcessManager)
        {
            this._standerdMinValueManager = standerdMinValue;
            this._omBuyOrdStyleManager = buyOrdStyleManager;
            this._buyerOrderManager = buyerOrderManager;
            this._subProcessManager = subProcessManager;
        }

  
        public ActionResult Index(StanderdMinValueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string closed = "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(closed,model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
      
        public ActionResult BuyerOrderStyleLsit(StanderdMinValueViewModel model)
        {
            ModelState.Clear();
            model.BuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return PartialView("~/Areas/Production/Views/StanderdMinValue/_OrderStyleList.cshtml", model);
        }

        public ActionResult SmvList(StanderdMinValueViewModel model)
        {
            model.SmvcList = _standerdMinValueManager.GetStanderdMinValuesByStyleOrder(model.OrderStyleRefId);
            return PartialView("~/Areas/Production/Views/StanderdMinValue/SmvList.cshtml", model);
        }
       [HttpGet]
        public ActionResult Edit(StanderdMinValueViewModel model)
        {
            ModelState.Clear();
            if (model.StanderdMinValueId > 0)
            {
                var smv = _standerdMinValueManager.GetSmvById(model.StanderdMinValueId);
                model.StanderdMinValueId = smv.StanderdMinValueId;
                model.OrderStyleRefId = smv.OrderStyleRefId;
                model.StMv = smv.StMv;
                model.CompId = smv.CompId;
                model.StanderdMinValueRefId = smv.StanderdMinValueRefId;
                model.SmvDtls = _standerdMinValueManager.GetVwSmvDetails(smv.StanderdMinValueId);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.OrderStyleRefId))
                {
                    model.StanderdMinValueRefId = _standerdMinValueManager.GetStanderdMinValueRefId();
                }
                else
                {
                    return ErrorResult("Select Any Style to assign SMV value");
                }
            }
            model.SubProcesses = _standerdMinValueManager.GetSubProcessList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(StanderdMinValueViewModel model)
        {
            try
            {
                var saveIndex = 0;
                var standerdMinValue = new PROD_StanderdMinValue
                {
                    OrderStyleRefId = model.OrderStyleRefId,
                    StMv = model.StMv,
                    CompId = model.CompId,
                    StanderdMinValueRefId = model.StanderdMinValueRefId,
                    StanderdMinValueId = model.StanderdMinValueId,

                    PROD_StanderdMinValDetail = model.SmvDtls.Select(x => new PROD_StanderdMinValDetail()
                    {
                        StMvD = x.Value.StMvD,
                        SubProcessRefId = x.Value.SubProcessRefId,
                        StanderdMinValueRefId = x.Value.StanderdMinValueRefId,
                        StanderdMinValueId = x.Value.StanderdMinValueId,
                        StanderdMinValDetailId = x.Value.StanderdMinValDetailId,
                        CompId = x.Value.CompId
                    }).ToList()
                };
                if (model.StanderdMinValueId > 0)
                {
                    saveIndex = _standerdMinValueManager.EditSmv(standerdMinValue);
                }
                else
                {
                    var isExist = _standerdMinValueManager.IsSmvCreated(model.OrderStyleRefId);
                    if (!isExist)
                    {
                        saveIndex = _standerdMinValueManager.SaveSmv(standerdMinValue);
                    }
                    else
                    {
                        return ErrorResult("SMV ALREADY CREATED !!!");
                    }
                }
                if (saveIndex > 0)
                {
                    return RedirectToAction("SmvList", new { model.OrderStyleRefId });
                }
                return ErrorResult("SAVE FAILED !!");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        [HttpGet]
        public ActionResult AddNewRow(StanderdMinValueViewModel model)
        {
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.SmvDetail.StanderdMinValueRefId = model.StanderdMinValueRefId;
            model.SmvDetail.StanderdMinValueId = model.StanderdMinValueId;
            model.SmvDetail.CompId = model.CompId;
            model.SmvDetail.SubProcessName = _subProcessManager.GetSubProcessNameByRefId(model.SmvDetail.SubProcessRefId);
            model.SmvDetail.CompId = PortalContext.CurrentUser.CompId;
            model.SmvDtls.Add(model.Key, model.SmvDetail);
            return PartialView("~/Areas/Production/Views/StanderdMinValue/_SmvDetailRow.cshtml", model);
        }
        
        public ActionResult Delete(StanderdMinValueViewModel model)
        {
            var deleteIndex = _standerdMinValueManager.DeleteStanderdMinValue(model.StanderdMinValueId);

            if (deleteIndex>0)
            {
                return RedirectToAction("SmvList", new { model.OrderStyleRefId });
            }
             return ErrorResult("Delate Fail");
        }

    }
}