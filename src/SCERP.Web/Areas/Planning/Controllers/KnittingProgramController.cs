using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class KnittingProgramController : BaseController
    {
        private readonly IProgramManager _programManager;
        private readonly IPartyManager _partyManager;
        private readonly IProcessorManager _processorManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public KnittingProgramController(IOmBuyOrdStyleManager buyOrdStyle, IOmBuyerManager buyerManager, IPartyManager partyManager, IProgramManager programManager, IProcessorManager processorManager)
        {
            _partyManager = partyManager;
            _programManager = programManager;
            _processorManager = processorManager;
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
        }

        [AjaxAuthorize(Roles = "knittingprogram-1,knittingprogram-2,knittingprogram-3")]
        public ActionResult Index(ProgramViewModel model)
        {
            var totalRecords = 0;
            var processRefId = ProcessType.KNITTING;
            model.VPrograms = _programManager.GetVwProgramsPaging(model.SearchString, processRefId, model.PageIndex, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingprogram-2,knittingprogram-3")]
        public ActionResult Edit(ProgramViewModel model)
        {
            ModelState.Clear();

            if (model.Program.ProgramId > 0)
            {
                bool isLocked = _programManager.ProgramIsLoked(model.Program.ProgramId);
                if (!isLocked)
                {
                    var program = _programManager.GetProgramById(model.Program.ProgramId);
                    model.Program = program;
                    var inputDetails = _programManager.GetInPutProgramDetails(model.Program.ProgramId);
                    var outputDetails = _programManager.GetOutPutProgramDetails(model.Program.ProgramId);
                    model.InputDataTable = inputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
                    model.OutputDataTable = outputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
                }
                else
                {
                    return ErrorResult("This Program is not edite able because yarn is issued according to this program !!");
                }

            }
            else
            {
                model.Program.PrgDate = DateTime.Now;
                const string prefixKnittingProgram = "PP";
                model.Program.ProgramRefId = _programManager.GetNewProgramRefId(prefixKnittingProgram, ProcessCode.KNITTING);
            }
            // const long partyIdFroPlammy = 1;
            model.Parties = _partyManager.GetParties("P");
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessCode.KNITTING, PortalContext.CurrentUser.CompId);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.Program.BuyerRefId);
            model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.Program.OrderNo);
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingprogram-2,knittingprogram-3")]
        public ActionResult Save(ProgramViewModel model)
        {
            try
            {
                model.InPutProgramDetails = model.InputDataTable.Select(x => x.Value).ToList();
                model.OutPutProgramDetails = model.OutputDataTable.Select(x => x.Value).ToList();
                model.Program.xStatus = "R";
                model.Program.CompId = PortalContext.CurrentUser.CompId;
                model.Program.ProcessRefId = ProcessCode.KNITTING;
                model.Program.PrepairedBy = PortalContext.CurrentUser.UserId;
                model.Program.IsApproved = false;
                model.Program.IsLock = false;
                int saveIndex = 0;
                if (model.InPutProgramDetails.Any() && model.OutPutProgramDetails.Any())
                {

                    model.InPutProgramDetails.AddRange(model.OutPutProgramDetails);
                    model.Program.PLAN_ProgramDetail = model.InPutProgramDetails.Select(x => new PLAN_ProgramDetail
                    {
                        CompId = PortalContext.CurrentUser.CompId,
                        PrgramRefId = model.Program.ProgramRefId,
                        ProgramId = model.Program.ProgramId,
                        ColorRefId = x.ColorRefId,
                        SizeRefId = x.SizeRefId,
                        MType = x.MType,
                        ItemCode = x.ItemCode,
                        Remarks = x.Remarks,
                        FinishSizeRefId = x.FinishSizeRefId,
                        Rate = x.Rate,
                        GSM = x.GSM,
                        LotRefId = x.LotRefId,
                        SleeveLength = x.SleeveLength,
                        Quantity = x.Quantity
                    }).ToList();
                    if (model.Program.ProgramId > 0)
                    {
                        saveIndex = _programManager.EditProgram(model.Program);
                    }
                    else
                    {
                        const string prefixKnittingProgram = "PP";
                        model.Program.ProgramRefId = _programManager.GetNewProgramRefId(prefixKnittingProgram, ProcessCode.KNITTING);
                        saveIndex = _programManager.SaveProgram(model.Program);

                    }
                }
                return saveIndex > 0 ? (ActionResult)RedirectToAction("Index") : ErrorMessageResult();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
        [AjaxAuthorize(Roles = "knittingprogram-3")]
        public ActionResult Delete(long programId)
        {
            try
            {
                bool isLocked = _programManager.ProgramIsLoked(programId);
                if (!isLocked)
                {
                    int deleted = _programManager.DeleteProgramById(programId);
                    return deleted > 0 ? Reload() : ErrorResult("Delete Failed");
                }
                else
                {
                    return ErrorResult("This Program is not deleted because yarn is issued according to this program !!");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }


        }

        public ActionResult AddOutputDetailRow(ProgramViewModel model)
        {
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.OutputProgramDetail.MType = "O";
            model.OutputProgramDetail.ItemCode = model.OutputProgramDetail.ItemCode ?? model.SampleItemCode;
            model.OutputProgramDetail.ItemName = model.OutputProgramDetail.ItemName ?? model.SampleItemName;
            model.OutputProgramDetail.ColorRefId = model.OutputProgramDetail.ColorRefId ?? model.SampleColorRefId;
            model.OutputProgramDetail.ColorName = model.OutputProgramDetail.ColorName ?? model.SampleColorName;
            model.OutputDataTable.Add(key, model.OutputProgramDetail);
            if (model.OutputProgramDetail.Rate<=0)
            {
                return ErrorResult("Knitting production rate missing ! Please put actual rate");
            }
            else
            {
                return View("~/Areas/Planning/Views/KnittingProgram/_OutputDetailRow.cshtml", model);
            }
            
        }
        public ActionResult AddInputDetailRow(ProgramViewModel model)
        {
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.InputProgramDetail.MType = "I";
            model.InputDataTable.Add(key, model.InputProgramDetail);
            return View("~/Areas/Planning/Views/KnittingProgram/_InputDetailRow.cshtml", model);

        }
        public ActionResult ApprovedKnittingProgramList(ProgramViewModel model)
        {

            var totalRecords = 0;
            model.VPrograms = _programManager.GetApprovedKnittingProgramByPaging(ProcessCode.KNITTING, model.PageIndex, model.sort, model.sortdir, model.IsApproved, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult ApprovedKnittingProgram(long programId)
        {
            int index = 0;
            index = _programManager.ApprovedKnittingProgram(programId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KnittingProgramStatus(ProgramViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now;
            model.ToDate = model.ToDate ?? DateTime.Now;
            model.Parties = _partyManager.GetParties("P");
            model.ProcessRefId = ProcessCode.KNITTING;
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                return View(model);
            }
            model.VPrograms = _programManager.GetKnittingProgramStatus(model.FromDate, model.ToDate, model.ProcessRefId, model.Program.PartyId, model.SearchString, PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult GetOrderByBuyer(string buyerRefId)
        {
            IEnumerable orderList = _buyOrdStyle.GetOrderByBuyer(buyerRefId);
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStyleByOrderNo(string orderNo)
        {
            IEnumerable styleList = _buyOrdStyle.GetStyleByOrderNo(orderNo);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KnittingProgramReport(long programId, int report)
        {

            var inPutProgramDetails = _programManager.GetInPutProgramDetails(programId);
            var outPutProgramDetails = _programManager.GetOutPutProgramDetails(programId);
            var program = _programManager.GetVProgramById(programId);
            string reportName = "KnittingProgramReport.rdlc";
            if (report == 1)
            {
                reportName = "KnittingProgramWithValueReport.rdlc";
            }
            var path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), reportName);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProgramDSet", program), new ReportDataSource("ProgramOutDset", outPutProgramDetails), new ReportDataSource("PrgramInputDSet", inPutProgramDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }


        public ActionResult LockedProgram(ProgramViewModel model)
        {
            var totalRecords = 0;
            model.VPrograms = _programManager.GetLokedProgramByPaging(model.SearchString, model.PageIndex, model.sort, model.sortdir, model.IsLock.GetValueOrDefault(), PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult IsLockedProgram(long programId)
        {
            int index = 0;
            index = _programManager.LockedProgram(programId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }




        public ActionResult GetFabricNameByStyle(string orderStyleRefId,string colorRefId)
        {
            List<Dropdown> fabrics = _programManager.GetConumptionFabrics(orderStyleRefId,colorRefId, PortalContext.CurrentUser.CompId);
            return Json(fabrics, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFabricColorNameByStyle(string orderStyleRefId)
        {
            List<Dropdown> fabrics = _programManager.GetFabricColorNameByStyle(orderStyleRefId,PortalContext.CurrentUser.CompId);
            return Json(fabrics, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "knittingprogram-2,knittingprogram-3")]
        public ActionResult CopyPast(ProgramViewModel model)
        {
            ModelState.Clear();

            if (model.Program.ProgramId > 0)
            {

                var program = _programManager.GetProgramById(model.Program.ProgramId);
                model.Program = program;
                var inputDetails = _programManager.GetInPutProgramDetails(model.Program.ProgramId);
                var outputDetails = _programManager.GetOutPutProgramDetails(model.Program.ProgramId);
                model.InputDataTable = inputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
                model.OutputDataTable = outputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
                model.Program.ProgramId = 0;
                model.Program.PrgDate = DateTime.Now;
                const string prefixKnittingProgram = "PP";
                model.Program.ProgramRefId = _programManager.GetNewProgramRefId(prefixKnittingProgram, ProcessCode.KNITTING);
            }
            // const long partyIdFroPlammy = 1;
            model.Parties = _partyManager.GetParties("P");
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessCode.KNITTING, PortalContext.CurrentUser.CompId);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.Program.BuyerRefId);
            model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.Program.OrderNo);
            return View("~/Areas/Planning/Views/KnittingProgram/Edit.cshtml", model);
        }

        public ActionResult ProgramList(ProgramViewModel model)
        {
            var totalRecords = 0;
            model.VPrograms = _programManager.GetLokedProgramByPaging(model.SearchString, model.PageIndex, model.sort, model.sortdir, model.IsLock.GetValueOrDefault(), PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult UpdateRate(ProgramViewModel model)
        {
            ModelState.Clear();

            if (model.ProgramId > 0)
            {
                var program = _programManager.GetProgramById(model.ProgramId);
                model.Program = program;
                var inputDetails = _programManager.GetInPutProgramDetails(model.ProgramId);
                var outputDetails = _programManager.GetOutPutProgramDetails(model.ProgramId);
                model.InputDataTable = inputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
                model.OutputDataTable = outputDetails.ToDictionary(x => Convert.ToString(x.ProgramDetailId), x => x);
  
            }
            return View(model);
        }

        public ActionResult SaveUpdatedRate(ProgramViewModel model)
        {
            int saved = 0;
            var values = model.OutputDataTable.Values.ToList();
            if (values.Where(x=>x.Rate>0).Any())
            {
                saved= _programManager.UpdateProgramRate(values);
            }
            if (saved>0)
            {
                return Reload();
            }
            else
            {
                return ErrorResult("Rate updat falied");
            }
       
        }
    }
}