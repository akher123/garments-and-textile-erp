using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class ReDyeingFabricReceiveController : BaseController
    {
        private readonly IReDyeingFabricReceiveManager _reDyeingFabricReceiveManager;
        private readonly IPartyManager _partyManager;
        private readonly IFinishFabricIssueManager _fabricIssueManager;
        public ReDyeingFabricReceiveController(IReDyeingFabricReceiveManager reDyeingFabricReceiveManager, IPartyManager partyManager, IFinishFabricIssueManager fabricIssueManager)
        {
            _reDyeingFabricReceiveManager = reDyeingFabricReceiveManager;
            _partyManager = partyManager;
            _fabricIssueManager = fabricIssueManager;
        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-1,redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public ActionResult Index(ReDyeingFabricReceiveViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            var compId = PortalContext.CurrentUser.CompId;
            model.FabricReceives = _reDyeingFabricReceiveManager.GetReDyeingFabReceivesByPaging(compId, model.SearchString,
                model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);

        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public ActionResult Edit(ReDyeingFabricReceiveViewModel model)
        {
            ModelState.Clear();
            if (model.ReDyeingFabricReceive.ReDyeingFabricReceiveId>0)
            {
                model.ReDyeingFabricReceive =
                    _reDyeingFabricReceiveManager.GetReDyeingFabricReceiveById(model.ReDyeingFabricReceive
                        .ReDyeingFabricReceiveId);
            
                  List<VwReDyeingFabricReceiveDetail> fabricReceiveDetails= _reDyeingFabricReceiveManager.GetVwReDyeingFabricReceiveDetailById(model.ReDyeingFabricReceive .ReDyeingFabricReceiveId);
                model.ReDyeingFabricReceiveDetails =
                    fabricReceiveDetails.ToDictionary(x => x.ReDyeingFabricReceiveDetailId.ToString(), x => x);
            }
            else
            {
                model.ReDyeingFabricReceive.RefNo = _reDyeingFabricReceiveManager.GetNewRefNo(PortalContext.CurrentUser.CompId);
                model.ReDyeingFabricReceive.ReceiveDate=DateTime.Now;
            }
            model.Parties = _partyManager.GetParties("P");
            return View(model);
        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public ActionResult Save(ReDyeingFabricReceiveViewModel model)
        {
            int saveIndex = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;

                model.ReDyeingFabricReceive.Inventory_ReDyeingFabricReceiveDetail = model.ReDyeingFabricReceiveDetails
                    .Select(x => x.Value)
                    .Select(x => new Inventory_ReDyeingFabricReceiveDetail()
                    {
                        ReDyeingFabricReceiveId = model.ReDyeingFabricReceive.ReDyeingFabricReceiveId,
                        RQty = x.RQty,
                        BatchId = x.BatchId,
                        BatchDetailId = x.BatchDetailId,
                     
                    }).ToList();

                if (model.ReDyeingFabricReceive.ReDyeingFabricReceiveId > 0)
                {
                    saveIndex = _reDyeingFabricReceiveManager.EditRedyeingFabricReceive(model.ReDyeingFabricReceive);
                }
                else
                {
                    model.ReDyeingFabricReceive.CompId = compId;
                    saveIndex = _reDyeingFabricReceiveManager.SaveFinishFabricIssue(model.ReDyeingFabricReceive);
                }
            }

            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Save Failed " + exception.Message);
            }
            return saveIndex > 0 ? Reload(): ErrorResult("Save Failed ");
        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-3")]
        public ActionResult Delete(long reDyeingFabricReceiveId)
        {
            int delted = 0;
            try
            {
                delted = _reDyeingFabricReceiveManager.DeleteRedyeingFabricReceive(reDyeingFabricReceiveId);
                return delted > 0 ? Reload() : ErrorResult("Delete Failed");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public ActionResult AddNewRow([Bind(Include = "ReDyeingFabricReceiveDetail")]ReDyeingFabricReceiveViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.ReDyeingFabricReceiveDetails.Add(model.Key, model.ReDyeingFabricReceiveDetail);
            return PartialView("~/Areas/Inventory/Views/ReDyeingFabricReceive/_ReDyeingFabricReceiveDetail.cshtml", model);
        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public JsonResult GetReceivedBatchAutocomplite(string searchString,long partyId)
        {

            object batchList = _fabricIssueManager.GetReceivedBatchAutocomplite(PortalContext.CurrentUser.CompId, searchString, partyId);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "redyeingfabricreceive-2,redyeingfabricreceive-3")]
        public JsonResult GetReceivedBatchDetailByBatchId(long batchId)
        {
            object batchDetailsList = _fabricIssueManager.GetReceivedBatchDetailByBatchId(batchId);
            return Json(batchDetailsList, JsonRequestBehavior.AllowGet);
        }
	}
}