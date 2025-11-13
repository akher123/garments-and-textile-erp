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
    public class KnittingOrderProgramController : BaseController
    {
        private readonly IProgramManager _programManager;
        private readonly IPartyManager _partyManager;
        private readonly IProcessorManager _processorManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public KnittingOrderProgramController(IOmBuyOrdStyleManager buyOrdStyle, IOmBuyerManager buyerManager, IPartyManager partyManager, IProgramManager programManager, IProcessorManager processorManager)
        {
            _partyManager = partyManager;
            _programManager = programManager;
            _processorManager = processorManager;
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
        }

        [AjaxAuthorize(Roles = "knittingorder-1,knittingorder-2,knittingorder-3")]
        public ActionResult Index(ProgramViewModel model)
        {
            var totalRecords = 0;
            var processRefId = ProcessType.KNITTING_ORDER;
            model.VPrograms = _programManager.GetVwProgramsPaging(model.SearchString, processRefId, model.PageIndex, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingorder-2,knittingorder-3")]
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
                const string prefixKnittingProgram = "OR";
                model.Program.ProgramRefId = _programManager.GetNewProgramRefId(prefixKnittingProgram, ProcessCode.KNITTING_ORDER);
            }
            // const long partyIdFroPlammy = 1;
            model.Parties = _partyManager.GetParties("P");
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessCode.KNITTING_ORDER, PortalContext.CurrentUser.CompId).ToList();
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingorder-2,knittingorder-3")]
        public ActionResult Save(ProgramViewModel model)
        {
            try
            {
                model.InPutProgramDetails = model.InputDataTable.Select(x => x.Value).ToList();
                model.OutPutProgramDetails = model.OutputDataTable.Select(x => x.Value).ToList();
                model.Program.xStatus = "R";
                model.Program.CompId = PortalContext.CurrentUser.CompId;
                model.Program.ProcessRefId = ProcessCode.KNITTING_ORDER;
                model.Program.PrepairedBy = PortalContext.CurrentUser.UserId;
                model.Program.IsApproved = false;
                model.Program.IsLock = false;
                model.Program.CID = "SC";
                model.Program.BuyerRefId = "092";
                model.Program.OrderNo = "PLF/ORD00001";
                model.Program.OrderStyleRefId = "ST00001";
                model.Program.ProgramType = "OR"; //Party Order Knitting Program
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
                        const string prefixKnittingProgram = "OR";
                        model.Program.ProgramRefId = _programManager.GetNewProgramRefId(prefixKnittingProgram, ProcessCode.KNITTING_ORDER);
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
        [AjaxAuthorize(Roles = "knittingorder-3")]
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
            model.OutputDataTable.Add(key, model.OutputProgramDetail);
            if (model.OutputProgramDetail.Rate <= 0)
            {
                return ErrorResult("Knitting production rate missing ! Please put actual rate");
            }
            else
            {
                return View("~/Areas/Planning/Views/KnittingOrderProgram/_OutputDetailRow.cshtml", model);
            }

        }
        public ActionResult AddInputDetailRow(ProgramViewModel model)
        {
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.InputProgramDetail.MType = "I";
            model.InputDataTable.Add(key, model.InputProgramDetail);
            return View("~/Areas/Planning/Views/KnittingOrderProgram/_InputDetailRow.cshtml", model);

        }
        public ActionResult KnittingProgramReport(long programId)
        {

            var inPutProgramDetails = _programManager.GetInPutProgramDetails(programId);
            var outPutProgramDetails = _programManager.GetOutPutProgramDetails(programId);
            var program = _programManager.GetVProgramById(programId);

            var path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), "KnittingOrderProgramReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProgramDSet", program), new ReportDataSource("ProgramOutDset", outPutProgramDetails), new ReportDataSource("PrgramInputDSet", inPutProgramDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }




    }
}