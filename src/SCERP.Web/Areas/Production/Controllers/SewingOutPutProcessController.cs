using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Hub;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class SewingOutPutProcessController : BaseController
    {
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IMachineManager _machineManager;
        private readonly IHourManager _hourManager;
        private readonly ISewingInputProcessManager _sewingInputProcessManager;
       

        public SewingOutPutProcessController(ISewingInputProcessManager sewingInputProcessManager, ISewingOutPutProcessManager sewingOutPutProcessManager, IOmBuyerManager buyerManager, IMachineManager machineManager, IHourManager hourManager)
        {

            _sewingOutPutProcessManager = sewingOutPutProcessManager;
            _buyerManager = buyerManager;
            _machineManager = machineManager;
            _hourManager = hourManager;
            _sewingInputProcessManager = sewingInputProcessManager;
        }
        [AjaxAuthorize(Roles = "sewingoutput-1,sewingoutput-2,sewingoutput-3")]
        public ActionResult Index(SewingOutputProcessViewModel model)
        {
         
            ModelState.Clear();
            try
            {
                model.SewingOutPutProcess.SewingOutPutProcessRefId = _sewingOutPutProcessManager.GetNewSewingOutputProcessRefId();
                model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Retive Data:");
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "sewingoutput-2,sewingoutput-3")]
        public ActionResult Find([Bind(Include = "SewingOutPutProcess")] SewingOutputProcessViewModel model)
        {
            var httpCookie = Request.Cookies["manPower"];
            if (httpCookie != null)
            {
                model.SewingOutPutProcess.ManPower = Convert.ToInt32(httpCookie.Value);
            }
            
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.VwSewingOutputs = _sewingInputProcessManager.GetVwSewingInput(model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId, model.SewingOutPutProcess.OrderShipRefId);
            if (model.VwSewingOutputs.Count <= 0)
            {
                return ErrorResult("Data Not Found.");
            }
            List<string> sizeList = model.VwSewingOutputs.Select(x => x.SizeName).ToList();
            sizeList.Add("TotalQty");
            List<string> sizeRefIdList = model.VwSewingOutputs.Select(x => x.SizeRefId).ToList();
            List<string> orderQtyList = model.VwSewingOutputs.Select(x => Convert.ToString(x.OrderQty)).ToList();
            orderQtyList.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.OrderQty)));
            List<string> totalInputList = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalInput)).ToList();
            totalInputList.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalInput)));
            List<string> totalOutputQtylist = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalOutput)).ToList();
            totalOutputQtylist.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalOutput)));
            List<string> outputPercentList = model.VwSewingOutputs.Select(x =>x.OrderQty>0? String.Format("{0:0.00}" + " " + "%", (x.TotalOutput * 100.00m) / x.OrderQty):"0").ToList();
            outputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwSewingOutputs.Sum(x => x.TotalOutput) * 100.0m / model.VwSewingOutputs.Sum(x => x.OrderQty)));
            model.PivotDictionary.Add("Size", sizeList);
            model.PivotDictionary.Add("OrderQty", orderQtyList);
            model.PivotDictionary.Add("InputQty", totalInputList);
            model.PivotDictionary.Add("TOutputQty", totalOutputQtylist);
            model.PivotDictionary.Add("Output(%)", outputPercentList);
            foreach (var sizeRefId in sizeRefIdList)
            {
                model.SewingOutputProcessDetailDictionary.Add(sizeRefId, new PROD_SewingOutPutProcessDetail() { SizeRefId = sizeRefId, CompId = PortalContext.CurrentUser.CompId });
            }
            model.SewingOutPutProcess.OutputDate = DateTime.Now;
            model.Machines = _machineManager.GetLines();
            model.HourList = _hourManager.GetAllHour();
            model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetSewingOutputProcessByStyleColor(model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId, model.SewingOutPutProcess.OrderShipRefId);
            model.SewingOutPutProcess = new PROD_SewingOutPutProcess
            {
                OutputDate = DateTime.Now,
                SewingOutPutProcessRefId = _sewingOutPutProcessManager.GetNewSewingOutputProcessRefId(),
                ManPower = model.SewingOutPutProcess.ManPower
            };
            return PartialView("~/Areas/Production/Views/SewingOutPutProcess/_Plate.cshtml", model);
        }
        [AjaxAuthorize(Roles = "sewingoutput-2,sewingoutput-3")]
        public ActionResult Save(SewingOutputProcessViewModel model)
        {
          
            int index = 0;
            string message = "";
            try
            {
                model.SewingOutPutProcess.CompId = PortalContext.CurrentUser.CompId;
                model.SewingOutPutProcess.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                Response.Cookies["manPower"].Value =Convert.ToString(model.SewingOutPutProcess.ManPower) ;
                Response.Cookies["manPower"].Expires = DateTime.Now.AddDays(1);
                bool isExist = _sewingOutPutProcessManager.IsSewingOutputExist(model.SewingOutPutProcess);
                model.SewingOutPutProcess.PROD_SewingOutPutProcessDetail = model.SewingOutputProcessDetailDictionary.Where(x => x.Value.Quantity > 0).Select(x => x.Value).ToList();
                if (!isExist)
                {
                    if (model.SewingOutPutProcess.SewingOutPutProcessId > 0)
                    {
                        if (model.SewingOutPutProcess.PROD_SewingOutPutProcessDetail.Any())
                        {
                            if (model.SewingOutPutProcess.LineId > 0 && model.SewingOutPutProcess.HourId > 0)
                            {
                                index = _sewingOutPutProcessManager.EditSewingOutputProcess(model.SewingOutPutProcess);
                                message = "Edit Successfully";
                            }
                            else
                            {
                                message = "Please Line and Houre Select Properly";
                            }
                        }
                    }
                    else
                    {
                        if (model.SewingOutPutProcess.LineId > 0 && model.SewingOutPutProcess.HourId>0)
                        {
                            index = _sewingOutPutProcessManager.SaveSewingOutputProcess(model.SewingOutPutProcess);
                            message = "Saved Successfully";
                        }
                        else
                        {
                            message = "Please Line and Houre Select Properly";
                            return ErrorResult(message);
                        }
                    }
                }
                else
                {
                    return ErrorResult("Already Exist.");
                }

                if (index > 0)
                {
                    ModelState.Clear();
                    model.VwSewingOutputs = _sewingInputProcessManager.GetVwSewingInput(model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId, model.SewingOutPutProcess.OrderShipRefId);
                    if (model.VwSewingOutputs.Count <= 0)
                    {
                        return ErrorResult("Data Not Found.");
                    }
                    List<string> sizeList = model.VwSewingOutputs.Select(x => x.SizeName).ToList();
                    sizeList.Add("TotalQty");
                    List<string> sizeRefIdList = model.VwSewingOutputs.Select(x => x.SizeRefId).ToList();
                    List<string> orderQtyList = model.VwSewingOutputs.Select(x => Convert.ToString(x.OrderQty)).ToList();
                    orderQtyList.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.OrderQty)));
                    List<string> totalInputList = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalInput)).ToList();
                    totalInputList.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalInput)));
                    List<string> totalOutputQtylist = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalOutput)).ToList();
                    totalOutputQtylist.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalOutput)));
                    List<string> outputPercentList = model.VwSewingOutputs.Select(x =>x.OrderQty>0? String.Format("{0:0.00}" + " " + "%", (x.TotalOutput * 100.00m) / x.OrderQty):"0").ToList();
                    outputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwSewingOutputs.Sum(x => x.TotalOutput) * 100.0m / model.VwSewingOutputs.Sum(x => x.OrderQty)));
                    model.PivotDictionary.Add("Size", sizeList);
                    model.PivotDictionary.Add("OrderQty", orderQtyList);
                    model.PivotDictionary.Add("InputQty", totalInputList);
                    model.PivotDictionary.Add("TOutputQty", totalOutputQtylist);
                    model.PivotDictionary.Add("Output(%)", outputPercentList);
                    model.SewingOutputProcessDetailDictionary = new Dictionary<string, PROD_SewingOutPutProcessDetail>();
                    foreach (var sizeRefId in sizeRefIdList)
                    {
                        model.SewingOutputProcessDetailDictionary.Add(sizeRefId, new PROD_SewingOutPutProcessDetail() { SizeRefId = sizeRefId, CompId = PortalContext.CurrentUser.CompId });
                    }
                    model.Machines = _machineManager.GetLines();
                    model.HourList = _hourManager.GetAllHour();
                    model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetSewingOutputProcessByStyleColor(model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId, model.SewingOutPutProcess.OrderShipRefId);
                    model.SewingOutPutProcess=new PROD_SewingOutPutProcess
                    {
                        OutputDate = DateTime.Now,
                        SewingOutPutProcessRefId = _sewingOutPutProcessManager.GetNewSewingOutputProcessRefId(),
                        ManPower = model.SewingOutPutProcess.ManPower
                    };
                    string partialViewString = RenderViewToString("~/Areas/Production/Views/SewingOutPutProcess/_Plate.cshtml", model);
                    ProductionHub.NotifyDailySewingProduction();
                    return Json(
                            new
                            {
                                SaveStatus = true,
                                StatusMessage = message,
                                partialView = partialViewString,
                                refId = model.SewingOutPutProcess.SewingOutPutProcessRefId,
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return ErrorResult("Fail to Save/Edit :");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Save/Edit :");
            }
           
        }
        [AjaxAuthorize(Roles = "sewingoutput-2,sewingoutput-3")]
        public ActionResult Edit([Bind(Include = "SewingOutPutProcess")] SewingOutputProcessViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;

            model.VwSewingOutputs = _sewingOutPutProcessManager.GetAllSewingOutputInfo(model.SewingOutPutProcess.SewingOutPutProcessId, PortalContext.CurrentUser.CompId);
            List<string> sizeList = model.VwSewingOutputs.Select(x => x.SizeName).ToList();
            sizeList.Add("TotalQty");
            List<string> orderQtyList = model.VwSewingOutputs.Select(x => Convert.ToString(x.OrderQty)).ToList();
            orderQtyList.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.OrderQty)));
            List<string> totalInputQtylist = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalInput)).ToList();
            totalInputQtylist.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalInput)));
            List<string> totalOutputQtylist = model.VwSewingOutputs.Select(x => Convert.ToString(x.TotalOutput)).ToList();
            totalOutputQtylist.Add(Convert.ToString(model.VwSewingOutputs.Sum(x => x.TotalOutput)));
            List<string> outputPercentList = model.VwSewingOutputs.Select(x => x.TotalInput>0? String.Format("{0:0.00}" + " " + "%",(x.TotalOutput * 100.00m) / x.TotalInput):"0").ToList();
            outputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwSewingOutputs.Sum(x => x.TotalOutput) * 100.0m / model.VwSewingOutputs.Sum(x => x.TotalInput)));
            model.PivotDictionary.Add("Size", sizeList);
            model.PivotDictionary.Add("OrderQty", orderQtyList);
            model.PivotDictionary.Add("InputQty", totalInputQtylist);
            model.PivotDictionary.Add("TOutputQty", totalOutputQtylist);
            model.PivotDictionary.Add("Output(%)", outputPercentList);
            string key = "1";
            foreach (var input in model.VwSewingOutputs)
            {
                model.SewingOutputProcessDetailDictionary.Add(key, new PROD_SewingOutPutProcessDetail() { Quantity = input.OutputQuantity,QcRejectQty=input.QcRejectQty, SizeRefId = input.SizeRefId, CompId = PortalContext.CurrentUser.CompId });
                key = key + "1";
            }
            model.SewingOutPutProcess = _sewingOutPutProcessManager.GetSewintOutputProcessBySewingOutputProcessId(model.SewingOutPutProcess.SewingOutPutProcessId, PortalContext.CurrentUser.CompId);
            model.Machines = _machineManager.GetLines();
            model.HourList = _hourManager.GetAllHour();
            return PartialView("~/Areas/Production/Views/SewingOutPutProcess/_SewingOutput.cshtml", model);
        }
        [AjaxAuthorize(Roles = "sewingoutput-3")]
        public ActionResult Delete([Bind(Include = "SewingOutPutProcess")] SewingOutputProcessViewModel model)
        {
            int deleted = 0;
            deleted = _sewingOutPutProcessManager.DeleteSewingOutputProcess(model.SewingOutPutProcess.SewingOutPutProcessId, PortalContext.CurrentUser.CompId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Sewing Output Process.");
        }

        [AjaxAuthorize(Roles = "dailysewingoutput-1,dailysewingoutput-2,dailysewingoutput-3")]
        public ActionResult DailySewingOut(SewingOutputProcessViewModel model)
        {
            ModelState.Clear();
            model.Machines = _machineManager.GetLines();
            int totalRecord = 0;
            long totalQty = 0;
            model.FromDate = model.FromDate ?? DateTime.Now;
            model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetDailySewingOut(model.PageIndex, model.FromDate.GetValueOrDefault(),
            model.SewingOutPutProcess.LineId, out  totalRecord, out totalQty);
            model.TotalRecords = totalRecord;
            model.TotalQty = totalQty; 
            model.RowNo = model.PageIndex * (AppConfig.PageSize + 10);
            return View(model);
        }
         [AjaxAuthorize(Roles = "hourlysewingoutput-1,hourlysewingoutput-2,hourlysewingoutput-3")]
        public ActionResult HourlySewingOutput(SewingOutputProcessViewModel model)
        {
            ModelState.Clear();
            model.SewingOutPutProcess.OutputDate = DateTime.Now;
            return View(model);
        }
         [AjaxAuthorize(Roles = "linesizewiseoutput-1,linesizewiseoutput-2,linesizewiseoutput-3")]
        public ActionResult SizeAndLineOutput(SewingOutputProcessViewModel model)
        {
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }

         public ActionResult SewingWIP(SewingOutputProcessViewModel model)
         {
             ModelState.Clear();
             model.HourList = _hourManager.GetAllHour();
             if (!model.IsSearch)
             {
                 model.IsSearch = true;
                 model.SewingOutPutProcess.OutputDate = DateTime.Now;
                 return View(model);
             }
             DataTable sewingwipDataTable = _sewingOutPutProcessManager.GetSewingWIP(model.SewingOutPutProcess.OutputDate,model.SewingOutPutProcess.HourId,PortalContext.CurrentUser.CompId)??new DataTable();
             model.DataTable = sewingwipDataTable;
             return View(model); 
         }
         public ActionResult SewingWIPDetail(SewingOutputProcessViewModel model)
         {
             ModelState.Clear(); 
             if (!model.IsSearch)
             {
                 model.IsSearch = true;
                 model.SewingOutPutProcess.OutputDate = DateTime.Now;
                 model.DataTable= _sewingOutPutProcessManager.GetSewingWIPSummary(model.SewingOutPutProcess.OutputDate, PortalContext.CurrentUser.CompId) ?? new DataTable();
               
             }
             else
             {
                 model.DataTable = _sewingOutPutProcessManager.GetSewingWIPSummary(model.SewingOutPutProcess.OutputDate, PortalContext.CurrentUser.CompId) ?? new DataTable();
             }
             int sum = model.DataTable.Rows.Cast<DataRow>().Sum(dr => Convert.ToInt32(dr["WIP"]));
             model.TotalQty = sum;
             return View(model); 
         }
         public ActionResult HourlyProduction(SewingOutputProcessViewModel model)
         {
             ModelState.Clear();
             model.FromDate = model.FromDate ?? DateTime.Now;
             DataTable hourlyProductionDataTable = _sewingOutPutProcessManager.GetHourlyProduction(model.FromDate.GetValueOrDefault(),PortalContext.CurrentUser.CompId);
             model.DataTable = hourlyProductionDataTable;
             return View(model);
         }
         public ActionResult ManMachineUtiliztion(SewingOutputProcessViewModel model)
        {
            ModelState.Clear();
             model.SewingOutPutProcess.OutputDate = DateTime.Now;
            return View(model);
        }


         [HttpGet]
         public ActionResult SweingOutBarcode()
         {
             SewingOutputProcessViewModel model = new SewingOutputProcessViewModel();
             model.SewingOutPutProcess.OutputDate = DateTime.Now;
             model.Machines = _machineManager.GetLines();
             model.HourList = _hourManager.GetAllHour();
             model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetDailySewingOutData(DateTime.Now.Date, model.SewingOutPutProcess.LineId, PortalContext.CurrentUser.CompId);
             return View(model);
         }
         [HttpPost]
         public ActionResult SweingOutBarcode(SewingOutputProcessViewModel model)
         {
             int saved= _sewingOutPutProcessManager.SaveBarcodeSewingOutputProcess(model.SewingOutPutProcess);
             ProductionHub.NotifyDailySewingProduction();
             model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetDailySewingOutData(DateTime.Now.Date, model.SewingOutPutProcess.LineId, PortalContext.CurrentUser.CompId);
             return PartialView("~/Areas/Production/Views/SewingOutPutProcess/_SweingOutPutList.cshtml", model);
         }
         [HttpGet]
         public ActionResult GetSweingOutPutList(int lineId)
         
    {
             SewingOutputProcessViewModel model = new SewingOutputProcessViewModel();
             model.VwSewingOutputProcesses = _sewingOutPutProcessManager.GetDailySewingOutData(DateTime.Now.Date, lineId, PortalContext.CurrentUser.CompId);
             return PartialView("~/Areas/Production/Views/SewingOutPutProcess/_SweingOutPutList.cshtml", model);
         }
    }
}