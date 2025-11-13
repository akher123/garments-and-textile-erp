using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingRollController : BaseController
    {
        private readonly IKnittingRollManager _knittingRollManager;
        private readonly IMachineManager _machineManager;
        private readonly IPartyManager _partyManager;
        private readonly IProgramManager _programManager;
        private readonly IOmBuyerManager _buyerManager;
        public KnittingRollController(IOmBuyerManager buyerManager, IKnittingRollManager knittingRollManager, IMachineManager machineManager, IPartyManager partyManager, IProgramManager programManager)
        {
            _knittingRollManager = knittingRollManager;
            _machineManager = machineManager;
            _partyManager = partyManager;
            _programManager = programManager;
            _buyerManager = buyerManager;
        }
        [AjaxAuthorize(Roles = "knittingroll-1,knittingroll-2,knittingroll-3")]
        public ActionResult Index(KnittingRollViewModel model)
        {
            int totalRecord;
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.FromDate = DateTime.Now;
                model.ToDate = DateTime.Now.AddDays(1);
                model.IsSearch = true;
            }
            model.KnittingRolls = _knittingRollManager.GetKnittingRollsByPaging(model.SearchString, model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.KnittingRoll.PartyId, out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingroll-2,knittingroll-3")]
        public ActionResult Edit(KnittingRollViewModel model)
        {
            const string prefix_knitting = "R";
            ModelState.Clear();
            if (model.KnittingRoll.KnittingRollId > 0)
            {
                model.KnittingRoll = _knittingRollManager.GetVwKnittingRollById(model.KnittingRoll.KnittingRollId);
                DataTable reportObject = _knittingRollManager.GetRollSticker(model.KnittingRoll.KnittingRollId, PortalContext.CurrentUser.CompId);
                model.DataTable = reportObject;
            }
            else
            {
                model.KnittingRoll.RollDate = DateTime.Now;
                model.KnittingRoll.RollRefNo = _knittingRollManager.GetTodayRollNo(PortalContext.CurrentUser.CompId, prefix_knitting);
           
            }
            model.Machines = _machineManager.GetMachines(ProcessCode.KNITTING);
            if (!string.IsNullOrEmpty(model.KnittingRoll.ComponentRefId))
            {
                return View("EditCollarCuff", model);
            }
            else
            {
                return View(model);
            }
           
        }
        [AjaxAuthorize(Roles = "knittingroll-2,knittingroll-3")]
        public ActionResult AddBulkRoll(KnittingRollViewModel model)
        {
            ModelState.Clear();

            if (model.NoRoll > 0)
            {
                for (int i = 1; i <= model.NoRoll; i++)
                {
                    model.RollDictionary.Add(i.ToString(), model.KnittingRoll);
                }

            }
            else
            {
                model.KnittingRoll.RollDate = DateTime.Now;
            }
            //new Developed for bulk
            //if (model.NoRoll > 0)
            //{
            //    model.KnittingRoll.Rmks = model.NoRoll.ToString();
            //    model.RollDictionary.Add(model.KnittingRoll.Rmks, model.KnittingRoll);
            //}
            //else
            //{
            //    model.KnittingRoll.RollDate = DateTime.Now;
            //}
         //end
            model.Parties = _partyManager.GetParties("P");
            model.Machines = _machineManager.GetMachines(ProcessCode.KNITTING);
        
            return View(model);
        }
        [AjaxAuthorize(Roles = "knittingroll-2,knittingroll-3")]
        public ActionResult SaveBullRolls(KnittingRollViewModel model)
        {
            try
            {
                List<PROD_KnittingRoll> knittingRolls = model.RollDictionary.Select(x => new PROD_KnittingRoll
                {
                    KnittingRollId = x.Value.KnittingRollId,
                    RollDate =x.Value.RollDate,
                    RollRefNo = x.Value.RollRefNo,
                    PartyId = x.Value.PartyId,
                    ProgramId = x.Value.ProgramId,
                    MachineId = x.Value.MachineId,
                    ItemCode = x.Value.ItemCode,
                    GSM = x.Value.GSM,
                    ColorRefId = x.Value.ColorRefId,
                    SizeRefId = x.Value.SizeRefId,
                    FinishSizeRefId = x.Value.FinishSizeRefId,
                    Quantity = x.Value.Quantity,
                    RollLength = x.Value.RollLength,
                    StLength = x.Value.StLength,
                    CharllRollNo =x.Value.CharllRollNo,
                    Rmks = x.Value.Rmks,
                    CompId = PortalContext.CurrentUser.CompId,
                }).ToList();
               
               int saveed = _knittingRollManager.SaveBullRolls(knittingRolls);
                return saveed > 0 ? Reload() : ErrorResult("Saved Successfully");
            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }


        }
        public ActionResult AddNewRow(KnittingRollViewModel model)
        {
            string key = DateTime.Now.ToString("HHmmss");
            model.RollDictionary.Add(key, model.KnittingRoll);
            return View("~/Areas/Production/Views/KnittingRoll/_AddNewRowRoll.cshtml", model);
        }



        [AjaxAuthorize(Roles = "knittingroll-2,knittingroll-3")]
        public ActionResult Save(KnittingRollViewModel model)
        {
            int saveIndex = 0;
        
            PROD_KnittingRoll knittingRoll = _knittingRollManager.GetKnittingRollById(model.KnittingRoll.KnittingRollId) ?? new PROD_KnittingRoll();
            knittingRoll.KnittingRollId = model.KnittingRoll.KnittingRollId;
            knittingRoll.RollDate = model.KnittingRoll.RollDate;
            knittingRoll.RollRefNo = model.KnittingRoll.RollRefNo;
            knittingRoll.PartyId = model.KnittingRoll.PartyId;
            knittingRoll.ProgramId = model.KnittingRoll.ProgramId;
            knittingRoll.MachineId = model.KnittingRoll.MachineId;
            knittingRoll.ItemCode = model.KnittingRoll.ItemCode;
            knittingRoll.GSM = model.KnittingRoll.GSM;
            knittingRoll.ColorRefId = model.KnittingRoll.ColorRefId;
            knittingRoll.SizeRefId = model.KnittingRoll.SizeRefId;
            knittingRoll.FinishSizeRefId = model.KnittingRoll.FinishSizeRefId;
            knittingRoll.Quantity = model.KnittingRoll.Quantity;
            knittingRoll.RollLength = model.KnittingRoll.RollLength;
            knittingRoll.StLength = model.KnittingRoll.StLength;
            knittingRoll.CharllRollNo = model.KnittingRoll.CharllRollNo;
            knittingRoll.Rmks = model.KnittingRoll.Rmks;
            knittingRoll.ComponentRefId = model.KnittingRoll.ComponentRefId;
            knittingRoll.CompId = PortalContext.CurrentUser.CompId;
           // model.KnittingRoll.RollRefNo = _knittingRollManager.GetTodayRollNo(PortalContext.CurrentUser.CompId);
            saveIndex = model.KnittingRoll.KnittingRollId > 0 ? _knittingRollManager.EditKnittingRoll(knittingRoll) : _knittingRollManager.SaveKnittingRoll(knittingRoll);
            if (!model.IsContinue)
            {
                return Reload();
            }
            else
            {
                const string knitting = "R";
                model.KnittingRoll.RollRefNo = _knittingRollManager.GetTodayRollNo(PortalContext.CurrentUser.CompId, knitting);

                DataTable reportObject = _knittingRollManager.GetRollSticker(knittingRoll.KnittingRollId, PortalContext.CurrentUser.CompId);
                string html = RenderViewToString("~/Areas/Production/Views/KnittingRoll/_rollSticker.cshtml", reportObject.Todynamic());
                return Json(new { RoleData = html, RollRefNo = model.KnittingRoll.RollRefNo }, JsonRequestBehavior.AllowGet);
            }
            // return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }

        [AjaxAuthorize(Roles = "knittingroll-2,knittingroll-3")]
        public ActionResult SaveCollarCuff(KnittingRollViewModel model)
        {
            int saveIndex = 0;

            PROD_KnittingRoll knittingRoll = _knittingRollManager.GetKnittingRollById(model.KnittingRoll.KnittingRollId) ?? new PROD_KnittingRoll();
            knittingRoll.KnittingRollId = model.KnittingRoll.KnittingRollId;
            knittingRoll.RollDate = model.KnittingRoll.RollDate;
            knittingRoll.RollRefNo = model.KnittingRoll.RollRefNo;
            knittingRoll.PartyId = model.KnittingRoll.PartyId;
            knittingRoll.ProgramId = model.KnittingRoll.ProgramId;
            knittingRoll.MachineId = model.KnittingRoll.MachineId;
            knittingRoll.ItemCode = model.KnittingRoll.ItemCode;
            knittingRoll.GSM = model.KnittingRoll.GSM;
            knittingRoll.ColorRefId = model.KnittingRoll.ColorRefId;
            knittingRoll.SizeRefId = model.KnittingRoll.SizeRefId;
            knittingRoll.FinishSizeRefId = model.KnittingRoll.FinishSizeRefId;
            knittingRoll.Quantity = model.KnittingRoll.Quantity;
            knittingRoll.RollLength = model.KnittingRoll.RollLength;
            knittingRoll.StLength = model.KnittingRoll.StLength;
            knittingRoll.CharllRollNo = model.KnittingRoll.CharllRollNo;
            knittingRoll.Rmks = model.KnittingRoll.Rmks;
            knittingRoll.ComponentRefId = model.KnittingRoll.ComponentRefId;
            knittingRoll.CompId = PortalContext.CurrentUser.CompId;
         
            saveIndex = model.KnittingRoll.KnittingRollId > 0 ? _knittingRollManager.EditKnittingRoll(knittingRoll) : _knittingRollManager.SaveKnittingRoll(knittingRoll);
            if (!model.IsContinue)
            {
                return Reload();
            }
            else
            {
                model.KnittingRoll.RollDate = DateTime.Now;
                const string prefix_collar_cuff = "C";
                model.KnittingRoll.RollRefNo = _knittingRollManager.GetTodayRollNo(PortalContext.CurrentUser.CompId, prefix_collar_cuff);
                return Json(new { RoleData = "", RollRefNo = model.KnittingRoll.RollRefNo }, JsonRequestBehavior.AllowGet);
            }
            // return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }
        [AjaxAuthorize(Roles = "knittingroll-3")]
        public ActionResult Delete(long knittingRollId)
        {
            int deleted = 0;
            deleted = _knittingRollManager.DeleteKnittingRollById(knittingRollId);
            return deleted > 0 ? Reload() : ErrorResult("Delete failed");
        }
      
        public JsonResult ProgramAllAutocomplite(string serachString)
        {
            IEnumerable program = _programManager.GetProgramAllAutocomplite(serachString, PortalContext.CurrentUser.CompId);
            return Json(program, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProgramAutocomplite(string serachString)
        {
            IEnumerable program = _programManager.GetProgramAutocomplite(serachString, PortalContext.CurrentUser.CompId);
            return Json(program, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProgramCollarCuffAutocomplite(string serachString)
        {
            IEnumerable program = _programManager.GetProgramCollarCuffAutocomplite(serachString, PortalContext.CurrentUser.CompId);
            return Json(program, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProgramOutput(long programId)
        {
            VProgramDetail programDetail = _programManager.GetOutPutProgramDetails(programId).FirstOrDefault();
            return Json(programDetail, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProgramCollarCuffOutput(long programId, string componentRefId)
        {
            VProgramDetail programDetail =
                _programManager.GetOutPutProgramDetails(programId)
                    .FirstOrDefault(x => x.ComponentRefId == componentRefId);

            return Json(programDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyKnittingRoll(KnittingRollViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now.Date;
            model.KnittingRolls = _knittingRollManager.GetDailyKnittingRollByDate(model.FromDate ?? DateTime.Now.Date, PortalContext.CurrentUser.CompId);
            model.TotalQty = model.KnittingRolls.Sum(x => x.Quantity);
            return View(model);
        }
        public ActionResult MachineWiseKnitting(KnittingRollViewModel model)
        {
            ModelState.Clear();
            model.FromDate = model.FromDate ?? DateTime.Now;
            model.KType = model.KType ?? "IN";
            model.DataTable = _knittingRollManager.MachineWiseKnitting(model.FromDate, model.KType, PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult KnittingReceiveBalance(KnittingRollViewModel model)
        {

            ModelState.Clear();
            model.BuyerList = _buyerManager.GetAllBuyers();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            else
            {
                var pandingConsumptionDataTable = _knittingRollManager.GetKnittingReceiveBalance(PortalContext.CurrentUser.CompId, ProcessCode.KNITTING, model.OrderStyleRefId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = pandingConsumptionDataTable,
                    Path = "~/Areas/Production/Report/KnittingReciveBalance.rdlc",
                    DataSetName = "KnittingRcvBalanceDSet",
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
       
            return View(model);
        }
        public ActionResult GetKnittingRollStatementReport(KnittingRollViewModel model)
        {

            List<VwKnittingRoll> rollStatus = _knittingRollManager.GetKnittingRollStatus(model.FromDate, model.ToDate, PortalContext.CurrentUser.CompId);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;

            if (model.FromDate == null && model.ToDate == null)
            {
                fromDateParameter = new ReportParameter("FromDate", "ALL");
                toDateParameter = new ReportParameter("ToDate", "ALL");

            }
            else
            {
                fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "KnittingRollStatementReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KnittingRollDSet", rollStatus) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.5, MarginRight = 0.5, MarginBottom = .1 };
            return ReportExtension.ToFile((ReportType)Convert.ToInt16(model.ReportType), path, reportDataSources, deviceInformation, reportParameters);
        }


        public ActionResult RollSticker(long knittingRollId)
        {

            ModelState.Clear();
            DataTable dataTable = _knittingRollManager.GetRollSticker(knittingRollId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "Rollsticker.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RollStickerDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 4, PageHeight =2, MarginTop = .2, MarginLeft = .2, MarginRight = 0, MarginBottom = 0 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult RollPrintView(long id)
        {
            dynamic dataTable = _knittingRollManager.GetRollSticker(id, PortalContext.CurrentUser.CompId).Todynamic();
            return PartialView("~/Areas/Production/Views/KnittingRoll/_rollSticker", dataTable);
        }

        public ActionResult EditCollarCuff(KnittingRollViewModel model)
        {
            ModelState.Clear();
            if (model.KnittingRoll.KnittingRollId > 0)
            {
                model.KnittingRoll = _knittingRollManager.GetVwKnittingRollById(model.KnittingRoll.KnittingRollId);
          
            }
            else
            {
                model.KnittingRoll.RollDate = DateTime.Now;
                const string  prefix_collar_cuff="C";
                model.KnittingRoll.RollRefNo = _knittingRollManager.GetTodayRollNo(PortalContext.CurrentUser.CompId, prefix_collar_cuff);

            }
            model.KnittingRoll.SizeRefId = "0000";
            model.Machines = _machineManager.GetMachines(ProcessCode.KNITTING).Where(x=>x.MachineId==1058).ToList();
            return View(model);
        }

        public ActionResult ReceivedRollsQc(KnittingRollViewModel model)
        {
            var knittingRolls =_knittingRollManager.GeKnittedRolls(model.KnittingRoll.ProgramRefId,model.SearchString);
            model.Dictionary = knittingRolls.ToDictionary(x =>Convert.ToString(x.KnittingRollId), x => x);
            return View(model);
        }

        public ActionResult CheckedRejectedRoll(int knittingRollId)
        {
            int status= _knittingRollManager.CheckedRejectedRoll(knittingRollId);
            if (status > 0)
            {
                return ErrorResult("Rejected Roll Successfully Updated");
            }
            else
            {
                return ErrorResult("Rejected Roll Update Faile");
            }
        }
    }
}