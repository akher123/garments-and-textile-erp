using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttFabRejectController : BaseController
    {
        private readonly IFinishFabricIssueManager _fabricIssueManager;
        private readonly ICuttFabRejectManager _cuttFabRejectManager;
        public CuttFabRejectController(IFinishFabricIssueManager fabricIssueManager, ICuttFabRejectManager cuttFabRejectManager)
        {
            _fabricIssueManager = fabricIssueManager;
            _cuttFabRejectManager = cuttFabRejectManager;
        }

        [AjaxAuthorize(Roles = "cuttfabricreject-1,cuttfabricreject-2,cuttfabricreject-3")]
        public ActionResult Index(CuttFabRejectViewModel model)
        {
            ModelState.Clear();
            int totalRecord = 0;
            model.CuttFabRejects = _cuttFabRejectManager.GetCuttFabRejectsByPaging(model.SearchString, model.PageIndex, out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }
        [AjaxAuthorize(Roles = "cuttfabricreject-2,cuttfabricreject-3")]
        public ActionResult Edit(CuttFabRejectViewModel model)
        {
            if (model.CuttFabReject.CuttFabRejectId>0)
            {
                var cuttFabReject= _cuttFabRejectManager.GetVwCuttFabRejectById(model.CuttFabReject.CuttFabRejectId);
                model.CuttFabReject.CuttFabRejectId = cuttFabReject.CuttFabRejectId;
                model.CuttFabReject.BatchId = cuttFabReject.BatchId;
                model.CuttFabReject.BatchDetailId = cuttFabReject.BatchDetailId;
                model.CuttFabReject.CuttingWit = cuttFabReject.CuttingWit;
                model.CuttFabReject.RejectWit = cuttFabReject.RejectWit;
                model.CuttFabReject.Remarks = cuttFabReject.Remarks;
                model.CuttFabReject.ChallanNo = cuttFabReject.ChallanNo;
                model.SearchString=cuttFabReject.BatchNo;

                model.Items = _fabricIssueManager.GetReceivedBatchDetailByBatchId(model.CuttFabReject.BatchId);
            }
       
            return View(model);
        }
        [AjaxAuthorize(Roles = "cuttfabricreject-2,cuttfabricreject-3")]
        public ActionResult Save(CuttFabRejectViewModel model)
        {
            try
            {
                var cuttFabReject = _cuttFabRejectManager.GetCuttFabRejectById(model.CuttFabReject.CuttFabRejectId) ?? new PROD_CuttFabReject();
                cuttFabReject.BatchDetailId = model.CuttFabReject.BatchDetailId;
                cuttFabReject.BatchId = model.CuttFabReject.BatchId;
                cuttFabReject.CompId = PortalContext.CurrentUser.CompId;
                cuttFabReject.CuttingWit = model.CuttFabReject.CuttingWit;
                cuttFabReject.ChallanNo = model.CuttFabReject.ChallanNo;
                cuttFabReject.RejectWit = model.CuttFabReject.RejectWit;
                cuttFabReject.Remarks = model.CuttFabReject.Remarks;
                cuttFabReject.EntryDate = DateTime.Now;
                int saveIndex = _cuttFabRejectManager.SaveFabReject(cuttFabReject);
                return saveIndex > 0 ? Reload() : ErrorResult("Save Failed ! Please contact with provider");
            }
            catch (Exception e)
            {
                return ErrorResult("Save Failed ! Please contact with provider"+"/n Error:"+e.Message);
            }
       
        }
        [AjaxAuthorize(Roles = "cuttfabricreject-3")]
        public ActionResult Delete(int cuttFabRejectId)
        {
            try
            {
                int deleteStatus = _cuttFabRejectManager.DeleteCuttFabReject(cuttFabRejectId);
                return deleteStatus > 0 ? Reload() : ErrorResult("Delete Failed ! Please contact with provider");
            }
            catch (Exception e)
            {
             Errorlog.WriteLog(e);
             return ErrorResult("Delete Failed ! Please contact with provider" + "/n Error:" + e.Message);
            }

        }

        public JsonResult GetReceivedBatchAutocomplite(string searchString)
        {
            const long  partyPlummyId = 1;
            object batchList = _fabricIssueManager.GetReceivedBatchAutocomplite(PortalContext.CurrentUser.CompId, searchString, partyPlummyId);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReceivedBatchDetailByBatchId(long batchId)
        {
            object batchDetailsList = _fabricIssueManager.GetReceivedBatchDetailByBatchId(batchId);
            return Json(batchDetailsList, JsonRequestBehavior.AllowGet);
        }
	}
}