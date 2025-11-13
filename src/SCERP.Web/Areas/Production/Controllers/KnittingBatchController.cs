using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingBatchController : BaseController
    {
        private readonly IBatchManager _batchManager;
        private readonly IColorManager _colorManager;
        private readonly IOmColorManager _omColorManager;
        private readonly IOmSizeManager _sizeManager;
        private readonly IMachineManager _machineManager;
        private readonly IPartyManager partyManager;
        public KnittingBatchController(IPartyManager partyManager,IMachineManager machineManager, IBatchManager batchManager, IOmColorManager omColorManager, IOmSizeManager sizeManager, IColorManager colorManager)
        {
            this._batchManager = batchManager;
            this._colorManager = colorManager;
            _omColorManager = omColorManager;
            _sizeManager = sizeManager;
            _machineManager = machineManager;
            this.partyManager = partyManager;
        }

        public ActionResult Index(BatchViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
         
            model.VProBatches = _batchManager.GetBachListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(BatchViewModel model)
        {
            const string partyType = "P";
            ModelState.Clear();
            model.Parties = partyManager.GetParties(partyType);

            model.Machines = _machineManager.GetMachines(ProcessCode.KNITTING);
            model.OmColors = _omColorManager.GetOmColors();
            model.OmSizes = _sizeManager.GetOmSizes();
            if (model.BatchId <= 0)
            {
            
                const string prefix = "KB";
                model.BtRefNo = _batchManager.GetBachNewRefNo( prefix);

            }
            else
            {
                var batch = _batchManager.GetBachById(model.BatchId);
                model.BatchNo = batch.BatchNo;
                model.BtRefNo = batch.BtRefNo;
                model.BatchId = batch.BatchId;
                model.BatchQty = batch.BatchQty;
                model.ColorId = batch.ColorId;
                model.PartyId = batch.PartyId;
                model.MachineId = batch.MachineId;
                model.BatchDate = batch.BatchDate;
                model.ItemId = batch.ItemId;
                model.ItemName = batch.ItemName;
                model.Gsm = batch.Gsm;
                model.GrColorRefId = batch.GrColorRefId;
                model.GColorName = batch.GColorName;
                model.GSizeRefId = batch.GSizeRefId;
                model.GSizeName = batch.GSizeName;
                model.FColorRefId = batch.FColorRefId;
                model.FColorName = batch.FColorName;
                model.FSizeRefId = batch.FSizeRefId;
                model.FSizeName = batch.FSizeName;
                model.ShadePerc = batch.ShadePerc;
                model.CostRate = batch.CostRate;
                model.BillRate = batch.BillRate;
                model.OrderStyleRefId = batch.OrderStyleRefId;
                model.StyleName = batch.StyleName;
                model.ColorName = batch.ColorName;
            }


            return View(model);
        }

        public ActionResult Save(Pro_Batch model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                if (model.ColorId == 0)
                {
                    errorMessage = "Invalid Color Name ! Select Correct One";
                }
                else
                {
                 
                    bool exist = _batchManager.IsBatchExist(model);
                    if (!exist)
                    {
                        index = model.BatchId > 0 ? _batchManager.EditBatch(model) : _batchManager.SaveBatch(model);
                    }
                    else
                    {
                        return ErrorResult("Batch No :" + model.BatchNo + " " + "Already Exsit ! Please Entry another one");
                    }
                }

            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }

            return index > 0 ? Reload() : ErrorResult("Failed to save Batch ! " + errorMessage);
        }

        public ActionResult GeBachInfo(string btRefNo)
        {
            var batch = _batchManager.GetBachByRefNo(btRefNo);
            return Json(batch, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchInActive(BatchViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
         
            model.VProBatches = _batchManager.GetBachListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult DailyKnittingBatch(BatchViewModel model)
        {
            var totalRecords = 0;
           
            ModelState.Clear();
            model.BatchDate = model.BatchDate ?? DateTime.Now.Date;
            model.FromDate = model.BatchDate;
            model.ToDate = model.BatchDate;
            model.VProBatches = _batchManager.GetBachListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult SaveInActiveBatch(int batchId)
        {
            int updateIndex = _batchManager.SaveInActiveBatch(batchId);
            return ErrorResult(updateIndex > 0 ? "Done" : "In Active Failed");
        }
        public JsonResult AutoCompliteColor(string searchString)
        {
            var colorList = _colorManager.AutoCompliteColor(searchString);
            return Json(colorList, JsonRequestBehavior.AllowGet);
        }
    }
}