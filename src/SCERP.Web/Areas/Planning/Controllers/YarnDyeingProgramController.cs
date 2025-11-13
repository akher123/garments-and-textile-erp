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
    public class YarnDyeingProgramController : BaseController
    {
        private readonly IProgramManager _programManager;
        private readonly IPartyManager _partyManager;
        private readonly IProcessorManager _processorManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public YarnDyeingProgramController(IOmBuyOrdStyleManager buyOrdStyle, IOmBuyerManager buyerManager, IPartyManager partyManager, IProgramManager programManager, IProcessorManager processorManager)
        {
            _partyManager = partyManager;
            _programManager = programManager;
            _processorManager = processorManager;
            _buyerManager = buyerManager;
            _buyOrdStyle = buyOrdStyle;
        }

        [AjaxAuthorize(Roles = "yarndyeingprogram-1,yarndyeingprogram-2,yarndyeingprogram-3")]
        public ActionResult Index(ProgramViewModel model)
        {
            var totalRecords = 0;
            var processRefId = ProcessType.YARNDYEING;
            model.VPrograms = _programManager.GetVwProgramsPaging(model.SearchString, processRefId, model.PageIndex, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarndyeingprogram-2,yarndyeingprogram-3")]
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
                const string yarnDyeingProgramPrefix = "DP";
                model.Program.ProgramRefId = _programManager.GetNewProgramRefId(yarnDyeingProgramPrefix, ProcessType.YARNDYEING);
            }
            const string partyType = "P";
            model.Parties = _partyManager.GetParties(partyType);
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessType.YARNDYEING, PortalContext.CurrentUser.CompId);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.Program.BuyerRefId);
            model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.Program.OrderNo);
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarndyeingprogram-2,yarndyeingprogram-3")]
        public ActionResult Save(ProgramViewModel model)
        {
            try
            {
                model.InPutProgramDetails = model.InputDataTable.Select(x => x.Value).ToList();
                model.OutPutProgramDetails = model.OutputDataTable.Select(x => x.Value).ToList();
                model.Program.xStatus = "R";
                model.Program.CompId = PortalContext.CurrentUser.CompId;
                model.Program.ProcessRefId = ProcessCode.YARNDYEING;
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
                        Rate = x.Rate.GetValueOrDefault(),
                        GSM = x.GSM,
                        LotRefId = x.LotRefId,
                        NoOfCone = x.NoOfCone.GetValueOrDefault(),
                        SleeveLength = x.SleeveLength,
                        Quantity = x.Quantity
                    }).ToList();

                    if (model.Program.ProgramId > 0)
                    {
                        saveIndex = _programManager.EditProgram(model.Program);
                    }
                    else
                    {
                        const string yarnDyeingProgramPrefix = "DP";
                        model.Program.ProgramRefId = _programManager.GetNewProgramRefId(yarnDyeingProgramPrefix, ProcessType.YARNDYEING);
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
        [AjaxAuthorize(Roles = "yarndyeingprogram-3")]
        public ActionResult Delete(long programId)
        {
            try
            {
                var isLocked = _programManager.ProgramIsLoked(programId);
                if (!isLocked)
                {
                    var deleted = _programManager.DeleteProgramById(programId);
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
            return View("~/Areas/Planning/Views/YarnDyeingProgram/_OutputDetailRow.cshtml", model);
        }
        public ActionResult AddInputDetailRow(ProgramViewModel model)
        {
            string key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.InputProgramDetail.MType = "I";
            model.InputDataTable.Add(key, model.InputProgramDetail);
            return View("~/Areas/Planning/Views/YarnDyeingProgram/_InputDetailRow.cshtml", model);

        }
        public ActionResult ApprovedYarnDyeingProgramList(ProgramViewModel model)
        {

            var totalRecords = 0;
            model.VPrograms = _programManager.GetApprovedKnittingProgramByPaging(ProcessCode.YARNDYEING, model.PageIndex, model.sort, model.sortdir, model.IsApproved, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult ApprovedYarnDyeingProgram(long programId)
        {
            int index = 0;
            index = _programManager.ApprovedKnittingProgram(programId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YarnDayeingProgramStatus(ProgramViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now;
            model.ToDate = model.ToDate ?? DateTime.Now;
            model.Buyers = _buyerManager.GetAllBuyers();
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessCode.YARNDYEING, PortalContext.CurrentUser.CompId);
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                return View(model);
            }
            model.VPrograms = _programManager.GetKnittingProgramStatus(model.FromDate, model.ToDate, ProcessCode.YARNDYEING, model.Program.PartyId, model.SearchString, PortalContext.CurrentUser.CompId);
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
        public ActionResult DayedYarnProgramReport(long programId, int report)
        {

            var inPutProgramDetails = _programManager.GetInPutProgramDetails(programId);
            var outPutProgramDetails = _programManager.GetOutPutProgramDetails(programId);
            var program = _programManager.GetVProgramById(programId);
            string reportName = "YarnDyeingProgramWithoutValue.rdlc";
            if (report == 1)
            {
                reportName = "YarnDyeingProgram.rdlc";
            }
            var path = Path.Combine(Server.MapPath("~/Areas/Planning/Reports"), reportName);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProgramDSet", program), new ReportDataSource("ProgramOutDset", outPutProgramDetails), new ReportDataSource("PrgramInputDSet", inPutProgramDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0, MarginRight = 0, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }


        [AjaxAuthorize(Roles = "yarndyeingprogram-2,yarndyeingprogram-3")]
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
                const string yarnDyeingProgramPrefix = "DP";
                model.Program.ProgramRefId = _programManager.GetNewProgramRefId(yarnDyeingProgramPrefix, ProcessType.YARNDYEING);
            }
            const string partyType = "P";
            model.Parties = _partyManager.GetParties(partyType);
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessType.YARNDYEING, PortalContext.CurrentUser.CompId);
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.Program.BuyerRefId);
            model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.Program.OrderNo);
            return View("~/Areas/Planning/Views/YarnDyeingProgram/Edit.cshtml", model);
        }
    }
}